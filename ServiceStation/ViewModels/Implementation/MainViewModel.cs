using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.Logging;
using ServiceStation.Services.Command;
using ServiceStation.Services.Navigation.Abstraction;
using ServiceStation.ViewModels.Abstraction;

namespace ServiceStation.ViewModels.Implementation;

public sealed class MainViewModel : AbstractViewModel
{
    private const int ExpandedNavigationPanelWidth = 50;
    private const int NormalNavigationPanelWidth = 300;

    private int _navigationColumnWidth;
    
    private readonly ILogger<MainViewModel> _logger;
    
    private readonly INavigationService _navigationService;
    
    private Page? _currentPage;
    private (Page, AbstractViewModel)? _vehiclesPageAndViewModel;
    
    public MainViewModel(ILogger<MainViewModel> logger, INavigationService navigationService)
    {
        _logger = logger;
        _navigationService = navigationService;
        
        ToggleNavigationPanelCommand = new RelayCommand(_ => ToggleNavigationPanelWidth(), _ => true);
        _navigationColumnWidth = NormalNavigationPanelWidth;
       
       _logger.LogInformation("MainViewModel initialized");
    }
    
    public ICommand ToggleNavigationPanelCommand { get; init; }

    public int NavigationColumnWidth
    {
        get => _navigationColumnWidth;
        
        set => SetField(ref _navigationColumnWidth, value);
    }

    public Page? CurrentPage
    {
        get => _currentPage;
        
        set => SetField(ref _currentPage, value);
    }

    private void ToggleNavigationPanelWidth()
    {
        NavigationColumnWidth = _navigationColumnWidth == ExpandedNavigationPanelWidth ? 
            NormalNavigationPanelWidth : ExpandedNavigationPanelWidth;
        
        _logger.LogInformation("NavigationColumnWidth toggled");
    }
}