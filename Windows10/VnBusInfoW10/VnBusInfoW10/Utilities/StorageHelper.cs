using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace VnBusInfoW10.Utilities
{
    public class StorageHelper
    {
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