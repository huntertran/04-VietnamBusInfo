using Windows.Devices.Geolocation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Media;
using GalaSoft.MvvmLight.Messaging;
using VnBusInfoW10.ViewModel;
using VnBusInfoW10.ViewModel.MapGroup;

namespace VnBusInfoW10.View.MapGroup
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapPage
    {
        private int _busIndex;
        private bool _direction;

        private MapViewModel _vm;

        public MapPage()
        {
            this.InitializeComponent();
            _vm = DataContext as MapViewModel;
            Messenger.Default.Register<int>(this, MessengerToken.BusIndexChanged, ChangeBusIndex);
            Messenger.Default.Register<bool>(this, MessengerToken.StationDirectionChanged, ChangeDirection);
            Loaded += MapPage_Loaded;
        }

        private void ChangeBusIndex(int newIndex)
        {
            _busIndex = newIndex;
            DisplayOnMap();
        }

        private void ChangeDirection(bool isGo)
        {
            _direction = isGo;
            DisplayOnMap();
        }

        private void DisplayOnMap()
        {
            StationPushPinList.ItemsSource = _direction
                ? _vm.AllBus[_busIndex].GoStationList
                : _vm.AllBus[_busIndex].BackStationList;

            var g = _direction
                ? new Geopath(_vm.AllBus[_busIndex].GoRoute)
                : new Geopath(_vm.AllBus[_busIndex].BackRoute);

            var routeDetailList = new MapPolyline
            {
                Path = g,
                StrokeThickness = 5,
                StrokeColor = Colors.Coral,
                ZIndex = 1
            };

            MainMap.Center = new Geopoint(g.Positions[0]);
            MainMap.ZoomLevel = 12;

            int index = -1;
            for (int i = 0; i < MainMap.MapElements.Count; i++)
            {
                if (MainMap.MapElements[i] is MapPolyline)
                {
                    index = i;
                    break;
                }
            }

            if (index != -1)
            {
                MainMap.MapElements.RemoveAt(index);
            }
            MainMap.MapElements.Add(routeDetailList);
        }

        private void MapPage_Loaded(object sender, RoutedEventArgs e)
        {
            SecondFrame.Navigate(typeof (InfoPage));
        }
    }
}
