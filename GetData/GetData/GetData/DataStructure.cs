using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetData.Model
{
    public class BusName
    {
        public string number { get; set; }

        public string name { get; set; }
    }

    public class BusNameList
    {
        public ObservableCollection<BusName> busNameCollection { get; set; } 
    }
}
