using Binding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

[Serializable]
public class BarSizeConverter : ValueConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        float fValue = (float)System.Convert.ChangeType(value ?? 0, typeof(float));
        if (targetType == typeof(float))
        {
            return fValue * 20;
        }
        if (targetType == typeof(Vector2))
        {
            // This example multiplies the input value and uses it as the width of a Vector2
            return new Vector2(fValue * 10, 20);
        }
        else if (targetType == typeof(Color))
        {
            // In this case, we will adjust the colour based on the value;
            // <= 3: red
            if (fValue <= 3) return Color.red;
            // <= 6: yellow
            else if (fValue <= 6) return Color.yellow;
            // otherwise, green
            else return Color.green;
        }
        else throw new NotImplementedException(string.Format("Conversion not implemented for type '{0}'", targetType));
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
