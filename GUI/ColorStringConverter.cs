using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace UVSim
{
    /// <summary>
    /// Handles the conversion of hexcolor <see cref="string"/>s to <see cref="Color"/> in a MVVM binding
    /// </summary>
    public class ColorStringConverter : IValueConverter
    {
        /// <summary>
        /// Converts an input hexcolor <see cref="string"/> to <see cref="Color"/> 
        /// </summary>
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

        /// <summary>
        /// Converts an input <see cref="Color"/> to hexcolor <see cref="string"/>
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Color color)
            {
                return ColorToHex(color);
            }
            return "#FFFFFF";
        }

        /// <summary>
        /// Converts an input <see cref="Color"/> to rgba hexcolor <see cref="string"/> 
        /// </summary>
        private string ColorToHex(Color color)
        {
            // Convert Color to Hex string
            return $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
        }
    }
}
