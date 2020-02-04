using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace CallScheduler.Global
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
