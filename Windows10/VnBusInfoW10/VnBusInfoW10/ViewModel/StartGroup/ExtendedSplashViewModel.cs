using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI.Core;
using VnBusInfoW10.Annotations;
using VnBusInfoW10.Model;
using VnBusInfoW10.Utilities;
using VnBusInfoW10.ViewModel.SettingGroup;

namespace VnBusInfoW10.ViewModel.StartGroup
{
    public class ExtendedSplashViewModel : INotifyPropertyChanged
    {

        private string _status;
        private double _progress;

        public string Status
        {
            get { return _status; }
            set
            {
                if (value == _status) return;
                _status = value;
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

        public async Task Initialize()
        {
            Notify("Initializing for the first use...");
            try
            {
                StaticData.MapVm.AllBus = await StorageHelper.Json2Object<ObservableCollection<BusTotal>>("data.dat");
            }
            catch
            {
                // ignored
            }
            if (StaticData.MapVm.AllBus == null)
            {
                //Copy data to local storage
                StorageFile file =
                    await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///Data/data.dat"));
                StorageFolder dataFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync("data");
                await file.CopyAsync(dataFolder);
                StaticData.MapVm.AllBus = await StorageHelper.Json2Object<ObservableCollection<BusTotal>>("data.dat");
                UpdateProgress(50);
            }

            try
            {
                StaticData.MapVm.AllStation =
                    await StorageHelper.Json2Object<ObservableCollection<StationTotal>>("allstation.dat");
            }
            catch
            {
                // ignored
            }
            if (StaticData.MapVm.AllStation == null)
            {
                Notify("Getting all bus station ready...");
                UpdateDatabaseViewModel vm = new UpdateDatabaseViewModel();
                await vm.CalculateStationTotal();
            }

            Notify("Completed");
        }

        private async void Notify(string s)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal,
                () =>
                {
                    Status = s;
                });
        }

        private async void UpdateProgress(double d)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal,
                () =>
                {
                    Progress = d;
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
