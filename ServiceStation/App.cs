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

    private static void ConfigureResourceDictionaries()
    {
        var strings = new ResourceDictionary
        {
            Source = new Uri("/Resources/Strings.xaml", UriKind.Relative)
        };
        Current.Resources.MergedDictionaries.Add(strings);
        Current.Resources["Strings"] = strings;
        
        /*var windowNavigationButtonsDictionary = new ResourceDictionary
        {
            Source = new Uri("/Resources/WindowNavigationButtons.xaml", UriKind.Relative)
        };
        Current.Resources.MergedDictionaries.Add(windowNavigationButtonsDictionary);*/
    }

    //TODO нужно ли закрывать логгер
    protected override void OnExit(ExitEventArgs e)
    {
        Log.CloseAndFlush();
    }
}