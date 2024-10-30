using System.Windows;
using System.Windows.Controls;

namespace ServiceStation.Views.UserControls;

public partial class ColorfulText : UserControl
{
    //TODO Обратить внимание на defaultValue в Metadata. Не уверен что хначение будет работать
    private new static readonly DependencyProperty BackgroundProperty =
        DependencyProperty.Register(nameof(BackgroundColor), typeof(string), typeof(ColorfulText),
            new FrameworkPropertyMetadata("FFFFFF", FrameworkPropertyMetadataOptions.None));
    
    private new static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(nameof(Text), typeof(string), typeof(ColorfulText),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.None));
    
    public ColorfulText()
    {
        InitializeComponent();
    }

    public string BackgroundColor
    {
        get => (string)GetValue(BackgroundProperty);
        
        set => SetValue(BackgroundProperty, value);
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        
        set => SetValue(TextProperty, value);
    }
}