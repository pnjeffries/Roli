using Binding;
using Nucleus.Model;
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
        if (values.Length > 3 && values[1] != null && values[3] != null && 
            values[3] is ElementCollection equippedItems && values[1] is Element item)
        {
            if (equippedItems.Contains(item.GUID))
            {
                //The item is equipped
                result = "<B>" + result + " [E]</B>";
            }
        }
        return result;
    }

    public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
