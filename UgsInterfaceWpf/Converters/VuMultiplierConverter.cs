using System;
using System.Windows.Data;

namespace UGS.Converters
{
    public class VuMultiplierConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((int)value) * 25;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            float v = (int)value;
            int r;
            Math.DivRem((int)v, 25, out r);
            if (r == 1)
                return 1 + ((int)v) / 25;
            else
                return ((int)v) / 25;
        }
    }
}
