using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;
using VietnamBusInfo.Model;
using VietnamBusInfo.ViewModel;

namespace VietnamBusInfo.Utilities
{
    public class StaticMethod
    {
        public static async Task<T> Xml2Object<T>(string fileName)
        {
            try
            {
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///Data/" + fileName));
                using (Stream x = await file.OpenStreamForReadAsync())
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    object data = (T)serializer.Deserialize(x);
                    return (T)data;
                }
            }
            catch
            {
                //add some code here
            }
            return default(T);
        }

        public static async Task LoadDataTask()
        {
            StaticData.newBusNameList = await Xml2Object<BusNameList>("BusNameList.xml");
        }
    }
}
