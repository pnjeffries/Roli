using System;
using System.Globalization;
using UnityEngine;

namespace Binding
{
    /// <summary>
    /// An abstract base class for converter objects which allow for bindings to provide
    /// customised type conversion logic for many-one binding types.
    /// Has the same signature as the equivalent interface in System.Windows.Data
    /// to allow for re-use of WPF converters.
    /// </summary>
    [Serializable]
    public abstract class MultiValueConverter : ScriptableObject, IMultiValueConverter
    {
        /// <summary>
        /// Converts a set of values to a single object of a target type.
        /// </summary>
        /// <param name="values">The value to be converted</param>
        /// <param name="targetType">The type to convert to</param>
        /// <param name="parameter">The converter parameter</param>
        /// <param name="culture">The culture to use in the converter</param>
        /// <returns></returns>
        public abstract object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);

        /// <summary>
        /// Converts a target value back to an array of values suitable for returning to the source
        /// </summary>
        /// <param name="value">The value to be converted</param>
        /// <param name="targetTypes">The list of target types to convert back to</param>
        /// <param name="parameter">The converter oarameter</param>
        /// <param name="culture">The culture to use in the converter</param>
        /// <returns></returns>
        public abstract object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture);
    }
}
