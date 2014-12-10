using Windows.Devices.Geolocation;
using VietnamBusInfo.Model;

namespace VietnamBusInfo.ViewModel
{
    public class StaticData
    {
        public static BusNameList newBusNameList = new BusNameList();
        public static CodedBusNameList codedBusNameList = new CodedBusNameList();
        public static GeneralStationList generalStationList = new GeneralStationList();

        public static Geopoint CenterGeopoint = new Geopoint(new BasicGeoposition() { Latitude = 10.814654,Longitude = 106.670158 });
    }
}
