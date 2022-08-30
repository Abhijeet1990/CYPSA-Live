# CyPSA-Live
It is an application for Cyber Physical Situational Awareness through interaction with NP-Live and NVD database through creation of attack graph templates. The focus is on critical asset ranking, and power system contingency analysis.

This is the windows desktop application with some backend programmed in python for betweenness centrality and cyber physical betweenness centrality.

**Description of the files and folders within `NpView` folder**
- `HttpTransactions.cs` : This class is used for parsing the open ports from the nmap file. Further utilize the open port information to obtain a set of CVEs so as to query the NVD database to obtain the scores using their APIs.
- `NPViewGraph.cs` : This is the class used to construct the attack graph by parsing the connectivity, vulnerable hosts, and access path information. The `ParseVulnerableHosts()` parses the Nessus results, maps each vulnerablie IP address to a list of vulnerabilities, 
with each vulnerability for the IP address tracking protocol, port, CVE and score. `ParseConnectivityMap()` constructs the network topology (basic structure of the attack graph). `ParsePaths()` function takes the access paths from the NP-Live application to add additional 
edge into the attack graph. `CreateVulnXML()` creates the nessus vulnerability xml file as per the template of the `mock-vulns.xml`. `GetScores()` gets the scores from the NVD database from cloud and returns the list of hosts along with their vulnerabilities and scores














