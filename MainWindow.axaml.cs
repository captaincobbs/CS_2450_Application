using Avalonia.Controls;
using Avalonia.Interactivity;
using Tmds.DBus.Protocol;

namespace CS_2450_Application;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    public void On_ButtonClick(object sender, RoutedEventArgs args)
    {
        message.Text = "Button clicked!";
    }
}