using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Binding
{
    /// <summary>
    /// A Component which can invoke a function on a databound object
    /// </summary>
    [AddComponentMenu("Binding/Invoke Bound Function")]
    public class InvokeBoundFunction : SingleBindingBase
    {
        //BODGE TIME! Override to prevent function being invoked accidentally
        public override object GetBoundValue()
        {
            return null;
        }

        public void InvokeFunction()
        {
            base.GetBoundValue();
        }
    }
}
