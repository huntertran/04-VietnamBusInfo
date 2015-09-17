using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Devices.Geolocation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using VnBusInfoW10.Model.GoogleMapApi;
using VnBusInfoW10.ViewModel.MapGroup;

namespace VnBusInfoW10.View.MapGroup
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RouteSearchPage
    {
        private readonly RouteSearchViewModel _vm;
        private readonly DispatcherTimer _timer = new DispatcherTimer();
        private bool _isStartLocation;

        public RouteSearchPage()
        {
            InitializeComponent();
            _vm = DataContext as RouteSearchViewModel;
            Loaded += RouteSearchPage_Loaded;
        }

        private void RouteSearchPage_Loaded(object sender, RoutedEventArgs e)
        {
            _timer.Interval = TimeSpan.FromMilliseconds(1500);
            _timer.Tick += _timer_Tick;
        }

        private async void _timer_Tick(object sender, object e)
        {
            if (_isStartLocation)
            {
                await _vm.SearchStartLocation(StartLocationAutoSuggestBox.Text);
                _timer.Stop();
            }
            else
            {
                await _vm.SearchEndLocation(EndlocationAutoSuggestBox.Text);
                _timer.Stop();
            }
        }

        private void StartLocationAutoSuggestBox_OnTextChanged(AutoSuggestBox sender,
            AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                _timer.Stop();
                _isStartLocation = true;
                _timer.Start();
            }
        }

        private void StartLocationAutoSuggestBox_SuggestionChosen(AutoSuggestBox sender,
            AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            Result r = args.SelectedItem as Result;
            Debug.Assert(r != null, "r != null");
            _vm.StartPoint = new BasicGeoposition
            {
                Latitude = r.geometry.location.lat,
                Longitude = r.geometry.location.lng
            };
        }

        private async void StartLocationAutoSuggestBox_QuerySubmitted(AutoSuggestBox sender,
            AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            await _vm.GetLocation(true);
        }
        private async void EndLocationAutoSuggestBox_QuerySubmitted(AutoSuggestBox sender,
            AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            await _vm.GetLocation(false);
        }

        private void EndLocationAutoSuggestBox_OnTextChanged(AutoSuggestBox sender,
            AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                _timer.Stop();
                _isStartLocation = false;
                _timer.Start();
            }
        }

        private void EndLocationAutoSuggestBox_SuggestionChosen(AutoSuggestBox sender,
            AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            Result r = args.SelectedItem as Result;
            Debug.Assert(r != null, "r != null");
            _vm.EndPoint = new BasicGeoposition
            {
                Latitude = r.geometry.location.lat,
                Longitude = r.geometry.location.lng
            };
        }

        
    }
}