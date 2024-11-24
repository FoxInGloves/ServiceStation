using System.Windows;
using System.Windows.Controls;

namespace ServiceStation.Views.UserControls;

public partial class WorkersPreview : UserControl
{
    private new static readonly DependencyProperty WorkerNameProperty =
        DependencyProperty.Register(nameof(WorkerName), typeof(string), typeof(WorkersPreview),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.None));
    
    public WorkersPreview()
    {
        InitializeComponent();
    }

    public string WorkerName
    {
        get => (string)GetValue(WorkerNameProperty);
        
        set => SetValue(WorkerNameProperty, value);
    }
}