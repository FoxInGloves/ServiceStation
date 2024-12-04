using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using ServiceStation.ViewModels.Abstraction;

namespace ServiceStation.ViewModels.Implementation;

public class AddNewDefectViewModel : AbstractViewModel
{
    private readonly ILogger<AddNewDefectViewModel> _logger;
    
    private string? _fault;
    private string? _description;

    public AddNewDefectViewModel(ILogger<AddNewDefectViewModel> logger)
    {
        _logger = logger;

        SaveCommand = new RelayCommand<(Window, bool)>(CloseWindow);
        CancelCommand = new RelayCommand<(Window, bool)>(CloseWindow);
    }
    
    public ICommand SaveCommand { get; init; }
    
    public ICommand CancelCommand { get; init; }

    public string? Fault
    {
        get => _fault;
        set => SetField(ref _fault, value);
    }

    public string? Description
    {
        get => _description;
        set => SetField(ref _description, value);
    }
    
    private void CloseWindow((Window? WindowToClose, bool DialogResult) parameters)
    {
        if (parameters.WindowToClose is null)
        {
            _logger.LogError("Parameter AddNewDefect window is null");
            return;
        }
        
        if (parameters.DialogResult is false)
        {
            parameters.WindowToClose.Close();
            _logger.LogError("Closing window");
            return;
        }
        
        if (string.IsNullOrWhiteSpace(Fault))
        {
            MessageBox.Show(" Поле \"название\" неисправности незаполнено");
            return;
        }
        
        parameters.WindowToClose.DialogResult = true;
    }
}