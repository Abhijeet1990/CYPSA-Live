using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CypsaLive.SimAuto
{
    public class TStore
    {
        public string Name { get; set; }
        public List<TItem> Items { get; set; }
        public List<TGen> GenItems { get; set; }
        public List<TLoad> LoadItems { get; set; }
        public List<TBus> BusItems { get; set; }
        public List<TBranch> BranchItems { get; set; }
        public List<TBreaker> BreakerItems { get; set; }
        public List<TRelay> RelayItems { get; set; }

        public List<TContingency> CtgItems { get; set; }

        public TStore() // kate 10/9/18 type ctor then tab tab to auto create this constructor
        {
            //
            //Vendors = new List<TVendor>();
            Items = new List<TItem>();
            GenItems = new List<TGen>();
            LoadItems = new List<TLoad>();
            BusItems = new List<TBus>();
            BranchItems = new List<TBranch>();
            BreakerItems = new List<TBreaker>();
            RelayItems = new List<TRelay>();
            CtgItems = new List<TContingency>();
        }

    }

}
