using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CypsaLive.SimAuto
{
    public class TItem
    {
        public string id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        //public bool Sold { get; set; }
        //public bool PaymentDistributed { get; set; }
        //public TVendor Owner { get; set; }
        public string Display
        {
            get
            {
                return String.Format("{0} - {1}", id, Price);
            }
        }

    }

    // kate 10/23/18 inherit from item
    public class TGen : TItem
    {
        //
        public decimal mw { get; set; }
        public string Display
        {
            get
            {
                return String.Format("{0} - MW: {1}", id, mw);
            }
        }
    }

    public class TLoad : TItem
    {
        //
        public string subname { get; set; }

    }

    public class TBus : TItem
    {
        public decimal busAngle { get; set; }
        public int subnum { get; set; }
        public double xcord { get; set; }
        public double ycord { get; set; }
        public string Display
        {
            get
            {
                return String.Format("{0} - Bus Angle: {1}", id, busAngle);
            }
        }
    }

    public class TBranch : TItem
    {
        public string fromBus { get; set; }
        public string toBus { get; set; }
        public decimal flow { get; set; }
        public bool connected { get; set; }
        public string Display
        {
            get
            {
                return String.Format("{0}->{1} Status:{2}", fromBus, toBus, connected);
            }
        }
    }

    public class TBreaker
    {
        public string name { get; set; }
        public string fromBus { get; set; }
        public string toBus { get; set; }
        public string status { get; set; }

    }

    public class TRelay
    {
        public string relayName { get; set; }
        public string relayType { get; set; }
        public string relayIP { get; set; }
    }

    public class TContingency
    {
        public string Name { get; set; }

        public string Display
        {
            get
            {
                return String.Format("{0}", Name);
            }
        }
    }

    public class AccessNode
    {
        public int node { get; set; }
        public string nodeIp { get; set; }
        public string vulnId { get; set; }
        public double cost { get; set; }
        public double cpbc { get; set; }
        public double bc { get; set; }
        public double phyIndex { get; set; }
        public double secIndex { get; set; }
        public bool isTarget { get; set; }
        public List<AccessPath> myAccessPaths { get; set; }
    }
    public class AccessPath
    {
        public int nodeId { get; set; }
        public string attackNode { get; set; }
        public double cost { get; set; }
        public int nodeCount { get; set; }
        public string attackPath { get; set; }
        public List<AccessNode> nodes { get; set; }

    }

    public class ProgressEventArgs : EventArgs
    {
        public int Progress { get; set; }
        public ProgressEventArgs(int progress)
        {
            Progress = progress;
        }
    }


}
