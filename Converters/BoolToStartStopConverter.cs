using System;
using System.Globalization;
using System.Windows.Data;

namespace Manager.Converters
{
    public class BoolToStartStopConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Console.WriteLine($"BoolToStartStopConverter.Convert: value={value}");
            if (value is bool boolValue)
            {
                return boolValue ? "Stop" : "Start";
            }
            return "Start";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Console.WriteLine($"BoolToStartStopConverter.ConvertBack: value={value}");
            if (value is string stringValue)
            {
                return stringValue == "Stop";
            }
            throw new NotSupportedException("ConvertBack is not supported.");
        }
    }
}
