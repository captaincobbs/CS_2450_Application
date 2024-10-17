﻿using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Windows.Controls;

namespace GUI
{
    public class Theme
    {
        [JsonProperty(PropertyName = "BACKGROUND")]
        public string Background { get; set; } = "#4C721D";

        [JsonProperty(PropertyName = "FOREGROUND")]
        public string Foreground { get; set; } = "#FFFFFF";

        [JsonProperty(PropertyName = "ACCENT")]
        public string Accent { get; set; } = "#D3D3D3";

        [JsonProperty(PropertyName = "BORDER")]
        public string Border { get; set; } = "#000000";

        [JsonProperty(PropertyName = "HEADER")]
        public string Header { get; set; } = "#808080";

        [JsonProperty(PropertyName = "TEXTBOX_BACKGROUND")]
        public string TextBoxBackground { get; set; } = "#39FFFFFF";

        [JsonProperty(PropertyName = "TEXTBOX_BORDER")]
        public string TextBoxBorder { get; set; } = "#3FABADB3";

        [JsonProperty(PropertyName = "BUTTON_BACKGROUND")]
        public string ButtonBackground { get; set; } = "#39FFFFFF";

        [JsonProperty(PropertyName = "BUTTON_BORDER")]
        public string ButtonBorder { get; set; } = "#3FABADB3";

        [JsonProperty(PropertyName = "CODESPACE")]
        public string CodeSpace { get; set; } = "#39FFFFFF";

        public static Theme Default => new()
        {
            Background = "#4C721D",
            Foreground = "#FFFFFF",
            Accent = "#D3D3D3",
            Border = "#000000",
            Header = "#808080",
            TextBoxBackground = "#39FFFFFF",
            TextBoxBorder = "#3FABADB3",
        };
    }
}
