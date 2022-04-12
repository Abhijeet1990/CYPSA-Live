using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using QuickGraph.Graphviz;
using QuickGraph.Graphviz.Dot;

namespace NpView
{

    // Used for JSON deserialization of NP-View REST API results.
    public class NPMap
    {
        public NPNode[] t_nodes { get; set; }
    }

    // Used for JSON deserialization of NP-View REST API results.
    public class NPNode
    {
        public string type { get; set; }
        public string id { get; set; }
        public object[] nic { get; set; }
        public string netAddress { get; set; }
        public string netMask { get; set; }
    }

    // Used for JSON deserialization of NP-View REST API results.
    public class NPPath
    {
        public PathInfo path_info { get; set; }
    }

    // Used for JSON deserialization of NP-View REST API results.
    public class PathInfo
    {
        public string protocol { get; set; }
        public SrcIPRange srcIPRange { get; set; }
        public DstIPRange dstIPRange { get; set; }
        public DstPortRange dstPortRange { get; set; }
    }

    // Used for JSON deserialization of NP-View REST API results.
    public class SrcIPRange
    {
        public string first { get; set; }
        public string last { get; set; }
    }

    // Used for JSON deserialization of NP-View REST API results.
    public class DstIPRange
    {
        public string first { get; set; }
        public string last { get; set; }
    }

    // Used for JSON deserialization of NP-View REST API results.
    public class DstPortRange
    {
        public int first { get; set; }
        public int last { get; set; }
    }

    // Track details of a Nessus vulnerability (`ReportItem` element in Nessus
    // XML results) needed for calculating attack paths.
    public class NessusVulnerability
    {
        public int Port { get; set; }
        public string Proto { get; set; }
        public string CVE { get; set; }
        public double Score { get; set; }

        public NessusVulnerability(int port, string proto, string cve, double score)
        {
            Port = port;
            Proto = proto;
            CVE = cve;
            Score = score;
        }
    }

    // Used for XML serialization of final attack path XML.
    public class VCReport
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlElement("NmapAnalysis")]
        public List<NmapAnalysis> Analysis { get; set; }

        public VCReport()
        {
            Type = "vuln";
            Analysis = new List<NmapAnalysis>();
        }

        public VCReport(List<NmapAnalysis> analysis)
        {
            Type = "vuln";
            Analysis = analysis;
        }
    }

    // Used for XML serialization of final attack path XML.
    public class NmapAnalysis
    {
        [XmlAttribute("sourceNode")]
        public string SourceNode { get; set; }

        [XmlAttribute("sourceNodeId")]
        public string SourceNodeID { get; set; }

        [XmlAttribute("destinationNode")]
        public string DestinationNode { get; set; }

        [XmlAttribute("destinationNodeId")]
        public string DestinationNodeID { get; set; }

        [XmlElement("Path")]
        public AnalysisPath AnalysisPath { get; set; }

        public NmapAnalysis() { }

        public NmapAnalysis(string src, string srcID, string dst, string dstID)
        {
            SourceNode = src;
            SourceNodeID = srcID;
            DestinationNode = dst;
            DestinationNodeID = dstID;
            AnalysisPath = new AnalysisPath(src, srcID);
        }
    }

    // Used for XML serialization of final attack path XML.
    public class AnalysisPath
    {
        [XmlAttribute("attackNode")]
        public string AttackNode { get; set; }

        [XmlAttribute("nodeId")]
        public string AttackNodeID { get; set; }

        [XmlAttribute("cost")]
        public double Cost { get; set; }

        [XmlElement("Node")]
        public List<AnalysisNode> Nodes { get; set; }

        public AnalysisPath()
        {
            Nodes = new List<AnalysisNode>();
        }

        public AnalysisPath(string attacker, string attackerID)
        {
            AttackNode = attacker;
            AttackNodeID = attackerID;
            Nodes = new List<AnalysisNode>();
        }
    }

    // Used for XML serialization of final attack path XML.
    public class AnalysisNode
    {
        [XmlAttribute("IPAddress")]
        public string IP { get; set; }

        [XmlAttribute("nodeId")]
        public string NodeID { get; set; }

        [XmlAttribute("vulnID")]
        public string CVE { get; set; }

        [XmlAttribute("cost")]
        public double Cost { get; set; }

        public AnalysisNode() { }

        public AnalysisNode(string ip, string id, string cve, double cost)
        {
            IP = ip;
            NodeID = id;
            CVE = cve;
            Cost = cost;
        }
    }

    // Used for writing GraphViz dot notation to file.
    public class FileDotEngine : IDotEngine
    {
        public string Run(GraphvizImageType _, string dot, string output)
        {
            using (StreamWriter writer = new StreamWriter(output))
            {
                writer.Write(dot);
            }

            return System.IO.Path.GetFileName(output);
        }
    }

    // Abhijeet 04/23 : for storing the host vulnerability and score details
    public class Host
    {
        public string IP { get; set; }

        public List<string> Cves { get; set; }

        public List<string> Ports { get; set; }

        public List<string> Protocols { get; set; }

        public List<string> Severity { get; set; }

        public List<double> Costs { get; set; }

        public List<double> Impacts { get; set; }

        public Host() { }

        public Host(string ip, List<string> cves, List<double> costs, List<double> impacts, List<string> ports, List<string> protocols, List<string> severity)
        {
            IP = ip;
            Cves = cves;
            Costs = costs;
            Impacts = impacts;
            Ports = ports;
            Protocols = protocols;
            Severity = severity;
        }
    }

}
