using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using VnBusInfoW10.ViewModel.MapGroup;

namespace VnBusInfoW10.View.MapGroup
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RouteSearchPage
    {
        private RouteSearchViewModel _vm;
        private DispatcherTimer _timer = new DispatcherTimer();
        private bool _isStartLocation = false;
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
        }

        private void StartLocationAutoSuggestBox_OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                _timer.Stop();
                _isStartLocation = true;
                _timer.Start();
            }
        }
    }
}
