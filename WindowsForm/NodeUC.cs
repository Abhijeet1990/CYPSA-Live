using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsForm
{
    public partial class NodeUC : UserControl
    {
        public int id;
        public string name;
        public int level;
        public int sectorId;
        public int hostId;
        public List<int> vulnIds = new List<int>();
        public List<string> vulnNames = new List<string>();
        public NodeUC()
        {
            InitializeComponent();
        }
        public NodeUC(int _id)
        {
            id = _id;
        }
    }
}
