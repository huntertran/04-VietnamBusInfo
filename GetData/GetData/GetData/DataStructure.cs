using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

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
        public int no { get; set; }
        public int stationId { get; set; }
        public int nextStationId { get; set; }
        public string route { get; set; }
        public string name { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }
        public string address { get; set; }
    }

    public class GPSPoint
    {
        public double lat { get; set; }
        public double lon { get; set; }
    }

    public class DirectionRoute
    {
        public bool isGo { get; set; }
        public ObservableCollection<RouteStation> routeStationCollection { get; set; }

        public ObservableCollection<GPSPoint> routePoints { get; set; } 

    }

    public class BusCodedName
    {
        public int number { get; set; }
        public string name { get; set; }

        public ObservableCollection<DirectionRoute> directionRouteCollection { get; set; } 
    }

    public class CodedBusNameList
    {
        public ObservableCollection<BusCodedName> codedBusNameCollection { get; set; } 
    }
}
