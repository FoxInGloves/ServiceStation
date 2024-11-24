using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ServiceStation.Services.XamlConverters;

public class StringToBrushConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string colorString) return Brushes.Transparent;
        
        try
        {
            return new BrushConverter().ConvertFromString(colorString) as Brush;
        }
        catch
        {
            // Возвращаем прозрачный цвет, если строка некорректна
            return Brushes.Transparent;
        }
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}