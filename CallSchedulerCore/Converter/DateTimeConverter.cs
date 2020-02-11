﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace CallSchedulerCore.Converter
{
    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DateTime target = (DateTime)value;

            return target.ToString("yyyy/MM/dd h:mm tt", CultureInfo.CreateSpecificCulture("en-US"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
