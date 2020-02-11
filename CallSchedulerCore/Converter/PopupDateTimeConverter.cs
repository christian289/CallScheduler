using System;
using System.Windows.Data;

namespace CallSchedulerCore.Converter
{
    public class PopupDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DateTime target = (DateTime)value;

            return target.ToString("yyyy년MM월dd일\r\nHH시mm분");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
