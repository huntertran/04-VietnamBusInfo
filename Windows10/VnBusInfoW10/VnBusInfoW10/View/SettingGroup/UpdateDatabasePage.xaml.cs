using Windows.UI.Xaml.Input;
using VnBusInfoW10.ViewModel.SettingGroup;

namespace VnBusInfoW10.View.SettingGroup
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UpdateDatabasePage
    {
        private readonly UpdateDatabaseViewModel _vm;
        public UpdateDatabasePage()
        {
            InitializeComponent();
            _vm = DataContext as UpdateDatabaseViewModel;
        }

        private async void UpdateButton_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            await _vm.GetData();
        }
    }
}