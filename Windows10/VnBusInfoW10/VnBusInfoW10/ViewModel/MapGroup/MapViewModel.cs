using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using VnBusInfoW10.Annotations;
using VnBusInfoW10.Model;

namespace VnBusInfoW10.ViewModel.MapGroup
{
    public class MapViewModel : INotifyPropertyChanged
    {

        private ObservableCollection<BusTotal> _allBus;
        private ObservableCollection<StationTotal> _allStation; 

        public ObservableCollection<BusTotal> AllBus
        {
            get { return _allBus; }
            set
            {
                if (Equals(value, _allBus)) return;
                _allBus = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<StationTotal> AllStation
        {
            get { return _allStation; }
            set
            {
                if (Equals(value, _allStation)) return;
                _allStation = value;
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
