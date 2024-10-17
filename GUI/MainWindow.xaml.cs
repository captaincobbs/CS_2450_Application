using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.IO;

namespace UVSim
{
    public partial class MainWindow : Window
    {
        public OperatingSystemGui VirtualMachine { get; set; }
        private string _consoleOutput = string.Empty;
        public string ConsoleOutput { get => _consoleOutput; set
            {
                _consoleOutput = value;
                UpdateOutput(_consoleOutput);
            }
        }
        public ObservableCollection<BasicML> Instructions { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            VirtualMachine = new();
            DataContext = this;
            ListboxCodeSpace.ItemsSource = VirtualMachine.MainMemory.Locations;

            // Subscribing changes to the Accumulator's data to the TextBlock
            VirtualMachine.CPU.Accumulator.OnPropertyChanged += UpdateAccumulator;
            VirtualMachine.CPU.Accumulator.Data = 0;
            UpdateAccumulator(VirtualMachine.CPU.Accumulator.Data);

            // Populate Enum list
            Instructions = new ObservableCollection<BasicML>(Enum.GetValues(typeof(BasicML)).Cast<BasicML>());
        }

        private void OnLoad_Click(object sender, RoutedEventArgs e)
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

            // Check if a file was selected
            if (result == true && !string.IsNullOrEmpty(dialog.FileName))
            {
                string filePath = dialog.FileName;
                // Process the file as needed (e.g., write to memory or load data)
                VirtualMachine.MainMemory.ReadFile(0, filePath);
            }
        }


        private void OnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new()
            {
                Title = "Save UVSim File",
                DefaultExt = ".uvsim",
                Filter = "UVSim Files (*.uvsim)|*.uvsim|Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                InitialDirectory = Directory.GetCurrentDirectory(),
                FileName = "memory.uvsim",
            };

            // Show the dialog and wait for the result
            bool? result = dialog.ShowDialog();

            if (result == true && !string.IsNullOrEmpty(dialog.FileName))
            {
                // data must be a string
                VirtualMachine.MainMemory.SaveFile(dialog.FileName);
            }
        }

        // Handle Run button
        private void OnRun_Click(object sender, RoutedEventArgs e)
        {
            // Run
        }

        private void OnHalt_Click(object sender, RoutedEventArgs e)
        {
            // Halt
        }

        private void OnStep_Click(object sender, RoutedEventArgs e)
        {
            // Step
        }

        private void OnData_TextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // For debugging UI
            if (e.Key == Key.F1)
            {
                MainGrid.ShowGridLines = !MainGrid.ShowGridLines;
            }

            if (e.Key == Key.F2)
            {
                Console.WriteLine("Use this as a manual breakpoint");
            }
        }

        #region Functions
        private void UpdateAccumulator(int data)
        {
            TextAccumulator.Text = $"{data}";
        }

        private void UpdateOutput(string output)
        {
            TextOutput.Text = output;
        }
        #endregion
    }
}
