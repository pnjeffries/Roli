using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using UnityEngine;

namespace Binding
{
    /// <summary>
    /// The default converter used in bindings which do not have an overriding converter
    /// applied.  Performs standard data conversion.
    /// </summary>
    [Serializable]
    public class DefaultConverter : ValueConverter
    {
        /// <summary>
        /// Cached DefaultConverter instance
        /// </summary>
        private static DefaultConverter _Instance;

        /// <summary>
        /// A reusable cached instance of the DefaultConverter
        /// </summary>
        public static DefaultConverter Instance
        {
            get
            {
                if (_Instance == null) _Instance = CreateInstance(typeof(DefaultConverter)) as DefaultConverter;
                return _Instance;
            }
        }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DefaultConvert(value, targetType, parameter, culture);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DefaultConvert(value, targetType, parameter, culture);
        }

    }
}
