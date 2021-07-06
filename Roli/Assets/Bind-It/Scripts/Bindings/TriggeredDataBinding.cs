using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Binding
{
    /// <summary>
    /// A specialised type of data binding which only updates when manually
    /// triggered to do so
    /// </summary>
    [AddComponentMenu("Binding/Triggered Data Binding")]
    public class TriggeredDataBinding : DataBinding
    {
        /// <summary>
        /// Always false
        /// </summary>
        protected override bool UIRefreshRequired { get => false; set => base.UIRefreshRequired = value; }

        protected override void InitialiseBinding()
        {
            // No initial setting
        }

    }
}
