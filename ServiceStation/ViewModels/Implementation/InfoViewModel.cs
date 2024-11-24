using ServiceStation.ViewModels.Abstraction;

namespace ServiceStation.ViewModels.Implementation;

public class InfoViewModel : AbstractViewModel
{
    public override Task UpdateAsync()
    {
        return Task.CompletedTask;
    }
}