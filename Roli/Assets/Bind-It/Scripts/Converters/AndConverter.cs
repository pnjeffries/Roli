using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace Binding
{
    /// <summary>
    /// Boolean MultiValueConverter that returns true only if every item in the source value array evaluates to true.
    /// </summary>
    [Serializable]
    public class AndConverter : MultiValueConverter
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            foreach (var value in values)
            {
                bool b = System.Convert.ToBoolean(value);
                if (!b) return false;
            }
            return true;
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("AndConverter is one-way only.");
        }
    }

}
