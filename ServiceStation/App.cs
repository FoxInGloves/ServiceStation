using System.Windows;
using Serilog;
using ServiceStation.ViewModels.Implementation;

namespace ServiceStation;

public class App(MainViewModel viewModel) : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        var mainWindow = new MainWindow
        {
            DataContext = viewModel
        };
        mainWindow.Show();
        
        base.OnStartup(e);
    }
    
    protected override void OnExit(ExitEventArgs e)
    {
        Log.CloseAndFlush();
    }
}