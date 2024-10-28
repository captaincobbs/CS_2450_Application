using Newtonsoft.Json;
using System.IO;
using System.Windows;

namespace UVSim
{
    public partial class App : Application
    {
        private static Theme? theme;
        public static Theme Theme { get => theme ?? new(); private set => theme = value; }

        private static string SettingsPath = string.Empty;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            LoadColorScheme();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            SaveColorScheme();
        }

        /// <summary>
        /// Loads the colors from the Theme.cs to help paint the styles for the GUI
        /// </summary>
        public static void LoadColorScheme()
        {
            // Get absolute location of the file
            string fileName = "settings.json";
            SettingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

            try
            {
                // If the file exists, load it
                if (File.Exists(SettingsPath))
                {
                    // Load Theme into nullable object
                    string json = File.ReadAllText(SettingsPath);
                    Theme? loadedTheme = JsonConvert.DeserializeObject<Theme>(json);

                    if (loadedTheme?.ValidateHexcolors() ?? false)
                    {
                        // If object is null, then just use a default theme
                        if (loadedTheme == null)
                        {
                            Theme = new();
                        }
                        // If not, then use the loaded theme
                        else
                        {
                            Theme = loadedTheme;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid theme, using default");
                        Theme = new();
                        SaveColorScheme();
                    }
                }
                // If the file doesn't exist, then just use the default theme
                else
                {
                    Console.WriteLine("No theme file found, using default");
                    Theme = new();
                    SaveColorScheme();
                }
            }
            // If something goes wrong when loading, then just use the default theme
            catch
            {
                Console.WriteLine("Something unexpected prevented the theme from being loaded, using default");
                Theme = new();
                SaveColorScheme();
            }
        }

        /// <summary>
        /// Saves current color scheme to predefined settings path
        /// </summary>
        public static void SaveColorScheme()
        {
            if (SettingsPath != string.Empty)
            {
                File.WriteAllText(SettingsPath, JsonConvert.SerializeObject(Theme, Formatting.Indented));
            }
        }
    }
}
