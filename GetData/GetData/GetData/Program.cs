﻿using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using GetData.Model;
using GetData.OldModel;
using HtmlAgilityPack;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;

namespace GetData
{
    public class Program
    {

        public static BusNameList newBusNameList = new BusNameList();
        public static CodedBusNameList codedBusNameList = new CodedBusNameList();
        public static GeneralStationList generalStationList = new GeneralStationList();

        #region old data

        public static ObservableCollection<BusContent> _busContent;
        public static ObservableCollection<BusRoute> _busRoute;
        public static ObservableCollection<BusStationCollection> _busStationCollection;
        public static ObservableCollection<StationTotal> _stationTotal = new ObservableCollection<StationTotal>();

        #endregion

        #region Helper

        public static string GetHttpAsString(string link)
        {
            string result = "";

            using (WebClient client = new WebClient())
            {
                result = client.DownloadString(link);
            }

            return result;
        }

        public static string UTF8Decode(string originalString)
        {
            byte[] bytes = Encoding.Default.GetBytes(originalString);
            return WebUtility.HtmlDecode(Encoding.UTF8.GetString(bytes));
        }

        public static ObservableCollection<GPSPoint> ConvertStringToGPSPoints(string original)
        {
            ObservableCollection<GPSPoint> gpsPointCollection = new ObservableCollection<GPSPoint>();

            string[] temp = original.Split(' ');
            foreach (string s in temp)
            {
                if (String.IsNullOrEmpty(s))
                {
                    continue;
                }
                GPSPoint point = new GPSPoint();
                point.lat = Convert.ToDouble(s.Split(',')[1]);
                point.lon = Convert.ToDouble(s.Split(',')[0]);

                gpsPointCollection.Add(point);
            }

            return gpsPointCollection;
        }

        public sealed class StringWriterWithEncoding : StringWriter
        {
            private readonly Encoding encoding;

            public StringWriterWithEncoding(Encoding encoding)
            {
                this.encoding = encoding;
            }

            public override Encoding Encoding
            {
                get { return encoding; }
            }
        }

        public static string Object2Xml<T>(T value, string fileName)
        {
            if (value == null)
            {
                return string.Empty;
            }
            try
            {
                var xmlserializer = new XmlSerializer(typeof(T));
                var stringWriter = new StringWriterWithEncoding(Encoding.UTF8);
                var xmlWriterSetting = new XmlWriterSettings();
                xmlWriterSetting.Encoding = Encoding.UTF8;
                using (var writer = XmlWriter.Create(stringWriter, xmlWriterSetting))
                {
                    xmlserializer.Serialize(writer, value);
                    File.WriteAllText(@"C:\Users\Tuan Tran\Desktop\VietnamBusInfo\Data\" + fileName + ".xml", stringWriter.ToString());
                    return stringWriter.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred", ex);
            }
        }

        public static T Xml2Object<T>(string fileName)
        {
            XmlSerializer srl = new XmlSerializer(typeof (T));
            using (
                XmlReader reader =
                    XmlReader.Create(@"E:\Store Project\Universal\45 - HDV\Data\\" + fileName))
            {
                var obj = srl.Deserialize(reader);
                return (T) obj;
            }
        }

        #endregion

        public static void Main(string[] args)
        {
            //For unicode output
            Console.OutputEncoding = System.Text.Encoding.Unicode;

            goto GetNewData;
            #region Load Old Data

            _stationTotal = Xml2Object<ObservableCollection<StationTotal>>("StationList.xml");
            _busRoute = Xml2Object<ObservableCollection<BusRoute>>("FullBusRoute.xml");
            _busStationCollection = Xml2Object<ObservableCollection<BusStationCollection>>("StationInfo.xml");
            _busContent = Xml2Object<ObservableCollection<BusContent>>("StationDetail.xml");

            newBusNameList = Xml2Object<BusNameList>("BusNameList.xml");
            codedBusNameList = Xml2Object<CodedBusNameList>("CodedBusNameList.xml");
            generalStationList = Xml2Object<GeneralStationList>("GeneralStationList.xml");

            //TODO: skip to update data here
            //goto UpdateData;

            #endregion

            GetNewData:
            _stationTotal = Xml2Object<ObservableCollection<StationTotal>>("StationList.xml");
            _busRoute = Xml2Object<ObservableCollection<BusRoute>>("FullBusRoute.xml");
            _busStationCollection = Xml2Object<ObservableCollection<BusStationCollection>>("StationInfo.xml");
            _busContent = Xml2Object<ObservableCollection<BusContent>>("StationDetail.xml");

            #region Get List of buses

            //Reuse data
            string lobHtml = GetHttpAsString("http://www.buyttphcm.com.vn/TTLT.aspx");
            HtmlDocument lobDocument = new HtmlDocument();

            //goto GetBusesCode;

            lobDocument.LoadHtml(lobHtml);
            
            newBusNameList.busNameCollection = new ObservableCollection<BusName>();

            var nodes =
                lobDocument.DocumentNode.SelectNodes("//select[@id='ctl00_ContentPlaceHolder1_DropDownList1']/option");
            foreach (HtmlNode htmlNode in nodes)
            {
                BusName newBusName = new BusName();
                newBusName.number = htmlNode.Attributes["value"].Value;

                if (newBusName.number == "0")
                {
                    continue;
                }

                newBusName.name = UTF8Decode(htmlNode.NextSibling.InnerText);

                newBusNameList.busNameCollection.Add(newBusName);
                Console.WriteLine(newBusName.name);
            }

            //Object2Xml(newBusNameList, "BusNameList");

            #endregion

            #region Get Buses text data

            foreach (BusName busName in newBusNameList.busNameCollection)
            {
                lobHtml = GetHttpAsString("http://www.buyttphcm.com.vn/Detail_TTLT.aspx?sl=" + busName.number);

                lobHtml = UTF8Decode(lobHtml);
                lobDocument.LoadHtml(lobHtml);

                BusTextData newBusTextData = new BusTextData();

                nodes =
                    lobDocument.DocumentNode.SelectNodes(
                        "//div[@id='ctl00_ContentPlaceHolder1_UpdatePanel2']/table[2]");

                newBusTextData.go = nodes[0].SelectNodes(".//tr[3]/td[@class='conten']")[0].InnerText.Trim();
                newBusTextData.back = nodes[0].SelectNodes(".//tr[4]/td[@class='conten']")[0].InnerText.Trim();
                newBusTextData.timeInfo = nodes[0].SelectNodes(".//tr[5]/td[@class='conten']")[0].InnerText.Trim();

                busName.busTextData = newBusTextData;

                Console.WriteLine(newBusTextData.go);
                Console.WriteLine(newBusTextData.back);
                Console.WriteLine(newBusTextData.timeInfo);

                Console.WriteLine(busName.name + " - Bus Text Data added!");

                
            }

            Object2Xml(newBusNameList, "BusNameList");

            #endregion

            #region Get Buses name code

            GetBusesCode:

            codedBusNameList.codedBusNameCollection = new ObservableCollection<BusCodedName>();

            lobHtml = GetHttpAsString("http://mapbus.ebms.vn/routeoftrunk.aspx");
            lobDocument.LoadHtml(lobHtml);

            nodes = lobDocument.DocumentNode.SelectNodes("//select[@id='lstTuyen']/option");
            foreach (HtmlNode htmlNode in nodes)
            {
                BusCodedName newBusName = new BusCodedName();
                newBusName.number = Convert.ToInt32(htmlNode.Attributes["value"].Value);

                if (newBusName.number == 0)
                {
                    continue;
                }

                newBusName.name = UTF8Decode(htmlNode.NextSibling.InnerText);

                codedBusNameList.codedBusNameCollection.Add(newBusName);
                Console.WriteLine("Coded: " + newBusName.number + " - " + newBusName.name);
            }

            #endregion

            #region Get List Route Station

            GetListRouteStation:

            foreach (BusCodedName bus in codedBusNameList.codedBusNameCollection)
            {

                bus.directionRouteCollection = new ObservableCollection<DirectionRoute>();

                //Go Direction

                DirectionRoute directionGoRoute = new DirectionRoute();
                directionGoRoute.isGo = true;

                lobHtml =
                    GetHttpAsString("http://mapbus.ebms.vn/ajax.aspx?action=listRouteStations&rid=" + bus.number +
                                    "&isgo=true");
                if (lobHtml == "null")
                {
                    Console.WriteLine("Bus: " + bus.name + " don't have route. Skip");
                    continue;
                }
                lobHtml = UTF8Decode(lobHtml);
                JObject jObject = new JObject();
                jObject = JObject.Parse(lobHtml);

                RootRouteStation rootRouteGoStation = jObject.ToObject<RootRouteStation>();

                directionGoRoute.routeStationCollection = new ObservableCollection<RouteStation>();
                directionGoRoute.routePoints = new ObservableCollection<GPSPoint>();

                foreach (ROW row in rootRouteGoStation.TABLE[0].ROW)
                {
                    RouteStation newRouteStation = new RouteStation();
                    newRouteStation.no = Convert.ToInt32(row.COL[0].DATA);
                    newRouteStation.stationId = Convert.ToInt32(row.COL[1].DATA);
                    if (row.COL[2].DATA != "")
                    {
                        newRouteStation.nextStationId = Convert.ToInt32(row.COL[2].DATA);
                    }
                    newRouteStation.route = row.COL[3].DATA;
                    ObservableCollection<GPSPoint> gpsPoints = ConvertStringToGPSPoints(row.COL[3].DATA);
                    foreach (GPSPoint point in gpsPoints)
                    {
                        directionGoRoute.routePoints.Add(point);
                    }
                    newRouteStation.name = row.COL[7].DATA;
                    newRouteStation.lon = row.COL[9].DATA;
                    newRouteStation.lat = row.COL[8].DATA;
                    newRouteStation.address = row.COL[12].DATA;

                    directionGoRoute.routeStationCollection.Add(newRouteStation);

                    Console.WriteLine("New station added: " + newRouteStation.stationId);
                }

                bus.directionRouteCollection.Add(directionGoRoute);

                //Back Direction

                DirectionRoute directionBackRoute = new DirectionRoute();
                directionBackRoute.isGo = false;

                lobHtml =
                    GetHttpAsString("http://mapbus.ebms.vn/ajax.aspx?action=listRouteStations&rid=" + bus.number +
                                    "&isgo=false");
                lobHtml = UTF8Decode(lobHtml);
                jObject = new JObject();
                jObject = JObject.Parse(lobHtml);

                RootRouteStation rootRouteBackStation = jObject.ToObject<RootRouteStation>();

                directionBackRoute.routeStationCollection = new ObservableCollection<RouteStation>();
                directionBackRoute.routePoints = new ObservableCollection<GPSPoint>();

                foreach (ROW row in rootRouteBackStation.TABLE[0].ROW)
                {
                    RouteStation newRouteStation = new RouteStation();
                    newRouteStation.no = Convert.ToInt32(row.COL[0].DATA);
                    newRouteStation.stationId = Convert.ToInt32(row.COL[1].DATA);
                    if (row.COL[2].DATA != "")
                    {
                        newRouteStation.nextStationId = Convert.ToInt32(row.COL[2].DATA);
                    }
                    newRouteStation.route = row.COL[3].DATA;
                    ObservableCollection<GPSPoint> gpsPoints = ConvertStringToGPSPoints(row.COL[3].DATA);
                    foreach (GPSPoint point in gpsPoints)
                    {
                        directionBackRoute.routePoints.Add(point);
                    }
                    newRouteStation.name = row.COL[7].DATA;
                    newRouteStation.lon = row.COL[9].DATA;
                    newRouteStation.lat = row.COL[8].DATA;
                    newRouteStation.address = row.COL[12].DATA;

                    directionBackRoute.routeStationCollection.Add(newRouteStation);

                    Console.WriteLine("New station added: " + newRouteStation.stationId);
                }

                bus.directionRouteCollection.Add(directionBackRoute);
            }

            #endregion

            #region Caculate RouteStation

            generalStationList.generalStationCollection = new ObservableCollection<GeneralStation>();

            foreach (BusCodedName bus in codedBusNameList.codedBusNameCollection)
            {
                if (bus.directionRouteCollection.Count == 0)
                {
                    continue;
                }
                //Go Direction
                foreach (RouteStation station in bus.directionRouteCollection[0].routeStationCollection)
                {
                    GeneralStation generalStation =
                        generalStationList.generalStationCollection.FirstOrDefault(i => i.stationId == station.stationId);
                    if (generalStation != null)
                    {
                        //Existed in list
                        //TODO: code here
                        ThroughStationBus thStationBus = new ThroughStationBus();
                        thStationBus.name = bus.name;
                        thStationBus.number = bus.number.ToString();
                        thStationBus.direction = DirectionType.Go;

                        generalStation.throughStationBusCollection.Add(thStationBus);

                        
                    }
                    else
                    {
                        //Not existed
                        GeneralStation newGeneralStation = new GeneralStation(station);
                        newGeneralStation.throughStationBusCollection = new ObservableCollection<ThroughStationBus>();

                        ThroughStationBus thStationBus = new ThroughStationBus();
                        thStationBus.name = bus.name;
                        thStationBus.number = bus.number.ToString();
                        thStationBus.direction = DirectionType.Go;

                        newGeneralStation.throughStationBusCollection.Add(thStationBus);
                        Console.WriteLine("New General Station Added: " + newGeneralStation.stationId);

                        generalStationList.generalStationCollection.Add(newGeneralStation);
                    }
                }

                //Back Direction
                foreach (RouteStation station in bus.directionRouteCollection[1].routeStationCollection)
                {
                    GeneralStation generalStation =
                        generalStationList.generalStationCollection.FirstOrDefault(i => i.stationId == station.stationId);
                    if (generalStation != null)
                    {
                        //Existed in list
                        //TODO: code here
                        ThroughStationBus thStationBus = new ThroughStationBus();
                        thStationBus.name = bus.name;
                        thStationBus.number = bus.number.ToString();
                        thStationBus.direction = DirectionType.Go;

                        bool isBoth = false;

                        foreach (ThroughStationBus directionBus in generalStation.throughStationBusCollection)
                        {
                            if (directionBus.number == thStationBus.number)
                            {
                                directionBus.direction = DirectionType.Both;
                                isBoth = true;
                                break;
                            }
                        }

                        if (!isBoth)
                        {
                            generalStation.throughStationBusCollection.Add(thStationBus);
                        }
                    }
                    else
                    {
                        //Not existed
                        GeneralStation newGeneralStation = new GeneralStation(station);
                        newGeneralStation.throughStationBusCollection = new ObservableCollection<ThroughStationBus>();

                        ThroughStationBus thStationBus = new ThroughStationBus();
                        thStationBus.name = bus.name;
                        thStationBus.number = bus.number.ToString();
                        thStationBus.direction = DirectionType.Back;

                        newGeneralStation.throughStationBusCollection.Add(thStationBus);
                        Console.WriteLine("New General Station Added: " + newGeneralStation.stationId);

                        generalStationList.generalStationCollection.Add(newGeneralStation);
                    }
                }
            }

            #endregion

            //Write to file
            Object2Xml(codedBusNameList, "CodedBusNameList");
            Object2Xml(generalStationList, "GeneralStationList");

            UpdateData:
            //Update old data to new one, using old structure

            //Update StationDetail
            foreach (BusName bus in newBusNameList.busNameCollection)
            {
                bool isOldData = false;
                foreach (BusContent content in _busContent)
                {
                    if (bus.number == content.id)
                    {
                        content.id = bus.number;
                        content.name = bus.name;
                        content.go = bus.busTextData.go;
                        content.back = bus.busTextData.back;
                        content.detail = bus.busTextData.timeInfo;
                        isOldData = true;
                        break;
                    }
                }

                if (!isOldData)
                {
                    BusContent content = new BusContent();
                    content.id = bus.number;
                    content.name = bus.name;
                    content.go = bus.busTextData.go;
                    content.back = bus.busTextData.back;
                    content.detail = bus.busTextData.timeInfo;
                    _busContent.Add(content);
                }
            }

            //Write Data
            Object2Xml(_busContent, "StationDetail");

            //Update StationInfo
            foreach (BusCodedName bus in codedBusNameList.codedBusNameCollection)
            {
                bool isOldData = false;
                foreach (BusStationCollection content in _busStationCollection)
                {
                    string name = bus.name.Split(']')[0].TrimStart('[');
                    int busNum = 0;
                    try
                    {
                        busNum = Convert.ToInt32(name);
                    }
                    catch (Exception)
                    {
                        //Cannot convert - new bus staton collection
                        Console.WriteLine("New strange name bus detected: " + name);
                    }
                    if (busNum == Convert.ToInt32(content.id))
                    {
                        content.id = busNum.ToString();
                        content.goDirection = new ObservableCollection<BusStation>();
                        content.backDirection = new ObservableCollection<BusStation>();
                        if (bus.directionRouteCollection[0].isGo)
                        {
                            //Correct go direction
                            int i = 1;
                            foreach (RouteStation station in bus.directionRouteCollection[0].routeStationCollection)
                            {
                                BusStation newStation = new BusStation(station);
                                newStation.id = "[" + i + "]";
                                i++;
                                content.goDirection.Add(newStation);
                            }
                            i = 1;
                            foreach (RouteStation station in bus.directionRouteCollection[1].routeStationCollection)
                            {
                                BusStation newStation = new BusStation(station);
                                newStation.id = "[" + i + "]";
                                i++;
                                content.backDirection.Add(newStation);
                            }
                        }
                        else
                        {
                            foreach (RouteStation station in bus.directionRouteCollection[0].routeStationCollection)
                            {
                                BusStation newStation = new BusStation(station);
                                content.backDirection.Add(newStation);
                            }
                            foreach (RouteStation station in bus.directionRouteCollection[1].routeStationCollection)
                            {
                                BusStation newStation = new BusStation(station);
                                content.goDirection.Add(newStation);
                            }
                        }
                        isOldData = true;
                        Console.WriteLine("Old bus: " + content.id);
                        break;
                    }
                }

                if (!isOldData)
                {
                    BusStationCollection newStationCollection = new BusStationCollection();
                    newStationCollection.id = bus.name.Split(']')[0].TrimStart('[');
                    newStationCollection.goDirection = new ObservableCollection<BusStation>();
                    newStationCollection.backDirection = new ObservableCollection<BusStation>();
                    if (bus.directionRouteCollection.Count == 0)
                    {
                        continue;
                    }
                    if (bus.directionRouteCollection[0].isGo)
                    {
                        //Correct go direction
                        int i = 1;
                        foreach (RouteStation station in bus.directionRouteCollection[0].routeStationCollection)
                        {
                            BusStation newStation = new BusStation(station);
                            newStation.id = "[" + i + "]";
                            i++;
                            newStationCollection.goDirection.Add(newStation);
                        }
                        i = 1;
                        foreach (RouteStation station in bus.directionRouteCollection[1].routeStationCollection)
                        {
                            BusStation newStation = new BusStation(station);
                            newStation.id = "[" + i + "]";
                            i++;
                            newStationCollection.backDirection.Add(newStation);
                        }
                    }
                    else
                    {
                        foreach (RouteStation station in bus.directionRouteCollection[0].routeStationCollection)
                        {
                            BusStation newStation = new BusStation(station);
                            newStationCollection.backDirection.Add(newStation);
                        }
                        foreach (RouteStation station in bus.directionRouteCollection[1].routeStationCollection)
                        {
                            BusStation newStation = new BusStation(station);
                            newStationCollection.goDirection.Add(newStation);
                        }
                    }

                    Console.WriteLine("Strange bus added to updated data: " + bus.name);

                }
            }

            //Write Data
            Object2Xml(_busStationCollection, "StationInfo");

            //Update FullBusRoute
            foreach (BusCodedName bus in codedBusNameList.codedBusNameCollection)
            {
                bool isOldData = false;
                foreach (BusRoute content in _busRoute)
                {
                    string name = bus.name.Split(']')[0].TrimStart('[');
                    int busNum = 0;
                    try
                    {
                        busNum = Convert.ToInt32(name);
                    }
                    catch (Exception)
                    {
                        //Cannot convert - new bus staton collection
                        Console.WriteLine("New strange name bus detected: " + name);
                    }
                    if (busNum == Convert.ToInt32(content.id))
                    {
                        content.id = busNum.ToString();
                        content.goDirection = new ObservableCollection<LocationPointWithId>();
                        content.backDirection = new ObservableCollection<LocationPointWithId>();
                        if (bus.directionRouteCollection[0].isGo)
                        {
                            //Correct go direction
                            int i = 1;
                            foreach (GPSPoint station in bus.directionRouteCollection[0].routePoints)
                            {
                                LocationPointWithId newStation = new LocationPointWithId(station);
                                newStation.id = "[" + i + "]";
                                i++;
                                content.goDirection.Add(newStation);
                            }
                            i = 1;
                            foreach (GPSPoint station in bus.directionRouteCollection[1].routePoints)
                            {
                                LocationPointWithId newStation = new LocationPointWithId(station);
                                newStation.id = "[" + i + "]";
                                i++;
                                content.backDirection.Add(newStation);
                            }
                        }
                        else
                        {
                            int i = 1;
                            foreach (GPSPoint station in bus.directionRouteCollection[0].routePoints)
                            {
                                LocationPointWithId newStation = new LocationPointWithId(station);
                                newStation.id = "[" + i + "]";
                                i++;
                                content.backDirection.Add(newStation);
                            }
                            i = 1;
                            foreach (GPSPoint station in bus.directionRouteCollection[1].routePoints)
                            {
                                LocationPointWithId newStation = new LocationPointWithId(station);
                                newStation.id = "[" + i + "]";
                                i++;
                                content.goDirection.Add(newStation);
                            }
                        }
                        isOldData = true;
                        Console.WriteLine("Old bus: " + content.id);
                        break;
                    }
                }

                if (!isOldData)
                {
                    BusRoute newStationCollection = new BusRoute();
                    newStationCollection.id = bus.name.Split(']')[0].TrimStart('[');
                    newStationCollection.goDirection = new ObservableCollection<LocationPointWithId>();
                    newStationCollection.backDirection = new ObservableCollection<LocationPointWithId>();
                    if (bus.directionRouteCollection.Count == 0)
                    {
                        continue;
                    }
                    if (bus.directionRouteCollection[0].isGo)
                    {
                        //Correct go direction
                        int i = 1;
                        foreach (GPSPoint station in bus.directionRouteCollection[0].routePoints)
                        {
                            LocationPointWithId newStation = new LocationPointWithId(station);
                            newStation.id = "[" + i + "]";
                            i++;
                            newStationCollection.goDirection.Add(newStation);
                        }
                        i = 1;
                        foreach (GPSPoint station in bus.directionRouteCollection[1].routePoints)
                        {
                            LocationPointWithId newStation = new LocationPointWithId(station);
                            newStation.id = "[" + i + "]";
                            i++;
                            newStationCollection.backDirection.Add(newStation);
                        }
                    }
                    else
                    {
                        int i = 1;
                        foreach (GPSPoint station in bus.directionRouteCollection[0].routePoints)
                        {
                            LocationPointWithId newStation = new LocationPointWithId(station);
                            newStation.id = "[" + i + "]";
                            i++;
                            newStationCollection.backDirection.Add(newStation);
                        }
                        i = 1;
                        foreach (GPSPoint station in bus.directionRouteCollection[1].routePoints)
                        {
                            LocationPointWithId newStation = new LocationPointWithId(station);
                            newStation.id = "[" + i + "]";
                            i++;
                            newStationCollection.goDirection.Add(newStation);
                        }
                    }

                    Console.WriteLine("Strange bus added to updated data: " + bus.name);

                }
            }

            //Write Data
            Object2Xml(_busRoute, "FullBusRoute");

            //Update StationList.xml
            _stationTotal = new ObservableCollection<StationTotal>();
            int StationListIndex = 0;
            UtmConverter converter;
            foreach (GeneralStation station in generalStationList.generalStationCollection)
            {
                StationTotal newStation = new StationTotal();
                newStation.id = StationListIndex.ToString();
                StationListIndex++;
                converter = new UtmConverter();
                converter.ToUTM(Convert.ToDouble(station.lat), Convert.ToDouble(station.lon));
                //newStation.latitude = converter.X;
                //newStation.longitude = converter.Y;
                newStation.latitude = Convert.ToDouble(station.lon);
                newStation.longitude = Convert.ToDouble(station.lat);
                newStation.stationId = station.stationId.ToString();
                newStation.addressNum = station.name;
                newStation.addressStreet = station.address;
                newStation.addressDistrict = station.address.Split(',').LastOrDefault().Trim();

                newStation.busList = new ObservableCollection<Bus>();
                foreach (ThroughStationBus throughStationBus in station.throughStationBusCollection)
                {
                    Bus newBus = new Bus();
                    newBus.busNumber = throughStationBus.name.Split(']')[0].TrimStart('[');
                    if (newBus.busNumber.Contains("-"))
                    {
                        break;
                    }
                    else
                    {
                        newBus.busNumber = Convert.ToInt32(newBus.busNumber).ToString();
                    }
                    newBus.name = throughStationBus.name;
                    newBus.busDirection = new ObservableCollection<Direction>();
                    if (throughStationBus.direction == DirectionType.Go)
                    {
                        Direction newD = new Direction();
                        newD.direction = 0.ToString();
                        newBus.busDirection.Add(newD);
                        Console.WriteLine("Go Direction added: " + newBus.busNumber);
                    }
                    if (throughStationBus.direction == DirectionType.Back)
                    {
                        Direction newD = new Direction();
                        newD.direction = 1.ToString();
                        newBus.busDirection.Add(newD);
                        Console.WriteLine("Back Direction added: " + newBus.busNumber);
                    }
                    if (throughStationBus.direction == DirectionType.Both)
                    {
                        Direction newD = new Direction();
                        newD.direction = 0.ToString();
                        newBus.busDirection.Add(newD);

                        Direction newD2 = new Direction();
                        newD2.direction = 1.ToString();
                        newBus.busDirection.Add(newD2);

                        Console.WriteLine("Both Direction added: " + newBus.busNumber);
                    }

                    newStation.busList.Add(newBus);
                    Console.WriteLine("New bus added: " + newBus.busNumber);
                }

                _stationTotal.Add(newStation);
                Console.WriteLine("Station added: " + newStation.stationId);
            }

            //Write Data
            Object2Xml(_stationTotal, "StationList");
        }
    }
}