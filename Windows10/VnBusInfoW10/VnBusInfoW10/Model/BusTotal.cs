using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Devices.Geolocation;
using VnBusInfoW10.Annotations;

namespace VnBusInfoW10.Model
{
    public class BusTotal : INotifyPropertyChanged
    {
        private int _id;
        private int _routeId;
        private BusTextInfo _textInfo;
        private ObservableCollection<BusStation> _goStationList;
        private ObservableCollection<BusStation> _backStationList;
        private ObservableCollection<Geocoordinate> _goRoute;
        private ObservableCollection<Geocoordinate> _backRoute;  

        public int Id
        {
            get { return _id; }
            set
            {
                if (value == _id) return;
                _id = value;
                OnPropertyChanged();
            }
        }

        public int RouteId
        {
            get { return _routeId; }
            set
            {
                if (value == _routeId) return;
                _routeId = value;
                OnPropertyChanged();
            }
        }

        public BusTextInfo TextInfo
        {
            get { return _textInfo; }
            set
            {
                if (Equals(value, _textInfo)) return;
                _textInfo = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<BusStation> GoStationList
        {
            get { return _goStationList; }
            set
            {
                if (Equals(value, _goStationList)) return;
                _goStationList = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<BusStation> BackStationList
        {
            get { return _backStationList; }
            set
            {
                if (Equals(value, _backStationList)) return;
                _backStationList = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Geocoordinate> GoRoute
        {
            get { return _goRoute; }
            set
            {
                if (Equals(value, _goRoute)) return;
                _goRoute = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Geocoordinate> BackRoute
        {
            get { return _backRoute; }
            set
            {
                if (Equals(value, _backRoute)) return;
                _backRoute = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}