using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace UVSim
{
    /// <summary>
    /// Handles the conversion of hexcolor <see cref="string"/>s to <see cref="Color"/>
    /// </summary>
    public class ColorStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string colorString)
            {
                try
                {
                    return (Color)ColorConverter.ConvertFromString(colorString);
                }
                catch
                {
                    return Colors.White;
                }
            }
            return Colors.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Color color)
            {
                return ColorToHex(color);
            }
            return "#FFFFFF";
        }

        private string ColorToHex(Color color)
        {
            // Convert Color to Hex string
            return $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
        }
    }
}
