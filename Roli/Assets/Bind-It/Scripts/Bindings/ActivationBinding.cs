using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace Binding
{

    /// <summary>
    /// A one-way data binding which allows game objects to be activated or deactivated
    /// </summary>
    [AddComponentMenu("Binding/Activation Binding")]
    public class ActivationBinding : ConverterBindingBase
    {
        /// <summary>
        /// The target Unity component to bind to
        /// </summary>
        [Tooltip("The target Unity game object to activate or deactivate.")]
        public GameObject Target;

        public override void UpdateTargetValue()
        {
            if (Target == null) return;
            var value = GetBoundValue();
            var converter = GetConverter();
            try
            {
                bool bValue = (bool)converter.Convert(value, typeof(bool), this, CultureInfo.CurrentCulture);
                Target.SetActive(bValue);
            }
            catch
            {
                //TODO
            }
        }

        protected override void InitialiseBinding()
        {
            UpdateTargetValue();
        }
    }
}
