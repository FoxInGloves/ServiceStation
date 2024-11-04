using System.Windows;
using System.Windows.Controls;
using ServiceStation.Services.ResultT.Implementation;
using ServiceStation.ViewModels.Abstraction;

namespace ServiceStation.Services.Navigation.Abstraction;

public interface INavigationService
{
    ResultT<(Page, AbstractViewModel)> NavigateToPage<TViewModel>() where TViewModel : AbstractViewModel;

    ResultT<(Window, AbstractViewModel)> NavigateToWindow<TViewModel>() where TViewModel : AbstractViewModel;
}