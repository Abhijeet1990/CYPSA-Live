using CypsaLive.SimAuto;
using FastMember;
using NpView;
using QuickGraph;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace WindowsForm
{
    public partial class cypsa_main : Form
    {
        //migrate all the properties variable to the static variables
        public static string PWCaseFile;
        public static string TigerGraphUrl;
        public static string ImagePath;
        public static string DotExecutablePath;
        public static string ProjectDirectory;
        public static string PWCaseDir;
        public static string Display_axd;
        public static bool CyberDefined;
        public static bool SubstationDefined;
        public static string algoType;
        public static NPViewGraph demoNPVGraph;
        public static double vulnPercentToPatch;

        // local data creation that will get created and distroyed with the main form
        private static TStore demoStore;
        static BindingSource genItemsBinding = new BindingSource(); // 

        //private TSimAuto demoSimAuto = new TSimAuto(Settings.Default.PWCaseDir, file); // kate 10/23/18 calls the constructor to run the sim auto demo
        private static TSimAuto demoSimAuto; // kate 10/23/18 calls the constructor to run the sim auto demo
        static BindingSource simAutoBinding = new BindingSource(); // now the form should be able to get items from sim auto. 1/2/19 I don't think this was finished.

        // kate 11/28/18 Fields related to NPV Graph
        static string path = ""; // for TAMUPC

        static string connFile = "";// // NPview connectivity file
        static string topoFile = "";// path + "\\npv\\topology-dict.json"; // Topology json file

        // kate 11/28/18 options related to NPV Graph. Had to make these static to get rid of compile error.
        static bool useEasiestPathIfMultiplePathsExist = true; // old var name: easiest
        static bool dynamicallyMonitorCompromisedAndPatchedFiles = true; // old var name: dynamic // "tells the process to run continuously and to monitor changes in the compromised hosts and patched hosts

        // kate 11/28/18 OBSOLETE: change the below to internal data structures that are lists, initially entered from the form, instead of external files.
        static string limitedTargets = "";//path + "\\critical-assets.txt"; // note, this one is at base folder
        static string compromisedFile = "";// path + "\\compromised.csv";
        static string patchedFile = ""; //path + "\\patched.csv";
        static string nodeIP = "";//"127.0.0.1"; // Node IP address old varname: var nodeIP = Console.ReadLine();
        static string projectName = "";// path + "\\npv";


        //private TNPVGraph demoNPVGraph = new TNPVGraph(useEasiestPathIfMultiplePathsExist, dynamicallyMonitorCompromisedAndPatchedFiles, connFile, topoFile, limitedTargets, compromisedFile, patchedFile, nodeIP, projectName); // kate 1/2/19
        static BindingSource npvGraphBinding = new BindingSource(); // kate 1/2/19 Add this
        static BindingSource busItemsBinding = new BindingSource();
        static BindingSource branchItemsBinding = new BindingSource();
        static BindingSource breakerItemsBinding = new BindingSource();
        static BindingSource relayItemsBinding = new BindingSource();
        static BindingSource relaySelectedBinding = new BindingSource();
        static BindingSource contingencyBinding = new BindingSource();

        //Abhijeet 20/1/19: Displaying the selected Attack path in the picture box
        //static string imagePath = "C:\\Users\\abhijeet_ntpc\\Downloads\\SwagShop\\SwagShopUI\\bin\\Debug\\test.png";
        //string executableLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        static string imagePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\test.png";

        // Abhijeet 28/1/19 : variables added for select all the relay features
        CheckBox HeaderCheckBox = null;
        int TotalCheckBoxes = 0;
        int TotalCheckedCheckBoxes = 0;
        bool IsHeaderCheckBoxClicked = false;

        public static List<AccessPath> storedAccessPath;
        public static List<AccessNode> storedAccessNodes;

        public OpenFileDialog ofd = new OpenFileDialog();
        public static bool firstLoad = true;
        public object OneLineDiagram { get; private set; }
        public object OneLineDiagramForm { get; private set; }

        public cypsa_main()
        {
            InitializeComponent();
            if (!firstLoad)
            {
                UpdatePWCase();
            }
            firstLoad = false;

        }

        public void UpdatePWCase()
        {
            demoStore = new TStore();
            demoSimAuto = new TSimAuto(PWCaseDir, PWCaseFile);
            path = ProjectDirectory;
            connFile = path + "npv\\connectivity.csv";
            topoFile = path + "npv\\topology-dict.json";

            limitedTargets = path + "\\critical-assets.txt"; // note, this one is at base folder
            compromisedFile = path + "\\compromised.csv";
            patchedFile = path + "\\patched.csv";
            nodeIP = "127.0.0.1"; // Node IP address old varname: var nodeIP = Console.ReadLine();
            projectName = path + "\\npv";

            accessPathsGridView.CellContentClick += delegate (object sender, DataGridViewCellEventArgs e) { accessPathsGridView_CellContentClick(sender, e, imagePath); };

            if (path == "") return;

            SetupData(); // This is where the form gets the data -- for the relays, this would be from PW

            // Abhijeet 1/19/2019: Reprogramming from scratch since i didnt get the logic of vendor and shopkeeper here...
            // Populate the lblItems with Gen and Bus and other device item data
            genItemsBinding.DataSource = demoStore.GenItems;
            lbGenItems.DataSource = genItemsBinding;
            lbGenItems.DisplayMember = "Display"; // This is the property from the itemsbinding to show up in the listbox, weird that it's a text field
            lbGenItems.ValueMember = "Display"; // 

            // Bottom left Vendors Binding
            busItemsBinding.DataSource = demoStore.BusItems;
            lbBusItems.DataSource = busItemsBinding;
            lbBusItems.DisplayMember = "Display"; // Display full name 
            lbBusItems.ValueMember = "Display";

            branchItemsBinding.DataSource = demoStore.BranchItems;
            lbBranchItems.DataSource = branchItemsBinding;
            lbBranchItems.DisplayMember = "Display";
            lbBranchItems.ValueMember = "Display";
        }

        private void SetupData()
        {
            // Abhijeet 19/1/19 : Fetching Generator,Bus, Branch and relay data from Power World
            demoSimAuto.myStore = demoStore;
            demoSimAuto.OpenCase(); // kate 1/28/19 this should not be happening in each function!
            demoSimAuto.SetupGenData();
            demoSimAuto.SetupBusData(SubstationDefined, Display_axd);
            demoSimAuto.SetupBranchData();
            demoSimAuto.SetupBreakerData();
            demoSimAuto.SetupRelayData(CyberDefined);
            //demoSimAuto.SetContingencies();
            demoSimAuto.SaveAndCloseCase(); // kate 1/28/19 this should not be happening in each fucntion!

            // kate 1/2/19
            //demoNPVGraph(); 
            //demoNPVGraph.SetUpLimitedTargets();
            breakerItemsBinding.DataSource = demoStore.BreakerItems;
            breakerGridView.DataSource = breakerItemsBinding;
            //breakerGridView.Columns["breakerCmd"].Visible = true;
            //breakerGridView.Columns[0].HeaderText = "Select";
            breakerGridView.Columns[0].HeaderText = "Breaker";
            breakerGridView.Columns[1].HeaderText = "From";
            breakerGridView.Columns[2].HeaderText = "To";
            breakerGridView.Columns[3].HeaderText = "Status";

            contingencyBinding.DataSource = demoStore.CtgItems;
            contingencyList.DataSource = contingencyBinding;
            contingencyList.DisplayMember = "Display"; // This is the property from the itemsbinding to show up in the listbox, weird that it's a text field
            contingencyList.ValueMember = "Display"; // 

        }

        private async Task run_scenario()
        {
            // To make calls to the NP-View REST API using this constructor, be
            // sure the NP-View II application is running on the same machine
            // you're running this program on. NP-View II **only** listens on
            // http://localhost:8080 (this is not a configurable option).
            //var npg = new NPViewGraph("abhijeet_ntpc@tamu.edu", "<my_password>", "demo", "files/demo-workspace-nessus-scan.xml", true);
            demoNPVGraph = new NPViewGraph("abhijeet_ntpc@tamu.edu", "<my_password>", "Eight_Sub_Case", "files/demo-workspace-nessus-scan.xml", true);

            // This constructor will cause mock data to be used.
            //var npg = new NPViewGraph();

        }

        private async void cypsa_main_Load(object sender, EventArgs e)
        {
            await run_scenario();
        }

        private void accessPathsGridView_CellContentClick(object sender, DataGridViewCellEventArgs e, string imagePath)
        {
            graphViewPanel.Controls.Clear();
            graphViewPanel.Invalidate();
            // Implement a graphical view of the network object with vulnerability details
            var path = accessPathsGridView.CurrentCell.Value.ToString();
            if (!path.Contains("->")) return;

            List<string> nodes = path.Split(new[] { "->" }, StringSplitOptions.None).ToList();
            List<host> hostUCs = new List<host>();
            foreach (var node in nodes)
            {
                var hostIp = demoNPVGraph.hostIds.FirstOrDefault(x => x.Value == "node").Key;
                host h = new host();
                h.id = node;
                h.hostIPLabel.Text = hostIp;
                h.hostIPLabel.Font = new Font("Arial", 8, FontStyle.Bold);
                hostUCs.Add(h);

            }

            int panelXorigin = graphViewPanel.Location.X;
            int panelYorigin = graphViewPanel.Location.Y;
            int panelWidth = graphViewPanel.Width;
            int panelHeight = graphViewPanel.Height;
            // place them in the visualization panel
            foreach (var h in hostUCs)
            {
                // divide the area into the number of hosts

                int xoffset = 5;
                int yoffset = 5;
                int xcoord = xoffset + Convert.ToInt16(panelWidth / hostUCs.Count) * hostUCs.IndexOf(h);
                int ycoord = yoffset;
                int height = h.Size.Height;
                int width = h.Size.Width;
                h.Location = new Point(xcoord / 2, ycoord);
                //this.Controls.Add(h);
                graphViewPanel.Controls.Add(h);
            }
            Graphics g;
            g = graphViewPanel.CreateGraphics();
            Pen p = new Pen(Brushes.DarkBlue);
            p.Width = 3;
            p.StartCap = LineCap.Round;
            p.EndCap = LineCap.ArrowAnchor;
            // Draw the edges 
            for (int i = 0; i < nodes.Count - 1; i++)
            {
                Point pt_from = hostUCs[i].Location;
                Point pt_to = hostUCs[i + 1].Location;
                pt_from.X += hostUCs[i].Size.Width;
                pt_from.Y += panelHeight / 2;
                pt_to.Y += panelHeight / 2;
                g.DrawLine(p, pt_from, pt_to);
            }
            g.Dispose();
            p.Dispose();
        }


        // Abhijeet 20/1/19: Use of Background Worker to show the progress bar and the progress label
        private async void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            // Update the critical-assets file 
            //var ips = lbLimitedTargets.Items.Cast<String>().ToList();
            var ips = lbLimitedTargets.SelectedItems.Cast<string>().ToList();
            File.WriteAllText(limitedTargets, String.Empty);

            string text = "";
            for (int i = 0; i < ips.Count; i++)
            {
                text += ips[i] + System.Environment.NewLine;
            }
            File.WriteAllText(limitedTargets, text);
            double perc = 0.0;
            List<string> hid = new List<string>();


            var output = Path.Combine("output", "attackgraph-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".xml");

            // await npg.ComputeAttack("dijkstra", new List<string>{"172.16.1.101"}, new List<string>{"10.2.1.201"}, output);
            await demoNPVGraph.ComputeAttack("dijkstra", null, null, output);

            // Got to update this portion of code as per vulnerability fixing

            //var pathNodes = lbPatchTargets.SelectedItems.Cast<string>().ToList();
            //if (vulnPercentToPatch == 0.0 || pathNodes.Count == 0)
            //{
            //    demoNPVGraph = new TNPVGraph(useEasiestPathIfMultiplePathsExist, dynamicallyMonitorCompromisedAndPatchedFiles, connFile, topoFile, limitedTargets, compromisedFile, patchedFile, nodeIP, projectName, algoType, backgroundWorker1, DotExecutablePath, hid, vulnPercentToPatch);
            //}
            //else
            //{
            //    hid.AddRange(pathNodes); //Amara: Made it a list of strings?
            //    demoNPVGraph = new TNPVGraph(useEasiestPathIfMultiplePathsExist, dynamicallyMonitorCompromisedAndPatchedFiles, connFile, topoFile, limitedTargets, compromisedFile, patchedFile, nodeIP, projectName, algoType, backgroundWorker1, DotExecutablePath, hid, vulnPercentToPatch);
            //}



            demoNPVGraph.OnProgressUpdate += npvGraphOnProgressUpdate;
        }

        // Abhijeet 20/1/19: Currently not utilizing this event, since the constructor design of the TPNVGraph should not call all the function to design the attack graph tree
        private void npvGraphOnProgressUpdate(string status)
        {
            // Its another thread so invoke back to UI thread
            base.Invoke((Action)delegate
            {
                progressLabel.Text = status;
            });
        }

        // Abhijeet 20/1/19: Updating the Progress Bar and Label. Whatever report is sent from the TPNVGraph process is reflected here
        private void backgroundWorker1_ProgressChanged_1(object sender, ProgressChangedEventArgs e)
        {
            progressBarGenerateAGT.Value = e.ProgressPercentage;
            if (e.UserState != null) progressLabel.Text = e.UserState.ToString();
        }

        // Abhijeet 20/1/19: Once the Background process is completed
        private void backgroundWorker1_RunWorkerCompleted_1(object sender, RunWorkerCompletedEventArgs e)
        {
            progressLabel.Text = "Fetched the Attack Graph Tree";
        }

        private void backgroundWorker1_DoWork_1(object sender, DoWorkEventArgs e)
        {

        }

        private async void btnCalculatePaths_Click(object sender, EventArgs e)
        {
            //backgroundWorker1.RunWorkerAsync();
            demoNPVGraph = new NPViewGraph("abhijeet_ntpc@tamu.edu", "Bunty#2021#", "Eight_Sub_Case", "files/demo-workspace-nessus-scan.xml", true);
            var output = Path.Combine("output", "attackgraph-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".xml");

            // await npg.ComputeAttack("dijkstra", new List<string>{"172.16.1.101"}, new List<string>{"10.2.1.201"}, output);
            await demoNPVGraph.ComputeAttack("dijkstra", null, null, output);
        }

        private void btnGetRelaysFromPW_Click(object sender, EventArgs e)
        {
            // Connnect to Sim Auto (use 8 substation case)
            //            o Use Komal SimAuto example code to start with(status: I got it to compile)
            //	1.Get all relays from PW, cycle through each one and open it in PW, calculate PI, display on form
            //•	Compare to Delphi example CyPSA
            //•	First goal: Get one relay from PW and display it

            AddHeaderCheckBox();
            HeaderCheckBox.MouseClick += new MouseEventHandler(HeaderCheckBox_MouseClick);

            // Get the list of relays in the case
            relayItemsBinding.DataSource = demoStore.RelayItems;
            //var list = new BindingList<TRelay>(accessPaths);
            relaysDataGridView.DataSource = relayItemsBinding;
            relaysDataGridView.Columns["Select"].Visible = true;
            relaysDataGridView.Columns[0].HeaderText = "Select";
            relaysDataGridView.Columns[1].HeaderText = "Name";
            relaysDataGridView.Columns[2].HeaderText = "Relay Type";
            relaysDataGridView.Columns[3].HeaderText = "Relay IP";
            AutoSizeGridView(relaysDataGridView);
        }

        private void AddHeaderCheckBox()
        {
            HeaderCheckBox = new CheckBox();

            HeaderCheckBox.Size = new Size(15, 15);

            //Add the CheckBox into the DataGridView
            this.relaysDataGridView.Controls.Add(HeaderCheckBox);
        }

        private void HeaderCheckBox_MouseClick(object sender, MouseEventArgs e)
        {
            HeaderCheckBoxClick((CheckBox)sender);
        }
        private void HeaderCheckBoxClick(CheckBox HCheckBox)
        {
            IsHeaderCheckBoxClicked = true;

            foreach (DataGridViewRow Row in relaysDataGridView.Rows)
                ((DataGridViewCheckBoxCell)Row.Cells["Select"]).Value = HCheckBox.Checked;

            relaysDataGridView.RefreshEdit();
            TotalCheckedCheckBoxes = HCheckBox.Checked ? TotalCheckBoxes : 0;
            IsHeaderCheckBoxClicked = false;
        }

        private void AutoSizeGridView(DataGridView dgv)
        {
            for (int i = 0; i < dgv.Columns.Count - 1; i++)
            {
                dgv.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            dgv.Columns[dgv.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            for (int i = 0; i < dgv.ColumnCount; i++)
            {
                int colw = dgv.Columns[i].Width;
                dgv.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dgv.Columns[i].Width = colw;
            }
        }

        private void btnAddToCart_Click(object sender, EventArgs e)
        {
            List<string> ipSelected = new List<string>();
            foreach (DataGridViewRow row in relaysDataGridView.Rows)
            {
                if (Convert.ToBoolean(row.Cells[0].Value))
                {
                    if (row.Cells[3].Value == null) continue;
                    if (!ipSelected.Contains(row.Cells[3].Value.ToString()))
                    {
                        ipSelected.Add(row.Cells[3].Value.ToString());
                    }
                }
            }
            relaySelectedBinding.DataSource = ipSelected;
            lbLimitedTargets.DataSource = relaySelectedBinding;
            lbPatchTargets.DataSource = lbLimitedTargets.DataSource;
        }

        private void populateAccessNodes_Click(object sender, EventArgs e)
        {
            // Parse attack graph tree
            // Get the latest attack Graph Tree xml
            var logDirectory = @"C:\Users\substationc\Desktop\cypsa_live\WindowsForm\bin\Debug\output\";
            var fileName = GetLastFileInDirectory(logDirectory, "attackgraph*");
            var path_nodes = ParseXMLNew(logDirectory + fileName);
            var AccessNodes = path_nodes.Item1;
            var AccessPaths = path_nodes.Item2;

            // Extract the vulngraph and compute the BC and CPBC metric
            var vulngraph = demoNPVGraph.vulnGraph;

            double ScoreTag(Edge<string> edge)
            {
                var t = (TaggedEdge<string, double>)edge;
                return t.Tag;
            }

            string amarapath = @"C:\Users\substationc\Desktop\cypsa_live\WindowsForm\BC\amara.csv";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("From,To,Cost,internet_node_id,relay_node_id");
            List<int> internet_nodes = new List<int>();
            List<int> relay_nodes = new List<int>();
            List<string> relays = new List<string> { "101", "102", "103", "104", "105", "106", "107", "108", "109", "110", "201", "202", "203", "204", "205", "206", "207", "208", "209", "210"};
            foreach (var item in AccessNodes)
            {
                if (relays.Any(s=>item.nodeIp.Contains(s))) relay_nodes.Add(item.node);
                else internet_nodes.Add(item.node);
            }

            for (int i = 0; i < vulngraph.Edges.Count(); i++)
            {
                
                var from = vulngraph.Edges.ToList()[i].Source.Split('#')[0];
                from = demoNPVGraph.hostIds[from];
                var to = vulngraph.Edges.ToList()[i].Target.Split('#')[0];
                to = demoNPVGraph.hostIds[to];
                var cost = ScoreTag(vulngraph.Edges.ToList()[i]);
                var oneEdge = from + "," + to + "," + cost.ToString();
                if ((i + 1) <= internet_nodes.Count()) oneEdge += "," + internet_nodes[i].ToString();
                if ((i + 1) <= relay_nodes.Count()) oneEdge += "," + relay_nodes[i].ToString();
                sb.AppendLine(oneEdge);
            }
            File.WriteAllText(amarapath, sb.ToString());

            // query the python code to obtain the BC and CPBC metrics
            // This part of the code can be automatized with the use of tools like Nessus

            // call the code behind of python to get the cves
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = @"C:\Users\substationc\AppData\Local\Programs\Python\Python37-32\python.exe";
            var script = @"C:\Users\substationc\Desktop\cypsa_live\WindowsForm\BC\cpbc.py";
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

            var BC = result_string.Split('*')[0];
            var CPBC = result_string.Split('*')[1];
            List<string> bcs = BC.Split(',').ToList<string>();
            List<string> cpbcs = CPBC.Split(',').ToList<string>();

            foreach (var bc in bcs)
            {
                var id = bc.Split('#')[0];
                var value = bc.Split('#')[1];
                var a = AccessNodes.FirstOrDefault(o => o.node.Equals(Convert.ToInt32(id)));
                a.bc = Convert.ToDouble(value);
            }

            foreach (var cpbc in cpbcs)
            {
                var id = cpbc.Split('#')[0];
                var value = cpbc.Split('#')[1];
                var a = AccessNodes.FirstOrDefault(o => o.node.Equals(Convert.ToInt32(id)));
                a.cpbc = Convert.ToDouble(value);
            }


            storedAccessNodes = AccessNodes;
            populateGridViewAccessNodes(AccessNodes);
        }

        public string GetLastFileInDirectory(string directory, string pattern)
        {
            if (directory.Trim().Length == 0)
                return string.Empty; //Error handler can go here

            //string pattern = "*.txt"

            var dirInfo = new DirectoryInfo(directory);
            var file = (from f in dirInfo.GetFiles(pattern) orderby f.LastWriteTime descending select f).First();

            return file.ToString();
        }

        // Abhijeet 28/1/19: Parse the attack Tree XML but form a different table
        private Tuple<List<AccessNode>, List<AccessPath>> ParseXMLNew(string filePath)
        {
            // create document instance using XML file path
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);

            XmlNodeList nmaps = doc.DocumentElement.SelectNodes("/VCReport/NmapAnalysis");
            List<AccessPath> accPaths = new List<AccessPath>();
            List<AccessNode> accNodes = new List<AccessNode>();

            List<String> lstitems = new List<String>();
            foreach (string str in lbLimitedTargets.Items) lstitems.Add(str);

            // OpenAllCyber() function may not work for the power world case where the cyber model is not ready 
            List<double> pi = new List<double>();
            if (CyberDefined) pi = demoSimAuto.OpenAllCyber(lstitems);
            else pi = demoSimAuto.OpenAllBreaker(lstitems);

            Dictionary<string, double> ip_pi = new Dictionary<string, double>();
            for (int i = 0; i < lstitems.Count; i++)
            {
                ip_pi[lstitems[i]] = pi[i];
            }

            foreach (XmlNode nmap in nmaps)
            {
                // Get NmapAnalysis Tags
                var srcNode = nmap.Attributes["sourceNode"].Value;
                var srcNodeID = nmap.Attributes["sourceNodeId"].Value;
                var destNode = nmap.Attributes["destinationNode"].Value;
                var destNodeID = nmap.Attributes["destinationNodeId"].Value;

                // Get Path tags
                AccessPath myPath = new AccessPath();
                var pathNode = nmap.SelectSingleNode("Path");
                var attackNode = pathNode.Attributes["attackNode"].Value;
                var nodeID = pathNode.Attributes["nodeId"].Value;
                var pathCyberCost = pathNode.Attributes["cost"].Value;
                myPath.attackNode = attackNode;
                myPath.cost = Convert.ToDouble(pathCyberCost);
                myPath.nodeId = Convert.ToInt32(nodeID);


                // It stores the list of nodes inside one set of NmapAnalysis Tag
                List<AccessNode> nodesTemp = new List<AccessNode>();
                // first attach the node of the source
                AccessNode myNode = new AccessNode();
                myNode.node = Convert.ToInt32(nodeID);
                myNode.nodeIp = attackNode;
                //myNode.vulnId = "None";
                myNode.isTarget = false;
                nodesTemp.Add(myNode);

                // Get Node tags
                var nodes = pathNode.ChildNodes;
                var last = nodes.Item(nodes.Count - 1);
                foreach (XmlNode node in nodes)
                {
                    var nodeIp = node.Attributes["IPAddress"].Value;
                    var nodeId = node.Attributes["nodeId"].Value;
                    var vulnId = node.Attributes["vulnID"].Value;
                    AccessNode an = new AccessNode();
                    an.node = Convert.ToInt32(nodeId);
                    an.nodeIp = nodeIp;
                    an.vulnId = vulnId;
                    if (node == last) an.isTarget = true; else an.isTarget = false;
                    // store the performance index
                    if (ip_pi.ContainsKey(nodeIp))
                    {
                        an.phyIndex = ip_pi[nodeIp];
                    }
                    nodesTemp.Add(an);
                }
                myPath.nodes = nodesTemp;
                myPath.nodeCount = myPath.nodes.Count;

                List<string> listNode = new List<string>();
                for (int i = 0; i < myPath.nodes.Count; i++)
                {
                    listNode.Add(myPath.nodes[i].node.ToString());
                }
                myPath.attackPath = listNode.Aggregate((i, j) => i + "->" + j);
                accPaths.Add(myPath);

                accNodes.AddRange(nodesTemp);
            }

            // There is redundancy in the nodes hence we extract the unique nodes based on the node Id
            List<AccessNode> uniqueNodeList = new List<AccessNode>();
            List<int> checkList = new List<int>();
            for (int i = 0; i < accNodes.Count; i++)
            {
                // if does exist continue
                if (checkList.Contains(accNodes[i].node)) continue;
                else
                {
                    uniqueNodeList.Add(accNodes[i]);
                    checkList.Add(accNodes[i].node);
                }
            }

            for (int i = 0; i < uniqueNodeList.Count; i++)
            {
                double cost = 0;
                List<AccessPath> pathsTemp = new List<AccessPath>();
                for (int j = 0; j < accPaths.Count; j++)
                {
                    for (int k = 0; k < accPaths[j].nodes.Count; k++)
                    {
                        if (accPaths[j].nodes[k].node == uniqueNodeList[i].node)
                        {
                            pathsTemp.Add(accPaths[j]);
                            //uniqueNodeList[i].myAccessPaths.Add(accPaths[j]);
                            cost += accPaths[j].cost;
                            if (string.IsNullOrEmpty(accPaths[j].nodes[k].vulnId)) uniqueNodeList[i].vulnId += "";
                            else
                            {
                                if (uniqueNodeList[i].vulnId.Contains(accPaths[j].nodes[k].vulnId)) uniqueNodeList[i].vulnId += "";                               
                                else uniqueNodeList[i].vulnId += "\n" + accPaths[j].nodes[k].vulnId;
                            }
                        }
                    }
                }
                uniqueNodeList[i].myAccessPaths = pathsTemp;
                uniqueNodeList[i].cost = cost;


                if (ip_pi.ContainsKey(uniqueNodeList[i].nodeIp))
                {
                    uniqueNodeList[i].secIndex = (double)uniqueNodeList[i].phyIndex / (double)uniqueNodeList[i].cost;
                }
            }

            // return the uniqueNodeList
            // return the Access Path List
            return Tuple.Create(uniqueNodeList, accPaths);
        }

        private void populateGridViewAccessPaths(List<AccessPath> accessPaths)
        {
            // Calculate the hop count distribution
            var grouped_data = accessPaths.GroupBy(p => p.nodeCount).Select(grp => grp.ToList()).ToList();

            DataTable table = new DataTable();
            using (var reader = ObjectReader.Create(accessPaths, "nodeId", "attackNode", "cost", "nodeCount", "attackPath"))
            {
                table.Load(reader);
            }
            accessPathsGridView.DataSource = table;
            Color curr = new Color();
            // bind the datagridview with the datatable content
            int c = 0;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (i == 0) curr = Color.LightSkyBlue;
                if (i >= 1 && table.Rows[i]["nodeId"].ToString() != table.Rows[i - 1]["nodeId"].ToString())
                {
                    c += 1;
                    Random rnd = new Random();
                    if (c % 2 == 0) curr = Color.LightSkyBlue; else curr = Color.WhiteSmoke;
                    Thread.Sleep(50);
                }
                accessPathsGridView.Rows[i].DefaultCellStyle.BackColor = curr;
                accessPathsGridView[0, i].Value = table.Rows[i]["nodeId"];
                accessPathsGridView[1, i].Value = table.Rows[i]["attackNode"];
                accessPathsGridView[2, i].Value = table.Rows[i]["cost"];
                accessPathsGridView[3, i].Value = table.Rows[i]["nodeCount"];
                accessPathsGridView[4, i].Value = table.Rows[i]["attackPath"];

            }

            // Set the column header names
            accessPathsGridView.Columns[0].HeaderText = "Node ID";
            accessPathsGridView.Columns[1].HeaderText = "Attacker IP";
            accessPathsGridView.Columns[2].HeaderText = "Cyber Cost";
            accessPathsGridView.Columns[3].HeaderText = "Number of Hosts";
            accessPathsGridView.Columns[4].HeaderText = "Path";
            AutoSizeGridView(accessPathsGridView);
        }

        //Abhijeet 28/1/2019: modified the code to distinguish target and othe nodes with addition of performance index
        // Abhijeet 17/1/2019:  Function to populate grid view
        private void populateGridViewAccessNodes(List<AccessNode> accessNodes)
        {
            // Try to change the color of the content based on the accessNode property
            DataTable table = new DataTable();
            using (var reader = ObjectReader.Create(accessNodes, "node", "nodeIp", "vulnId", "cost", "phyIndex", "secIndex","bc","cpbc", "isTarget", "myAccessPaths"))
            {
                table.Load(reader);
            }
            accessNodesGridView.DataSource = table;
            if (!accessNodesGridView.Columns.Contains("Path"))
                accessNodesGridView.Columns.Add("Path", "Paths");

            // bind the datagridview with the datatable content
            for (int i = 0; i < table.Rows.Count; i++)
            {
                accessNodesGridView["node", i].Value = table.Rows[i]["node"];
                accessNodesGridView["nodeIp", i].Value = table.Rows[i]["nodeIp"];

                // Populate the unique VUlnerability ID
                var vulnIds = table.Rows[i]["vulnId"].ToString().Split(new[] { "\n" }, StringSplitOptions.None).ToList();
                var unique = vulnIds.Distinct().ToList();
                string vuln = unique.Aggregate((m, n) => m + "\n" + n);
                accessNodesGridView["vulnId", i].Value = vuln;

                accessNodesGridView["cost", i].Value = Math.Round(Convert.ToDouble(table.Rows[i]["cost"]), 2);
                accessNodesGridView["phyIndex", i].Value = Math.Round(Convert.ToDouble(table.Rows[i]["phyIndex"]), 2);
                accessNodesGridView["secIndex", i].Value = Math.Round(Convert.ToDouble(table.Rows[i]["secIndex"]), 2);
                accessNodesGridView["bc", i].Value = Math.Round(Convert.ToDouble(table.Rows[i]["bc"]), 2);
                accessNodesGridView["cpbc", i].Value = Math.Round(Convert.ToDouble(table.Rows[i]["cpbc"]), 2);
                if (Convert.ToBoolean(table.Rows[i]["isTarget"]) == true) accessNodesGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
                else accessNodesGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;

                var nodes = table.Rows[i]["myAccessPaths"] as List<AccessPath>;
                string paths = "";
                for (int m = 0; m < nodes.Count; m++)
                {
                    paths += nodes[m].attackPath + "\n";
                }
                accessNodesGridView["Path", i].ToolTipText = paths;
                accessNodesGridView["Path", i].Value = nodes.Count().ToString();
            }
            // Set the column header names
            accessNodesGridView.Columns["node"].HeaderText = "Node ";
            accessNodesGridView.Columns["nodeIp"].HeaderText = "IP";
            accessNodesGridView.Columns["vulnId"].HeaderText = "Vuln";
            accessNodesGridView.Columns["cost"].HeaderText = "Cost";
            accessNodesGridView.Columns["phyIndex"].HeaderText = "PI";
            accessNodesGridView.Columns["secIndex"].HeaderText = "SI";
            accessNodesGridView.Columns["bc"].HeaderText = "BC";
            accessNodesGridView.Columns["cpbc"].HeaderText = "CPBC";
            accessNodesGridView.Columns["isTarget"].Visible = false;
            //accessNodesGridView.Columns[5].Visible = false;
            AutoSizeGridView(accessNodesGridView);

        }
        
        private void populateAccessPaths_Click(object sender, EventArgs e)
        {
            // Parse attack graph tree
            // Get the latest attack Graph Tree xml
            var logDirectory = @"C:\Users\substationc\Desktop\cypsa_live\WindowsForm\bin\Debug\output\";
            var fileName = GetLastFileInDirectory(logDirectory, "attackgraph*");
            //var path_nodes = ParseXML(logDirectory + fileName);
            var path_nodes = ParseXMLNew(logDirectory + fileName);

            var AccessNodes = path_nodes.Item1;
            var AccessPaths = path_nodes.Item2;

            storedAccessPath = AccessPaths;

            populateGridViewAccessPaths(AccessPaths);
        }

        private void loadCaseButton_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                displayCaseTB.Text = ofd.FileName;
                PWCaseFile = ofd.FileName.Split('\\').Last();

                PWCaseDir = ofd.FileName.TrimEnd('\\');
                PWCaseDir = PWCaseDir.Remove(PWCaseDir.LastIndexOf('\\') + 1);

                ProjectDirectory = PWCaseDir.TrimEnd('\\');
                //ProjectDirectory = ofd.FileName.TrimEnd('\\');
                ProjectDirectory = ProjectDirectory.Remove(ProjectDirectory.LastIndexOf('\\') + 1);

                if (ProjectDirectory.Contains("8sub"))
                {
                    CyberDefined = true;
                    SubstationDefined = true;
                    old_Button.Visible = true;
                    Display_axd = ProjectDirectory + "display.axd";
                }
                if (ProjectDirectory.Contains("24bus"))
                {
                    CyberDefined = false;
                    SubstationDefined = false;
                    old_Button.Visible = false;
                }
                if (ProjectDirectory.Contains("300bus"))
                {
                    CyberDefined = true;
                    SubstationDefined = false;
                    old_Button.Visible = false;
                    Display_axd = ProjectDirectory + "display.axd";
                }

                // Update Power World Case
                this.Hide();
                cypsa_main f = new cypsa_main();
                f.Show();
            }
        }

        private void AtvButton_Click(object sender, EventArgs e)
        {
            if (storedAccessPath == null)
            {
                accessPathStatusLabel.Text = "No access path generated";
                return;
            }
            AttackTreeView atv = new AttackTreeView(storedAccessPath, storedAccessNodes);
            atv.Show();
        }
    }
}
