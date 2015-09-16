using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using VnBusInfoW10.Model.GoogleMapApi;

namespace VnBusInfoW10.Utilities
{
    public class GoogleMapApi
    {
        public static async Task<GeoCodingResults> GetGeoPosition(string address)
        {
            string api = "http://maps.googleapis.com/maps/api/geocode/json?address=";

            string j = await NetwordMethod.GetHttpAsString(api + address);
            JObject jObject = JObject.Parse(j);

            GeoCodingResults g = jObject.ToObject<GeoCodingResults>();

            return g;
        }
    }
}
