using System.Threading.Tasks;
using Windows.UI.Xaml;
using VnBusInfoW10.ViewModel.StartGroup;

namespace VnBusInfoW10.View.StartGroup
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExtendedSplashScreen
    {
        private readonly ExtendedSplashViewModel _vm;
        public ExtendedSplashScreen()
        {
            InitializeComponent();
            _vm = DataContext as ExtendedSplashViewModel;
            Loaded += ExtendedSplashScreen_Loaded;
        }

        private async void ExtendedSplashScreen_Loaded(object sender, RoutedEventArgs e)
        {
            await StartUp();
        }

        private async Task StartUp()
        {
            await _vm.Initialize();
            Frame.Navigate(typeof (StartPage));
        }
    }
}
