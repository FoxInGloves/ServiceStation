using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using ServiceStation.ViewModels.Abstraction;

namespace ServiceStation.ViewModels.Implementation;

public class AddNewWorkerViewModel : AbstractViewModel
{
    private readonly ILogger<AddNewWorkerViewModel> _logger;
    
    private string? _lastName;
    private string? _firstName;
    private string? _middleName;

    public AddNewWorkerViewModel(ILogger<AddNewWorkerViewModel> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        
        SaveCommand = new RelayCommand<(Window?, bool)>(CloseWindow);
        CancelCommand = new RelayCommand<(Window?, bool)>(CloseWindow);
        
        _logger.LogInformation("Add new worker viewmodel initialized");
    }
    
    public ICommand SaveCommand { get; set; }
    
    public ICommand CancelCommand { get; set; }
    
    public string? LastName
    {
        get => _lastName;
        set => SetField(ref _lastName, value);
    }

    public string? FirstName
    {
        get => _firstName;
        set => SetField(ref _firstName, value);
    }

    public string? MiddleName
    {
        get => _middleName;
        set => SetField(ref _middleName, value);
    }

    private void CloseWindow((Window? WindowToClose, bool DialogResult) parameters)
    {
        /*if (parameters is null)
        {
            _logger.LogError("Closing window parameters is null");
            return;
        }*/

        if (parameters.WindowToClose is null)
        {
            _logger.LogError("Parameter window is null");
            return;
        }
        
        if (parameters.DialogResult is false)
        {
            parameters.WindowToClose.Close();
            _logger.LogError("Closing window");
            return;
        }
        
        if (string.IsNullOrWhiteSpace(LastName) && string.IsNullOrWhiteSpace(FirstName))
        {
            MessageBox.Show("Одно из полей незаполнено");
        }

        parameters.WindowToClose.DialogResult = true;
    }
}