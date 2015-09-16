using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace VnBusInfoW10.View.MapGroup
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapPage : Page
    {
        public MapPage()
        {
            this.InitializeComponent();
            Loaded += MapPage_Loaded;
        }

        private void MapPage_Loaded(object sender, RoutedEventArgs e)
        {
            SecondFrame.Navigate(typeof (InfoPage));
        }
    }
}
