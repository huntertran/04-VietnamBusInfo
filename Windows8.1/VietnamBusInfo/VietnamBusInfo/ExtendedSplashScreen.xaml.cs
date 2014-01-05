using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using VietnamBusInfo.Model;
using VietnamBusInfo.Utilities;
using VietnamBusInfo.ViewModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace VietnamBusInfo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExtendedSplashScreen : Page
    {
        public ExtendedSplashScreen()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private async void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            MovingRect.Begin();
            await Task.Run(() => LoadAllStations());
            await Task.Run(() => LoadDataFile());
            await Task.Run(() => LoadStationInfo());
            await Task.Run(() => LoadAllBus());
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame != null) rootFrame.Navigate(typeof (PageGroups.MainPage));
        }

        //Load All Stations to RAM
        private void LoadAllStations()
        {
            if (StaticData._stationTotal.Count == 0)
            {
                XDocument doc = XDocument.Load("Data/StationList.xml");
                ObservableCollection<Bus> _busList;
                ObservableCollection<Direction> _busDirection;

                int i = 0;

                foreach (var item in doc.Element("root").Elements("station"))
                {
                    _busList = new ObservableCollection<Bus>();

                    foreach (var busListTemp in item.Elements("BusList"))
                    {

                        foreach (var busNumTemp in busListTemp.Elements("busNum"))
                        {
                            _busDirection = new ObservableCollection<Direction>();
                            foreach (var BusDirectionTemp in busNumTemp.Elements("busDirection"))
                            {
                                var newDirection = new Direction()
                                {
                                    direction = BusDirectionTemp.Value,
                                };

                                _busDirection.Add(newDirection);
                            }

                            var newBus = new Bus()
                            {
                                busNumber = busNumTemp.Attribute("busNum").Value,
                                busDirection = _busDirection,
                            };

                            _busList.Add(newBus);
                        }

                    }

                    var newStationTotal = new StationTotal()
                    {
                        //id = item.Element("id").Value,
                        id = i.ToString(),
                        stationId = item.Element("stationId").Value,
                        longitude = Convert.ToDouble(item.Element("longitude").Value),
                        latitude = Convert.ToDouble(item.Element("latitude").Value),
                        addressNum = item.Element("addressNum").Value,
                        addressStreet = item.Element("addressStreet").Value,
                        addressDistrict = item.Element("addressDistrict").Value,
                        busList = _busList,
                    };

                    i++;

                    StaticData._stationTotal.Add(newStationTotal);
                }
            }

            //Enable Async
            //StorageFile destinationFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(
            //        "placeHolderData.dat", CreationCollisionOption.ReplaceExisting);
        }

        //FullBusRoute.xml - All Bus Route Available
        private void LoadDataFile()
        {
            if (StaticData._busRoute == null)
            {
                StaticData._busRoute = new ObservableCollection<BusRoute>();

                XDocument doc = XDocument.Load("Data/FullBusRoute.xml");

                ObservableCollection<LocationPointWithId> tempGoLocationPoint;
                ObservableCollection<LocationPointWithId> tempBackLocationPoint;
                foreach (var item in doc.Element("root").Elements("BusRoute"))
                {

                    tempGoLocationPoint = new ObservableCollection<LocationPointWithId>();
                    tempBackLocationPoint = new ObservableCollection<LocationPointWithId>();

                    foreach (var locationPoint in item.Element("GoDirection").Elements("LocationPoint"))
                    {
                        var newLocationPoint = new LocationPointWithId()
                        {
                            id = locationPoint.Attribute("Id").Value,
                            longitude = Convert.ToDouble(locationPoint.Element("Lon").Value),
                            latitude = Convert.ToDouble(locationPoint.Element("Lat").Value),
                        };

                        tempGoLocationPoint.Add(newLocationPoint);
                    }

                    foreach (var locationPoint in item.Element("BackDirection").Elements("LocationPoint"))
                    {
                        var newLocationPoint = new LocationPointWithId()
                        {
                            id = locationPoint.Attribute("Id").Value,
                            longitude = Convert.ToDouble(locationPoint.Element("Lon").Value),
                            latitude = Convert.ToDouble(locationPoint.Element("Lat").Value),
                        };

                        tempBackLocationPoint.Add(newLocationPoint);
                    }

                    var newBusRoute = new BusRoute()
                    {
                        id = item.Attribute("BusId").Value,
                        goDirection = tempGoLocationPoint,
                        backDirection = tempBackLocationPoint,
                    };

                    StaticData._busRoute.Add(newBusRoute);
                }
            }

            //Enable Async
            //StorageFile destinationFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(
            //        "placeHolderData.dat", CreationCollisionOption.ReplaceExisting);

            //busRouteStatusTextBlock.Text = "Bus Routes Data Ready";
        }

        //StationInfo.xml - All Station Information - Each bus have 1 record in this Collection
        private void LoadStationInfo()
        {
            UTMConverter utmConverter;
            StaticData._busStationCollection = new ObservableCollection<BusStationCollection>();

            if (StaticData._busStationCollection.Count() == 0)
            {
                StaticData._busStationCollection = new ObservableCollection<BusStationCollection>();

                XDocument doc = XDocument.Load("Data/StationInfo.xml");

                BusStationCollection newBusStationCollection;
                foreach (var item in doc.Element("Stations").Elements("Stations"))
                {
                    newBusStationCollection = new BusStationCollection();

                    newBusStationCollection.id = item.Attribute("BusNum").Value;

                    foreach (var direction in item.Elements("Direction"))
                    {
                        if (direction.Attribute("go").Value == "1")
                        {
                            newBusStationCollection.goDirection = new ObservableCollection<BusStation>();

                            foreach (var station in direction.Elements("ID"))
                            {
                                utmConverter = new UTMConverter();
                                double tempLat = Convert.ToDouble(station.Element("latitude").Value);
                                double tempLon = Convert.ToDouble(station.Element("longitude").Value);

                                //Lat and Lon is in reverse order
                                utmConverter.ToLatLon(tempLon, tempLat, 48, 0);

                                BusStation newBusStation = new BusStation()
                                {
                                    id = "[" + station.Attribute("id").Value + "]",
                                    number = station.Element("number").Value,
                                    address = "đường " + station.Element("address").Value,
                                    district = "quận " + station.Element("district").Value,
                                    lat = utmConverter.Latitude,
                                    lon = utmConverter.Longitude,
                                    stationId = station.Element("stationId").Value,
                                };

                                newBusStationCollection.goDirection.Add(newBusStation);
                            }
                        }

                        if (direction.Attribute("go").Value == "0")
                        {

                            newBusStationCollection.backDirection = new ObservableCollection<BusStation>();

                            foreach (var station in direction.Elements("ID"))
                            {
                                utmConverter = new UTMConverter();
                                double tempLat = Convert.ToDouble(station.Element("latitude").Value);
                                double tempLon = Convert.ToDouble(station.Element("longitude").Value);
                                utmConverter.ToLatLon(tempLon, tempLat, 48, 0);

                                BusStation newBusStation = new BusStation()
                                {
                                    id = "[" + station.Attribute("id").Value + "]",
                                    number = station.Element("number").Value,
                                    address = "đường " + station.Element("address").Value,
                                    district = "quận " + station.Element("district").Value,
                                    lat = utmConverter.Latitude,
                                    lon = utmConverter.Longitude,
                                    stationId = station.Element("stationId").Value,
                                };

                                newBusStationCollection.backDirection.Add(newBusStation);
                            }
                        }
                    }

                    StaticData._busStationCollection.Add(newBusStationCollection);
                }

                //Enable Async
                //StorageFile destinationFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(
                //        "placeHolderData.dat", CreationCollisionOption.ReplaceExisting);
            }
        }

        //StationDetails - Bus Details to be exact
        private void LoadAllBus()
        {
            //await Task.Run(() =>
            //{
                if (StaticData._busContent == null)
                {
                    StaticData._busContent = new ObservableCollection<BusContent>();

                    XDocument doc = XDocument.Load("Data/StationDetail.xml");

                    foreach (var item in doc.Element("rool").Elements("BusDetail"))
                    {
                        var newBusContent = new BusContent()
                        {
                            id = item.Element("id").Value,
                            name = item.Element("name").Value,
                            go = item.Element("go").Value,
                            back = item.Element("back").Value,
                            detail = item.Element("detail").Value,
                        };

                        StaticData._busContent.Add(newBusContent);
                    }
                }
                //Enable Async
                //StorageFile destinationFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(
                //        "placeHolderData.dat", CreationCollisionOption.ReplaceExisting);

                //busListBox.ItemsSource = StaticData._busContent;
            //});
        }
    }
}
