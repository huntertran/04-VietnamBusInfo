using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Devices.Geolocation;
using VnBusInfoW10.Annotations;

namespace VnBusInfoW10.Model
{
    public class BusStation : INotifyPropertyChanged
    {
        private int _stationId;
        private int _nextStationId;
        private string _miniRoute;
        private string _u1;
        private int _no;
        private string _u2;
        private string _name;
        private string _lon;
        private string _lat;
        private string _address;
        private string _codeName;

        public int StationId
        {
            get { return _stationId; }
            set
            {
                if (value == _stationId) return;
                _stationId = value;
                OnPropertyChanged();
            }
        }

        public int NextStationId
        {
            get { return _nextStationId; }
            set
            {
                if (value == _nextStationId) return;
                _nextStationId = value;
                OnPropertyChanged();
            }
        }

        public string MiniRoute
        {
            get { return _miniRoute; }
            set
            {
                if (value == _miniRoute) return;
                _miniRoute = value;
                OnPropertyChanged();
            }
        }

        public string U1
        {
            get { return _u1; }
            set
            {
                if (value == _u1) return;
                _u1 = value;
                OnPropertyChanged();
            }
        }

        public int No
        {
            get { return _no; }
            set
            {
                if (value == _no) return;
                _no = value;
                OnPropertyChanged();
            }
        }

        public string U2
        {
            get { return _u2; }
            set
            {
                if (value == _u2) return;
                _u2 = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        public string Lon
        {
            get { return _lon; }
            set
            {
                if (value == _lon) return;
                _lon = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Location));
            }
        }

        public string Lat
        {
            get { return _lat; }
            set
            {
                if (value == _lat) return;
                _lat = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Location));
            }
        }

        public string Address
        {
            get { return _address; }
            set
            {
                if (value == _address) return;
                _address = value;
                OnPropertyChanged();
            }
        }

        public string CodeName
        {
            get { return _codeName; }
            set
            {
                if (value == _codeName) return;
                _codeName = value;
                OnPropertyChanged();
            }
        }

        public Geopoint Location
        {
            get
            {
                BasicGeoposition b = new BasicGeoposition
                {
                    Latitude = Convert.ToDouble(Lat),
                    Longitude = Convert.ToDouble(Lon)
                };

                Geopoint g = new Geopoint(b);
                return g;
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
