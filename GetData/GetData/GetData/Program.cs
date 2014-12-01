using GetData.Model;
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

        public static void Main(string[] args)
        {
            //For unicode output
            Console.OutputEncoding = System.Text.Encoding.Unicode;

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
        }
    }
}