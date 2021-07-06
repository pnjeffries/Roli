using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binding
{
    /// <summary>
    /// The default converter used in bindings which do not have an overriding converter
    /// applied.  Performs no conversion - simply passes the initial object array directly
    /// to the target.
    /// </summary>
    [Serializable]
    public class DefaultMultiConverter : MultiValueConverter
    {
        /// <summary>
        /// Cached DefaultConverter instance
        /// </summary>
        private static DefaultMultiConverter _Instance;

        /// <summary>
        /// A reusable cached instance of the DefaultConverter
        /// </summary>
        public static DefaultMultiConverter Instance
        {
            get
            {
                if (_Instance == null) _Instance = CreateInstance(typeof(DefaultMultiConverter)) as DefaultMultiConverter;
                return _Instance;
            }
        }

        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values;
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("ConvertBack operations are not supported by the default MultiValueConverter");
        }
    }
}
