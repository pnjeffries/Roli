using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binding
{
    /// <summary>
    /// MultiValueConverter which returns true if all passed in values are equal, or false if not
    /// </summary>
    [Serializable]
    public class AllEqualConverter : MultiValueConverter
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 0) return false;
            var testValueA = values[0];
            for (int i = 1; i < values.Length; i++)
            {
                var testValueB = values[i];
                if (!object.Equals(testValueA, testValueB)) return false;
            }
            return true;
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("AllEqualConverter is one-way only.");
        }

    }
}
