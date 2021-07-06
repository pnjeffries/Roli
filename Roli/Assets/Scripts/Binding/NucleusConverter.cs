using Binding;
using Nucleus.Geometry;
using Nucleus.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;


public class NucleusConverter : DefaultConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        Debug.Log("Converting " + (value?.ToString() ?? "null"));
        if (value is Vector vector)
        {
            if (targetType == typeof(Vector2)) return ToUnity.Convert2D(vector);
            if (targetType.IsAssignableFrom(typeof(Vector3))) return ToUnity.Convert(vector);
        }
        return base.Convert(value, targetType, parameter, culture);
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

