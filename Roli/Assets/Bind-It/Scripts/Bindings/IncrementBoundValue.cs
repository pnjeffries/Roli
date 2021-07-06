using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Binding
{
    /// <summary>
    /// A component which increments a bound numerical value
    /// </summary>
    [AddComponentMenu("Binding/Increment Bound Value")]
    public class IncrementBoundValue : SingleBindingBase
    {
        [Tooltip("The size of the increment")]
        public double Increment = 1.0;

        public void IncrementValue()
        {
            double value = GetBoundValue<double>(DefaultConverter.Instance);
            SetBoundValue(value + Increment, DefaultConverter.Instance);
        }
    }

}