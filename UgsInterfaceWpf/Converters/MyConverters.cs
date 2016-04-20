using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UGS.Models;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Media;
using System.Windows.Markup;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows;

namespace UGS.Converters
{


    [ValueConversion(typeof(int), typeof(string))]
    public class AnimationNameToIdConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Do the conversion from int to string
            switch (int.Parse(value.ToString()))
            {
                case 0: return "None";
                case 1: return "Volume";
                case 2: return "All Blinking";
                case 3: return "All On";
                case 4: return "VU Meter";
                case 5: return "Chase Clockwise";
                case 6: return "Chase Counter-clockwise";

                case 7: return "Active inputs";
                // case "All Blinking": return 2;
                // case "All On": return 3;
                case 8: return "Chase Parallel";
                case 9: return "Chase Serial";
                default: return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Do the conversion from string to int
            if (value == null)
                return 0;

            switch (value.ToString())
            {
                case "None": return 0;
                case "Volume": return 1;
                case "All Blinking": return 2;
                case "All On": return 3;
                case "VU Meter": return 4;
                case "Chase Clockwise": return 5;
                case "Chase Counter-clockwise": return 6;

                case "Active inputs": return 7;
                // case "All Blinking": return 2;
                // case "All On": return 3;
                case "Chase Parallel": return 8;
                case "Chase Serial": return 9;
                default: return 0;
            }
        }
    }

    [ValueConversion(typeof(int), typeof(Color))]
    public class RGB565ToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Do the conversion from int to Color

            float blue = ((int)value & 0x1F);            // 5
            float green = (((int)value >> 5) & 0x3F);    // 6
            float red = (((int)value >> 11) & 0x1F);     // 5

            blue = 255 * blue / 31;
            green = 255 * green / 63;
            red = 255 * red / 31;

            return Color.FromRgb((byte)red, (byte)green, (byte)blue);

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Do the conversion from Color to int

            int blue = (((Color)value).B);     // 5
            int green = (((Color)value).G);    // 6
            int red = (((Color)value).R);      // 5

            int b = ((blue >> 3) & 0x1F);
            int g = ((green >> 2) & 0x3F) << 5;
            int r = ((red >> 3) & 0x1F) << 11;

            return (r + g + b);

        }

    }

    [ValueConversion(typeof(int), typeof(SolidColorBrush))]
    public class RGB565ToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            // Do the conversion from int to Color

            float blue = ((int)value & 0x1F);            // 5
            float green = (((int)value >> 5) & 0x3F);    // 6
            float red = (((int)value >> 11) & 0x1F);     // 5

            blue = 255 * blue / 31;
            green = 255 * green / 63;
            red = 255 * red / 31;

            return new SolidColorBrush(Color.FromRgb((byte)red, (byte)green, (byte)blue));

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            // Do the conversion from Color to int

            int blue = (((SolidColorBrush)value).Color.B);     // 5
            int green = (((SolidColorBrush)value).Color.G);    // 6
            int red = (((SolidColorBrush)value).Color.R);      // 5

            int b = ((blue >> 3) & 0x1F);
            int g = ((green >> 2) & 0x3F) << 5;
            int r = ((red >> 3) & 0x1F) << 11;

            return (r + g + b);
        }

    }

    [ValueConversion(typeof(SolidColorBrush), typeof(Color))]
    public class BrushToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Do the conversion from SolidColorBrush to Color

            return ((SolidColorBrush)value).Color;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Do the conversion from Color to SolidColorBrush

            return new SolidColorBrush((Color)value);
        }

    }

    [ValueConversion(typeof(Image), typeof(Brush))]
    public class ImageToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Do the conversion from Image to Brush
            if (value == null) return null;
            return new ImageBrush((BitmapImage)value);

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Do the conversion from Brush to Image
            throw new NotImplementedException();
        }

    }

    [ValueConversion(typeof(int), typeof(string))]
    public class VolumeToDbConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return UGS.Models.UGS.ConvertAttenuationToDb(int.Parse(value.ToString()));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Do the conversion from Brush to Image
            throw new NotImplementedException();
        }

    }

    [ValueConversion(typeof(int), typeof(string))]
    public class BalanceToDbConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return UGS.Models.UGS.ConvertValueToDb(int.Parse(value.ToString()));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Do the conversion from Brush to Image
            throw new NotImplementedException();
        }

    }


    #region binding the icon objects to the corresponding image according to the icon object's "content" value
    [ValueConversion(typeof(Ic), typeof(BitmapImage))]
    class ValueToBitmapResourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((value ?? "").ToString() == "") return null;
            if (Icon.Icons == null) return null;
            Ic ic2 = (Ic)value;
            try
            {
                if (!Icon.Icons.ContainsKey(ic2)) return null;
                return Icon.Icons[ic2].Item1.FrozenBitmap;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null; // Icon.Icons.Where(b => b.Value.BitmapFromFile == (BitmapImage)value).Select(x => x.Key);
        }
    }

    [ValueConversion(typeof(Ic), typeof(BitmapImage))]
    class ValueToSelectedBitmapResourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((value ?? "").ToString() == "") return null;
            if (Icon.Icons == null) return null;
            Ic ic2 = (Ic)value;
            try
            {
                if (!Icon.Icons.ContainsKey(ic2)) return null;
                return Icon.Icons[ic2].Item2.FrozenBitmap;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null; // Icon.Icons.Where(b => b.Value.BitmapFromFile == (BitmapImage)value).Select(x => x.Key);
        }
    }


    public static class ResourceKeyBindings
    {
        public static DependencyProperty SourceResourceKeyBindingProperty = ResourceKeyBindingPropertyFactory.CreateResourceKeyBindingProperty(
            Image.SourceProperty,
            typeof(ResourceKeyBindings));

        public static void SetSourceResourceKeyBinding(DependencyObject dp, object resourceKey)
        {
            dp.SetValue(SourceResourceKeyBindingProperty, resourceKey);
        }

        public static object GetSourceResourceKeyBinding(DependencyObject dp)
        {
            return dp.GetValue(SourceResourceKeyBindingProperty);
        }
    }
    public static class ResourceKeyBindingPropertyFactory
    {
        public static DependencyProperty CreateResourceKeyBindingProperty(DependencyProperty boundProperty, Type ownerClass)
        {
            var property = DependencyProperty.RegisterAttached(
                boundProperty.Name + "ResourceKeyBinding",
                typeof(object),
                ownerClass,
                new PropertyMetadata(null, (dp, e) =>
                {
                    var element = dp as FrameworkElement;
                    if (element == null)
                    {
                        return;
                    }

                    element.SetResourceReference(boundProperty, e.NewValue);
                }));

            return property;
        }
    }

    #endregion

    public class IconToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return null;
            return (int)(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return null;
            return (Ic)value;
        }
    }
    public class IsParameterEqualToValue : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return null;
            try
            {
                var ret = (int)(((Icon)value).IconId) == (int)(parameter ?? 0);
                return ret;
            }
            catch (Exception)
            {
                try
                {
                    var ret = (int)value == (int)(parameter ?? 0);
                    return ret;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value) return parameter;
            else return null;
        }
    }

    public class IsIconEqualToIconId : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                var ret = (int)(((Icon)value).IconId) == (int)(parameter ?? 0);
                return ret;
            }
            catch (Exception)
            {
                try
                {
                    var ret = (int)value == (int)(parameter ?? 0);
                    return ret;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value) return Icon.Icons[(Ic)parameter];
            else return null;
        }
    }


    [ValueConversion(typeof(int), typeof(int))]
    public class AnimationDelayToMs : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 25 * int.Parse(value.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var intValue = int.Parse(value.ToString());
            var nominalValue = Math.Round(((float)intValue) / 25) * 25;
            var delta = intValue - nominalValue;

            return (int)Math.Round(nominalValue / 25.0) + delta;
        }

    }


    [ContentProperty("Converter")]
    public class AreValuesEqual : IMultiValueConverter
    {
        public IValueConverter Converter { get; set; }

        private object lastParameter;

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (Converter == null) return values[0]; // Required for VS design-time
            if (values.Length > 1) lastParameter = values[1];
            return Converter.Convert(values[0], targetType, lastParameter, culture);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if (Converter == null) return new object[] { value }; // Required for VS design-time
            return new object[] { Converter.ConvertBack(value, targetTypes[0], lastParameter, culture) };
        }
    }

    public class ConverterBindableBinding : MarkupExtension
    {
        public Binding Binding { get; set; }
        public IValueConverter Converter { get; set; }
        public Binding ConverterParameterBinding { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            MultiBinding multiBinding = new MultiBinding();
            multiBinding.Bindings.Add(Binding);
            multiBinding.Bindings.Add(ConverterParameterBinding);
            AreValuesEqual adapter = new AreValuesEqual();
            adapter.Converter = Converter;
            multiBinding.Converter = adapter;
            return multiBinding.ProvideValue(serviceProvider);
        }
    }

}
