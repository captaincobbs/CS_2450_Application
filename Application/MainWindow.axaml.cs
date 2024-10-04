using Avalonia.Controls;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using UVSim;


namespace CS_2450_Application;

public partial class MainWindow : Window
{
    public ViewModel ViewModelData;
    public MainWindow()
    {
        InitializeComponent();
        ViewModelData = new();
        DataContext = ViewModelData;
        //ViewModelData.LoadLocations();
    }

    #region Events
    public async void OnUpload_Click(object sender, RoutedEventArgs args)
    {
        OpenFileDialog dialog = new()
        {
            Title = "Open UVSim File",
            AllowMultiple = false, // Single file selection
            Filters =
            [
                new() { Name = "UVSim Files", Extensions = ["uvsim"] },
                new() { Name = "Text Files", Extensions = ["txt"] },
                new() { Name = "All Files", Extensions = ["*"] }
            ],
            // Set the default directory to the program's current directory
            Directory = Directory.GetCurrentDirectory()
        };

        var result = await dialog.ShowAsync(this);

        if (result != null && result.Length > 0) {
            string filePath = result[0];

            // string contents = File.ReadAllText(filePath);
        }
    }

    public async void OnSave_Click(object sender, RoutedEventArgs args)
    {
        SaveFileDialog dialog = new()
        {
            Title = "Save UVSim File",
            DefaultExtension = ".uvsim",
            Filters =
            [
                new() { Name = "UVSim Files", Extensions = ["uvsim"] }
            ],
            InitialFileName = "memory.uvsim",
            Directory = Directory.GetCurrentDirectory()
        };

        // Show the dialog and wait for the result
        var result = await dialog.ShowAsync(this);

        if (!string.IsNullOrEmpty(result))
        {
            // data must be a string
            //File.WriteAllText(result, data);
        }
    }

    private void Window_Resized(object? sender, WindowResizedEventArgs e)
    {
        // This ensures that the Listbox resizes with the rest of the GUI since apparently listboxes are weird
        ListboxCodeSpace.MaxHeight = BorderOutput.Bounds.Bottom - ListboxCodeSpace.Bounds.Top - BorderCodeSpaceFooter.Bounds.Height;
    }

    public async void OnRun_Click(object sender, RoutedEventArgs args)
    {
        await Task.Run(() =>
            {
                ViewModelData.VirtualMachine.Execute();
            }
        );
        ViewModelData.Register = ViewModelData.VirtualMachine.CPU.GetAccumulator();
    }
    #endregion

    #region Classes
    public partial class ViewModel : ObservableObject
    {
        [ObservableProperty]
        public ObservableCollection<Memory.MemoryLine> _loadedMemory;
        public OperatingSystemGUI VirtualMachine;
        [ObservableProperty]
        public int _register;

        public ViewModel()
        {
            VirtualMachine = new OperatingSystemGUI();
            _loadedMemory = VirtualMachine.MainMemory.locations;
            Register = VirtualMachine.CPU.GetAccumulator();
        }

        //public void AddItem(int lineNumber, int data, BasicML instruction)
        //{
        //    Memory.MemoryLine memory = new()
        //    {
        //        LineNumber = lineNumber,
        //        Data = data,
        //        Instruction = instruction
        //    };
        //    LoadedMemory.Add(memory);
        //}
        ///// <summary>
        ///// Loads all memory locations from mainMemory into the GUI.
        ///// </summary>
        //public void LoadLocations()
        //{
        //    for (int i = 0; i < VirtualMachine.MainMemory.Capacity; i++)
        //    {
        //        int data = VirtualMachine.MainMemory.Read(i);
        //        int instruct_data = data / 100;
        //        BasicML instruction = Enum.IsDefined(typeof(BasicML), instruct_data) ? (BasicML)instruct_data : BasicML.NONE;

        //        AddItem(i, data, instruction);
        //    }
        //}
        ///// <summary>
        ///// Updates the corresponding memory location in mainMemory when it is changed manually in the GUI.
        ///// </summary>
        //public void UpdateLocation()
        //{

        //}
    }
    #endregion
}

