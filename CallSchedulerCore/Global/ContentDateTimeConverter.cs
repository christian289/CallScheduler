using System;
using System.Windows.Data;

namespace CallSchedulerCore.Global
{
    public class ContentDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DateTime target = (DateTime)value;

            return target.ToString("yyyy/MM/dd HH:mm");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
