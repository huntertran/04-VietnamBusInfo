using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Controls;
using VnBusInfoW10.Annotations;
using VnBusInfoW10.Model;
using VnBusInfoW10.View.MapGroup;
using VnBusInfoW10.View.SettingGroup;

namespace VnBusInfoW10.ViewModel.SettingGroup
{
    public class SettingViewModel : INotifyPropertyChanged
    {

        private ObservableCollection<MenuListItem> _functionList;

        public ObservableCollection<MenuListItem> FunctionList
        {
            get { return _functionList; }
            set
            {
                if (Equals(value, _functionList)) return;
                _functionList = value;
                OnPropertyChanged();
            }
        }

        public SettingViewModel()
        {
            Initialize();
        }

        public void Initialize()
        {
            FunctionList = new ObservableCollection<MenuListItem>();

            MenuListItem m = new MenuListItem
            {
                Name = "Map Source",
                MenuF = MenuFunc.MapSource,
                Icon =
                    "M40.764999,0.046999931L64,10.093993 64,15.637031 40.764999,25.752998 40.764999,19.216354 56.9785,12.841311 40.764999,6.5366688z M23.212,0L23.212,6.5818844 7.0216017,12.772051 23.212,19.192526 23.212,25.729 0,15.636739 0,10.046861z"
            };
            FunctionList.Add(m);

            m = new MenuListItem
            {
                Name = "Update database",
                MenuF = MenuFunc.UpdateDatabase,
                Icon =
                    "M0,33.893959L4.4794149,33.893959 4.4794149,44.677959 48.903614,44.677959 48.903614,33.893959 53.333035,33.893959 53.333035,49.104958 0,49.104958z M24.842734,0L28.513577,0 28.513577,24.615005 35.345016,17.78297 40.346104,17.78297 40.436089,17.883007 26.673088,31.644001 24.072548,29.047991 12.910089,17.883007 13.010208,17.78297 18.001283,17.78297 24.842734,24.615005z"
            };
            FunctionList.Add(m);
        }

        public void NavigateToFunction(Frame frame, MenuFunc func)
        {
            switch (func)
            {
                case MenuFunc.MapSource:
                    {
                        //frame.Navigate(typeof(MapPage));
                        break;
                    }
                case MenuFunc.UpdateDatabase:
                    {
                        frame.Navigate(typeof(UpdateDatabasePage));
                        break;
                    }
                default:
                    {
                        frame.Navigate(typeof(UpdateDatabasePage));
                        break;
                    }
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
