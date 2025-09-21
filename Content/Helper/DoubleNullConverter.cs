using System.Globalization;

namespace wwrc_maui.Content.Helper
{
    public class DoubleNullConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var _no = (double?)value;
            return _no == 0.0 ? false : true;
        }
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var _no = (double?)value;
            return _no == 0.0 ? true : false;
        }
    }
}
