using GUI;
using Newtonsoft.Json;
using System.IO;
using System.Windows;

namespace UVSim
{
    public partial class App : Application
    {
        private static Theme? theme;
        public static Theme? Theme { get => theme ?? Theme.Default; private set => theme = value; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            LoadColorScheme();
        }

        private static void LoadColorScheme()
        {
            // Get absolute location of the file
            string fileName = "settings.json";
            string settingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            try
            {
                // If the file exists, load it
                if (File.Exists(settingsPath))
                {
                    // Load Theme into nullable object
                    string json = File.ReadAllText(settingsPath);
                    Theme? loadedTheme = JsonConvert.DeserializeObject<Theme>(json);

                    // If object is null, then just use a default theme
                    if (loadedTheme == null)
                    {
                        Theme = Theme.Default;
                    }
                    // If not, then use the loaded theme
                    else
                    {
                        Theme = loadedTheme;
                    }
                }
                // If the file doesn't exist, then just use the default theme
                else
                {
                    Theme = Theme.Default;
                    File.WriteAllText(settingsPath, JsonConvert.SerializeObject(Theme));
                }
            }
            // If something goes wrong when loading, then just use the default theme
            catch
            {
                Theme = Theme.Default;
                File.WriteAllText(settingsPath, JsonConvert.SerializeObject(Theme));
            }
        }
    }
}
