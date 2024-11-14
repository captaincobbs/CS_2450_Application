using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;

namespace UVSim
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Properties
        public OperatingSystemGui VirtualMachine { get; set; }
        public ObservableCollection<BasicML> Instructions { get; set; }
        private int maxLines = 0;
        public int MaxLines
        {
            get => maxLines; set
            {
                maxLines = value;
                UpdateAccumulator(VirtualMachine.CPU.Accumulator.Data);
            }
        }

        private int sendLocation = -1;

        public bool IsUILocked { get; set; } = false;

        private int inputMaximum = 0;
        public int InputMaximum
        {
            get => inputMaximum;
            set
            {
                inputMaximum = value;
                OnPropertyChanged(nameof(InputMaximum));
            }
        }

        private int inputMinimum = 0;
        public int InputMinimum
        {
            get => inputMinimum;
            set
            {
                inputMinimum = value;
                OnPropertyChanged(nameof(InputMinimum));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            VirtualMachine = new();
            DataContext = this;
            MaxLines = VirtualMachine.MainMemory.Capacity;

            // Populate default
            for (int i = 0; i < MaxLines; i++)
            {
                VirtualMachine.MainMemory.Locations.Add(new MemoryLine()
                {
                    LineNumber = i,
                    Data = 0,
                    Instruction = BasicML.NONE
                });
            }

            ListboxCodeSpace.ItemsSource = VirtualMachine.MainMemory.Locations;
            VirtualMachine.CPU.OnOutput += UpdateOutput;
            VirtualMachine.CPU.AwaitingInput += PrepareForInput;
            VirtualMachine.MainMemory.OnProgramTypeChanged += ChangeProgramType;

            // Subscribing changes to the Accumulator's data to the TextBlock
            VirtualMachine.CPU.Accumulator.OnPropertyChanged += UpdateAccumulator;
            VirtualMachine.CPU.Accumulator.Data = 0;
            MaxLines = VirtualMachine.MainMemory.Capacity;
            UpdateAccumulator(VirtualMachine.CPU.Accumulator.Data);
            TextLocations.Text = $"Locations ({VirtualMachine.MainMemory.Locations.Count})";
            // Populate Enum list
            Instructions = new ObservableCollection<BasicML>(Enum.GetValues(typeof(BasicML)).Cast<BasicML>());
            ChangeProgramType(VirtualMachine.MainMemory.ProgramType);
        }

        #region Functions
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Updates the TextAccumulator with the most recent value from the Accumulator
        /// </summary>
        /// <param name="data">Input value to set TextAccumulator to</param>
        private void UpdateAccumulator(int data)
        {
            TextAccumulator.Text = $"{data}";
            TextMaxMemory.Text = $"{MaxLines}";
        }

        /// <summary>
        /// Updates TextOutput with new texts from the Processor
        /// </summary>
        /// <param name="output">String to append to output console</param>
        private void UpdateOutput(string output)
        {
            if (string.IsNullOrEmpty(TextOutput.Text))
            {
                TextOutput.Text = output;
            }
            else
            {
                TextOutput.Text += $"\n{output}";
            }
        }

        /// <summary>
        /// Opens the load file dialog for the user to select a file to load, does not check for errors
        /// </summary>
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
                if(!VirtualMachine.MainMemory.ReadFile(0, filePath))
                {
                    ResetMemory();
                    MessageBox.Show("Invalid file!");
                }
            }
            TextLocations.Text = $"Locations ({VirtualMachine.MainMemory.Locations.Count})";
        }

        /// <summary>
        /// Opens the save file dialog for the user to define where they want the current loaded memory lines saved
        /// </summary>
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
            RecountLineNumbers();
        }

        /// <summary>
        /// Enables the text input functionality, locks editing other UI until input is provided.
        /// </summary>
        private void PrepareForInput(bool isAwaiting, int location)
        {
            TextBoxInput.IsReadOnly = !isAwaiting;
            ButtonSendInput.IsEnabled = isAwaiting;
            sendLocation = location;

            if (isAwaiting)
            {
                TextBoxInput.Focus();
            }
        }

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

        /// <summary>
        /// Adds a line to the bottom of the codespace, then scrolls to the bottom
        /// </summary>
        private void AddLine()
        {
            // If the Virtual Machine has room for more lines
            if (VirtualMachine.MainMemory.Locations.Count <= MaxLines - 1)
            {
                // Get the last index (If you are adding the 100th item, it will have an index of 99, so nothing needs to be done here)
                int lastIndex = VirtualMachine.MainMemory.Locations.Count;

                // Then create the empty line and add it
                VirtualMachine.MainMemory.Locations.Add(new() { Data = 0, LineNumber = lastIndex, Instruction = BasicML.NONE});
                // Recount the line numbers
                RecountLineNumbers();
                // Then scroll to the last item
                ListboxCodeSpace.ScrollIntoView(ListboxCodeSpace.Items[^1]);
            }
        }

        /// <summary>
        /// Deletes the line currently selected in the CodeSpace, if no item is selected, nothing happens, doesn't check for errors
        /// </summary>
        private void DeleteLine(int location = -1)
        {
            if (location == -1)
            {
                location = ListboxCodeSpace.SelectedIndex;
            }

            // Is an item in the CodeSpace selected?
            if (location != -1)
            {
                // If so, delete the item at the selected index
                VirtualMachine.MainMemory.Locations.RemoveAt(location);
                // Then recount all line numbers
                RecountLineNumbers();
            }
        }

        private void ResetMemory()
        {
            VirtualMachine.MainMemory.Locations.Clear();
            for (int i = 0; i < MaxLines; i++)
            {
                VirtualMachine.MainMemory.Locations.Add(new() { Data = 0, LineNumber = i, Instruction = BasicML.NONE});
            }
            RecountLineNumbers(); // just in case
        }

        /// <summary>
        /// Relabels all memory lines with their appropriate line, used after adding or removing a line
        /// </summary>
        private void RecountLineNumbers()
        {
            for (int i = 0; i < VirtualMachine.MainMemory.Locations.Count; i++)
            {
                VirtualMachine.MainMemory.Locations[i].LineNumber = i;
            }
            TextLocations.Text = $"Locations ({VirtualMachine.MainMemory.Locations.Count})";
        private void ChangeProgramType(ProgramType newType)
        {
            if (newType == ProgramType.FourDigit)
            {
                InputMaximum = 99;
                InputMinimum = -99;
                for (int i = 0; i < VirtualMachine.MainMemory.Locations.Count; i++)
                {
                    VirtualMachine.MainMemory.Locations[i].Maximum = InputMaximum;
                    VirtualMachine.MainMemory.Locations[i].Minimum = InputMinimum;
                    TextData.Text = "Data (±99)";
                }
            }
            else
            {
                InputMaximum = 999;
                InputMinimum = -999;
                for (int i = 0; i < VirtualMachine.MainMemory.Locations.Count; i++)
                {
                    VirtualMachine.MainMemory.Locations[i].Maximum = InputMaximum;
                    VirtualMachine.MainMemory.Locations[i].Minimum = InputMinimum;
                    TextData.Text = "Data (±999)";
                }
            }
        }
        #endregion

        #region Events
        ///<summary>
        /// allows user to import/load a file into the codespace.
        /// </summary>
        private void OnLoad_Click(object sender, RoutedEventArgs e)
        {
            if (!IsUILocked)
            {
                LoadFile();
            }
        }

        /// <summary>
        /// Allows user to save their code into a file.
        /// </summary>
        private void OnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!IsUILocked)
            {
                SaveFile();
            }
        }

        /// <summary>
        /// Allows user to edit their current theme
        /// </summary>
        private void OnTheme_Click(object sender, RoutedEventArgs e)
        {
            OpenThemeWindow();
        }

        /// <summary>
        /// Lets the user execute their code and displays it onto the output section.
        /// </summary>
        private void OnExecute_Click(object sender, RoutedEventArgs e)
        {
            if (!IsUILocked && ListboxCodeSpace.Items.Count > 0)
            {
                IsUILocked = true;
                if (VirtualMachine.Execute(0))
                {
                    IsUILocked = false;
                }
            }
        }

        /// <summary>
        /// Adds a new line, if able to, to the table
        /// </summary>
        private void OnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!IsUILocked)
            {
                AddLine();
            }
        }

        /// <summary>
        /// Removes the currently selected line, if able to, from the table
        /// </summary>
        private void OnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!IsUILocked)
            {
                DeleteLine();
            }
        }

        /// <summary>
        /// Resets the table to its default state
        /// </summary>
        private void OnReset_Click(object sender, RoutedEventArgs e)
        {
            if (!IsUILocked)
            {
                ResetMemory();
            }
        }

        /// <summary>
        /// Called when input button clicks, sends input in textbox to memory, continues processor
        /// </summary>
        private void ButtonSendInput_Click(object sender, RoutedEventArgs e)
        {
            VirtualMachine.CPU.ReceiveInput(TextBoxInput.Value ?? 0);
        }

        /// <summary>
        /// Handles exception of user input
        /// </summary>
        private void OnData_TextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }

        /// <summary>
        /// Allows user to enable various debugging functions while running the program with a Debugger
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
                    case Key.F6:
                        OpenThemeWindow();
                        break;
                    case Key.F7:
                        AddLine();
                        break;
                    case Key.F8:
                        DeleteLine(ListboxCodeSpace.Items.Count - 1);
                        break;
                    case Key.F11:
                        VirtualMachine.Execute(0);
                        break;
                }
            }
        }
        #endregion
    }
}