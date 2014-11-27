using GetData.Model;
using HtmlAgilityPack;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Text;

namespace GetData
{
    public class Program
    {
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

        public static void Main(string[] args)
        {
            //For unicode output
            Console.OutputEncoding = System.Text.Encoding.Unicode;

            #region Get List of buses

            string lobHtml = GetHttpAsString("http://www.buyttphcm.com.vn/TTLT.aspx");

            HtmlDocument lobDocument = new HtmlDocument();
            lobDocument.LoadHtml(lobHtml);

            BusNameList newBusNameList = new BusNameList();
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
        }
    }
}