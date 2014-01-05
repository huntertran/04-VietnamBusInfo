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
using VietnamBusInfo.Model;
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
        }

        private void MapSourcesMenuFlyout_OnTap(object sender, TappedRoutedEventArgs e)
        {
            MenuFlyoutItem selectedItem = sender as MenuFlyoutItem;
            int selectedIndex = Convert.ToInt32(selectedItem.Tag);

            switch (selectedIndex)
            {
                case 0:
                    //Bing Map
                    EnableMapStyleComboBox(false);
                    break;
                case 1:
                    //Google Map
                    EnableMapStyleComboBox(true);
                    AddGoogleMapStyle();
                    break;
                case 2:
                    //Vietbando Map
                    EnableMapStyleComboBox(false);
                    break;
                default:
                    break;
            }
        }

        private void AddGoogleMapStyle()
        {
            ObservableCollection<MapStyle> mapStyleCollection = new ObservableCollection<MapStyle>();

            MapStyle drawingMap = new MapStyle()
            {
                id = 1,
                name = "Map",
                key = "m",
            };
            mapStyleCollection.Add(drawingMap);

            MapStyle hybridMap = new MapStyle()
            {
                id = 2,
                name = "Hybrid",
                key = "y",
            };
            mapStyleCollection.Add(hybridMap);

            //mapStyleComboBox.ItemsSource = mapStyleCollection;
            //mapStyleComboBox.SelectedIndex = 0;
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
