using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Binding
{
    /// <summary>
    /// Component to trigger playing an animation when a bound value changes
    /// </summary>
    [AddComponentMenu("Binding/Animation Trigger Binding")]
    public class AnimationTriggerBinding : TriggerBindingBase
    {
        [Tooltip("The animation to play when the bound value changes")]
        public Animation Animation;

        protected override void FireTrigger()
        {
            if (Animation == null) return;

            Animation.Play();
        }
    }

}
