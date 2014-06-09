using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236
using Windows.UI;
using Bing.Maps;
using VietnamBusInfo.Model;
using VietnamBusInfo.Utilities;
using VietnamBusInfo.ViewModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace VietnamBusInfo.PageGroups.StationDetailGroup
{
    public sealed partial class StationDetailControl : UserControl
    {
        private StationTotal data;
        private ObservableCollection<LocationPointWithId> busRouteCollection = new ObservableCollection<LocationPointWithId>();
        public Map mapControl;

        public StationDetailControl()
        {
            this.InitializeComponent();
            data = StaticData.SelectedStationTotal;
            this.Loaded += OnLoaded;
        }

        public StationDetailControl(Map parentMap)
        {
            this.InitializeComponent();
            data = StaticData.SelectedStationTotal;
            mapControl = parentMap;
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            AddressTextBlock.Text = data.addressNum + " " + data.addressStreet + ", " + data.addressDistrict;
            BusListView.ItemsSource = StaticData.SelectedStationTotal.busList;
        }

        private void BusItem_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            Bus selectedBus = ((Grid) sender).Tag as Bus;
            if (selectedBus.busDirection.Count > 1)
            {
                var detail =
                    from x in StaticData._busContent
                    where x.id == selectedBus.busNumber
                    select x.go;

                GoDetailTextBlock.Text = detail.First();

                detail =
                    from x in StaticData._busContent
                    where x.id == selectedBus.busNumber
                    select x.back;

                BackDetailTextBlock.Text = detail.First();

                var temp =
                    from x in StaticData._busRoute
                    where x.id == selectedBus.busNumber
                    select x.goDirection;

                busRouteCollection = temp.ToList()[0];
            }
            else
            {
                if (selectedBus.busDirection[0].direction == "1")
                {
                    var detail =
                        from x in StaticData._busContent
                        where x.id == selectedBus.busNumber
                        select x.go;

                    GoDetailTextBlock.Text = detail.First();
                    BackDetailTextBlock.Text = "";

                    var temp =
                        from x in StaticData._busRoute
                        where x.id == selectedBus.busNumber
                        select x.goDirection;

                    busRouteCollection = temp.ToList()[0];
                }
                else
                {
                    var detail =
                        from x in StaticData._busContent
                        where x.id == selectedBus.busNumber
                        select x.back;

                    BackDetailTextBlock.Text = detail.First();
                    GoDetailTextBlock.Text = "";

                    var temp =
                        from x in StaticData._busRoute
                        where x.id == selectedBus.busNumber
                        select x.goDirection;

                    busRouteCollection = temp.ToList()[0];

                    //busRouteCollection =
                    //    new ObservableCollection<LocationPointWithId>(temp as IEnumerable<LocationPointWithId>);
                }

                DrawBusRoute();

            }
        }

        private void DrawBusRoute()
        {
            if (busRouteCollection == null || busRouteCollection.Count == 0)
            {
                return;
            }

            MapPolyline busGoRoutePolyLine = new MapPolyline();
            busGoRoutePolyLine.Locations = new LocationCollection();
            busGoRoutePolyLine.Color = Colors.Red;
            busGoRoutePolyLine.Width = 5;

            var tempLocations = new LocationCollection();
            foreach (LocationPointWithId locationPointWithId in busRouteCollection)
            {
                Location location = new Location(locationPointWithId.latitude, locationPointWithId.longitude);
                tempLocations.Add(location);
            }

            busGoRoutePolyLine.Locations = tempLocations;

            MapShapeLayer busRouteShapeLayer = new MapShapeLayer();
            busRouteShapeLayer.Shapes.Add(busGoRoutePolyLine);

            mapControl.ShapeLayers.Clear();
            mapControl.ShapeLayers.Add(busRouteShapeLayer);
        }
    }
}