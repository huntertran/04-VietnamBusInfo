using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
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

            string output = JsonConvert.SerializeObject(data);

            //using (Stream x = await file.OpenStreamForWriteAsync())
            //{
            //    using (StreamWriter sw = new StreamWriter(x))
            //    {
            //        using (JsonWriter writer = new JsonTextWriter(sw))
            //        {
            //            serializer.Serialize(writer, data);
            //        }
            //    }
            //}

            byte[] writeData = EncryptionService.Encrypt(output, "duyendo178", "salt");
            await FileIO.WriteBytesAsync(file, writeData);
        }

        public static async Task<T> Json2Object<T>(string fileName, StorageFolder folder = null)
        {
            try
            {
                if (folder == null)
                {
                    folder =
                        await
                            ApplicationData.Current.LocalFolder.CreateFolderAsync("data",
                                CreationCollisionOption.OpenIfExists);
                }


                StorageFile file = await folder.GetFileAsync(fileName);
                string json;

                using (IRandomAccessStreamWithContentType stream = await file.OpenReadAsync())
                {
                    var fileBytes = new byte[stream.Size];
                    using (DataReader dataReader = new DataReader(stream))
                    {
                        await dataReader.LoadAsync((uint) stream.Size);
                        dataReader.ReadBytes(fileBytes);
                        json = EncryptionService.Decrypt(fileBytes, "duyendo178", "salt");
                    }
                }

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
            catch (Exception)
            {
                // ignored
            }
            return default(T);
        }
    }
}