using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CypsaLive;
using CypsaLive.SimAuto;

namespace WindowsForm
{
    public partial class AttackTreeView : Form
    {

        public Graphics mygraph;
        public List<NodeUC> nodeUCs = new List<NodeUC>();
        //public List<List<int>> attackpaths = new List<List<int>>();

        static int udCircleDia = 400;
        static int ucCircleDia = 300;
        static int scCircleDia = 200;
        static int sdCircleDia = 100;

        static string[] vulns = new string[5] { "CVE-2015-6300", "CVE-2019-1010003", "CVE-2019-15869", "CVE-2019-14805", "CVE-2019-14792" };

        static int xorigin = 50, yorigin = 50;

        static int centerX = xorigin + (int)(udCircleDia / 2);
        static int centerY = yorigin + (int)(udCircleDia / 2);

        Pen utilityDMZ = new Pen(Color.Azure);
        SolidBrush fillUtilityDMZ = new SolidBrush(Color.Azure);
        Pen utilityControl = new Pen(Color.PowderBlue);
        SolidBrush fillUtilityControl = new SolidBrush(Color.PowderBlue);
        Pen substationControl = new Pen(Color.DeepSkyBlue);
        SolidBrush fillSubstationControl = new SolidBrush(Color.DeepSkyBlue);
        Pen substationDevice = new Pen(Color.SteelBlue);
        SolidBrush fillSubstationDevice = new SolidBrush(Color.SteelBlue);


        Rectangle udCircle = new Rectangle(xorigin, yorigin, udCircleDia, udCircleDia);
        Rectangle ucCircle = new Rectangle(xorigin + (int)((udCircleDia - ucCircleDia) / 2), yorigin + (int)((udCircleDia - ucCircleDia) / 2), ucCircleDia, ucCircleDia);
        Rectangle scCircle = new Rectangle(xorigin + (int)((udCircleDia - scCircleDia) / 2), yorigin + (int)((udCircleDia - scCircleDia) / 2), scCircleDia, scCircleDia);
        Rectangle sdCircle = new Rectangle(xorigin + (int)((udCircleDia - sdCircleDia) / 2), yorigin + (int)((udCircleDia - sdCircleDia) / 2), sdCircleDia, sdCircleDia);

        public List<AccessPath> accessPaths;
        public List<AccessNode> accessNodes;

        private void AttackTreeView_Load(object sender, EventArgs e)
        {
            Random scoreAllocator = new Random();
            Random vulnAllocator = new Random();

            for (int i = 0; i < this.accessPaths.Count; i++)
            {
                for (int j = 0; j < this.accessPaths[i].nodeCount; j++)
                {
                    NodeUC nc = new NodeUC();
                    nc.name = "H" + this.accessPaths[i].nodes[j].node.ToString();
                    nc.id = this.accessPaths[i].nodes[j].node;
                    nc.sectorId = 0;
                    for (int k = 0; k < vulnAllocator.Next(1, 4); k++)
                    {
                        nc.vulnIds.Add(k + 1);
                        nc.vulnNames.Add(vulns[vulnAllocator.Next(0, 5)]);
                    }

                    var selNode = this.accessNodes.FirstOrDefault(q => q.node == this.accessPaths[i].nodes[j].node);
                    List<string> vulnerabilities = new List<string>();
                    if (selNode.vulnId != null)
                    {
                        if (selNode.vulnId.Contains(','))
                        {
                            vulnerabilities.AddRange(selNode.vulnId.Split(','));
                        }
                        else
                        {
                            vulnerabilities.Add(selNode.vulnId);
                        }
                    }

                    // if found vulnerabilities then replace the nc's by the one obtained then the static ones that are used
                    nc.vulnIds.Clear();
                    nc.vulnNames.Clear();

                    for (int m = 0; m < vulnerabilities.Count(); m++)
                    {
                        nc.vulnIds.Add(m + 1);
                        nc.vulnNames.Add(vulnerabilities[m]);
                    }

                    nc.Click += new EventHandler(node_Click);
                    nodeUCs.Add(nc);
                }

            }

            // Based on the number of attack paths split circles in sector
            //List<Point> equidistantPoint = new List<Point>();

            DataTable t = new DataTable();
            t.Columns.Add("Id");
            t.Columns.Add("Target");
            t.Columns.Add("Cyber");
            t.Columns.Add("PI");
            foreach (var path in this.accessPaths)
            {
                double cc = 0;
                double phy = 0;
                foreach (var node in path.nodes)
                {
                    var n = nodeUCs.Where(k => k.id == node.node).FirstOrDefault();
                    n.level = 4 - (path.nodes.IndexOf(node) + 1);
                    n.sectorId = this.accessPaths.IndexOf(path) + 1;
                    var selNode = accessNodes.FirstOrDefault(q => q.node == node.node);
                    //cc += n.vulnIds.Count * scoreAllocator.Next(7, 11);
                    cc += selNode.cost;
                    if (phy == 0)
                    {
                        phy = selNode.phyIndex;
                    }
                }
                var dw = t.NewRow();
                dw["Id"] = this.accessPaths.IndexOf(path) + 1;
                dw["Target"] = "H" + path.nodes[path.nodeCount - 1].node.ToString();
                dw["Cyber"] = cc;
                //dw["PI"] = scoreAllocator.Next(2, 7) + scoreAllocator.NextDouble();
                dw["PI"] = phy;
                t.Rows.Add(dw);
            }
            scoreDGV.DataSource = t;
            scoreDGV.Columns[0].Width = 40;
            scoreDGV.Columns[1].Width = 50;
            scoreDGV.Columns[2].Width = 50;
            scoreDGV.Columns[3].Width = 80;

            scoreDGV.Columns[0].Frozen = false;
            scoreDGV.Columns[1].Frozen = false;
            scoreDGV.Columns[2].Frozen = false;
            scoreDGV.Columns[3].Frozen = false;

            scoreDGV.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            scoreDGV.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            scoreDGV.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            scoreDGV.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            scoreDGV.AutoSize = true;
        }

        private void createLinks()
        {

            int radius = 0;
            Random rn = new Random();
            foreach (var node in nodeUCs)
            {
                if (node.sectorId != 0)
                {
                    if (node.level == 1) radius = udCircleDia / 2;
                    else if (node.level == 2) radius = ucCircleDia / 2;
                    else if (node.level == 3) radius = scCircleDia / 2;
                    else if (node.level == 4) radius = sdCircleDia / 2;
                    else radius = sdCircleDia / 2;
                    int x = centerX + (int)(radius * Math.Cos((double)(6.28 * (node.sectorId - 1)) / (double)(this.accessPaths.Count())));
                    int y = centerY + (int)(radius * Math.Sin((double)(6.28 * (node.sectorId - 1)) / (double)(this.accessPaths.Count())));
                    int x_adj = centerX + (int)(radius * Math.Cos((double)(6.28 * (node.sectorId)) / (double)(this.accessPaths.Count())));
                    int y_adj = centerY + (int)(radius * Math.Sin((double)(6.28 * (node.sectorId)) / (double)(this.accessPaths.Count())));

                    // allocate a random point between the two extreme points
                    int xcoord = (int)(x + (double)(x_adj - x) * (double)(rn.Next(1, 4)) / (double)4);
                    int ycoord = (int)(y + (double)(y_adj - y) * (double)(rn.Next(1, 4)) / (double)4);

                    if (node.Location.X == 0 && node.Location.Y == 0)
                    {
                        node.Location = new Point(xcoord, ycoord);
                        Label tb = new Label();
                        tb.BackColor = System.Drawing.Color.Transparent;
                        tb.AutoSize = true;
                        tb.Location = node.Location;
                        tb.Text = node.name;
                        this.Controls.Add(node);
                        this.Controls.Add(tb);
                    }
                }
            }

            Random rn2 = new Random();
            mygraph.DrawEllipse(utilityDMZ, udCircle);
            mygraph.DrawEllipse(utilityControl, ucCircle);
            mygraph.DrawEllipse(substationControl, scCircle);
            mygraph.DrawEllipse(substationDevice, sdCircle);

            mygraph.FillEllipse(fillUtilityDMZ, udCircle);
            mygraph.FillEllipse(fillUtilityControl, ucCircle);
            mygraph.FillEllipse(fillSubstationControl, scCircle);
            mygraph.FillEllipse(fillSubstationDevice, sdCircle);

            Pen p = new Pen(Brushes.Yellow);

            p.StartCap = LineCap.Round;
            p.EndCap = LineCap.ArrowAnchor;


            foreach (var path in this.accessPaths)
            {
                if (rn2.NextDouble() > 0.8 && rn2.NextDouble() < 0.85) p.Brush = Brushes.Red;
                else if (rn2.NextDouble() > 0.85 && rn2.NextDouble() < 0.9) p.Brush = Brushes.Green;
                else if (rn2.NextDouble() > 0.9 && rn2.NextDouble() < 0.95) p.Brush = Brushes.Chocolate;
                else if (rn2.NextDouble() > 0.95 && rn2.NextDouble() < 1.0) p.Brush = Brushes.LavenderBlush;
                for (int i = 0; i < path.nodes.Count() - 1; i++)
                {


                    var src = nodeUCs.Where(k => k.id == path.nodes[i].node).FirstOrDefault().Location;
                    var dest = nodeUCs.Where(k => k.id == path.nodes[i + 1].node).FirstOrDefault().Location;

                    // width of the path will be based on the number of vulnerabilities on the destination
                    p.Width = nodeUCs.Where(k => k.id == path.nodes[i + 1].node).FirstOrDefault().vulnIds.Count;
                    mygraph.DrawLine(p, src, dest);
                }
            }

        }

        private void node_Click(object sender, EventArgs e)
        {
            vulnStatusBox.Visible = true;
            StringBuilder sb = new StringBuilder();
            var node = (NodeUC)sender;
            foreach (var path in this.accessPaths)
            {
                foreach (var n in path.nodes)
                {
                    if (node.id == n.node)
                    {
                        //var line = string.Join<int>(" -> ", path);
                        var line = "";
                        foreach (var na in path.nodes)
                        {
                            line += "H" + na.node.ToString() + "->";
                        }
                        line += "Contingency";
                        sb.AppendLine(line);
                        foreach (var no in path.nodes)
                        {
                            //var vulns = nodeUCs.Where(k => k.id == no).FirstOrDefault().vulnIds;
                            //var vulntext = "Vuln of Host " +no.ToString()+": v"+string.Join<int>(", v", vulns) + "\n";
                            var vulns = nodeUCs.Where(k => k.id == no.node).FirstOrDefault().vulnNames;
                            var vulntext = "Vuln of Host H" + no.node + ": " + Environment.NewLine + string.Join("," + Environment.NewLine, vulns) + Environment.NewLine;
                            sb.AppendLine(vulntext);
                        }
                        continue;
                    }
                }

            }
            vulnStatusBox.Text = sb.ToString();
        }

        public AttackTreeView()
        {
            InitializeComponent();
            mygraph = CreateGraphics();
            timer1.Enabled = true;
        }


        private void Timer1_Tick_1(object sender, EventArgs e)
        {
            createLinks();
        }

        public AttackTreeView(List<AccessPath> _accessPaths, List<AccessNode> _accessNodes)
        {
            InitializeComponent();
            mygraph = CreateGraphics();
            timer1.Enabled = true;
            accessPaths = _accessPaths;
            accessNodes = _accessNodes;
        }
    }
}
