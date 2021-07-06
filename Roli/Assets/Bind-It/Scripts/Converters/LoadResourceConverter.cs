using System;
using System.Globalization;
using UnityEngine;

namespace Binding
{
    /// <summary>
    /// A ValueConverter which takes in a path string and loads and returns the resource at that path.
    /// </summary>
    [Serializable]
    public class LoadResourceConverter : ValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Resources.Load(value.ToString());
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("LoadResourceConverter is one-way only.");
        }
    }
}
