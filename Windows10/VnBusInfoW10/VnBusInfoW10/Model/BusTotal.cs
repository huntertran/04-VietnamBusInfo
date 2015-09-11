using System.ComponentModel;
using System.Runtime.CompilerServices;
using VnBusInfoW10.Annotations;

namespace VnBusInfoW10.Model
{
    public class BusTotal : INotifyPropertyChanged
    {

        private int _id;
        private BusTextInfo _textInfo;

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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
