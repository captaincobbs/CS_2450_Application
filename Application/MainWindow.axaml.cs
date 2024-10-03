using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.Generic;


namespace CS_2450_Application;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;

        // var items = new List<MyItem>();
        //
        // for (int i = 0; i < 100; i++)
        // {
        //     items.Add(new MyItem
        //     {
        //         Text1 = "Hello World",
        //         InputText = $"Input {i}",
        //         Text2 = "Stuff"
        //     });
        // }
        // //
        // Assign items to the ListBox using ItemsSource
        // MyListBox.ItemsSource = items;
    }

    public static string Test => "This is a test";

    // public class MyItem
    // {
    //     public string? Text1 { get; set; }
    //     public string? InputText { get; set; }
    //     public string? Text2 { get; set; }
    // }

    public void RunProg(object sender, RoutedEventArgs args)
    {
        //Message.Text = "Ran";
    }
}

