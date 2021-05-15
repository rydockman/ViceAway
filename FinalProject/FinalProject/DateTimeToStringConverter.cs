using System;
using System.Globalization;
using Xamarin.Forms;

namespace FinalProject
{
    public class DateTimeToStringConverter : IValueConverter
    {
        public DateTimeToStringConverter()
        {

        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            return ((DateTime)value).ToShortDateString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
