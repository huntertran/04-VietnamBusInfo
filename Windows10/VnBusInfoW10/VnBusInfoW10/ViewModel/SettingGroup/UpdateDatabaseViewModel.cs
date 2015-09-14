using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Devices.Geolocation;
using Windows.UI.Core;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using VnBusInfoW10.Annotations;
using VnBusInfoW10.Model;
using VnBusInfoW10.Utilities;
// ReSharper disable UnusedMember.Local

namespace VnBusInfoW10.ViewModel.SettingGroup
{
    public class UpdateDatabaseViewModel : INotifyPropertyChanged
    {
        private Stopwatch _sw = new Stopwatch();

        private string _textNotification;

        private double _updateProgress;
        private int _notiCount;

        private string _runTime;

        public string TextNotification
        {
            get { return _textNotification; }
            set
            {
                if (value == _textNotification) return;
                _textNotification = value;
                OnPropertyChanged();
            }
        }

        public double UpdateProgress
        {
            get { return _updateProgress; }
            set
            {
                if (value.Equals(_updateProgress)) return;
                _updateProgress = value;
                OnPropertyChanged();
            }
        }

        public int NotiCount
        {
            get { return _notiCount; }
            set
            {
                if (value == _notiCount) return;
                _notiCount = value;
                OnPropertyChanged();
            }
        }

        public string RunTime
        {
            get { return _runTime; }
            set
            {
                if (value == _runTime) return;
                _runTime = value;
                OnPropertyChanged();
            }
        }

        public async Task GetData()
        {
            NotiCount = 0;
            //await Task.Run(() => GetBusInfo());
            //await Task.Run(() => GetStation());
            //await Task.Run(() => GetRoute());
            //await Task.Run(() => ConvertToJson());
            await Task.Run(() => CalculateStationTotal());
        }

        private async Task GetBusInfo()
        {
            string html = await NetwordMethod.GetHttpAsString("http://buyttphcm.com.vn/TTLT.aspx");
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            var nodes =
                doc.DocumentNode
                    .Descendants("select")
                    .First(d => d.Attributes.Contains("id") &&
                                d.Attributes["id"].Value == "ctl00_ContentPlaceHolder1_DropDownList1")
                    .Descendants("option")
                    .ToList();

            StaticData.MapVm.AllBus = new ObservableCollection<BusTotal>();

            for (int index = 1; index < nodes.Count; index++)
            {
                HtmlNode htmlNode = nodes[index];
                BusTotal b = new BusTotal
                {
                    Id = index,
                    TextInfo = new BusTextInfo {RouteNumber = htmlNode.Attributes["value"].Value}
                };

                string[] temp = WebUtility.HtmlDecode(htmlNode.NextSibling.InnerText).Split('-');
                string r = "";
                foreach (string s in temp)
                {
                    try
                    {
                        // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                        Convert.ToInt32(s.Trim());
                    }
                    catch (Exception)
                    {
                        r = r + "-" + s;
                    }
                }

                b.TextInfo.RouteName = r.TrimStart(' ', '-');
                StaticData.MapVm.AllBus.Add(b);
                
                Notify("New bus info added: " + b.TextInfo.RouteNumber + " " + b.TextInfo.RouteName);
                
            }

            foreach (BusTotal busTotal in StaticData.MapVm.AllBus)
            {
                Notify("Getting data for route: " + busTotal.TextInfo.RouteNumber + " - " + busTotal.TextInfo.RouteName);
                html = await
                    NetwordMethod.GetHttpAsString("http://www.buyttphcm.com.vn/Detail_TTLT.aspx?sl=" +
                                                  busTotal.TextInfo.RouteNumber);
                doc = new HtmlDocument();
                doc.LoadHtml(html);

                nodes =
                    doc.DocumentNode.Descendants("td")
                        .Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value == "conten").ToList();

                busTotal.TextInfo.GoDirection = WebUtility.HtmlDecode(nodes[4].InnerText);
                busTotal.TextInfo.BackDirection = WebUtility.HtmlDecode(nodes[5].InnerText);
                busTotal.TextInfo.Details = WebUtility.HtmlDecode(nodes[6].InnerText);
            }

            Notify("Save data");
            await StorageHelper.Object2Json(StaticData.MapVm.AllBus, "data.json");
        }

        private async Task GetStation()
        {
            //Load all data
            if (StaticData.MapVm.AllBus == null)
            {
                StaticData.MapVm.AllBus = await StorageHelper.Json2Object<ObservableCollection<BusTotal>>("data.json");
            }

            //Get bus id
            string html = await NetwordMethod.GetHttpAsString("http://mapbus.ebms.vn/routeoftrunk.aspx");

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            var nodes =
                doc.DocumentNode
                    .Descendants("select").ToList()[0];

            Notify("Started get route id", true);

            for (int index = 1; index < nodes.ChildNodes.Count; index = index + 3)
            {
                HtmlNode htmlNode = nodes.ChildNodes[index];
                int id = Convert.ToInt32(htmlNode.Attributes["value"].Value);
                string routeNumber = nodes.ChildNodes[index + 1].InnerText.Split(']')[0].TrimStart('[');
                foreach (BusTotal busTotal in StaticData.MapVm.AllBus)
                {
                    if (busTotal.TextInfo.RouteNumber == routeNumber)
                    {
                        busTotal.RouteId = id;
                        Notify("Route: " + busTotal.TextInfo.RouteName);
                    }
                }
            }

            foreach (BusTotal busTotal in StaticData.MapVm.AllBus)
            {
                //Go direction

                busTotal.GoStationList = new ObservableCollection<BusStation>();

                html =
                    await
                        NetwordMethod.GetHttpAsString(
                            "http://mapbus.ebms.vn/ajax.aspx?action=listRouteStations&isgo=true&rid=" +
                            busTotal.RouteId);

                if (html == "null")
                {
                    continue;
                }

                JObject j = JObject.Parse(html);
                RawBusStationRootObject r = j.ToObject<RawBusStationRootObject>();

                foreach (ROW row in r.TABLE[0].ROW)
                {
                    BusStation b = new BusStation
                    {
                        StationId = Convert.ToInt32(row.COL[1].DATA),
                        MiniRoute = row.COL[3].DATA,
                        U1 = row.COL[4].DATA,
                        No = Convert.ToInt32(row.COL[5].DATA),
                        U2 = row.COL[6].DATA,
                        Name = row.COL[7].DATA,
                        Lon = row.COL[8].DATA,
                        Lat = row.COL[9].DATA,
                        Address = row.COL[12].DATA,
                        CodeName = row.COL[13].DATA
                    };

                    try
                    {
                        b.NextStationId = Convert.ToInt32(row.COL[2].DATA);
                    }
                    catch
                    {
                        // ignored
                    }

                    busTotal.GoStationList.Add(b);
                    Notify("New station added: Go: " + b.Name);
                }

                //Back direction
                busTotal.BackStationList = new ObservableCollection<BusStation>();
                html =
                    await
                        NetwordMethod.GetHttpAsString(
                            "http://mapbus.ebms.vn/ajax.aspx?action=listRouteStations&isgo=false&rid=" +
                            busTotal.RouteId);

                j = JObject.Parse(html);
                r = j.ToObject<RawBusStationRootObject>();

                foreach (ROW row in r.TABLE[0].ROW)
                {
                    BusStation b = new BusStation
                    {
                        StationId = Convert.ToInt32(row.COL[1].DATA),
                        MiniRoute = row.COL[3].DATA,
                        U1 = row.COL[4].DATA,
                        No = Convert.ToInt32(row.COL[5].DATA),
                        U2 = row.COL[6].DATA,
                        Name = row.COL[7].DATA,
                        Lon = row.COL[8].DATA,
                        Lat = row.COL[9].DATA,
                        Address = row.COL[12].DATA,
                        CodeName = row.COL[13].DATA
                    };
                    
                    try
                    {
                        b.NextStationId = Convert.ToInt32(row.COL[2].DATA);
                    }
                    catch
                    {
                        // ignored
                    }

                    busTotal.BackStationList.Add(b);
                    Notify("New station added: Back: " + b.Name);
                }
                
                Notify("BUS: " + busTotal.TextInfo.RouteNumber + " COMPLETED: ALL STATION ADDED");
            }

            await StorageHelper.Object2Json(StaticData.MapVm.AllBus, "data.json");
        }

        private async Task GetRoute()
        {
            //Load all data
            if (StaticData.MapVm.AllBus == null)
            {
                StaticData.MapVm.AllBus = await StorageHelper.Json2Object<ObservableCollection<BusTotal>>("data.json");
            }

            foreach (BusTotal busTotal in StaticData.MapVm.AllBus)
            {
                //Go Direction
                busTotal.GoRoute = new ObservableCollection<BasicGeoposition>();

                string html =
                    await
                        NetwordMethod.GetHttpAsString(
                            "http://mapbus.ebms.vn/ajax.aspx?action=GetFullRoute&isgo=true&rid=" + busTotal.RouteId);

                if (!string.IsNullOrEmpty(html))
                {
                    html = html.Trim();
                    string[] goRoute = html.Split(' ');
                    foreach (string s in goRoute)
                    {
                        if (string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s))
                        {
                            continue;
                        }
                        BasicGeoposition b = new BasicGeoposition
                        {
                            Latitude = Convert.ToDouble(s.Split(',')[0]),
                            Longitude = Convert.ToDouble(s.Split(',')[1])
                        };
                        busTotal.GoRoute.Add(b);
                    }
                }

                //Back Direction
                busTotal.BackRoute = new ObservableCollection<BasicGeoposition>();

                html =
                    await
                        NetwordMethod.GetHttpAsString(
                            "http://mapbus.ebms.vn/ajax.aspx?action=GetFullRoute&isgo=false&rid=" + busTotal.RouteId);

                if (!string.IsNullOrEmpty(html))
                {
                    html = html.Trim();
                    string[] backRoute = html.Split(' ');
                    foreach (string s in backRoute)
                    {
                        if (string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s))
                        {
                            continue;
                        }
                        BasicGeoposition b = new BasicGeoposition
                        {
                            Latitude = Convert.ToDouble(s.Split(',')[0]),
                            Longitude = Convert.ToDouble(s.Split(',')[1])
                        };
                        busTotal.BackRoute.Add(b);
                    }
                }

                Notify("New route added: " + busTotal.TextInfo.RouteNumber);
            }

            await StorageHelper.Object2Json(StaticData.MapVm.AllBus, "data.json");
        }

        private async Task ConvertToJson()
        {
            //Load all data
            if (StaticData.MapVm.AllBus == null)
            {
                StaticData.MapVm.AllBus = await StorageHelper.Json2Object<ObservableCollection<BusTotal>>("data.json");
            }

            //Save to Json
            await StorageHelper.Object2Json(StaticData.MapVm.AllBus, "data.json");

            //Try read
            StaticData.MapVm.AllBus = await StorageHelper.Json2Object<ObservableCollection<BusTotal>>("data.json");
        }

        private async Task CalculateStationTotal()
        {
            //Load all data
            if (StaticData.MapVm.AllBus == null)
            {
                StaticData.MapVm.AllBus = await StorageHelper.Json2Object<ObservableCollection<BusTotal>>("data.json");
            }

            StaticData.MapVm.AllStation = new ObservableCollection<StationTotal>();
            foreach (BusTotal busTotal in StaticData.MapVm.AllBus)
            {
                if (busTotal.GoStationList != null)
                {
                    //Go direction
                    foreach (BusStation busStation in busTotal.GoStationList)
                    {
                        StationTotal s = null;
                        if (StaticData.MapVm.AllStation.Any(d => d.StationId == busStation.StationId))
                        {
                            s = StaticData.MapVm.AllStation.First(d => d.StationId == busStation.StationId);
                        }
                        if (s == null)
                        {
                            //Station is not added
                            StationTotal newStationTotal = new StationTotal(busStation)
                            {
                                BusAtStationList = new ObservableCollection<BusAtStation>()
                            };

                            BusAtStation b = new BusAtStation
                            {
                                Id = busTotal.Id,
                                BusDirection = Direction.Go
                            };
                            newStationTotal.BusAtStationList.Add(b);

                            Notify("GO|" + busTotal.Id + "|" + busStation.StationId + "|NEW STATION|NEW BUS");

                            StaticData.MapVm.AllStation.Add(newStationTotal);
                        }
                        else
                        {
                            BusAtStation busAtStation = null;
                            if (s.BusAtStationList.Any(d => d.Id == busTotal.Id))
                            {
                                busAtStation = s.BusAtStationList.First(d => d.Id == busTotal.Id);
                            }
                            if (busAtStation == null)
                            {
                                BusAtStation b = new BusAtStation
                                {
                                    Id = busTotal.Id,
                                    BusDirection = Direction.Go
                                };
                                s.BusAtStationList.Add(b);
                                Notify("GO|" + busTotal.Id + "|" + busStation.StationId + "|OLD STATION|NEW BUS");
                            }
                            else
                            {
                                busAtStation.BusDirection = Direction.Both;
                                Notify("GO|" + busTotal.Id + "|" + busStation.StationId + "|OLD STATION|OLD BUS");
                            }
                            StaticData.MapVm.AllStation.Add(s);
                        }
                    }
                }

                if (busTotal.BackStationList != null)
                {
                    //Back direction
                    foreach (BusStation busStation in busTotal.BackStationList)
                    {
                        StationTotal s = null;
                        if (StaticData.MapVm.AllStation.Any(d => d.StationId == busStation.StationId))
                        {
                            s = StaticData.MapVm.AllStation.First(d => d.StationId == busStation.StationId);
                        }
                        if (s == null)
                        {
                            //Station is not added
                            StationTotal newStationTotal = new StationTotal(busStation)
                            {
                                BusAtStationList = new ObservableCollection<BusAtStation>()
                            };

                            BusAtStation b = new BusAtStation
                            {
                                Id = busTotal.Id,
                                BusDirection = Direction.Back
                            };

                            newStationTotal.BusAtStationList.Add(b);
                            Notify("BA|" + busTotal.Id + "|" + busStation.StationId + "|NEW STATION|NEW BUS");
                            StaticData.MapVm.AllStation.Add(newStationTotal);
                        }
                        else
                        {
                            BusAtStation busAtStation = null;
                            if (s.BusAtStationList.Any(d => d.Id == busTotal.Id))
                            {
                                busAtStation = s.BusAtStationList.First(d => d.Id == busTotal.Id);
                            }
                            if (busAtStation == null)
                            {
                                BusAtStation b = new BusAtStation
                                {
                                    Id = busTotal.Id,
                                    BusDirection = Direction.Back
                                };
                                s.BusAtStationList.Add(b);
                                Notify("BA|" + busTotal.Id + "|" + busStation.StationId + "|OLD STATION|NEW BUS");
                            }
                            else
                            {
                                busAtStation.BusDirection = Direction.Both;
                                Notify("BA|" + busTotal.Id + "|" + busStation.StationId + "|OLD STATION|OLD BUS");
                            }
                            StaticData.MapVm.AllStation.Add(s);
                        }
                    }
                }
            }

            await StorageHelper.Object2Json(StaticData.MapVm.AllStation, "allstation.json");
        }

        private async void Notify(string s, bool isReset = false)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal,
                () =>
                {
                    if (!isReset)
                    {
                        NotiCount++;
                    }
                    else
                    {
                        NotiCount = 0;
                    }
                    TextNotification = s;
                });
        }

        private void Start()
        {
            _sw = Stopwatch.StartNew();
        }

        private void Stop()
        {
            _sw.Stop();

            var elapsedMs = _sw.ElapsedMilliseconds;
            RunTime = elapsedMs.ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}