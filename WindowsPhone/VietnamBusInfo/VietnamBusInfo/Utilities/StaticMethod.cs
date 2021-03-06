﻿using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using VietnamBusInfo.Model;
using VietnamBusInfo.ViewModel;
using Windows.Storage;

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
            StaticData.codedBusNameList = await Xml2Object<CodedBusNameList>("CodedBusNameList.xml");
            StaticData.generalStationList = await Xml2Object<GeneralStationList>("GeneralStationList.xml");
        }
    }
}
