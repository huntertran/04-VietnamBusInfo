using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using VnBusInfoW10.Annotations;

namespace VnBusInfoW10.Model
{
    public enum Direction
    {
        Go,
        Back,
        Both
    }
    public class BusAtStation : INotifyPropertyChanged
    {
        private int _id;
        private Direction _busDirection;

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

        public Direction BusDirection
        {
            get { return _busDirection; }
            set
            {
                if (value == _busDirection) return;
                _busDirection = value;
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

    public class StationTotal : BusStation
    {
        public StationTotal(BusStation b)
        {
            StationId = b.StationId;
            NextStationId = b.StationId;
            MiniRoute = b.MiniRoute;
            U1 = b.U1;
            No = b.No;
            U2 = b.U2;
            Name = b.Name;
            Lon = b.Lon;
            Lat = b.Lat;
            Address = b.Address;
            CodeName = b.CodeName;
        }
        public StationTotal()
        {
            
        }

        private ObservableCollection<BusAtStation> _busAtStationList;

        public ObservableCollection<BusAtStation> BusAtStationList
        {
            get { return _busAtStationList; }
            set
            {
                if (Equals(value, _busAtStationList)) return;
                _busAtStationList = value;
                OnPropertyChanged();
            }
        }
    }
}
