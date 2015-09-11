using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using VnBusInfoW10.Model;
using VnBusInfoW10.ViewModel.SettingGroup;

namespace VnBusInfoW10.View.SettingGroup
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingPage : Page
    {
        private SettingViewModel _vm;
        public SettingPage()
        {
            this.InitializeComponent();
            _vm = DataContext as SettingViewModel;
            Loaded += SettingPage_Loaded;

        }

        private void SettingPage_Loaded(object sender, RoutedEventArgs e)
        {
            SettingsListView.SelectedIndex = 1;
        }

        private void SettingsListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SettingsListView.SelectedIndex != -1)
            {
                MenuListItem m = SettingsListView.SelectedItem as MenuListItem;
                Debug.Assert(m != null, "m != null");
                _vm.NavigateToFunction(MainFrame, m.MenuF);
            }
        }
    }
}
