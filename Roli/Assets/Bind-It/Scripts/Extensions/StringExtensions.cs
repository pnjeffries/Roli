using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Binding
{

    /// <summary>
    /// Extension methods for the standard string class
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Determines whether one string and another are equal, ignoring differences in case
        /// </summary>
        /// <param name="thisString"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool EqualsIgnoreCase(this string thisString, string other)
        {
            if (thisString == null) return (other == null);
            return thisString.Equals(other, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
