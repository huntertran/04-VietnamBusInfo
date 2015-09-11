using System.ComponentModel;
using System.Runtime.CompilerServices;
using VnBusInfoW10.Annotations;

namespace VnBusInfoW10.Model
{
    public class BusTextInfo : INotifyPropertyChanged
    {
        private string _routeNumber;
        private string _routeName;
        private string _goDirection;
        private string _backDirection;
        private string _details;

        public string RouteNumber
        {
            get { return _routeNumber; }
            set
            {
                if (value == _routeNumber) return;
                _routeNumber = value;
                OnPropertyChanged();
            }
        }

        public string RouteName
        {
            get { return _routeName; }
            set
            {
                if (value == _routeName) return;
                _routeName = value;
                OnPropertyChanged();
            }
        }

        public string GoDirection
        {
            get { return _goDirection; }
            set
            {
                if (value == _goDirection) return;
                _goDirection = value;
                OnPropertyChanged();
            }
        }

        public string BackDirection
        {
            get { return _backDirection; }
            set
            {
                if (value == _backDirection) return;
                _backDirection = value;
                OnPropertyChanged();
            }
        }

        public string Details
        {
            get { return _details; }
            set
            {
                if (value == _details) return;
                _details = value;
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
