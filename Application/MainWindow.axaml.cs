using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using UVSim;


namespace CS_2450_Application;

public partial class MainWindow : Window
{

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;

        for (int i = 0; i < 5; i++)
        {
            var textBlock = new TextBlock
            {
                Text = $"Text Block {i + 1}",
                FontSize = 16,
                Margin = new Thickness(5)
            };

            StackPanelCodeSpace.Children.Add(textBlock);
        }
    }

    public static string Test => "This is a test";
    

    public async void OnUpload_Click(object sender, RoutedEventArgs args)
    {
        var dialog = new OpenFileDialog
        {
            Title = "Select a File",
            AllowMultiple = false // Set to true if you want to allow multiple file selection
        };

        // Show the dialog and wait for the result
        var result = await dialog.ShowAsync(this);
    }
}

