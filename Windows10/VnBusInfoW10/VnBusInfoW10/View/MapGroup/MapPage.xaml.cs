using Windows.UI.Xaml;
using GalaSoft.MvvmLight.Messaging;
using VnBusInfoW10.ViewModel;

namespace VnBusInfoW10.View.MapGroup
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapPage
    {
        private int _busIndex;
        public MapPage()
        {
            this.InitializeComponent();
            Messenger.Default.Register<int>(this, MessengerToken.BusIndexChanged, ChangeBusIndex);
            Loaded += MapPage_Loaded;
        }

        private void ChangeBusIndex(int newIndex)
        {
            _busIndex = newIndex;
        }

        private void MapPage_Loaded(object sender, RoutedEventArgs e)
        {
            SecondFrame.Navigate(typeof (InfoPage));
        }
    }
}
