using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace UVSim
{
    // If you add non-hexcolor string properties to this class, the program will not launch. I will not fix this.
    /// <summary>
    /// Stores hexcolor data for user theme
    /// </summary>
    public class Theme
    {
        private static readonly Regex rgbPattern = new(@"^#([0-9A-Fa-f]{3}|[0-9A-Fa-f]{6})$", RegexOptions.Compiled);
        private static readonly Regex rgbaPattern = new(@"^#([0-9A-Fa-f]{4}|[0-9A-Fa-f]{8})$", RegexOptions.Compiled);

        private string background = "#4C721D";
        [DefaultValue("#4C721D")]
        [JsonProperty(PropertyName = "BACKGROUND")]
        public string Background
        {
            get => background;
            set { background = value; OnPropertyChanged(nameof(Background)); }
        }

        private string foreground = "#FFFFFF";
        [DefaultValue("#FFFFFF")]
        [JsonProperty(PropertyName = "FOREGROUND")]
        public string Foreground
        {
            get => foreground;
            set
            {
                foreground = value;
                OnPropertyChanged(nameof(Foreground));
            }
        }

        private string border = "#000000";
        [DefaultValue("#000000")]
        [JsonProperty(PropertyName = "BORDER")]
        public string Border
        {
            get => border;
            set
            {
                border = value;
                OnPropertyChanged(nameof(Border));
            }
        }

        private string header = "#808080";
        [DefaultValue("#808080")]
        [JsonProperty(PropertyName = "HEADER")]
        public string Header
        {
            get => header;
            set
            {
                header = value;
                OnPropertyChanged(nameof(Header));
            }
        }

        private string textBoxBackground = "#39FFFFFF";
        [DefaultValue("#39FFFFFF")]
        [JsonProperty(PropertyName = "TEXTBOX_BACKGROUND")]
        public string TextBoxBackground
        {
            get => textBoxBackground;
            set
            {
                textBoxBackground = value;
                OnPropertyChanged(nameof(TextBoxBackground));
            }
        }

        private string textBoxBorder = "#3FABADB3";
        [DefaultValue("#3FABADB3")]
        [JsonProperty(PropertyName = "TEXTBOX_BORDER")]
        public string TextBoxBorder
        {
            get => textBoxBorder;
            set
            {
                textBoxBorder = value;
                OnPropertyChanged(nameof(TextBoxBorder));
            }
        }

        private string buttonBackground = "#39FFFFFF";
        [DefaultValue("#39FFFFFF")]
        [JsonProperty(PropertyName = "BUTTON_BACKGROUND")]
        public string ButtonBackground
        {
            get => buttonBackground;
            set
            {
                buttonBackground = value;
                OnPropertyChanged(nameof(ButtonBackground));
            }
        }

        private string buttonBorder = "#3FABADB3";
        [DefaultValue("#3FABADB3")]
        [JsonProperty(PropertyName = "BUTTON_BORDER")]
        public string ButtonBorder
        {
            get => buttonBorder;
            set
            {
                buttonBorder = value;
                OnPropertyChanged(nameof(ButtonBorder));
            }
        }

        private string codeSpace = "#39FFFFFF";
        [DefaultValue("#39FFFFFF")]
        [JsonProperty(PropertyName = "CODESPACE")]
        public string CodeSpace
        {
            get => codeSpace;
            set
            {
                codeSpace = value;
                OnPropertyChanged(nameof(CodeSpace));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Iterates through each property, determining whether they are valid RGB or RGBA hexcolors of either full or half length
        /// </summary>
        /// <returns>Whether all properties are valid hexcolors or not</returns>
        public bool ValidateHexcolors()
        {
            var properties = typeof(Theme).GetProperties();
            foreach (var property in properties)
            {
                var value = property.GetValue(this)?.ToString();

                if (string.IsNullOrEmpty(value))
                {
                    Console.WriteLine($"Invalid property value: {property.Name} is null or empty");
                    return false;
                }

                if (value.Length == 5 || value.Length == 9) // 6 or 8 character RGBA
                {
                    if (!rgbaPattern.IsMatch(value))
                    {
                        Console.WriteLine($"Invalid RGB color: {property.Name} - '{value}'");
                        return false;
                    }
                }

                else if (value.Length == 4 || value.Length == 7) // 3 or 4 character RGB
                {
                    if (!rgbPattern.IsMatch(value))
                    {
                        Console.WriteLine($"Invalid RGB color: {property.Name} - '{value}'");
                        return false;
                    }
                }

                else
                {
                    Console.WriteLine($"Invalid color: {property.Name} - '{value}'");
                    return false;
                }
            }

            return true;
        }
    }
}
