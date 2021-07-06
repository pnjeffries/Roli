using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Binding
{
    /// <summary>
    /// A base class for bindings which trigger some kind of action when the
    /// bound value changes
    /// </summary>
    public abstract class TriggerBindingBase : SingleBindingBase
    {
        [Tooltip("The type of change on which the trigger should fire.  " +
            "By default any change will do, but if the bound type is IComparable " +
            "'Increased' or 'Decreased' may be used to only trigger on positive or " +
            "negative changes respectively.")]
        public TriggerCondition TriggerWhen = TriggerCondition.Changed;

        /// <summary>
        /// The cached last value of the binding, used to test whether the change is an increase
        /// or a decrease.
        /// </summary>
        private IComparable _LastValue;

        protected override void InitialiseBinding()
        {
            base.InitialiseBinding();

            if (TriggerWhen != TriggerCondition.Changed)
            {
                var value = GetBoundValue();
                if (value != null && value is IComparable comp) _LastValue = comp;
            }
        }

        /// <summary>
        /// Trigger the event/animation/whatever
        /// </summary>
        protected abstract void FireTrigger();

        public override void UpdateTargetValue()
        {
            base.UpdateTargetValue();

            if (TriggerWhen != TriggerCondition.Changed)
            {
                bool trigger = true;
                var value = GetBoundValue();
                if (_LastValue != null)
                {
                    int comparison = _LastValue.CompareTo(value);
                    if ((TriggerWhen == TriggerCondition.Increased && comparison >= 0) ||
                        (TriggerWhen == TriggerCondition.Decreased && comparison <= 0))
                    {
                        // Don't trigger now
                        trigger = false;
                    }
                }
                if (value != null && value is IComparable comp) _LastValue = comp;
                else _LastValue = null;
                if (!trigger) return;
            }
            FireTrigger();
        }

        /// <summary>
        /// Enum describing the condition under which the trigger will fire
        /// </summary>
        [Serializable]
        public enum TriggerCondition
        {
            Changed = 0,
            Increased = 1,
            Decreased = 2
        }
    }
}
