using Binding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class InventorySlotConverter : MultiValueConverter
{
    public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        var result = string.Format("{0}: {1}", values[0], values[1]);
        if (values.Length > 2 && values[2] != null)
        {
            result += string.Format("({0})", values[2]);
        }
        return result;
    }

    public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
