using System.Collections.ObjectModel;
using VietnamBusInfo.Model;

namespace VietnamBusInfo.ViewModel
{
    public static class StaticData
    {
        public static ObservableCollection<StationTotal> _stationTotal = new ObservableCollection<StationTotal>();

        /// <summary>
        /// Station Details. More exactly: Bus Details
        /// </summary>
        public static ObservableCollection<BusContent> _busContent;
        public static ObservableCollection<BusRoute> _busRoute;

        //All station in both direction of a bus line. ObservableCollection means many bus line included
        public static ObservableCollection<BusStationCollection> _busStationCollection;

        /// <summary>
        /// This hold StationTotal around the location
        /// </summary>
        public static ObservableCollection<StationTotal> _stationCollection = new ObservableCollection<StationTotal>();

        /// <summary>
        /// This hold the selected pushpin
        /// </summary>
        public static StationTotal SelectedStationTotal;

        public static BusStationCollection tempBusStationCollection;

        public static string busId = "";
        public static string startStationId = "";
        public static string endStationId = "";

        public static double defaultDistance = 0.5;
    }
}
