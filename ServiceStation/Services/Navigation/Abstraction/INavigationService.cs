using System.Windows;
using System.Windows.Controls;
using ServiceStation.ViewModels.Abstraction;

namespace ServiceStation.Services.Navigation.Abstraction;

public interface INavigationService
{
    public (Page, BaseViewModel) NavigateToPage(BaseViewModel viewModel);
    
    public (Window, BaseViewModel) NavigateToWindow(BaseViewModel viewModel);
}