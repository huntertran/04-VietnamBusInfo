using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Services.Maps;
using Windows.UI;
using Windows.UI.Xaml.Controls.Maps;
using VietnamBusInfo.Custom.CustomControl;
using VietnamBusInfo.Model;
using VietnamBusInfo.ViewModel;

namespace VietnamBusInfo.Utilities
{
    public class MapHelper
    {
        public static void DisplayBusStation(int busCode, MapControl mapControl)
        {
            var busStaionList = from bus in StaticData.codedBusNameList.codedBusNameCollection
                where bus.number == busCode
                select bus;

            //MapRoute route = new MapRoute();
            MapPolyline line = new MapPolyline();
            line.StrokeColor = Colors.Green;
            line.StrokeThickness = 5;

            ObservableCollection<BasicGeoposition> linePath = new ObservableCollection<BasicGeoposition>();

            foreach (BusCodedName bus in busStaionList)
            {
                foreach (RouteStation station in bus.directionRouteCollection[0].routeStationCollection)
                {
                    BusStationPushPin mapIcon = new BusStationPushPin(station.no.ToString());

                    BasicGeoposition basic = new BasicGeoposition();
                    basic.Latitude = Convert.ToDouble(station.lon);
                    basic.Longitude = Convert.ToDouble(station.lat);

                    Geopoint geopoint = new Geopoint(basic);

                    //mapIcon.Location = geopoint;

                    foreach (string s in station.route.Split(' '))
                    {
                        if (!String.IsNullOrEmpty(s))
                        {
                            BasicGeoposition pos = new BasicGeoposition();
                            pos.Latitude = Convert.ToDouble(s.Split(',')[1]);
                            pos.Longitude = Convert.ToDouble(s.Split(',')[0]);

                            linePath.Add(pos);
                        }
                    }

                    mapControl.Children.Add(mapIcon);
                    MapControl.SetLocation(mapIcon, geopoint);
                }
            }

            line.Path = new Geopath(linePath.ToArray());

            mapControl.MapElements.Add(line);
        }
    }
}
