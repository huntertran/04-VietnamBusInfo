using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace VnBusInfoW10.Utilities
{
    public class NetwordMethod
    {
        public static async Task<string> GetHttpAsString(string uriString)
        {
            string result;

            Uri targetUri = new Uri(uriString);

            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, targetUri);
            HttpResponseMessage response = await client.SendAsync(request);

            using (Stream responseStream = await response.Content.ReadAsStreamAsync())
            {
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                result = reader.ReadToEnd();
            }

            return result;
        }
    }
}
