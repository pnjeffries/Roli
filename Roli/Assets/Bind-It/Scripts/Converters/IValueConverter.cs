using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Binding
{
    /// <summary>
    /// An interface for converter objects which allow for bindings to provide
    /// customised type conversion logic.
    /// Has the same signature as the equivalent interface in System.Windows.Data
    /// to allow for re-use of WPF converters.
    /// </summary>
    public interface IValueConverter
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value to be converted</param>
        /// <param name="targetType">The type to convert to</param>
        /// <param name="parameter">The converter parameter</param>
        /// <param name="culture">The culture to use in the converter</param>
        /// <returns></returns>
        object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture);

        /// <summary>
        /// Converts a value
        /// </summary>
        /// <param name="value">The value to be converted</param>
        /// <param name="targetType">The type to convert to</param>
        /// <param name="parameter">The converter parameter</param>
        /// <param name="culture">The culture to use in the converter</param>
        /// <returns></returns>
        object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture);
    }
}
