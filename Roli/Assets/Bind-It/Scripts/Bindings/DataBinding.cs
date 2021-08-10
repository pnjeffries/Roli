using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Binding
{
    /// <summary>
    /// A customisable data binding which allows for direct binding of a model object
    /// property to a field of a target component
    /// </summary>
    [AddComponentMenu("Binding/Data Binding")]
    public class DataBinding : SingleBindingBase
    {
        /// <summary>
        /// The target Unity component to bind to
        /// </summary>
        [Tooltip("The target Unity component to bind to.")]
        public Component Target;

        /// <summary>
        /// The path on the target component to bind to
        /// </summary>
        [Tooltip("The path on the target component to bind to.")]
        public string TargetPath;

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

        private ReflectionInfo _TargetInfo = null;

        /// <summary>
        /// Get the target binding ReflectionInfo
        /// </summary>
        private ReflectionInfo TargetInfo
        {
            get
            {
                return ReflectionInfo.GetReflectionInfoFromPath(Target, TargetPath);
                //if (_TargetInfo == null)
                //{
                //  _TargetInfo = ReflectionInfo.GetReflectionInfoFromPath(Target, TargetPath);
                //}
                //return _TargetInfo;
            }
        }

        public override void UpdateTargetValue()
        {
            if (Target == null) return;
            var value = GetBoundValue();
            var converter = GetConverter();
            try
            {
                TargetInfo.SetValue(value, converter.Convert);
            }
            catch
            {
                //TODO
            }
        }

        /// <summary>
        /// Update the bound value from the current value of the target field
        /// </summary>
        public void UpdateSourceValue()
        {
            if (Target == null) return;

            object newValue = Binding.GetFromPath(Target, TargetPath);

            SetBoundValue(newValue, GetConverter());
        }

        protected override void InitialiseBinding()
        {
            UpdateTargetValue();
        }
    }
}
