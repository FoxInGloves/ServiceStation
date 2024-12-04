using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ServiceStation.Services.Navigation.Abstraction;
using ServiceStation.Services.ResultT.Abstraction;
using ServiceStation.Services.ResultT.Implementation;
using ServiceStation.ViewModels.Abstraction;

namespace ServiceStation.Services.Navigation.Implementation;

public class NavigationService(ILogger<NavigationService> logger, IServiceProvider serviceProvider) : INavigationService
{
    //TODO попробовать унифицировать метод для страниц и окон
    public ResultT<(Page, AbstractViewModel)> NavigateToPage<TViewModel>() where TViewModel : AbstractViewModel
    {
        try
        {
            var viewModel = serviceProvider.GetService<TViewModel>();
            if (viewModel is null)
                throw new NullReferenceException("ViewModel is null");
            
            var pageType = GetViewType<Page>(typeof(TViewModel));

            if (pageType is null)
                return Error.NotFound("ViewTypeNotFound", $"View type for '{typeof(TViewModel)}' not found.");

            if (Activator.CreateInstance(pageType) is not Page page)
                return Error.Failure("PageInstanceError", $"Failed to create '{pageType}' instance.");

            page.DataContext = viewModel;
            return ResultT<(Page, AbstractViewModel)>.Success((page, viewModel));
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message, "An unexpected error occurred while creating the page.");
        }
    }

    public ResultT<(Window, AbstractViewModel)> NavigateToWindow<TViewModel>() where TViewModel : AbstractViewModel
    {
        try
        {
            var viewModel = serviceProvider.GetService<TViewModel>();
            if (viewModel is null)
                throw new NullReferenceException("ViewModel is null");

            var windowType = GetViewType<Window>(typeof(TViewModel));

            if (windowType is null)
                return Error.NotFound("ViewTypeNotFound", $"View type for '{typeof(TViewModel)}' not found.");

            if (Activator.CreateInstance(windowType) is not Window window)
                return Error.Failure("WindowInstanceError", $"Failed to create '{windowType}' instance.");

            window.DataContext = viewModel;
            return ResultT<(Window, AbstractViewModel)>.Success((window, viewModel));
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message, "An unexpected error occurred while creating the window.");
        }
    }
    
    private Type? GetViewType<TView>(Type viewModelType) where TView : class
    {
        var importViewType = typeof(TView);
        string? viewName;
        
        if (importViewType == typeof(Page))
        { 
            viewName = viewModelType.FullName?
                .Replace("ViewModels.Implementation", "Views")
                .Replace("ViewModel", "Page");
        }
        else if (importViewType == typeof(Window))
        {
            viewName = viewModelType.FullName?
                .Replace("ViewModels.Implementation", "Views")
                .Replace("ViewModel", "Window");
        }
        else
        {
            logger.LogCritical("Unsupported view type '{typeof(TView)}'. Only Page or Window are allowed.", typeof(TView));
            throw new InvalidOperationException(
                $"Unsupported view type '{typeof(TView)}'. Only Page or Window are allowed.");
        }
        
        var viewType = Assembly.GetExecutingAssembly().GetType(viewName!);
        return viewType;
    }
}