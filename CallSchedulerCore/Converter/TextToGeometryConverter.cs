using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace CallSchedulerCore.Converter
{
    [ValueConversion(typeof(Point[]), typeof(Geometry))]
    public class TextToGeometryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string target = parameter.ToString();

            Geometry obj = new FormattedText
                (
                    target,
                    CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    new Typeface
                    (
                        new FontFamily("맑은 고딕"),
                        FontStyles.Normal,
                        FontWeights.Normal,
                        FontStretches.Normal
                    ),
                    30,
                    Brushes.Black,
                    30
                ).BuildGeometry(new Point(0, 0));

            return obj;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
