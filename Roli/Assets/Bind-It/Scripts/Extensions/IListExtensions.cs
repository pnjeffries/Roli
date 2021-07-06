using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Binding
{
    public static class IListExtensions
    {
        /// <summary>
        /// Remove all items from this collection for which the specified delegate function returns true
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="removeIfTrue"></param>
        /// <returns></returns>
        public static int RemoveIf<T>(this IList<T> items, Func<T, bool> removeIfTrue)
        {
            int count = 0;
            for (int i = items.Count - 1; i >= 0; i--)
            {
                if (removeIfTrue(items[i]))
                {
                    items.RemoveAt(i);
                    count++;
                }
            }
            return count;
        }
    }
}
