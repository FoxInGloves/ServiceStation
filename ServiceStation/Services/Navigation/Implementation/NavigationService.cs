using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.Logging;
using ServiceStation.Services.Navigation.Abstraction;
using ServiceStation.Services.ResultT.Abstraction;
using ServiceStation.Services.ResultT.Implementation;
using ServiceStation.ViewModels.Abstraction;

namespace ServiceStation.Services.Navigation.Implementation;

public class NavigationService(ILogger<NavigationService> logger) : INavigationService
{
    public ResultT<(Page, AbstractViewModel)> NavigateToPage(AbstractViewModel viewModel)
    {
        var result = GetPage(viewModel);

        if (!result.IsSuccess)
        {
            logger.LogError("Failed to navigate to page: {Error}", result.Error);
            return ResultT<(Page, AbstractViewModel)>.Failure(result.Error!);
        }
        
        result.Value.Item1.DataContext = viewModel;

        return result;
    }

    public ResultT<(Window, AbstractViewModel)> NavigateToWindow(AbstractViewModel viewModel)
    {
        var result = GetWindow(viewModel);

        if (!result.IsSuccess)
        {
            logger.LogError("Failed to navigate to window: {Error}", result.Error);
            return ResultT<(Window, AbstractViewModel)>.Failure(result.Error!);
        }

        result.Value.Item1.DataContext = viewModel;

        return result;
    }

    private ResultT<(Page, AbstractViewModel)> GetPage(AbstractViewModel viewModel)
    {
        try
        {
            var viewModelType = viewModel.GetType();

            var viewName = viewModelType.FullName?
                .Replace("ViewModels.Implementation", "Views")
                .Replace("ViewModel", "Page");

            if (string.IsNullOrEmpty(viewName))
                return ResultT<(Page, AbstractViewModel)>.Failure(Error
                    .Failure("ViewNameError", "View name could not be determined."));

            var viewType = Assembly.GetExecutingAssembly().GetType(viewName);

            if (viewType is null)
                return ResultT<(Page, AbstractViewModel)>.Failure(Error
                    .NotFound("ViewTypeNotFound", $"View type '{viewName}' not found."));

            if (Activator.CreateInstance(viewType) is not Page page)
                return ResultT<(Page, AbstractViewModel)>.Failure(Error
                    .Failure("PageInstanceError", "Failed to create page instance."));
            
            return ResultT<(Page, AbstractViewModel)>.Success((page, viewModel));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected exception in while creating the page");
            return ResultT<(Page, AbstractViewModel)>.Failure(Error
                .Failure("UnexpectedException", "An unexpected error occurred while creating the page."));
        }
    }

    private ResultT<(Window, AbstractViewModel)> GetWindow(AbstractViewModel viewModel)
    {
        try
        {
            var viewModelType = viewModel.GetType();

            //TODO изменить путь к моим страницам
            var viewName = viewModelType.FullName?
                .Replace("ViewModels.Implementation.AdditionalViewModels", "Views.AdditionalViews")
                .Replace("ViewModel", "Window");

            if (string.IsNullOrEmpty(viewName))
                return ResultT<(Window, AbstractViewModel)>.Failure(Error
                    .Failure("ViewNameError", "View name could not be determined."));

            var viewType = Assembly.GetExecutingAssembly().GetType(viewName);

            if (viewType is null)
                return ResultT<(Window, AbstractViewModel)>.Failure(Error
                    .NotFound("ViewTypeNotFound", $"View type '{viewName}' not found."));

            if (Activator.CreateInstance(viewType) is not Window window)
                return ResultT<(Window, AbstractViewModel)>.Failure(Error
                    .Failure("PageInstanceError", "Failed to create page instance."));
            
            return ResultT<(Window, AbstractViewModel)>.Success((window, viewModel));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected exception in while creating the page");
            return ResultT<(Window, AbstractViewModel)>.Failure(Error
                .Failure("UnexpectedException", "An unexpected error occurred while creating the page."));
        }
    }
}