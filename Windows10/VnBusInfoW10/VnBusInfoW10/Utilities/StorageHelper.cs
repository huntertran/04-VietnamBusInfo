using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Windows.Storage;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace VnBusInfoW10.Utilities
{
    public class StorageHelper
    {
        public static async Task Object2Xml<T>(T data, string fileName)
        {
            var xmlWriterSettings = new XmlWriterSettings {Indent = true};

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
                    XmlSerializer serializer = new XmlSerializer(typeof (T));
                    object data = (T) serializer.Deserialize(x);
                    return (T) data;
                }
            }
            catch
            {
                //add some code here
            }
            return default(T);
        }

        public static async Task Object2Json<T>(T data, string fileName, StorageFolder folder = null)
        {
            StorageFile file;
            if (folder == null)
            {
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                StorageFolder d = await localFolder.CreateFolderAsync("data", CreationCollisionOption.OpenIfExists);
                file = await d.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            }
            else
            {
                file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            }

            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;

            using (Stream x = await file.OpenStreamForWriteAsync())
            {
                using (StreamWriter sw = new StreamWriter(x))
                {
                    using (JsonWriter writer = new JsonTextWriter(sw))
                    {
                        serializer.Serialize(writer, data);
                    }
                }
            }
        }

        public static async Task<T> Json2Object<T>(string fileName, StorageFolder folder = null)
        {
            try
            {
                if (folder == null)
                {
                    folder = await ApplicationData.Current.LocalFolder.GetFolderAsync("data");
                }


                StorageFile file = await folder.GetFileAsync(fileName);
                using (Stream x = await file.OpenStreamForReadAsync())
                {

                    StreamReader reader = new StreamReader(x);
                    string json = reader.ReadToEnd();
                    try
                    {
                        JObject jObject = JObject.Parse(json);
                        T data = jObject.ToObject<T>();
                        return data;
                    }

                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("not an object"))
                        {
                            JArray jArray = JArray.Parse(json);
                            T data = jArray.ToObject<T>();
                            return data;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // ignored
            }
            return default(T);
        }
    }
}