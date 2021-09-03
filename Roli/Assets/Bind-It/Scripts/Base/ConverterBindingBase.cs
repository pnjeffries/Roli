using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Binding
{
    /// <summary>
    /// Abstract base class for binding components which use ValueConverters
    /// </summary>
    public abstract class ConverterBindingBase : SingleBindingBase
    {
        /// <summary>
        /// The converter to use to convert the bound value to the type of the target field and back again.
        /// </summary>
        [HideInInspector]
        public ValueConverter Converter;

        protected IValueConverter GetConverter()
        {
            if (Converter == null) return DefaultConverter.Instance;
            return Converter;
        }
    }
}
