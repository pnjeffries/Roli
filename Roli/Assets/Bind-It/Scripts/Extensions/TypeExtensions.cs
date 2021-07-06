using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binding
{
    /// <summary>
    /// Extension methods on types and collections of types
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// The number of levels of inheritance between this type and a type that
        /// is somewhere in its inheritance chain.
        /// </summary>
        /// <param name="type">This type</param>
        /// <param name="ancestorType">A type which is an ancestor of this one</param>
        /// <param name="interfaceProxy">The value to be returned in the case that the 
        /// specified type is not a direct ancestor but is still assignable</param>
        /// <returns>If the specified type is an ancestor of this one, the number of
        /// inheritance levels between the two types.  If the specified type is this 
        /// type, 0.  If the specified type cannot be found in the inheritance chain,
        /// -1.</returns>
        public static int InheritanceLevelsTo(this Type type, Type ancestorType, int interfaceProxy = -1)
        {
            int count = 0;
            while (type != null && type != ancestorType)
            {
                count++;
                type = type.BaseType;
            }
            if (type == ancestorType) return count;
            else if (interfaceProxy >= 0 && ancestorType.IsAssignableFrom(type))
                return interfaceProxy;
            else return -1;
        }

        /// <summary>
        /// Find the type in this set of types which is the least number of
        /// inheritance levels above the specified type.
        /// </summary>
        /// <param name="forType">The type to seach for</param>
        /// <param name="inTypes">The collection of types to look within</param>
        /// <param name="includeSelf">If true (default) the type itself may be returned if found.
        /// Otherwise it will be excluded from the search and only its ancestors may be returned.</param>
        /// <param name="interfaceProxy">The number of inheritance levels to be assumed in the case of
        /// compatible interfaces</param>
        /// <returns>The type in this collection that is closest in the inheritance
        /// hierarchy to the specified type.  Or, null if the type does not have an
        /// ancestor in the collection.</returns>
        public static Type ClosestAncestor(this IEnumerable<Type> inTypes, Type forType, bool includeSelf = true, int interfaceProxy = 100000)
        {
            int minDist = -1;
            Type closest = null;
            int distLimit = 0;
            if (!includeSelf) distLimit = 1;
            foreach (Type ancestorType in inTypes)
            {
                int dist = forType.InheritanceLevelsTo(ancestorType, interfaceProxy);
                if (dist >= distLimit && (minDist < 0 || dist < minDist))
                {
                    minDist = dist;
                    closest = ancestorType;
                }
            }
            return closest;
        }

        /// <summary>
        /// Find the type in this set of types which is the least number of
        /// inheritance levels below the specified type.
        /// </summary>
        /// <param name="forType">The type to seach for</param>
        /// <param name="inTypes">The collection of types to look within</param>
        /// <returns>The type in this collection that is closest in the inheritance
        /// hierarchy to the specified type.  Or, null if the type does not have a
        /// descendent in the collection.</returns>
        public static Type ClosestDescendent(this IEnumerable<Type> inTypes, Type forType)
        {
            int minDist = -1;
            Type closest = null;
            foreach (Type descendentType in inTypes)
            {
                int dist = descendentType.InheritanceLevelsTo(forType);
                if (dist >= 0 && (minDist < 0 || dist < minDist))
                {
                    minDist = dist;
                    closest = descendentType;
                }
            }
            return closest;
        }

        /// <summary>
        /// Is this a collection type? i.e. does it implement ICollection?
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsCollection(this Type type)
        {
            return typeof(ICollection).IsAssignableFrom(type)
                   || typeof(ICollection<>).IsAssignableFrom(type);
        }

        /// <summary>
        /// Is this an enumerable type?  i.e. does it implement IEnumerable?
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsEnumerable(this Type type)
        {
            return typeof(IEnumerable).IsAssignableFrom(type)
                   || typeof(IEnumerable<>).IsAssignableFrom(type);
        }

        /// <summary>
        /// Is this a List type?  i.e. does it implement IList?
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsList(this Type type)
        {
            return typeof(IList).IsAssignableFrom(type)
                   || typeof(IList<>).IsAssignableFrom(type);
        }

        /// <summary>
        /// Is this a dictionary type?
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsDictionary(this Type type)
        {
            return typeof(IDictionary).IsAssignableFrom(type)
                || typeof(IDictionary<,>).IsAssignableFrom(type);
        }

        /// <summary>
        /// Is this the standard CLR Dictionary type or a subclass of it?
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsStandardDictionary(this Type type)
        {
            return (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>));
        }
    }
}
