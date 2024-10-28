using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;

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

    ///<summary>
    /// allows user to import/load a file into the codespace.
    /// </summary>
        private void OnLoad_Click(object sender, RoutedEventArgs e)
        {
            LoadFile();
        }

    /// <summary>
    /// Allows user to save their code into a file.
    /// </summary>
        private void OnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFile();
        }

    /// <summary>
    /// Lets the user execute their code and displays it onto the output section.
    /// </summary>
        private void OnExecute_Click(object sender, RoutedEventArgs e)
        {
            VirtualMachine.Execute();
        }

    /// <summary>
    /// Allows user to halt the program while it is running
    /// </summary>
        private void OnHalt_Click(object sender, RoutedEventArgs e)
        {
            VirtualMachine.Halt();
        }

    /// <summary>
    /// Allows user to step into code to handle manual debugging and testing
    /// </summary>
        private void OnStep_Click(object sender, RoutedEventArgs e)
        {
            VirtualMachine.Step();
        }

    /// <summary>
    /// Handles exception of user input
    /// </summary>
        private void OnData_TextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }

    /// <summary>
    /// Allows user to handle debugging in a code editior
    /// </summary>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // If running in a code editor, enable debug tools
            if (Debugger.IsAttached)
            {
                switch (e.Key)
                {
                    case Key.Escape:
                        Application.Current.Shutdown();
                        break;
                    case Key.F1:
                        MainGrid.ShowGridLines = !MainGrid.ShowGridLines;
                        break;
                    case Key.F2:
                        Console.WriteLine("Use this as a manual breakpoint");
                        break;
                    case Key.F3:
                        VirtualMachine = new();
                        ListboxCodeSpace.ItemsSource = VirtualMachine.MainMemory.Locations;

                        // Subscribing changes to the Accumulator's data to the TextBlock
                        VirtualMachine.CPU.Accumulator.OnPropertyChanged += UpdateAccumulator;
                        VirtualMachine.CPU.Accumulator.Data = 0;
                        UpdateAccumulator(VirtualMachine.CPU.Accumulator.Data);
                        break;
                    case Key.F4:
                        LoadFile();
                        break;
                    case Key.F5:
                        SaveFile();
                        break;
                    case Key.F10:
                        VirtualMachine.Execute();
                        break;
                    case Key.F11:
                        VirtualMachine.Step();
                        break;
                    case Key.F12:
                        VirtualMachine.Halt();
                        break;
                }
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

        private void LoadFile()
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

        private void SaveFile()
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
        #endregion
    }
}
