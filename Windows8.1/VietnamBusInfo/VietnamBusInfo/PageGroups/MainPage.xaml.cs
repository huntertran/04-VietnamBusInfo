using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
using Bing.Maps;
using VietnamBusInfo.CustomControl;
using VietnamBusInfo.Model;
using VietnamBusInfo.Utilities;
using VietnamBusInfo.ViewModel;

namespace VietnamBusInfo.PageGroups
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private Geolocator _geolocator = null;
        private CancellationTokenSource _cts = null;
        private LocationIcon10m _locationIcon10m;
        private LocationIcon100m _locationIcon100m;

        public MainPage()
        {
            this.InitializeComponent();
            LoadMapStyle();
            Loaded += OnLoaded;
        }

        private async void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            await Relocation();
        }

        private async Task Relocation()
        {

            LocationGrid.Visibility = Visibility.Visible;

            _geolocator = new Geolocator();
            _locationIcon10m = new LocationIcon10m();
            _locationIcon100m = new LocationIcon100m();

            // Remove any previous location icon.
            if (map.Children.Count > 0)
            {
                map.Children.RemoveAt(0);
            }

            try
            {
                // Get the cancellation token.
                _cts = new CancellationTokenSource();
                CancellationToken token = _cts.Token;

                //MessageTextbox.Text = "Waiting for update...";

                // Get the location.
                Geoposition pos = await _geolocator.GetGeopositionAsync().AsTask(token);

                //MessageTextbox.Text = "";

                Location location = new Location(pos.Coordinate.Latitude, pos.Coordinate.Longitude);

                // Now set the zoom level of the map based on the accuracy of our location data.
                // Default to IP level accuracy. We only show the region at this level - No icon is displayed.
                double zoomLevel = 13.0f;

                // if we have GPS level accuracy
                if (pos.Coordinate.Accuracy <= 10)
                {
                    // Add the 10m icon and zoom closer.
                    map.Children.Add(_locationIcon10m);
                    MapLayer.SetPosition(_locationIcon10m, location);
                    zoomLevel = 15.0f;
                }
                // Else if we have Wi-Fi level accuracy.
                else if (pos.Coordinate.Accuracy <= 100)
                {
                    // Add the 100m icon and zoom a little closer.
                    map.Children.Add(_locationIcon100m);
                    MapLayer.SetPosition(_locationIcon100m, location);
                    zoomLevel = 14.0f;
                }

                // Set the map to the given location and zoom level.
                map.SetView(location, zoomLevel);

                // Display the location information in the textboxes.
                //LatitudeTextbox.Text = pos.Coordinate.Latitude.ToString();
                //LongitudeTextbox.Text = pos.Coordinate.Longitude.ToString();
                //AccuracyTextbox.Text = pos.Coordinate.Accuracy.ToString();
            }
            catch (System.UnauthorizedAccessException)
            {
                //MessageTextbox.Text = "Location disabled.";

                //LatitudeTextbox.Text = "No data";
                //LongitudeTextbox.Text = "No data";
                //AccuracyTextbox.Text = "No data";
            }
            catch (TaskCanceledException)
            {
                //MessageTextbox.Text = "Operation canceled.";
            }
            finally
            {
                _cts = null;
            }

            // Reset the buttons.
            //MapLocationButton.IsEnabled = true;
            //CancelGetLocationButton.IsEnabled = false;

            LocationGrid.Visibility = Visibility.Collapsed;
        }

        private void LoadMapStyle()
        {
            if (StaticMethod.GetSettings("mapIndex") == "not set")
            {
                StaticMethod.SetSettings("mapIndex", "0");
            }
            if (StaticMethod.GetSettings("mapStyleKey") == "not set")
            {
                StaticMethod.SetSettings("mapStyleKey", "m");
            }

            int selectedIndex = Convert.ToInt32(StaticMethod.GetSettings("mapIndex"));

            switch (selectedIndex)
            {
                case 0:
                    //Bing Map
                    EnableMapStyleComboBox(false);
                    map.TileLayers.Clear();
                    map.MapType = MapType.Road;
                    break;
                case 1:
                    //Google Map
                    EnableMapStyleComboBox(true);
                    map.MapType = MapType.Empty;
                    MapTileLayer googleTileLayer = new MapTileLayer();
                    googleTileLayer.GetTileUri += GoogleTileLayerOnGetTileUri;
                    map.TileLayers.Add(googleTileLayer);
                    //AddGoogleMapStyle();
                    break;
                case 2:
                    //Vietbando Map
                    EnableMapStyleComboBox(false);
                    map.MapType = MapType.Empty;
                    MapTileLayer vietBanDoMapTileLayer = new MapTileLayer();
                    vietBanDoMapTileLayer.GetTileUri += VietBanDoMapTileLayerOnGetTileUri;
                    map.TileLayers.Add(vietBanDoMapTileLayer);
                    break;
                default:
                    break;
            }
        }

        private void MapSourcesMenuFlyout_OnTap(object sender, TappedRoutedEventArgs e)
        {
            MenuFlyoutItem selectedItem = sender as MenuFlyoutItem;

            if (StaticMethod.GetSettings("mapIndex") == "not set")
            {
                StaticMethod.SetSettings("mapIndex", "0");
            }
            if (StaticMethod.GetSettings("mapStyleKey") == "not set")
            {
                StaticMethod.SetSettings("mapStyleKey", "m");
            }

            int selectedIndex = Convert.ToInt32(selectedItem.Tag);
            StaticMethod.SetSettings("mapIndex", selectedIndex.ToString());

            switch (selectedIndex)
            {
                case 0:
                    //Bing Map
                    EnableMapStyleComboBox(false);
                    map.TileLayers.Clear();
                    map.MapType = MapType.Road;
                    break;
                case 1:
                    //Google Map
                    EnableMapStyleComboBox(true);
                    map.MapType = MapType.Empty;
                    MapTileLayer googleTileLayer = new MapTileLayer();
                    googleTileLayer.GetTileUri += GoogleTileLayerOnGetTileUri;
                    map.TileLayers.Add(googleTileLayer);
                    //AddGoogleMapStyle();
                    break;
                case 2:
                    //Vietbando Map
                    EnableMapStyleComboBox(false);
                    map.MapType = MapType.Empty;
                    MapTileLayer vietBanDoMapTileLayer = new MapTileLayer();
                    vietBanDoMapTileLayer.GetTileUri += VietBanDoMapTileLayerOnGetTileUri;
                    map.TileLayers.Add(vietBanDoMapTileLayer);
                    break;
                default:
                    break;
            }
        }

        private void MapStyleMenuFlyoutItem_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            MenuFlyoutItem selectedItem = sender as MenuFlyoutItem;
            int selectedIndex = Convert.ToInt32(selectedItem.Tag);

            string key = "";

            switch (selectedIndex)
            {
                case 1:
                    //Drawing Map style
                    key = "m";
                    break;
                case 2:
                    //Hybrid Map style
                    key = "y";
                    break;
                default:
                    key = "m";
                    break;
            }

            StaticMethod.SetSettings("mapStyleKey", key);
            LoadMapStyle();
        }

        private void GoogleTileLayerOnGetTileUri(object sender, GetTileUriEventArgs getTileUriEventArgs)
        {
            getTileUriEventArgs.Uri =
                new Uri(string.Format(@"http://mts{0}.google.com/vt/lyrs={1}&z={2}&x={3}&y={4}", 1,
                    StaticMethod.GetSettings("mapStyleKey"), getTileUriEventArgs.LevelOfDetail, getTileUriEventArgs.X,
                    getTileUriEventArgs.Y));
        }

        private void VietBanDoMapTileLayerOnGetTileUri(object sender, GetTileUriEventArgs getTileUriEventArgs)
        {
            getTileUriEventArgs.Uri =
                new Uri(
                    string.Format(
                        @"http://www.vietbando.com/Maps/Handler/ImageLoader.ashx?TYPE=BACKGROUD&Level={0}&X={1}&Y={2}",
                        getTileUriEventArgs.LevelOfDetail, getTileUriEventArgs.X, getTileUriEventArgs.Y));
        }

        private void EnableMapStyleComboBox(bool value)
        {
            MapStylesAppBarButton.IsEnabled = value;
        }

        private void BottomGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            BottomAppBar.IsOpen = true;
        }

        private void BottomCommandBar_Opened(object sender, object e)
        {
            BottomGrid.Visibility = Visibility.Collapsed;
        }

        private void BottomCommandBar_Closed(object sender, object e)
        {
            BottomGrid.Visibility = Visibility.Visible;
        }

        private async void RelocationAppBarButton_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            await Relocation();
        }

        private void StopLocateButton_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts = null;
            }

            LocationGrid.Visibility = Visibility.Collapsed;

            //MapLocationButton.IsEnabled = true;
            //CancelGetLocationButton.IsEnabled = false;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts = null;
            }

            base.OnNavigatingFrom(e);
        }
    }
}
