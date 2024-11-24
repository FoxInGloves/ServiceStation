using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using ServiceStation.Services.Navigation.Abstraction;
using ServiceStation.ViewModels.Abstraction;

namespace ServiceStation.ViewModels.Implementation;

public sealed class MainViewModel : AbstractViewModel
{
    private const int ExpandedNavigationPanelWidth = 40;
    private const int NormalNavigationPanelWidth = 200;

    private int _navigationColumnWidth;

    private readonly ILogger<MainViewModel> _logger;

    private readonly INavigationService _navigationService;

    private Page? _currentPage;
    private (Page, AbstractViewModel)? _vehiclesPageAndViewModel;
    private (Page, AbstractViewModel)? _workersPageAndViewModel;
    private (Page, AbstractViewModel)? _infoPageAndViewModel;
    private (Page, AbstractViewModel)? _settingsPageAndViewModel;

    public MainViewModel(ILogger<MainViewModel> logger, INavigationService navigationService)
    {
        _logger = logger;
        _navigationService = navigationService;

        ToggleNavigationPanelCommand = new RelayCommand(ToggleNavigationPanelWidth);
        _navigationColumnWidth = ExpandedNavigationPanelWidth;
        NavigateToVehiclePageCommandAsync = new AsyncRelayCommand(NavigateToVehiclesPageAsync);
        NavigateToWorkersPageCommandAsync = new AsyncRelayCommand(NavigateToWorkersPageAsync);
        NavigateToFeedbackWindowCommand = new RelayCommand(NavigateToFeedbackWindow);
        NavigateToInfoPageCommand = new AsyncRelayCommand(NavigateToInfoPageAsync);
        NavigateToSettingsPageCommand = new AsyncRelayCommand(NavigateToSettingsPageAsync);

        _logger.LogInformation("MainViewModel initialized");
    }

    public ICommand ToggleNavigationPanelCommand { get; init; }

    public ICommand NavigateToVehiclePageCommandAsync { get; init; }

    public ICommand NavigateToWorkersPageCommandAsync { get; init; }
    
    public ICommand NavigateToFeedbackWindowCommand { get; init; }
    
    public ICommand NavigateToInfoPageCommand { get; init; }
    
    public ICommand NavigateToSettingsPageCommand { get; init; }

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
        NavigationColumnWidth = _navigationColumnWidth == ExpandedNavigationPanelWidth
            ? NormalNavigationPanelWidth
            : ExpandedNavigationPanelWidth;
    }

    private async Task NavigateToVehiclesPageAsync()
    {
        if (_vehiclesPageAndViewModel is null)
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

    private async Task NavigateToWorkersPageAsync()
    {
        if (_workersPageAndViewModel is null)
        {
            var navigationResult = _navigationService.NavigateToPage<WorkersViewModel>();

            if (!navigationResult.IsSuccess)
            {
                _logger.LogError("Failed navigated to workers page.\nCode: {Code}\nDescription: {Description}",
                    navigationResult.Error?.Code, navigationResult.Error?.Description);
                return;
            }

            var workersViewAndViewModel = navigationResult.Value;
            _workersPageAndViewModel = workersViewAndViewModel;
        }

        await SetCurrentPage(_workersPageAndViewModel.Value);
    }

    private void NavigateToFeedbackWindow()
    {
        var feedbackViewAndViewModelResult = _navigationService.NavigateToWindow<FeedbackViewModel>();

        if (!feedbackViewAndViewModelResult.IsSuccess)
        {
            _logger.LogError("Failed navigated to feedback window.\nCode: {Code}\nDescription: {Description}",
                feedbackViewAndViewModelResult.Error?.Code, feedbackViewAndViewModelResult.Error?.Description);
            return;
        }

        var feedbackViewAndViewModel = feedbackViewAndViewModelResult.Value;
        feedbackViewAndViewModel.Item1.Owner = Application.Current.MainWindow;

        var dialog = feedbackViewAndViewModel.Item1.ShowDialog();
    }

    private async Task NavigateToInfoPageAsync()
    {
        if (_infoPageAndViewModel is null)
        {
            var navigationResult = _navigationService.NavigateToPage<InfoViewModel>();

            if (!navigationResult.IsSuccess)
            {
                _logger.LogError("Failed navigated to info page.\nCode: {Code}\nDescription: {Description}",
                    navigationResult.Error?.Code, navigationResult.Error?.Description);
                return;
            }

            var infoViewAndViewModel = navigationResult.Value;
            _infoPageAndViewModel = infoViewAndViewModel;
        }

        await SetCurrentPage(_infoPageAndViewModel.Value);
    }

    private async Task NavigateToSettingsPageAsync()
    {
        if (_settingsPageAndViewModel is null)
        {
            var navigationResult = _navigationService.NavigateToPage<SettingsViewModel>();

            if (!navigationResult.IsSuccess)
            {
                _logger.LogError("Failed navigated to settings page.\nCode: {Code}\nDescription: {Description}",
                    navigationResult.Error?.Code, navigationResult.Error?.Description);
                return;
            }

            var settingsViewAndViewModel = navigationResult.Value;
            _settingsPageAndViewModel = settingsViewAndViewModel;
        }

        await SetCurrentPage(_settingsPageAndViewModel.Value);
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
}