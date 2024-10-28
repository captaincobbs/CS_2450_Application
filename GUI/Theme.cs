using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GUI
{
    /// <summary>
    /// uses JSON to help set the theme and color of the brushes used to stylize the UI
    /// </summary>
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
    }
}
