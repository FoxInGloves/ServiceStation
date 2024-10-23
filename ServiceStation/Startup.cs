using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ServiceStation;

public static class Startup
{
    [STAThread]
    public static void Main()
    {
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddSingleton<App>();
                
                
            })
            .Build();
        
        var app = host.Services.GetService<App>();
        
        app?.Run();
    }
}