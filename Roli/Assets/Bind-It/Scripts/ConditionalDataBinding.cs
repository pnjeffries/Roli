using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Binding
{
    /// <summary>
    /// A data binding based on a condition comparing the bound value to
    /// </summary>
    public class ConditionalDataBinding : DataBinding
    {
        [SerializeField]
        [Tooltip("The conditional operator to use.")]
        public ComparisonType Comparison = ComparisonType.Equal;

        [Tooltip("The value or object to compare the source value to.")]
        public object CompareTo = null;

        /// <summary>
        /// The comparison condition
        /// </summary>
        [Serializable]
        public enum ComparisonType
        {
            Equal = 0,
            NotEqual = 1,
            GreaterThan = 2,
            LessThan = 3,
            GreaterThanOrEqual = 4,
            LessThanOrEqual = 5,
        }

    }
}