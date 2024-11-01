using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using ServiceStation.Repository;
using ServiceStation.Repository.Abstraction;
using ServiceStation.Repository.Implementation;
using ServiceStation.Services.Navigation.Abstraction;
using ServiceStation.Services.Navigation.Implementation;
using ServiceStation.ViewModels.Implementation;

namespace ServiceStation;

public static class Startup
{
    [STAThread]
    public static void Main()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                restrictedToMinimumLevel: LogEventLevel.Information)
            .CreateLogger();

        //TODO Добавить переменную окружения
        const string connectingString = "Host=localhost;Port=5432;Database=Temp;Username=postgres;Password=123456789";
        
        var host = Host.CreateDefaultBuilder()
            .UseSerilog()
            .ConfigureServices(services =>
            {
                services.AddSingleton<App>();

                services.AddSingleton<MainViewModel>();
                services.AddSingleton<VehiclesViewModel>();

                services.AddDbContext<DataBaseContext>(options => 
                    options.UseNpgsql(connectingString));
                services.AddScoped<IUnitOfWork, UnitOfWork>();
                services.AddScoped<INavigationService, NavigationService>();
            })
            .Build();

        try
        {
            var app = host.Services.GetService<App>();

            var strings = new ResourceDictionary
            {
                Source = new Uri("/Resources/Strings.xaml", UriKind.Relative)
            };
            app?.Resources.MergedDictionaries.Add(strings);

            app?.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "The application failed to start");
        }
    }
}