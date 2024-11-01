using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using ServiceStation.Services.Navigation.Abstraction;
using ServiceStation.ViewModels.Abstraction;

namespace ServiceStation.Services.Navigation.Implementation;

public class NavigationService : INavigationService
{
    public (Page, AbstractViewModel) NavigateToPage(AbstractViewModel viewModel)
    {
        var pageAndVm = GetPage(viewModel);

        pageAndVm.Item1.DataContext = viewModel;

        return pageAndVm;
    }

    public (Window, AbstractViewModel) NavigateToWindow(AbstractViewModel viewModel)
    {
        var windowAndVm = GetWindow(viewModel);

        windowAndVm.Item1.DataContext = viewModel;

        return windowAndVm;
    }

    private static (Page, AbstractViewModel) GetPage(AbstractViewModel viewModel)
    {
        var viewModelType = viewModel.GetType();

        //TODO изменить путь к моим страницам и добавить try catch 
        var viewName = viewModelType.FullName?.Replace("ViewModels.Implementation.MainViewModels", "Views.MainViews")
            .Replace("ViewModel", "Page");

        if (viewName is null) throw new ArgumentNullException(viewName);

        var viewType = Assembly.GetExecutingAssembly().GetType(viewName);

        if (viewType is null) throw new ArgumentNullException(viewName);

        if (Activator.CreateInstance(viewType) is not Page page) throw new ArgumentNullException(viewName);

        return (page, viewModel);
    }

    private static (Window, AbstractViewModel) GetWindow(AbstractViewModel viewModel)
    {
        var viewModelType = viewModel.GetType();

        var viewName = viewModelType.FullName
            ?.Replace("ViewModels.Implementation.AdditionalViewModels", "Views.AdditionalViews")
            .Replace("ViewModel", "Window");

        if (viewName is null) throw new ArgumentNullException(viewName);

        var viewType = Assembly.GetExecutingAssembly().GetType(viewName);

        if (viewType is null) throw new ArgumentNullException(viewName);

        if (Activator.CreateInstance(viewType) is not Window window) throw new ArgumentNullException(viewName);

        return (window, viewModel);
    }
}