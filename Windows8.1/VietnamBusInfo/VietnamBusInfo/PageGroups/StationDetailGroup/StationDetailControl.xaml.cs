using Bing.Maps;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using VietnamBusInfo.Model;
using VietnamBusInfo.ViewModel;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace VietnamBusInfo.PageGroups.StationDetailGroup
{
    public sealed partial class StationDetailControl : UserControl
    {
        private StationTotal data;
        private ObservableCollection<LocationPointWithId> goBusRouteCollection = new ObservableCollection<LocationPointWithId>();
        private ObservableCollection<LocationPointWithId> backBusRouteCollection = new ObservableCollection<LocationPointWithId>();

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
            RouteAndDetailSwitch.IsOn = false;
            goBusRouteCollection = new ObservableCollection<LocationPointWithId>();
            backBusRouteCollection = new ObservableCollection<LocationPointWithId>();
            Bus selectedBus = ((Grid) sender).Tag as Bus;
            if (selectedBus.busDirection.Count > 1)
            {
                var detail =
                    from x in StaticData._busContent
                    where Convert.ToInt32(x.id) == Convert.ToInt32(selectedBus.busNumber)
                    select x.go;

                GoDetailTextBlock.Text = detail.First();

                detail =
                    from x in StaticData._busContent
                    where Convert.ToInt32(x.id) == Convert.ToInt32(selectedBus.busNumber)
                    select x.back;

                BackDetailTextBlock.Text = detail.First();

                var temp =
                    from x in StaticData._busRoute
                    where Convert.ToInt32(x.id) == Convert.ToInt32(selectedBus.busNumber)
                    select x.goDirection;

                goBusRouteCollection = temp.ToList()[0];

               temp =
                    from x in StaticData._busRoute
                    where Convert.ToInt32(x.id) == Convert.ToInt32(selectedBus.busNumber)
                    select x.backDirection;
                backBusRouteCollection = temp.ToList()[0];
            }
            else
            {
                if (selectedBus.busDirection[0].direction == "1")
                {
                    var detail =
                        from x in StaticData._busContent
                        where Convert.ToInt32(x.id) == Convert.ToInt32(selectedBus.busNumber)
                        select x.go;

                    GoDetailTextBlock.Text = detail.First();
                    BackDetailTextBlock.Text = "";

                    var temp =
                        from x in StaticData._busRoute
                        where Convert.ToInt32(x.id) == Convert.ToInt32(selectedBus.busNumber)
                        select x.goDirection;

                    goBusRouteCollection = temp.ToList()[0];
                }
                else
                {
                    var detail =
                        from x in StaticData._busContent
                        where Convert.ToInt32(x.id) == Convert.ToInt32(selectedBus.busNumber)
                        select x.back;

                    BackDetailTextBlock.Text = detail.First();
                    GoDetailTextBlock.Text = "";

                    var temp =
                        from x in StaticData._busRoute
                        where Convert.ToInt32(x.id) == Convert.ToInt32(selectedBus.busNumber)
                        select x.backDirection;

                    backBusRouteCollection = temp.ToList()[0];
                }
            }
            DrawBusRoute();
        }

        private void DrawBusRoute()
        {
            if (goBusRouteCollection.Count == 0 && backBusRouteCollection.Count == 0)
            {
                return;
            }

            //Go Direction
            MapPolyline busGoRoutePolyLine = new MapPolyline();
            busGoRoutePolyLine.Locations = new LocationCollection();
            busGoRoutePolyLine.Color = Colors.Red;
            busGoRoutePolyLine.Width = 5;

            var tempLocations = new LocationCollection();
            foreach (LocationPointWithId locationPointWithId in goBusRouteCollection)
            {
                Location location = new Location(locationPointWithId.latitude, locationPointWithId.longitude);
                tempLocations.Add(location);
            }

            busGoRoutePolyLine.Locations = tempLocations;

            //Back Direction
            MapPolyline busBackRoutePolyLine = new MapPolyline();
            busBackRoutePolyLine.Locations = new LocationCollection();
            busBackRoutePolyLine.Color = Colors.Blue;
            busBackRoutePolyLine.Width = 5;

            var tempBackLocations = new LocationCollection();
            foreach (LocationPointWithId locationPointWithId in backBusRouteCollection)
            {
                Location location = new Location(locationPointWithId.latitude, locationPointWithId.longitude);
                tempBackLocations.Add(location);
            }

            busBackRoutePolyLine.Locations = tempBackLocations;

            MapShapeLayer busRouteShapeLayer = new MapShapeLayer();
            busRouteShapeLayer.Shapes.Add(busGoRoutePolyLine);
            busRouteShapeLayer.Shapes.Add(busBackRoutePolyLine);

            mapControl.ShapeLayers.Clear();
            mapControl.ShapeLayers.Add(busRouteShapeLayer);
        }

        private void RouteAndDetailSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            if (RouteAndDetailSwitch.IsOn)
            {
                BusDetailInfoScrollViewer.Visibility = Visibility.Visible;
                BusStopListGrid.Visibility = Visibility.Collapsed;
                return;
            }
        }
    }
}