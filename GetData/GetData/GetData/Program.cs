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

        public static void Main(string[] args)
        {

            Console.OutputEncoding = System.Text.Encoding.Unicode;

            //Get list of buses

            string lobHtml = GetHttpAsString("http://www.buyttphcm.com.vn/TTLT.aspx");

            HtmlDocument lobDocument = new HtmlDocument();
            lobDocument.LoadHtml(lobHtml);

            UTF8Encoding utf8 = new UTF8Encoding();

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

                byte[] bytes = Encoding.Default.GetBytes(htmlNode.NextSibling.InnerText);
                string temp = WebUtility.HtmlDecode(Encoding.UTF8.GetString(bytes));

                newBusName.name = temp;

                newBusNameList.busNameCollection.Add(newBusName);
                Console.WriteLine(newBusName.name);
            }
        }
    }
}
