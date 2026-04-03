using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace GestionnaireLivresMAUI.Converters
{
    public class BoolToStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool lu)
            {
                return lu ? "Livre lu" : "Livre non lu";
            }

            return "Livre non lu";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string text)
            {
                return text.Equals("Livre lu", StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }
    }
}
