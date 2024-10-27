using System.Windows.Input;

namespace ServiceStation.Services.Command;

public class RelayCommand(Action<object> execute, Func<object, bool> canExecute) : ICommand
{
    public bool CanExecute(object? parameter)
    {
        return canExecute(parameter!);
    }

    public void Execute(object? parameter)
    {
        execute(parameter!);
    }

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
}