using Avalonia.Controls;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UVSim;


namespace CS_2450_Application;

public partial class MainWindow : Window
{
    public ViewModel viewModel;
    public MainWindow()
    {
        InitializeComponent();
        viewModel = new();
        DataContext = viewModel;
        viewModel.AddItem(0, "xxxx", BasicML.READ);
        viewModel.AddItem(1, "xxxx", BasicML.READ);
        viewModel.AddItem(2, "xxxx", BasicML.READ);
        viewModel.AddItem(3, "xxxx", BasicML.READ);
        viewModel.AddItem(4, "xxxx", BasicML.READ);
        viewModel.AddItem(5, "xxxx", BasicML.READ);
        viewModel.AddItem(6, "xxxx", BasicML.READ);
        viewModel.AddItem(7, "xxxx", BasicML.READ);
        viewModel.AddItem(8, "xxxx", BasicML.READ);
        viewModel.AddItem(9, "xxxx", BasicML.READ);
    }

    #region Events
    public async void OnUpload_Click(object sender, RoutedEventArgs args)
    {
        OpenFileDialog dialog = new()
        {
            Title = "Load a File",
            AllowMultiple = false // Set to true if you want to allow multiple file selection
        };

        // Show the dialog and wait for the result
        var result = await dialog.ShowAsync(this);
    }

    public async void OnSave_Click(object sender, RoutedEventArgs args)
    {
        SaveFileDialog dialog = new()
        {
            Title = "Save File",
        };

        // Show the dialog and wait for the result
        var result = await dialog.ShowAsync(this);
    }
    #endregion

    #region Classes
    public class MemoryLine : ObservableObject
    {
        private int _lineNumber;
        private string? _hexNumber;
        private BasicML _instruction;
        public int LineNumber
        {
            get { return _lineNumber; }
            set { SetProperty(ref _lineNumber, value); }
        }

        public string? HexNumber
        {
            get { return _hexNumber; }
            set { SetProperty(ref _hexNumber, value); }
        }
        public BasicML Instruction
        {
            get { return _instruction; }
            set { SetProperty(ref _instruction, value); }
        }
    }

    public class ViewModel : ObservableObject
    {
        public ObservableCollection<MemoryLine> LoadedMemory { get; set; }

        public ViewModel()
        {
            LoadedMemory = new();
        }

        public void AddItem(int lineNumber, string hexNumber, BasicML instruction)
        {
            MemoryLine memory = new()
            {
                LineNumber = lineNumber,
                HexNumber = hexNumber,
                Instruction = instruction
            };

            LoadedMemory.Add(memory);
        }
    }
    #endregion
}

