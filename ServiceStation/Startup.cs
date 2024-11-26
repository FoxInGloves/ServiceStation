using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using ServiceStation.Models.DTOs.Implementation;
using ServiceStation.Models.Entities.Implementation;
using ServiceStation.Repository;
using ServiceStation.Repository.Abstraction;
using ServiceStation.Repository.Implementation;
using ServiceStation.Services.Mapping.Abstraction;
using ServiceStation.Services.Mapping.Implementation;
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
        const string connectingString = "Host=localhost;Port=5432;Database=service_station;Username=postgres;Password=123456789";
        
        var host = Host.CreateDefaultBuilder()
            .UseSerilog()
            .ConfigureServices(services =>
            {
                services.AddScoped<App>();

                services.AddScoped<MainViewModel>();
                services.AddScoped<VehiclesViewModel>();
                services.AddScoped<WorkersViewModel>();
                services.AddScoped<FeedbackViewModel>();
                services.AddScoped<InfoViewModel>();
                services.AddScoped<SettingsViewModel>();
                services.AddScoped<AddNewWorkerViewModel>();
                services.AddScoped<AddNewVehicleViewModel>();
                services.AddScoped<AddNewDefectViewModel>();
                services.AddScoped<VehicleDetailsViewModel>();
                services.AddScoped<OwnerDetailsViewModel>();
                services.AddScoped<DefectDetailsViewModel>();
                services.AddScoped<WorkerDetailsViewModel>();

                services.AddDbContext<ApplicationDatabaseContext>(options => 
                    options.UseLazyLoadingProxies().UseNpgsql(connectingString));
                
                services.AddScoped<IUnitOfWork, UnitOfWork>();
                services.AddScoped<INavigationService, NavigationService>();
                
                services.AddScoped<IMapper<Vehicle, VehicleDto>, VehicleMapper>();
                services.AddScoped<IMapper<Worker, WorkerDto>, WorkerMapper>();
                services.AddScoped<IMapper<Owner, OwnerDto>, OwnerMapper>();
                services.AddScoped<IMapper<Defect, DefectDto>, DefectMapper>();
            })
            .Build();

        try
        {
            var app = host.Services.GetService<App>();

            app?.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "The application dropped due to unexpected exception.");
        }
    }
}