using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace VnBusInfoW10.Utilities.Converters
{
    public class IndexToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ((int) value == -1)
            {
                return parameter == null ? Visibility.Collapsed : Visibility.Visible;
            }
            return parameter == null ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
