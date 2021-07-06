using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binding
{
    /// <summary>
    /// Boolean MultiValueConverter which returns true if any of the source array values equate to true.
    /// </summary>
    [Serializable]
    public class OrConverter : MultiValueConverter
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            foreach (var value in values)
            {
                bool b = System.Convert.ToBoolean(value);
                if (b) return true;
            }
            return false;
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("OrConverter is one-way only.");
        }
    }
}
