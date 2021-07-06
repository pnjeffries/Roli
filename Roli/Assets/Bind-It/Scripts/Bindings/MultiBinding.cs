using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Binding
{
    /// <summary>
    /// A binding type which combines together multiple bindings
    /// to different objects to derive a value for one target value
    /// </summary>
    [AddComponentMenu("Binding/Multi-Binding")]
    public class MultiBinding : MultiBindingBase
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
        public MultiValueConverter Converter;

        protected IMultiValueConverter GetConverter()
        {
            if (Converter == null) return DefaultMultiConverter.Instance;
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
                if (_TargetInfo == null)
                {
                    _TargetInfo = ReflectionInfo.GetReflectionInfoFromPath(Target, TargetPath);
                }
                return _TargetInfo;
            }
        }

        public override void UpdateTargetValue()
        {
            if (Target == null) return;

            TargetInfo.SetValue(GetBoundValues(), GetConverter().Convert);
        }

        protected override void InitialiseBinding()
        {
            base.InitialiseBinding();
            UpdateTargetValue();
        }
    }
}

