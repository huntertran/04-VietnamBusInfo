using Windows.UI.Xaml;
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
        }

        private void ChangeDirection(bool isGo)
        {
            _direction = isGo;
            StationPushPinList.ItemsSource = _direction ? _vm.AllBus[_busIndex].GoStationList : _vm.AllBus[_busIndex].BackStationList;
        }

        private void MapPage_Loaded(object sender, RoutedEventArgs e)
        {
            SecondFrame.Navigate(typeof (InfoPage));
        }
    }
}
