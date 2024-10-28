using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using ServiceStation.Services.Command;
using ServiceStation.ViewModels.Abstraction;

namespace ServiceStation.ViewModels.Implementation;

public class MainViewModel : BaseViewModel
{
    private double _widthOfNavigationPanel;
    
    private bool _isExpanded = false;
    private const double CollapsedWidth = 50; // Минимальная ширина
    private const double ExpandedWidth = 300;
    
    public MainViewModel()
    {
        MoveNavigationPanelCommand = new RelayCommand(_ => ToggleSize(), _ => true);
       _widthOfNavigationPanel = CollapsedWidth;
    }
    
    public ICommand MoveNavigationPanelCommand { get; init; }
    
    public double WidthOfNavigationPanel 
    {
        get => _widthOfNavigationPanel;
        
        set => SetField(ref _widthOfNavigationPanel, value);
    }
    
    private void ToggleSize()
    {
        if (_isExpanded)
        {
            WidthOfNavigationPanel = CollapsedWidth;
            _isExpanded = !_isExpanded;
        }
        else
        {
            WidthOfNavigationPanel = ExpandedWidth;
            _isExpanded = !_isExpanded;
        }
        
    }
}