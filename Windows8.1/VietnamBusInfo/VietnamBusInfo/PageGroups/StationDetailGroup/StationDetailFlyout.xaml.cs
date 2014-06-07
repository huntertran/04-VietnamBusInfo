// The Settings Flyout item template is documented at http://go.microsoft.com/fwlink/?LinkId=273769

using System.Linq;
using Windows.ApplicationModel.Activation;
using VietnamBusInfo.Model;
using VietnamBusInfo.ViewModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace VietnamBusInfo.PageGroups.StationDetailGroup
{
    public sealed partial class StationDetailFlyout : SettingsFlyout
    {
        private StationTotal data;

        public StationDetailFlyout()
        {
            this.InitializeComponent();

            data = StaticData.SelectedStationTotal;

            this.Loaded += StationDetailFlyout_Loaded;
        }

        void StationDetailFlyout_Loaded(object sender, RoutedEventArgs e)
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
                }
                else
                {
                    var detail =
                    from x in StaticData._busContent
                    where x.id == selectedBus.busNumber
                    select x.back;

                    BackDetailTextBlock.Text = detail.First();
                }
            }
        }
    }
}
