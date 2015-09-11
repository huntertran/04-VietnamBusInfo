using Windows.UI.Xaml;

namespace VnBusInfoW10.View.StartGroup
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExtendedSplashScreen
    {
        public ExtendedSplashScreen()
        {
            InitializeComponent();
            Loaded += ExtendedSplashScreen_Loaded;
        }

        private void ExtendedSplashScreen_Loaded(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof (StartPage));
        }
    }
}
