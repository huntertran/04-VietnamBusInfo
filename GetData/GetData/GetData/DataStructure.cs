using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using GetData.Model;

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

namespace GetData.OldModel
{
    public class PushPinInfo
    {
        public string name { get; set; }
        public StationTotal stations { get; set; }
    }

    public class Bus
    {
        public string busNumber { get; set; }
        public ObservableCollection<Direction> busDirection { get; set; }
    }

    public class BusSimple
    {
        public string busNumber { get; set; }
        public string busDirection { get; set; }
    }

    public class Direction
    {
        public string direction { get; set; }
    }

    public class StationTotal
    {
        public string id { get; set; }
        public string stationId { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }
        public string addressNum { get; set; }
        public string addressStreet { get; set; }
        public string addressDistrict { get; set; }
        public ObservableCollection<Bus> busList { get; set; }
    }

    public class LocationPointWithId
    {
        public string id { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }
    }

    public class BusContent : IComparable
    {
        public string id { get; set; }
        public string name { get; set; }
        public string go { get; set; }
        public string back { get; set; }
        public string detail { get; set; }
        public int CompareTo(object obj)
        {
            var busContent = obj as BusContent;
            string idNumber = "";
            string compareId = busContent.id;
            idNumber = id.Contains("-") ? id.Split('-')[0] : id;
            compareId = compareId.Contains("-") ? compareId.Split('-')[0] : compareId;

            return Convert.ToInt32(idNumber).CompareTo(Convert.ToInt32(compareId));
        }

        public BusContent()
        {
            
        }

        public BusContent(BusName busName)
        {
            id = busName.number;
            name = busName.name;
            go = busName.busTextData.go;
            back = busName.busTextData.back;
            detail = busName.busTextData.timeInfo;
        }
    }

    public class BusRoute
    {
        public string id { get; set; }
        public ObservableCollection<LocationPointWithId> goDirection { get; set; }
        public ObservableCollection<LocationPointWithId> backDirection { get; set; }
    }

    public class BusStationCollection
    {
        public string id { get; set; }
        public ObservableCollection<BusStation> goDirection { get; set; }
        public ObservableCollection<BusStation> backDirection { get; set; }
    }

    public class BusStation
    {
        public string id { get; set; }
        public string number { get; set; }
        public string address { get; set; }
        public string district { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public string stationId { get; set; }
    }

    public class OneRoute
    {
        public string busNum { get; set; }
        public string direction { get; set; }
        public BusStation startStation { get; set; }
        public BusStation endStation { get; set; }
        ObservableCollection<LocationPointWithId> route { get; set; }
    }

    public class TwoRoute
    {
        public OneRoute firstRoute { get; set; }
        public OneRoute secondRoute { get; set; }
    }

    public class StepData
    {
        public Bus bus { get; set; }
        public StationTotal station { get; set; }
    }

    public class StepResult
    {
        public StepData start { get; set; }
        public StepData end { get; set; }
    }

    public class BusDataTemplate
    {
        public string stationId { get; set; }
        public string startStationId { get; set; }
        public string endStationId { get; set; }
        public string busId { get; set; }
    }

    public class BusDataTemplate2
    {
        public string stationId { get; set; }
        public string startStationId { get; set; }
        public string endStationId { get; set; }
        public string busId { get; set; }
        public ObservableCollection<BusDataTemplate> route2 { get; set; }
    }

    public class StepStationSimple
    {
        public double distance { get; set; }
        public string id { get; set; }
    }

    public class ResultLocation
    {
        public string name { get; set; }
        public string address { get; set; }
        public string longitude { get; set; }
        public string latitude { get; set; }
    }
}
