# CyPSA-Live
It is an application for Cyber Physical Situational Awareness through interaction with NP-Live and NVD database through creation of attack graph templates. The focus is on critical asset ranking, and power system contingency analysis.

This is the windows desktop application with some backend programmed in python for betweenness centrality and cyber physical betweenness centrality.

**Description of the files and folders within `NpView` folder**
- `HttpTransactions.cs` : This class is used for parsing the open ports from the nmap file. Further utilize the open port information to obtain a set of CVEs so as to query the NVD database to obtain the scores using their APIs.
- `NPViewGraph.cs` : This is the class used to construct the attack graph by parsing the connectivity, vulnerable hosts, and access path information. The `ParseVulnerableHosts()` parses the Nessus results, maps each vulnerablie IP address to a list of vulnerabilities, 
with each vulnerability for the IP address tracking protocol, port, CVE and score. `ParseConnectivityMap()` constructs the network topology (basic structure of the attack graph). `ParsePaths()` function takes the access paths from the NP-Live application to add additional 
edge into the attack graph. `CreateVulnXML()` creates the nessus vulnerability xml file as per the template of the `mock-vulns.xml`. `GetScores()` gets the scores from the NVD database from cloud and returns the list of hosts along with their vulnerabilities and scores
- `util.cs` : Contains the declaration of all the user-defined class considered in the `NPViewGraph.cs` class for attack graph construction.

`CVE_Search` folder contains code extended from `https://github.com/cve-search/cve-search`. This is primarily used to perform local searches for known vulnerabilities. The main objective of the software is to avoid doing direct and public lookups into the public CVE databases. Local lookups are usually faster and you can limit your sensitive queries via the Internet.

**Description of the files and folders within `WindowsForm` folder**
This folder contain the main code for the windows application for interacting with the *PowerWorld* using SimAuto object, and combine it with the attack graph from the `NPViewGraph` object to compute the security index (SI)
```math
SI = \frac{PI}{CC}
```
where, *PI* is the performance index computed from the electric grid depending on the grid components that can be compromised based on the relays in compromised paths.
While *CC* is computed based on the summation of CVSS scores based on the vulnerabilities on the nodes in the access paths to the target node. 

- `SimAuto\SimAuto.cs` : This code opens the power world case and loads the branch, breaker, contingency, bus, generator, relay etc information. It is also considered for operating the grid using the simauto .com object from this code.
- `BC\cpbc.py` : This is the python script to obtain the betweenness centrality and cyber-physical betweenness centrality measures of each node from the attack graph. The users can refer to our paper [Cyber-physical component ranking for risk sensitivity analysis using betweenness centrality](https://ietresearch.onlinelibrary.wiley.com/doi/full/10.1049/cps2.12010)
- `AttackTreeView.cs` : This is a windows form for attack tree visualization.
- `cypsa_main.cs` : This is the main windows desktop application. 

The initial version of the CYPSA application is published in Intelligent Systems Applications to Power Systems (ISAP) 2019, [A Framework for Cyber-Physical Model Creation and Evaluation](https://ieeexplore.ieee.org/abstract/document/9065990)





