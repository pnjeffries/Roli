using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace Binding
{

    /// <summary>
    /// A simple converter which returns true if the passed-in value is not null,
    /// or false if it is.
    /// </summary>
    [Serializable]
    public class NotNullConverter : ValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value != null);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
