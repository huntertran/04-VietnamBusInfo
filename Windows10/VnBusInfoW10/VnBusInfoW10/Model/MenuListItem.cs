using System.ComponentModel;
using System.Runtime.CompilerServices;
using VnBusInfoW10.Annotations;

namespace VnBusInfoW10.Model
{
    public enum MenuFunc
    {
        Map,
        Search,
        Alarm,
        Settings,
        MapSource,
        UpdateDatabase
    };

    public class MenuListItem : INotifyPropertyChanged
    {
        private string _name;
        private string _icon;
        private MenuFunc _menuF;
        private bool _isEnabled = true;

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

        public string Icon
        {
            get { return _icon; }
            set
            {
                if (value == _icon) return;
                _icon = value;
                OnPropertyChanged();
            }
        }

        public MenuFunc MenuF
        {
            get { return _menuF; }
            set
            {
                if (value == _menuF) return;
                _menuF = value;
                OnPropertyChanged();
            }
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (value.Equals(_isEnabled)) return;
                _isEnabled = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
