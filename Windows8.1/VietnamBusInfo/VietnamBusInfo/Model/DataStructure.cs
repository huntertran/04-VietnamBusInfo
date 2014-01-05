using System.Collections.ObjectModel;

namespace VietnamBusInfo.Model
{

    #region DataStructure

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

    //public class LocationPoint
    //{
    //    public double longitude { get; set; }
    //    public double latitude { get; set; }
    //}

    public class LocationPointWithId
    {
        public string id { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }
    }

    public class BusContent
    {
        public string id { get; set; }
        public string name { get; set; }
        public string go { get; set; }
        public string back { get; set; }
        public string detail { get; set; }
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
        private ObservableCollection<LocationPointWithId> route { get; set; }
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

    public class MapSource
    {
        public int id { get; set; }
        public string name { get; set; }
        public string link { get; set; }
    }

    public class MapStyle
    {
        public int id { get; set; }
        public string name { get; set; }
        public string key { get; set; }
    }

    #endregion
}
