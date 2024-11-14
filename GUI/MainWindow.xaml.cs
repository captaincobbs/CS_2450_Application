using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace UVSim
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Methods
        /// <summary>
        /// Opens a new modal ThemeWindow and disables the MainWindow until it is closed
        /// </summary>
        private void OpenThemeWindow()
        {
            ThemeWindow themeWindow = new()
            {
                Owner = this,
                ShowInTaskbar = false,
            };
            themeWindow.ShowDialog();
        }
        #endregion

        #region Events
        private void ButtonNew_Click(object sender, RoutedEventArgs e)
        {
            InputWindow inputWindow = new()
            {
                Owner = this,
                ShowInTaskbar = false,
            };

            if (inputWindow.ShowDialog() == true)
            {
                SimulationWindow simWindow = new((int)inputWindow.Capacity, (int)inputWindow.InitialLines)
                {
                    Owner = this,
                    ShowInTaskbar = false,
                };
                simWindow.Show();
            }
        }

        private void ButtonOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new()
            {
                Title = "Open UVSim File",
                Multiselect = false, // Single file selection
                Filter = "UVSim Files (*.uvsim)|*.uvsim|Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                InitialDirectory = Directory.GetCurrentDirectory() // Set the initial directory to the program's current directory
            };

            // Show the dialog and get the result
            bool? result = dialog.ShowDialog();

            if (result == true && !string.IsNullOrEmpty(dialog.FileName))
            {
                SimulationWindow simWindow = new()
                {
                    Owner = this,
                    ShowInTaskbar = false,
                };

                string filePath = dialog.FileName;
                if (!simWindow.VirtualMachine.MainMemory.ReadFile(0, filePath))
                {
                    simWindow.ResetMemory();
                    MessageBox.Show("Invalid file!");
                }
                else
                {
                    simWindow.TextLocations.Text = $"Locations ({simWindow.VirtualMachine.MainMemory.Locations.Count})";
                    simWindow.ChangeProgramType(simWindow.VirtualMachine.MainMemory.ProgramType);
                    simWindow.Show();
                }
            }
        }

        private void ButtonTheme_Click(object sender, RoutedEventArgs e)
        {
            OpenThemeWindow();
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        #endregion
    }
}
