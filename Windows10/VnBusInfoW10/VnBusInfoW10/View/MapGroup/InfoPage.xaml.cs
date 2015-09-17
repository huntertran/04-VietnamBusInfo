using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight.Messaging;
using VnBusInfoW10.ViewModel;

namespace VnBusInfoW10.View.MapGroup
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InfoPage
    {
        public InfoPage()
        {
            InitializeComponent();
        }

        private void BusListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Messenger.Default.Send(BusListView.SelectedIndex, MessengerToken.BusIndexChanged);
        }

        private void DirectionSwitch_Toggled(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Messenger.Default.Send(((ToggleSwitch)sender).IsOn, MessengerToken.StationDirectionChanged);
        }

        private void StationListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Messenger.Default.Send(((ListView) sender).SelectedIndex, MessengerToken.StationIndexChanged);
        }

        private void BusFilterBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {

        }
    }
}
