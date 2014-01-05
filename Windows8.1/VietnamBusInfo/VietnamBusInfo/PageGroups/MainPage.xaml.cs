using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
        public MainPage()
        {
            this.InitializeComponent();
            LoadMapStyle();
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

    }
}
