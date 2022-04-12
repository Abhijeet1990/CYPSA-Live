using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Xml;
using System.Linq;

namespace NpView
{
    public static class HttpTransactions
    {
        // Abhijeet 04/23: parse the open ports and generate a set of CVEs that will be used to query the NVD database to get the scores
        public static List<Host> parseNmapXML(List<string> filePaths)
        {
            List<Host> Hosts = new List<Host>();
            XmlDocument xDoc = new XmlDocument();
            List<string> cves = new List<string>();

            // Do that for all the nmap files present in the nmap scanned results
            foreach (var filePath in filePaths)
            {
                xDoc.Load(filePath);
                XmlNodeList hosts = xDoc.GetElementsByTagName("host");
                foreach (XmlNode host in hosts)
                {
                    var cnodes = host.ChildNodes;
                    var h = new Host();
                    List<string> services = new List<string>();
                    List<string> protocols = new List<string>();
                    foreach (XmlNode cnode in cnodes)
                    {
                        if (cnode.Name == "address")
                        {
                            if (cnode.Attributes["addr"].Value.Contains("."))
                            {
                                h.IP = cnode.Attributes["addr"].Value;
                            }
                        }

                        if (cnode.Name == "ports")
                        {
                            var gcnodes = cnode.ChildNodes;
                            foreach (XmlNode gcnode in gcnodes)
                            {
                                if (gcnode.Name == "port")
                                {
                                    services.Add(gcnode.Attributes["portid"].Value);
                                    protocols.Add(gcnode.Attributes["protocol"].Value);
                                }
                            }
                        }
                    }

                    //if (_cves.Count() == 0)
                    //{
                        // This part of the code can be automatized with the use of tools like Nessus
                        

                    if (cves.Count() == 0)
                    {
                        // call the code behind of python to get the cves
                        ProcessStartInfo start = new ProcessStartInfo();
                        start.FileName = @"C:\ProgramData\Anaconda3\python.exe";
                        //start.arguments = string.format("{0} {1}", "d:\\project_codes\\bryan_npview\\npview\\cve_search\\nmap2cve-search.py", services[0]);
                        var script = @"C:\Users\substationc\Desktop\cypsa_live\NpView\CVE_Search\Nmap2CVE-Search.py";
                        //start.arguments = string.format("{0}", "d:\\project_codes\\bryan_npview\\npview\\cve_search\\nmap2cve-search.py");
                        start.Arguments = $"\"{script}\"";
                        start.UseShellExecute = false;
                        start.RedirectStandardOutput = true;
                        var result_string = "";
                        using (Process process = Process.Start(start))
                        {
                            using (StreamReader reader = process.StandardOutput)
                            {
                                string result = reader.ReadToEnd();
                                result_string = result;
                            }
                        }

                        cves = result_string.Split(',').ToList<string>();
                    }
                        
                        h.Cves = new List<string>();
                        List<string> randomnumberlist = new List<string>();

                        // randomly pick any two cves for a host
                        randomnumberlist = GetRandomElements(cves, 2);
                        var host_cves = new List<string>();
                        for (int i = 0; i < randomnumberlist.Count(); i++)
                        {
                            h.Cves.Add(randomnumberlist[i].TrimEnd('\r', '\n'));
                            //_cves.Add(randomnumberlist[i].TrimEnd('\r', '\n'));
                            //h.Cves = _cves;
                        }

                    //}
                    //else
                    //{
                    //    h.Cves = _cves;
                    //}

                    //foreach (var serv in services)
                    //{
                    //    if (serv == "22") cves.Add("CVE-2015-7845");
                    //    else if (serv == "443") cves.Add("CVE-2015-6316");
                    //    else if (serv == "21") cves.Add("CVE-2015-7752");
                    //    else if (serv == "23") cves.Add("CVE-2015-6333");
                    //    else cves.Add("CVE-2015-7760");
                    //}
                    //h.Cves = cves;

                    h.Ports = services;
                    h.Protocols = protocols;
                    Hosts.Add(h);

                }
            }
            return Hosts;
        }

        public static List<t> GetRandomElements<t>(IEnumerable<t> list, int elementsCount)
        {
            return list.OrderBy(x => Guid.NewGuid()).Take(elementsCount).ToList();
        }


        // Abhijeet 04/23:  This is the function that uses http client to get the CVE details from the NVD database in cloud
        public async static System.Threading.Tasks.Task<string> GetScores(string cve)
        {
            if (cve == "\r\n") return "";
            var url = @"https://services.nvd.nist.gov/rest/json/cve/1.0/";
            var client = new HttpClient();
            try
            {
                var content = await client.GetStringAsync(url + cve);
                return content;
            }
            catch(Exception e)
            {
                return string.Empty;
            }
            

            
        }

    }
}
