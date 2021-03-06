/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:BrandedApp"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using Windows.Devices.Bluetooth.Advertisement;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using VnBusInfoW10.ViewModel.MapGroup;
using VnBusInfoW10.ViewModel.SettingGroup;
using VnBusInfoW10.ViewModel.StartGroup;

namespace VnBusInfoW10.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<ExtendedSplashViewModel>();
            SimpleIoc.Default.Register<StartViewModel>();
            SimpleIoc.Default.Register<SettingViewModel>();
            SimpleIoc.Default.Register<UpdateDatabaseViewModel>();
            SimpleIoc.Default.Register<RouteSearchViewModel>();
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();

        public ExtendedSplashViewModel ExtendedSplashVm => 
            ServiceLocator.Current.GetInstance<ExtendedSplashViewModel>();

        public StartViewModel StartVm => ServiceLocator.Current.GetInstance<StartViewModel>();
        public SettingViewModel SettingVm => ServiceLocator.Current.GetInstance<SettingViewModel>();
        public UpdateDatabaseViewModel UpdateInfoVm => ServiceLocator.Current.GetInstance<UpdateDatabaseViewModel>();
        public MapViewModel MapVm => StaticData.MapVm;
        public RouteSearchViewModel RouteSearchVm => ServiceLocator.Current.GetInstance<RouteSearchViewModel>();

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }

    public enum MessengerToken
    {
        StationDirectionChanged,
        BusIndexChanged,
        StationIndexChanged
    }
}