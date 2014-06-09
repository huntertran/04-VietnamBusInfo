using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VietnamBusInfo.Model;
using Windows.Storage;

namespace VietnamBusInfo.Utilities
{
    public class StaticMethod
    {
        public static double Distance(LocationPointWithId a, LocationPointWithId b)
        {

            double e = (3.1415926538*a.latitude/180);
            double f = (3.1415926538*a.longitude/180);
            double g = (3.1415926538*b.latitude/180);
            double h = (3.1415926538*b.longitude/180);
            double i = (Math.Cos(e)*Math.Cos(g)*Math.Cos(f)*Math.Cos(h) +
                        Math.Cos(e)*Math.Sin(f)*Math.Cos(g)*Math.Sin(h) + Math.Sin(e)*Math.Sin(g));
            double j = (Math.Acos(i));
            double k = (6371*j);

            return k;
        }

        public static void SetSettings(string settingName, string settingValue)
        {
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey(settingName))
            {
                //Setting Existed
                //Remove it
                ApplicationData.Current.LocalSettings.Values.Remove(settingName);
            }

            //Re-set setting
            ApplicationData.Current.LocalSettings.Values.Add(settingName, settingValue);
        }

        public static string GetSettings(string settingName)
        {
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey(settingName))
            {
                return ApplicationData.Current.LocalSettings.Values[settingName].ToString();
            }
            else
                return "not set";
        }

        public static async Task<XDocument> GetHttp(string uriString)
        {
            XDocument xmlDoc;
            string result;

            Uri targetUri = new Uri(uriString);

            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, targetUri);
            HttpResponseMessage response = await client.SendAsync(request);

            using (Stream responseStream = await response.Content.ReadAsStreamAsync())
            {
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                result = reader.ReadToEnd();
                responseStream.Position = 0;
                xmlDoc = XDocument.Load(responseStream);
            }

            return xmlDoc;
        }
    }

    public static class Extensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> col)
        {
            return new ObservableCollection<T>(col);
        }

        public static ObservableCollection<LocationPointWithId> Convert(IEnumerable original)
        {
            return new ObservableCollection<LocationPointWithId>(original.Cast<LocationPointWithId>());
        }
    }
}
