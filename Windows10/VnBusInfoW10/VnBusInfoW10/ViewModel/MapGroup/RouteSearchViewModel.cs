using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using VnBusInfoW10.Annotations;
using VnBusInfoW10.Model.GoogleMapApi;
using VnBusInfoW10.Utilities;

namespace VnBusInfoW10.ViewModel.MapGroup
{
    public class RouteSearchViewModel : INotifyPropertyChanged
    {
        private GeoCodingResults _startLocationResults;
        private GeoCodingResults _endLocationResults;

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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
