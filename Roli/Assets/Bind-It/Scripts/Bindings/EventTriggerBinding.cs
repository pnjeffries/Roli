using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Binding
{
    /// <summary>
    /// Component which raises a Unity event when a bound value changes
    /// </summary>
    [AddComponentMenu("Binding/Event Trigger Binding")]
    public class EventTriggerBinding : TriggerBindingBase
    {
        [Tooltip("Event raised when a change of the specified type occurs.  " +
            "The parameter passed is the new value of the bound property.")]
        public UnityEvent<object> ValueChanged = new UnityEvent<object>();

        protected override void FireTrigger()
        {
            var value = GetBoundValue();
            ValueChanged?.Invoke(value);
        }
    }
}