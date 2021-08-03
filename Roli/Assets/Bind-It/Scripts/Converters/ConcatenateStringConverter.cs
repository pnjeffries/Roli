using Binding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;

namespace Binding
{
    [Serializable]
    public class ConcatenateStringConverter : MultiValueConverter
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var sb = new StringBuilder();
            foreach (var value in values)
            {
                sb.Append(value?.ToString());
            }
            return sb.ToString();
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
