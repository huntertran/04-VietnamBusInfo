using System;
using Windows.ApplicationModel.Resources.Core;
using Windows.UI.Xaml.Data;
using VnBusInfoW10.View.StartGroup;

namespace VnBusInfoW10.Utilities.Converters
{
    public class PageToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string defaultName = ResourceManager.Current.MainResourceMap.GetValue("Resources/App/Name", new ResourceContext()).ValueAsString;

            if (value is StartPage)
            {
                return defaultName;
            }
            
            return defaultName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
