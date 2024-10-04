using Avalonia.Controls;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
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
        viewModel.LoadLocations();
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
        public UVSim.GuiOs virtualMachine;

        public ViewModel()
        {
            LoadedMemory = new ObservableCollection<MemoryLine>();
            virtualMachine = new UVSim.GuiOs();
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
            for(int i = 0; i < virtualMachine.mainMemory.capacity; i++)
            {
                int data = virtualMachine.mainMemory.Read(i);
                int instruct_data = data / 100;
                BasicML instruction = Enum.IsDefined(typeof(BasicML), instruct_data) ? (BasicML) instruct_data : BasicML.NONE;

                AddItem(i, data, instruction);
            }
        }
    }
    #endregion
}

