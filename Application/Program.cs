using Avalonia;
using System;
using UVSim;

namespace CS_2450_Application;

class Program
{
    //Lines 9 - 22 for GUI version

    //Initialization code. Don't use any Avalonia, third-party APIs or any
    //SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    //yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        UVSim.OperatingSystem virtualMachine = new UVSim.OperatingSystem();

        BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);
    }

    //// Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
