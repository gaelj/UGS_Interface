using System;
using System.Windows.Data;
using UGS.Models;
using UGS.ViewModels;

namespace UGS.Converters
{
    public class CommandToShapeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch ((Cm)value)
            {
                case Cm.IC_CM_FR:
                    return UGS.ViewModels.UGSViewModel.CommandIcons[0];
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                var v = (Cl)value;
                return v;
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            } 
        }
    }
}
