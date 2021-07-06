using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using UnityEngine;

namespace Binding
{
    /// <summary>
    /// An abstract base class for converter objects which allow for bindings to provide
    /// customised type conversion logic.
    /// </summary>
    /// <remarks>The ValueConverter base class implements the IValueConverter interface abstractly and
    /// inherits from UnityEngine.Object to allow it to be serialized and used as a script component input field</remarks>
    [Serializable]
    public abstract class ValueConverter : ScriptableObject, IValueConverter
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value to be converted</param>
        /// <param name="targetType">The type to convert to</param>
        /// <param name="parameter">The converter parameter</param>
        /// <param name="culture">The culture to use in the converter</param>
        /// <returns></returns>
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        /// <summary>
        /// Converts a value
        /// </summary>
        /// <param name="value">The value to be converted</param>
        /// <param name="targetType">The type to convert to</param>
        /// <param name="parameter">The converter parameter</param>
        /// <param name="culture">The culture to use in the converter</param>
        /// <returns></returns>
        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);


        /// <summary>
        /// Convert an object using standard object conversion rules.
        /// If the value is not already of an appropriate type it will be converted first by trying
        /// to use an appropriate converter defined in the TypeDescriptor and then if one is not defined
        /// for IConvertible types Convert.ChangeType will be attempted.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static object DefaultConvert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == null || value != null && targetType.IsAssignableFrom(value.GetType())) return value;

            var conv = TypeDescriptor.GetConverter(targetType);
            if (conv != null && value != null && conv.CanConvertFrom(value.GetType()))
            {
                return conv.ConvertFrom(value);
            }

            if (value != null && value is IConvertible) return System.Convert.ChangeType(value, targetType);

            return value;
        }
    }

}
