using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GetData.Model
{
    public class BusTextData
    {
        public string go { get; set; }
        public string back { get; set; }
        public string timeInfo { get; set; }
    }

    public class BusName
    {
        public string number { get; set; }

        public string name { get; set; }

        public BusTextData busTextData { get; set; }
    }

    public class BusNameList
    {
        public ObservableCollection<BusName> busNameCollection { get; set; } 
    }

    public class COL
    {
        public string DATA { get; set; }
    }

    public class ROW
    {
        public List<COL> COL { get; set; }
    }

    public class TABLE
    {
        public List<ROW> ROW { get; set; }
    }

    public class RootRouteStation
    {
        public List<TABLE> TABLE { get; set; }
    }

    public class RouteStation
    {
        
    }

    public class BusCodedName
    {
        public int number { get; set; }
        public string name { get; set; }
    }
}
