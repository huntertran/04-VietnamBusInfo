using System;
using Windows.UI.Xaml.Data;

namespace VietnamBusInfo.Utilities.Converter
{
    public class BitToDirectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value.ToString() == "1")
            {
                return "Go";
            }
            else
            {
                return "Back";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
