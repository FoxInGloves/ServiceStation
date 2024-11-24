using ServiceStation.ViewModels.Abstraction;

namespace ServiceStation.ViewModels.Implementation;

public class SettingsViewModel : AbstractViewModel
{
    public override Task UpdateAsync()
    {
        return Task.CompletedTask;
    }
}