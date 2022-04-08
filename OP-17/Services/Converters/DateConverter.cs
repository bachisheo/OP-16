using System;
using System.Windows.Data;

namespace OP_17.Services.Converters;

public class DateConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        return ((DateTime?) value)?.ToString("dd.MM.yyyy");
    }

    public object? ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (string.IsNullOrEmpty((string)value)) return null;
        return DateTime.ParseExact((string)value, "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
    }
}