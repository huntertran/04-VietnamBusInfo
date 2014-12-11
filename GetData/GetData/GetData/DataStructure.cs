using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace GetData.Model
{
    public class BasicBus
    {
        public string number { get; set; }
        public string name { get; set; }
    }

    public class BusTextData
    {
        public string go { get; set; }
        public string back { get; set; }
        public string timeInfo { get; set; }
    }

    public class BusName : BasicBus
    {
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

    public class BasicStation
    {
        public int no { get; set; }
        public int stationId { get; set; }
        public string name { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }
        public string address { get; set; }
    }

    public class RouteStation : BasicStation
    {
        public int nextStationId { get; set; }
        public string route { get; set; }
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

        public BusTextData busTextData { get; set; }

        public ObservableCollection<DirectionRoute> directionRouteCollection { get; set; } 
    }

    public class CodedBusNameList
    {
        public ObservableCollection<BusCodedName> codedBusNameCollection { get; set; } 
    }

    public enum DirectionType
    {
        Back = -1,
        Both = 0,
        Go = 1
    }

    public class ThroughStationBus : BasicBus
    {
        public DirectionType direction { get; set; }
    }

    public class GeneralStation : BasicStation
    {
        public GeneralStation()
        {
            
        }

        public GeneralStation(BasicStation basic)
        {
            no = basic.no;
            stationId = basic.stationId;
            name = basic.name;
            lat = basic.lat;
            lon = basic.lon;
            address = basic.address;
        }

        public GeneralStation(RouteStation basic)
        {
            no = basic.no;
            stationId = basic.stationId;
            name = basic.name;
            lat = basic.lat;
            lon = basic.lon;
            address = basic.address;
        }

        public ObservableCollection<ThroughStationBus> throughStationBusCollection { get; set; }
    }

    public class GeneralStationList
    {
        public ObservableCollection<GeneralStation> generalStationCollection { get; set; } 
    }
}