using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using NetTools;
using Newtonsoft.Json.Linq;
using QuickGraph;
using QuickGraph.Algorithms;
using QuickGraph.Graphviz;

namespace NpView
{
    public class NPViewGraph
    {
        private static bool isValidIP(string ip)
        {
            try
            {
                IPAddress.Parse(ip);

                if (ip == "0.0.0.0")
                {
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public delegate void ProgressUpdate(string taskName);
        public event ProgressUpdate OnProgressUpdate;

        // There may be multiple viable paths between two hosts if the target
        // host (or intermediary hosts) has multiple vulnerabilities. This
        // function finds the single shortest path based on cumulative edge
        // score.
        private static List<Edge<string>> singleShortestPath(List<List<Edge<string>>> paths, string src, string dst)
        {
            var max = 0.0;
            var shortest = new List<Edge<string>>();

            foreach (var path in paths)
            {
                var cost = 0.0;

                foreach (var edge in path)
                {
                    var casted = (TaggedEdge<string, double>)edge;
                    cost += casted.Tag;
                }

                if (cost > max)
                {
                    max = cost;
                    shortest = path;
                }
            }

            return shortest;
        }

        // Track unique list of hosts in NP-View workspace.
        HashSet<string> hosts = new HashSet<string>();

        // Track mapping of IP addresses to host IDs.
        public Dictionary<string, string> hostIds = new Dictionary<string, string>();

        // Track unique list of networks in NP-View workspace, along with all
        // the hosts in each network.
        Dictionary<string, HashSet<string>> networks = new Dictionary<string, HashSet<string>>();

        // Map vulnerable IPs to their vulnerabilities. Vulnerabilities are
        // mapped by CVE.
        private Dictionary<string, Dictionary<string, NessusVulnerability>> vulnHosts = new Dictionary<string, Dictionary<string, NessusVulnerability>>();

        // Track vulnerable paths between hosts, using tagged edges with CVE
        // vulnerability scores as the tag for each edge.
        public AdjacencyGraph<string, Edge<string>> vulnGraph = new AdjacencyGraph<string, Edge<string>>();

        // NP-View username
        private string username;

        // NP-View password
        private string password;

        // Name of the NP-View workspace to interact with.
        private string workspace;

        // Username used for accessing workspaces. Will be same as username, but with "@" and "." replaced with "_".
        private string workspaceUser;

        // Path to Nessus scan results XML file for NP-View workspace.
        private string workspaceNessusResults;

        // HTTP client used to talk to NP-View REST API.
        private HttpClient client;

        private bool mock = false;
        private bool log = false;

        public NPViewGraph()
        {
            mock = true;
            log = true;
        }

        public NPViewGraph(string user, string pass, string space, string nessus)
        {
            username = user;
            password = pass;
            workspace = space;

            workspaceUser = username.Replace("@", "_").Replace(".", "_");
            workspaceNessusResults = nessus;
        }

        public NPViewGraph(string user, string pass, string space, string nessus, bool _log) : this(user, pass, space, nessus)
        {
            log = true;
        }

        // Do all the heavy lifing to compute the attack paths for the NP-View
        // workspace. This includes parsing connections and paths, parsing
        // Nessus scan results, and calculating shortest paths to vulnerable
        // hosts. The result is an XML file depicting all the possible attack
        // paths.
        public async Task ComputeAttack(string algo, List<string> sources, List<string> targets, string output)
        {
            if (mock)
            {
                ParseVulnerableHosts("mock/mock-vulns.xml");
            }
            else
            {
                // Abhijeet 04/24
                // Code for querying from NVD-database
                var hosts = await GetScores("files/nmap/");

                // Create xml file with the host details
                CreateVulnXML(hosts);

                // currently storing the updated xml file in mock-latest.xml
                workspaceNessusResults = "mock-latest.xml";

                ParseVulnerableHosts(workspaceNessusResults);
                await Login();
            }

            //bw.ReportProgress(1, "Parsing Connectivity");

            await ParseConnectivityMap();

            //bw.ReportProgress(2, "Parsing Paths");
            await ParsePaths();

            DumpToGraphViz("dot/vuln.dot");

            VCReport report;

            if (sources == null || sources.Count() == 0)
            {
                report = ShortestPath(algo, targets);
            }
            else
            {
                report = ShortestPath(algo, sources, targets);
            }
            //bw.ReportProgress(3, "Computing Attack Graph");

            DumpToXML(report, output);
            //bw.ReportProgress(4, "Exporting XML");
        }


        /*
        Parse Nessus results file, mapping each vulnerable IP address to a list of
        vulnerabilities, with each vulnerability for the IP address tracking
        protocol, port, CVE, and score.
        */
        public void ParseVulnerableHosts(string file)
        {
            var root = new XmlDocument();

            root.Load(file);

            var hosts = root.SelectNodes("//ReportHost");

            foreach (XmlNode host in hosts)
            {
                var name = host.Attributes["name"].Value;

                if (log) Console.WriteLine($"Host: {name}");

                var items = host.SelectNodes("ReportItem");

                // Map of vulnerabilities for this host, keyed by CVE.
                var vulns = new Dictionary<string, NessusVulnerability>();

                foreach (XmlNode item in items)
                {
                    var port = item.Attributes["port"];

                    if (port == null) continue;

                    var proto = item.Attributes["protocol"];

                    if (proto == null) continue;

                    if (port.Value == "0" && proto.Value != "icmp") continue;

                    var severity = item.Attributes["severity"].Value;

                    if (severity == "0") continue;

                    var score = item.SelectSingleNode("cvss3_temporal_score");

                    if (score == null) score = item.SelectSingleNode("cvss3_base_score");
                    if (score == null) score = item.SelectSingleNode("cvss_temporal_score");
                    if (score == null) score = item.SelectSingleNode("cvss_base_score");
                    if (score == null) continue;

                    var cves = item.SelectNodes("cve");

                    if (cves.Count == 0) continue;

                    foreach (XmlNode cve in cves)
                    {
                        if (log) Console.WriteLine($"  {cve.InnerText} ({proto.Value} - {port.Value} - {score.InnerText})");
                        vulns.Add(cve.InnerText, new NessusVulnerability(Int32.Parse(port.Value), proto.Value, cve.InnerText, Double.Parse(score.InnerText)));
                    }
                }

                vulnHosts.Add(name, vulns);
            }
        }

        public async Task Login()
        {
            HttpClientHandler handler = new HttpClientHandler();

            client = new HttpClient(handler);
            handler.CookieContainer = new CookieContainer();

            await client.GetAsync("http://localhost:8080/login");

            var form = new List<KeyValuePair<string, string>> {
                { new KeyValuePair<string, string>("userid", username) },
                { new KeyValuePair<string, string>("password", password) }
            };

            await client.PostAsync("http://localhost:8080/login", new FormUrlEncodedContent(form));
        }

        /*
        For each node in the NP-View map:
          * for each of the node's IP addresses:
            * associate the IP address with a network from the NP-View map
            * if the IP address has a vulnerability associated with it, add a vertex
              for each vulnerability
            * if the IP address isn't vulnerable, add a vertex for the IP address

        For each network in the NP-View map:
          * for each vulnerable node in the network, add a tagged edge between all the
            other nodes in the network
            * note that multiple edges for the same source node may be added if the
              source node has multiple vulnerabilities of its own
        */
        public async Task ParseConnectivityMap()
        {
            string body;

            //if (mock)
            //{
            //    body = await File.ReadAllTextAsync(@"mock/mock-map.json");
            //}
            //else
            //{
            //    var url = $"http://localhost:8080/map/{workspaceUser}@{workspace}/__default__";
            //    var response = await client.GetAsync(url);

            //    body = await response.Content.ReadAsStringAsync();
            //}

            //var url = $"http://localhost:8080/map/{workspaceUser}@{workspace}";
            //var response = await client.GetAsync(url);

            //body = await response.Content.ReadAsStringAsync();

            body = File.ReadAllText(@"C:\Users\substationc\Desktop\cypsa_live\NpView\files\eight_sub_connectivity_map.json");

            var map = JObject.Parse(body).ToObject<NPMap>();

            foreach (var node in map.t_nodes)
            {
                switch (node.type)
                {
                    case "network":
                        if (isValidIP(node.netAddress))
                        {
                            var net = $"{node.netAddress}/{node.netMask}";
                            networks.Add(net, new HashSet<string>());
                        }

                        break;
                    case "host":
                        foreach (var n in node.nic)
                        {
                            if (!n.GetType().Equals(typeof(string)))
                            {
                                continue;
                            }

                            var ip = (string)n;

                            if (isValidIP(ip))
                            {
                                hosts.Add(ip);
                                hostIds.Add(ip, node.id);
                            }
                        }

                        break;
                    default:
                        break;
                }
            }

            // Loop through all the host IPs and add them to their corresponding
            // network. This has to be done as a separate loop since it's
            // possible in the above loop that an IP address would be parsed
            // before its corresponding network is parsed.
            foreach (var host in hosts)
            {
                foreach (var network in networks.Keys)
                {
                    var net = IPAddressRange.Parse(network);

                    if (net.Contains(IPAddress.Parse(host)))
                    {
                        networks[network].Add(host);
                        break;
                    }
                }
            }

            foreach (var hosts in networks.Values)
            {
                foreach (var dst in hosts)
                {
                    // Only add edges for destinations that are vulnerable.
                    if (!vulnHosts.ContainsKey(dst))
                    {
                        continue;
                    }

                    foreach (var src in hosts)
                    {
                        if (src == dst)
                        {
                            continue;
                        }

                        foreach (var dstVuln in vulnHosts[dst].Values)
                        {
                            //if (log) Console.WriteLine($"Adding Edge (intra-network): {src} --> {dst}#{dstVuln.CVE}");
                            vulnGraph.AddVerticesAndEdge(new TaggedEdge<string, double>(src, $"{dst}#{dstVuln.CVE}", dstVuln.Score));
                        }

                        // If the source is vulnerable too, we also need to add
                        // an edge for each of the source's vulnerabilities.
                        if (vulnHosts.ContainsKey(src))
                        {
                            foreach (var srcVuln in vulnHosts[src].Values)
                            {
                                foreach (var dstVuln in vulnHosts[dst].Values)
                                {
                                    //if (log) Console.WriteLine($"Adding Edge (intra-network): {src}#{srcVuln.CVE} --> {dst}#{dstVuln.CVE}");
                                    vulnGraph.AddVerticesAndEdge(new TaggedEdge<string, double>($"{src}#{srcVuln.CVE}", $"{dst}#{dstVuln.CVE}", dstVuln.Score));
                                }
                            }
                        }
                    }
                }
            }
        }

        /*
        For each path in the NP-View paths:
          * confirm destination has a vulnerability exposed via the path
            * if destination is a range of IPs, get list of vertices within the range
              that have a vulnerability exposed via the path
          * confirm source is a vertex in the graph
            * if source is a range of IPs, get list of vertices within the range
          * for each source and vulnerable destination, add a tagged edge using the
            vulnerability's score as the tag value
            * note that there may be multiple source vertices if the source node has
              multiple vulnerabilities of its own
        */
        public async Task ParsePaths()
        {
            string body;

            //if (mock)
            //{
            //    body = await File.ReadAllTextAsync(@"mock/mock-path.json");
            //}
            //else
            //{
            //    var url = $"http://localhost:8080/path/{workspaceUser}@{workspace}";
            //    var response = await client.GetAsync(url);

            //    body = await response.Content.ReadAsStringAsync();
            //}

            //var url = $"http://localhost:8080/path/{workspaceUser}@{workspace}";
            //var response = await client.GetAsync(url);

            //body = await response.Content.ReadAsStringAsync();

            body = File.ReadAllText(@"C:\Users\substationc\Desktop\cypsa_live\NpView\files\eight_sub_access_paths.json");

            var obj = JObject.Parse(body);

            JObject xxx = (JObject)obj.SelectToken("paths.results.bySrcDst");

            // "xxx" --> "yyy" --> "zzz" --> path
            // "xxx", "yyy", and "zzz" are dynamic key names.
            // Path values will be structured.

            foreach (var x in xxx)
            {
                JObject yyy = (JObject)x.Value;

                if (yyy.HasValues)
                {
                    foreach (var y in yyy)
                    {
                        JObject zzz = (JObject)y.Value;

                        if (zzz.HasValues)
                        {
                            List<string> rules = new List<string>();

                            foreach (var z in zzz)
                            {
                                var path = ((JObject)z.Value).ToObject<NPPath>();
                                var info = path.path_info;

                                var srcIPs = IPAddressRange.Parse($"{info.srcIPRange.first} - {info.srcIPRange.last}");
                                var dstIPs = IPAddressRange.Parse($"{info.dstIPRange.first} - {info.dstIPRange.last}");

                                foreach (var dst in vulnHosts.Keys)
                                {
                                    if (!dstIPs.Contains(IPAddress.Parse(dst)))
                                    {
                                        continue;
                                    }

                                    // Track all the valid vulnerabilities the
                                    // destination host has for this path so we
                                    // can add an edge for each. Using a
                                    // dictionary here to ensure no duplicate
                                    // vulnerabilities.
                                    var vulns = new Dictionary<string, NessusVulnerability>();

                                    // Destination has vulnerabilities, so see
                                    // if any of them are accessible via the
                                    // allowed protocol and ports for this path.
                                    foreach (var v in vulnHosts[dst].Values)
                                    {
                                        if (v.Proto != info.protocol)
                                        {
                                            continue;
                                        }

                                        if (v.Port == info.dstPortRange.first || v.Port == info.dstPortRange.last)
                                        {
                                            vulns.Add(v.CVE, v);
                                            continue;
                                        }

                                        for (var i = info.dstPortRange.first; i < info.dstPortRange.last; i++)
                                        {
                                            if (v.Port == i)
                                            {
                                                vulns.Add(v.CVE, v);
                                                break;
                                            }
                                        }
                                    }

                                    // The destination host has no
                                    // vulnerabilites that are accessible via
                                    // this path's rules.
                                    if (vulns.Count == 0)
                                    {
                                        continue;
                                    }

                                    foreach (var src in hosts)
                                    {
                                        if (!srcIPs.Contains(IPAddress.Parse(src)))
                                        {
                                            continue;
                                        }

                                        foreach (var dstVuln in vulns.Values)
                                        {
                                            //if (log) Console.WriteLine($"Adding Edge: {src} --> {dst}#{dstVuln.CVE}");
                                            vulnGraph.AddVerticesAndEdge(new TaggedEdge<string, double>(src, $"{dst}#{dstVuln.CVE}", dstVuln.Score));
                                        }

                                        // If the source is vulnerable, we also
                                        // need to add an edge for each of the
                                        // source's vulnerabilities.
                                        if (vulnHosts.ContainsKey(src))
                                        {
                                            foreach (var srcVuln in vulnHosts[src].Values)
                                            {
                                                foreach (var dstVuln in vulns.Values)
                                                {
                                                    //if (log) Console.WriteLine($"Adding Edge: {src}#{srcVuln.CVE} --> {dst}#{dstVuln.CVE}");
                                                    vulnGraph.AddVerticesAndEdge(new TaggedEdge<string, double>($"{src}#{srcVuln.CVE}", $"{dst}#{dstVuln.CVE}", dstVuln.Score));
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        // Determine paths from all nodes to all vulnerable target nodes.
        public VCReport ShortestPath(string algo, List<string> targets)
        {
            var analysis = new List<NmapAnalysis>();

            foreach (var ip in hosts)
            {
                var a = ShortestPath(algo, ip, targets);

                if (a.Count() != 0)
                {
                    analysis.AddRange(a);
                }
            }

            return new VCReport(analysis);
        }

        // Determine paths from list of source nodes to all vulnerable target nodes.
        public VCReport ShortestPath(string algo, List<string> sources, List<string> targets)
        {
            var analysis = new List<NmapAnalysis>();

            foreach (var ip in sources)
            {
                if (!hosts.Contains(ip))
                {
                    continue;
                }

                var a = ShortestPath(algo, ip, targets);

                if (a.Count() != 0)
                {
                    analysis.AddRange(a);
                }
            }

            return new VCReport(analysis);
        }

        // Determine paths from given starting node to all vulnerable target nodes.
        public List<NmapAnalysis> ShortestPath(string algo, string from, List<string> targets)
        {
            var paths = new List<NmapAnalysis>();

            if (!vulnGraph.Vertices.Contains(from))
            {
                return paths;
            }

            double ScoreTag(Edge<string> edge)
            {
                var t = (TaggedEdge<string, double>)edge;
                return t.Tag;
            }

            Func<Edge<string>, double> edgeCost = ScoreTag;

            var tryGetPath = vulnGraph.TreeBreadthFirstSearch(from);

            if (algo.Contains("dijkstra")) tryGetPath = vulnGraph.ShortestPathsDijkstra(edgeCost, from);
            if (algo.Contains("bellman")) tryGetPath = vulnGraph.ShortestPathsBellmanFord(edgeCost, from);
            if (algo.Contains("dfs")) tryGetPath = vulnGraph.TreeDepthFirstSearch(from);

            foreach (var ip in vulnHosts.Keys)
            {
                if (ip == from)
                {
                    continue;
                }

                // If the list of target hosts is not null, is not empty, and
                // doesn't contain the current vulnerable target host, then skip
                // this shortest path search. If the list of target hosts is
                // null or empty, then paths to all vulnerable hosts will be
                // discovered.
                if (targets != null && targets.Count() > 0 && !targets.Contains(ip))
                {
                    continue;
                }

                // Since a target host (or intermediary hosts) may have multiple
                // vulnerabilities, multiple viable attack paths may be
                // possible. Accumulate them here so we can calculate the single
                // vulnerable path we're interested in based on edge score. The
                // other option would be to add all viable attack paths between
                // two nodes to the XML file as its own NmapAnalysis object.
                var allPaths = new List<List<Edge<string>>>();

                foreach (var cve in vulnHosts[ip].Keys)
                {
                    var to = $"{ip}#{cve}";
                    IEnumerable<Edge<string>> path;

                    tryGetPath(to, out path);

                    if (path == null)
                    {
                        continue;
                    }

                    allPaths.Add(path.ToList());
                }

                if (allPaths.Count() == 0)
                {
                    continue;
                }

                // Determine single shortest path to use between the `from` node
                // and this particular vulnerable node based on max cumulative
                // score of edges in the paths.
                var shortest = singleShortestPath(allPaths, from, ip);
                var analysis = new NmapAnalysis(from, hostIds[from], ip, hostIds[ip]);

                foreach (var edge in shortest)
                {
                    var casted = (TaggedEdge<string, double>)edge;
                    var tokens = edge.Target.Split('#');

                    var target = tokens[0];
                    var vuln = tokens[1];
                    var score = casted.Tag;

                    analysis.AnalysisPath.Cost += casted.Tag;
                    analysis.AnalysisPath.Nodes.Add(new AnalysisNode(target, hostIds[target], vuln, score));
                }

                paths.Add(analysis);
            }

            return paths;
        }

        public void DumpToXML(VCReport report, string output)
        {
            var serializer = new XmlSerializer(report.GetType());
            string path = @"C:\Users\substationc\Desktop\cypsa_live\WindowsForm\bin\Debug\";
            var file = File.Create(path+output);

            serializer.Serialize(file, report);
            file.Close();
        }

        public void DumpToGraphViz(string output)
        {
            var viz = new GraphvizAlgorithm<string, Edge<string>>(vulnGraph);
            viz.FormatVertex += (sender, args) => args.VertexFormatter.Comment = args.Vertex;
            viz.FormatEdge += (sender, args) => { args.EdgeFormatter.Label.Value = args.Edge.ToString(); };
            string path = @"C:\Users\substationc\Desktop\cypsa_live\WindowsForm\bin\Debug\";
            viz.Generate(new FileDotEngine(), path+output);
        }



        // Abhijeet 04/24:  Create the nessus vulnerability xml file as per the template followed in the mock-vulns.xml
        public void CreateVulnXML(List<Host> hosts)
        {
            var xml = @"<?xml version = ""1.0"" ?>" + System.Environment.NewLine;
            xml += @"<NessusClientData_v2>" + System.Environment.NewLine;
            xml += @"   <Report name=""testbed"" xmlns:cm=""http://www.nessus.org/cm"">" + System.Environment.NewLine;

            foreach (var host in hosts)
            {
                xml += @"        <ReportHost name=""" + host.IP + "\">" + System.Environment.NewLine;
                for (int i = 0; i < host.Ports.Count(); i++)
                {
                    if ((i+1) <= host.Costs.Count())
                    {
                        xml += @"           <ReportItem port=""" + host.Ports[i] + "\" protocol=\"" + host.Protocols[i] + "\" severity=\"" + host.Severity[i] + "\">" + System.Environment.NewLine;
                        xml += @"               <cve>" + host.Cves[i] + "</cve>" + System.Environment.NewLine;
                        xml += @"               <cvss_base_score>" + host.Costs[i] + "</cvss_base_score>" + System.Environment.NewLine;
                        xml += @"               <cvss_temporal_score>" + host.Impacts[i] + "</cvss_temporal_score>" + System.Environment.NewLine;
                        xml += @"           </ReportItem>" + System.Environment.NewLine;
                    }
                    
                }
                xml += @"        </ReportHost>" + System.Environment.NewLine;
            }
            xml += @"    </Report>" + System.Environment.NewLine;
            xml += @"</NessusClientData_v2>";
            File.WriteAllText("mock-latest.xml", xml);
        }


        // Abhijeet 04/24:  Get the scores from the NVD database from cloud and returns the list of hosts along with their vulnerabilities and scores
        public async Task<List<Host>> GetScores(string folderpath)
        {
            string[] files = Directory.GetFiles(folderpath, "*nmap*", SearchOption.AllDirectories);
            var filePaths = files.ToList();

            var hosts = HttpTransactions.parseNmapXML(filePaths);

            var results = new Dictionary<string, string>();

            foreach (var host in hosts)
            {
                List<Double> scores = new List<Double>();
                List<string> severity = new List<string>();
                List<Double> impact_scores = new List<Double>();
                string res = "";
                foreach (var cve in host.Cves)
                {
                    if (!results.ContainsKey(cve))
                    {
                        res = await HttpTransactions.GetScores(cve);
                        results[cve] = res;
                    }
                    else res = results[cve];
                                       
                    if (string.IsNullOrEmpty((string)res)){
                        continue;
                    }
                    
                    var jsonData = JObject.Parse(res).Children();

                    List<JToken> tokens = jsonData.Children().ToList();
                    var exp_score = tokens.Last()["CVE_Items"][0]["impact"]["baseMetricV2"]["exploitabilityScore"].ToString();
                    var impact_score = tokens.Last()["CVE_Items"][0]["impact"]["baseMetricV2"]["impactScore"].ToString();
                    var sev = tokens.Last()["CVE_Items"][0]["impact"]["baseMetricV2"]["severity"].ToString();
                    //var x = jsonString.Last["result"]["CVE_Items"];
                    scores.Add(Convert.ToDouble(exp_score));
                    impact_scores.Add(Convert.ToDouble(impact_score));
                    severity.Add(sev);
                }
                host.Costs = scores;
                host.Severity = severity;
                host.Impacts = impact_scores;
            }
            return hosts;
        }




    }
}
