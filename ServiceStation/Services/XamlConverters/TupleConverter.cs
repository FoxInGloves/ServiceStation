using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ServiceStation.Services.XamlConverters;

public class TupleConverter : IMultiValueConverter
{
    public object? Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values is [Window window, bool dialogResult])
        {
            return (window, dialogResult);
        }
        return null;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}