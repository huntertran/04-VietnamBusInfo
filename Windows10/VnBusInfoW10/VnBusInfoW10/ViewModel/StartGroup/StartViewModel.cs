﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Controls;
using VnBusInfoW10.Annotations;
using VnBusInfoW10.Model;
using VnBusInfoW10.View.MapGroup;
using VnBusInfoW10.View.SettingGroup;

namespace VnBusInfoW10.ViewModel.StartGroup
{
    public class StartViewModel : INotifyPropertyChanged
    {

        private ObservableCollection<MenuListItem> _functionList;
        private ObservableCollection<MenuListItem> _bottomFunctionList;
        private string _pageName;

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

        public ObservableCollection<MenuListItem> BottomFunctionList
        {
            get { return _bottomFunctionList; }
            set
            {
                if (Equals(value, _bottomFunctionList)) return;
                _bottomFunctionList = value;
                OnPropertyChanged();
            }
        }

        public string PageName
        {
            get { return _pageName; }
            set
            {
                if (value == _pageName) return;
                _pageName = value;
                OnPropertyChanged();
            }
        }

        public StartViewModel()
        {
            Initialize();
        }

        public void Initialize()
        {
            FunctionList = new ObservableCollection<MenuListItem>();
            BottomFunctionList = new ObservableCollection<MenuListItem>();

            MenuListItem m = new MenuListItem
            {
                Name = "Map",
                MenuF = MenuFunc.Map,
                Icon =
                    "M261.812,82.4125L261.812,406.848 499.238,437.495 499.238,112.012z M199.759,81.3682L47.5926,144.239 47.5926,473.608 199.759,401.726z M698.175,55.752L566.681,107.553 566.681,422.657 698.175,368.836z M745.768,0L745.768,386.544 531.549,474.902 214.219,433.633 214.219,433.446 0,536 0,125.805 214.219,43.5135 531.549,83.3014z"
            };
            FunctionList.Add(m);

            m = new MenuListItem
            {
                Name = "Search",
                MenuF = MenuFunc.Search,
                Icon =
                    "M126.657970428467,122.742496490479L158.354923248291,122.742496490479 158.354923248291,179.566555023193 199.302227020264,179.566555023193 199.302227020264,191.499996185303 84.3891906738281,191.499996185303 84.3891906738281,179.566555023193 126.657970428467,179.566555023193z M73.9703731536865,74.9139022827148L246.564800262451,74.9139022827148 285.305004119873,92.4833106994629 246.564800262451,109.388332366943 73.9703731536865,109.388332366943z M38.7448372840881,27.1810321807861L211.339275360107,27.1810321807861 211.339275360107,61.6555194854736 38.7448372840881,61.6555194854736 0,44.7505570948124z M126.657970428467,0L158.354923248291,0 158.354923248291,17.3319454193115 126.657970428467,17.3319454193115z"
            };
            FunctionList.Add(m);

            m = new MenuListItem
            {
                Name = "Alarm",
                MenuF = MenuFunc.Alarm,
                Icon =
                    "M10.05,5.0760006L11.384528,5.0760006 11.384528,9.4289235 14.873,9.4289235 14.873,10.765 11.384528,10.765 10.716714,10.765 10.05,10.765z M10.73913,2.9416503C6.7547405,2.9416503 3.5139221,6.1813825 3.5139223,10.164588 3.5139221,14.146413 6.7547405,17.386114 10.73913,17.386114 14.720629,17.386114 17.960248,14.146413 17.960248,10.164588 17.960248,6.1813825 14.720629,2.9416503 10.73913,2.9416503z M10.73913,1.0340003C15.780502,1.0340003 19.869,5.1214088 19.869,10.164588 19.869,13.673698 17.884551,16.716793 14.979723,18.246841L16.843076,20.11 13.88215,20.11 12.819678,19.047666C12.149094,19.203972 11.455212,19.294975 10.73913,19.294975 10.050247,19.294975 9.3823044,19.212973 8.7364399,19.067169L7.6948469,20.11 4.7339809,20.11 6.564625,18.278044C3.6233492,16.763697 1.6089999,13.701098 1.6089998,10.164588 1.6089999,5.1214088 5.6961172,1.0340003 10.73913,1.0340003z M17.033309,8.7380409E-05C17.92249,-0.0075750351 19.01958,0.49064106 19.93044,1.4016109 21.319918,2.7910937 21.749642,4.6115907 20.890294,5.4697789 20.749587,5.6116933 20.576377,5.7042331 20.388966,5.7770003 19.417512,3.611488 17.756016,1.8235244 15.675,0.70371947 15.732403,0.61246994 15.785705,0.51870531 15.86381,0.44191551 16.158833,0.14691561 16.567549,0.0041009188 17.033309,8.7380409E-05z M4.2990081,8.7022781E-05C4.7646682,0.0041009188 5.1732659,0.14691561 5.4681668,0.44191515 5.5463498,0.51870531 5.5998151,0.61246994 5.6569998,0.70371914 3.5764955,1.8235244 1.9152038,3.611488 0.94392025,5.7770003 0.75649714,5.7042331 0.58335257,5.6116933 0.44261813,5.4697789 -0.41655827,4.6115907 0.013091445,2.7910937 1.4021895,1.4016109 2.313062,0.49064106 3.4100208,-0.0075750351 4.2990081,8.7022781E-05z"
            };
            FunctionList.Add(m);

            m = new MenuListItem
            {
                Name = "Setting",
                MenuF = MenuFunc.Settings,
                Icon =
                    "M383.482,203.57C284.07,203.57 203.534,284.142 203.534,383.554 203.534,482.93 284.07,563.502 383.482,563.502 482.894,563.502 563.431,482.93 563.431,383.554 563.431,284.142 482.894,203.57 383.482,203.57z M338.073,0L428.927,0 428.927,117.641C469.52,124.544,507.055,140.471,539.377,163.41L622.574,80.1771 686.823,144.462 603.627,227.659C626.565,259.982,642.457,297.481,649.396,338.073L767,338.073 767,428.963 649.432,428.963C642.492,469.52,626.565,507.091,603.627,539.378L686.823,622.574 622.538,686.788 539.377,603.626C507.055,626.601,469.555,642.528,428.927,649.432L428.927,767 338.073,767 338.073,649.432C297.409,642.528,259.909,626.601,227.587,603.626L144.426,686.788 80.2128,622.574 163.374,539.378C140.399,507.091,124.508,469.591,117.569,428.963L0,428.963 0,338.073 117.569,338.073C124.508,297.481,140.435,259.982,163.374,227.659L80.1766,144.462 144.426,80.2133 227.623,163.41C259.909,140.471,297.445,124.544,338.073,117.641z"
            };
            BottomFunctionList.Add(m);
        }

        public void NavigateToFunction(Frame frame, MenuFunc func)
        {
            switch (func)
            {
                case MenuFunc.Map:
                {
                    frame.Navigate(typeof (MapPage));
                    break;
                }
                case MenuFunc.Settings:
                {
                    frame.Navigate(typeof (SettingPage));
                    break;
                }
                case MenuFunc.Search:
                {
                    MapPage page = frame.Content as MapPage;
                    if (page != null)
                    {
                        //Navigate inside MapPage
                        page.SecondFrame.Navigate(typeof (RouteSearchPage));
                    }
                    else
                    {
                        frame.Navigate(typeof (MapPage));
                        Debug.Assert(frame.Content != null, "frame.Content != null");
                        ((MapPage) frame.Content).SecondFrame.Navigate(typeof (RouteSearchPage));
                    }
                    break;
                }
                default:
                {
                    frame.Navigate(typeof (MapPage));
                    break;
                }
            }
        }

        #region Command

        //private RelayCommand<MenuFunc> _functionListViewChanged;

        //public RelayCommand<MenuFunc> FunctionListViewChanged => _functionListViewChanged ?? (_functionListViewChanged = new RelayCommand<MenuFunc>(NavigateToFunction()));

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
