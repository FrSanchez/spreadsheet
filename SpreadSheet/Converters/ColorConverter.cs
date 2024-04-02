using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace SpreadSheet.Converters;

public class ColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        Console.WriteLine(value);
        return new SolidColorBrush(0xFF808080);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}