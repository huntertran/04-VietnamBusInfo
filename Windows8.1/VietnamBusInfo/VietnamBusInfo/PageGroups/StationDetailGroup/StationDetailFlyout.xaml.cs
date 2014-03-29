using System;
using System.Collections.Generic;
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
// The Settings Flyout item template is documented at http://go.microsoft.com/fwlink/?LinkId=273769
using VietnamBusInfo.Model;
using VietnamBusInfo.ViewModel;

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


    }
}
