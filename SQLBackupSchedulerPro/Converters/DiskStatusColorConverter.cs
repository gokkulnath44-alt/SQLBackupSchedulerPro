using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace SQLBackupSchedulerPro.Converters
{
    public class DiskStatusColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Colors.Gray;

            string status = value.ToString().ToLower();

            // Return Color for direct use in SolidColorBrush binding
            if (targetType == typeof(Color))
            {
                return status switch
                {
                    "healthy" => Color.FromRgb(46, 125, 50),      // Green #2E7D32
                    "good" => Color.FromRgb(46, 125, 50),         // Green #2E7D32
                    "warning" => Color.FromRgb(245, 127, 23),     // Orange #F57F17
                    "low space" => Color.FromRgb(245, 127, 23),   // Orange #F57F17
                    "critical" => Color.FromRgb(198, 40, 40),     // Red #C62828
                    "low" => Color.FromRgb(198, 40, 40),          // Red #C62828
                    _ => Color.FromRgb(108, 117, 125)             // Gray #6C757D
                };
            }

            // Return Brush for legacy compatibility
            return status switch
            {
                "healthy" => new SolidColorBrush(Color.FromRgb(46, 125, 50)),
                "good" => new SolidColorBrush(Color.FromRgb(46, 125, 50)),
                "warning" => new SolidColorBrush(Color.FromRgb(245, 127, 23)),
                "low space" => new SolidColorBrush(Color.FromRgb(245, 127, 23)),
                "critical" => new SolidColorBrush(Color.FromRgb(198, 40, 40)),
                "low" => new SolidColorBrush(Color.FromRgb(198, 40, 40)),
                _ => new SolidColorBrush(Color.FromRgb(108, 117, 125))
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
