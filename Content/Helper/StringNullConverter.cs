using System.Globalization;

namespace wwrc_maui.Content.Helper
{
    public class StringNullConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var text = " - ";
            if (value != null) text = value.ToString();
            if (!string.IsNullOrEmpty(text))
            {
                if (text.Contains("NULL"))
                    text = text.ToString().Replace("NULL", " - ");
            }
            return text;
        }
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
