using System.Collections.ObjectModel;
using VietnamBusInfo.Model;

namespace VietnamBusInfo.ViewModel
{
    public static class StaticData
    {
        public static ObservableCollection<StationTotal> _stationTotal = new ObservableCollection<StationTotal>();
        public static StationTotal _stationTotalStationInfo;
        public static ObservableCollection<BusContent> _busContent;
        public static ObservableCollection<BusRoute> _busRoute;

        //All station in both direction of a bus line. ObservableCollection means many bus line included
        public static ObservableCollection<BusStationCollection> _busStationCollection;

        public static BusStationCollection tempBusStationCollection;

        public static string busId = "";
        public static string startStationId = "";
        public static string endStationId = "";

        public static double defaultDistance = 0.5;
    }
}
