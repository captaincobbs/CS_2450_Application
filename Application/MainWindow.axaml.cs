using Avalonia.Controls;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
        ViewModelData.LoadLocations();
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
    #endregion

    #region Classes
    public class MemoryLine : ObservableObject
    {
        private int _lineNumber;
        private int _data;
        private BasicML _instruction;
        public int LineNumber
        {
            get { return _lineNumber; }
            set { SetProperty(ref _lineNumber, value); }
        }

        public int Data
        {
            get { return _data; }
            set { SetProperty(ref _data, value); }
        }
        public BasicML Instruction
        {
            get { return _instruction; }
            set { SetProperty(ref _instruction, value); }
        }
    }

    public class ViewModel : ObservableObject
    {
        public ObservableCollection<MemoryLine> LoadedMemory { get; }
        public OperatingSystemGUI VirtualMachine;

        public ViewModel()
        {
            LoadedMemory = [];
            VirtualMachine = new OperatingSystemGUI();
        }

        public void AddItem(int lineNumber, int data, BasicML instruction)
        {
            MemoryLine memory = new()
            {
                LineNumber = lineNumber,
                Data = data,
                Instruction = instruction
            };
            LoadedMemory.Add(memory);
        }

        public void LoadLocations()
        {
            for (int i = 0; i < VirtualMachine.MainMemory.Capacity; i++)
            {
                int data = VirtualMachine.MainMemory.Read(i);
                int instruct_data = data / 100;
                BasicML instruction = Enum.IsDefined(typeof(BasicML), instruct_data) ? (BasicML)instruct_data : BasicML.NONE;

                AddItem(i, data, instruction);
            }
        }
    }
    #endregion
}

