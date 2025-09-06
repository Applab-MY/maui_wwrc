using System.Globalization;

namespace wwrc_maui.Content.Helper
{
    public class InverseBoolConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value != null) return !(bool)value;
            else return false;
        }
        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value != null) return !(bool)value;
            else return false;
        }
    }
}
