using System.Windows.Controls;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using ServiceStation.Services.Navigation.Abstraction;
using ServiceStation.ViewModels.Abstraction;
using RelayCommand = ServiceStation.Services.Command.RelayCommand;

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
        NavigateToPageCommandAsync = new AsyncRelayCommand(_ => NavigateToVehiclesPageAsync());
       
       _logger.LogInformation("MainViewModel initialized");
    }
    
    public ICommand ToggleNavigationPanelCommand { get; init; }
    
    public ICommand NavigateToPageCommandAsync { get; init; }

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

    }

    private async Task SetCurrentPage((Page, AbstractViewModel) pageAndViewModel)
    {
        var updateVm = UpdateViewModel(pageAndViewModel.Item2);
        CurrentPage = pageAndViewModel.Item1;
        await updateVm;
    }

    private static async Task UpdateViewModel(AbstractViewModel viewModel)
    {
        await viewModel.UpdateAsync();
    }

    private async Task NavigateToVehiclesPageAsync()
    {
        if (_vehiclesPageAndViewModel == null)
        {

            var navigationResult = _navigationService.NavigateToPage<VehiclesViewModel>();

            if (!navigationResult.IsSuccess)
            {
                _logger.LogError("Failed navigated to vehicles page.\nCode: {Code}\nDescription: {Description}",
                    navigationResult.Error?.Code, navigationResult.Error?.Description);
                return;
            }

            var vehicleViewAndViewModel = navigationResult.Value;
            _vehiclesPageAndViewModel = vehicleViewAndViewModel;
        }

        await SetCurrentPage(_vehiclesPageAndViewModel.Value);
    }
}