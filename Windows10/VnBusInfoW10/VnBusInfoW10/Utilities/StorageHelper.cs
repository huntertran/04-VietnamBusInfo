using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Windows.Storage;

namespace VnBusInfoW10.Utilities
{
    public class StorageHelper
    {
        public static async Task Object2Xml<T>(T data, string fileName)
        {
            var xmlWriterSettings = new XmlWriterSettings { Indent = true };

            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFolder d = await localFolder.CreateFolderAsync("data", CreationCollisionOption.OpenIfExists);
            StorageFile file = await d.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            using (Stream x = await file.OpenStreamForWriteAsync())
            {
                var serializer = new XmlSerializer(data.GetType());
                using (XmlWriter xmlWriter = XmlWriter.Create(x, xmlWriterSettings))
                {
                    serializer.Serialize(xmlWriter, data);
                }
            }
        }

        public static async Task<T> Xml2Object<T>(string fileName)
        {
            try
            {
                StorageFolder folder = await ApplicationData.Current.LocalFolder.GetFolderAsync("data");
                StorageFile file = await folder.GetFileAsync(fileName);
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
    }
}
