using System.Windows;
using System.Windows.Controls;
using ServiceStation.ViewModels.Abstraction;

namespace ServiceStation.Services.Navigation.Abstraction;

public interface INavigationService
{
    (Page, AbstractViewModel) NavigateToPage(AbstractViewModel viewModel);

    (Window, AbstractViewModel) NavigateToWindow(AbstractViewModel viewModel);
}