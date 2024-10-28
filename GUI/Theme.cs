using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.ComponentModel;

namespace UVSim
{
    /// <summary>
    /// Stores hexcolor data for user theme
    /// </summary>
    public class Theme
    {
        private string background = "#4C721D";
        [JsonProperty(PropertyName = "BACKGROUND")]
        public string Background
        {
            get => background;
            set { background = value; OnPropertyChanged(nameof(Background)); }
        }

        private string foreground = "#FFFFFF";
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
    }
}
