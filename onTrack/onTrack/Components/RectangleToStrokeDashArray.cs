using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;

namespace onTrack.Components
{
    public class RectangleToStrokeDashArray : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double current = (double)value;
            double max = 251;
            DoubleCollection doubles = new DoubleCollection();
            doubles.Add(current);
            doubles.Add(max);
            return doubles;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((DoubleCollection)value).ToArray()[0];
        }
    }
}