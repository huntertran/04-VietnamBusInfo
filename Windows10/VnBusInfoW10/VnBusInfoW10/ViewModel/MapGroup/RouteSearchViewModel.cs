using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Devices.Geolocation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using VnBusInfoW10.Annotations;
using VnBusInfoW10.Model.GoogleMapApi;
using VnBusInfoW10.Utilities;

namespace VnBusInfoW10.ViewModel.MapGroup
{
    public class RouteSearchViewModel : INotifyPropertyChanged
    {
        private uint _desireAccuracyInMetersValue = 0;
        private CancellationTokenSource _cts;

        private GeoCodingResults _startLocationResults;
        private GeoCodingResults _endLocationResults;
        private BasicGeoposition _startPoint;
        private BasicGeoposition _endPoint;

        private string _statusText;
        private bool _isIndetemine;
        private double _progress;

        private Visibility _progressVisibility;

        public GeoCodingResults StartLocationResults
        {
            get { return _startLocationResults; }
            set
            {
                if (Equals(value, _startLocationResults)) return;
                _startLocationResults = value;
                OnPropertyChanged();
            }
        }

        public GeoCodingResults EndLocationResults
        {
            get { return _endLocationResults; }
            set
            {
                if (Equals(value, _endLocationResults)) return;
                _endLocationResults = value;
                OnPropertyChanged();
            }
        }

        public BasicGeoposition StartPoint
        {
            get { return _startPoint; }
            set
            {
                if (value.Equals(_startPoint)) return;
                _startPoint = value;
                OnPropertyChanged();
            }
        }

        public BasicGeoposition EndPoint
        {
            get { return _endPoint; }
            set
            {
                if (value.Equals(_endPoint)) return;
                _endPoint = value;
                OnPropertyChanged();
            }
        }

        public Visibility ProgressVisibility
        {
            get { return _progressVisibility; }
            set
            {
                if (value == _progressVisibility) return;
                _progressVisibility = value;
                OnPropertyChanged();
            }
        }

        public string StatusText
        {
            get { return _statusText; }
            set
            {
                if (value == _statusText) return;
                _statusText = value;
                OnPropertyChanged();
            }
        }

        public bool IsIndetemine
        {
            get { return _isIndetemine; }
            set
            {
                if (value == _isIndetemine) return;
                _isIndetemine = value;
                OnPropertyChanged();
            }
        }

        public double Progress
        {
            get { return _progress; }
            set
            {
                if (value.Equals(_progress)) return;
                _progress = value;
                OnPropertyChanged();
            }
        }

        public RouteSearchViewModel()
        {
            ProgressVisibility = Visibility.Collapsed;
        }

        private async Task<GeoCodingResults> SearchLocation(string address)
        {
            return await GoogleMapApi.GetGeoPosition(address);
        }

        public async Task SearchStartLocation(string address)
        {
            StartLocationResults = await SearchLocation(address);
        }

        public async Task SearchEndLocation(string address)
        {
            EndLocationResults = await SearchLocation(address);
        }

        public async Task GetLocation(bool isStartPoint)
        {
            try
            {
                // Request permission to access location
                var accessStatus = await Geolocator.RequestAccessAsync();

                switch (accessStatus)
                {
                    case GeolocationAccessStatus.Allowed:

                        // Get cancellation token
                        _cts = new CancellationTokenSource();
                        CancellationToken token = _cts.Token;

                        UpdateStatus("Waiting for update...", true);

                        // If DesiredAccuracy or DesiredAccuracyInMeters are not set (or value is 0), DesiredAccuracy.Default is used.
                        Geolocator geolocator = new Geolocator { DesiredAccuracyInMeters = _desireAccuracyInMetersValue };

                        // Carry out the operation
                        Geoposition pos = await geolocator.GetGeopositionAsync().AsTask(token);

                        if (isStartPoint)
                        {
                            StartPoint = pos.Coordinate.Point.Position;
                        }
                        else
                        {
                            EndPoint = pos.Coordinate.Point.Position;
                        }

                        UpdateStatus("Location updated");
                        break;

                    case GeolocationAccessStatus.Denied:
                        UpdateStatus("Access to location is denied.");
                        break;

                    case GeolocationAccessStatus.Unspecified:
                        UpdateStatus("Unspecified error.");
                        break;
                }
            }
            catch (TaskCanceledException)
            {
                UpdateStatus("Canceled.");
            }
            catch (Exception ex)
            {
                UpdateStatus(ex.ToString());
            }
            finally
            {
                _cts = null;
            }
        }

        private async void UpdateStatus(string s, bool isShowProgress = false, bool isIndetemine = true,
            double progress = 0)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal,
                () =>
                {
                    StatusText = s;
                    if (isShowProgress)
                    {
                        _progressVisibility = Visibility.Visible;
                        IsIndetemine = isIndetemine;
                        Progress = progress;
                    }
                    else
                    {
                        _progressVisibility = Visibility.Collapsed;
                    }
                });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
