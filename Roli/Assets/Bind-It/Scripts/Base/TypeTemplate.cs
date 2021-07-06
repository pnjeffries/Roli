using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Binding
{

    /// <summary>
    /// A record which matches a type to a prefab template
    /// </summary>
    [Serializable]
    public class TypeTemplate : object
    {
        public string _FullTypeName = null;

        /// <summary>
        /// The target type of the item template
        /// </summary>
        public Type TargetType
        {
            get
            {
                if (_FullTypeName == null) return null;
                return Type.GetType(_FullTypeName);
            }
            set
            {
                _FullTypeName = value?.AssemblyQualifiedName;
            }
        }

        /// <summary>
        /// The name to be displayed in the editor UI
        /// </summary>
        public virtual string DisplayName
        {
            get
            {
                if (AllEnums)
                {
                    if (TargetType == null) return "Enums";
                    else return TargetType.Name + "<[Enum]>";
                }
                return TargetType == null ? "[Default]" : TypeNameWithGenericParameters(TargetType);
            }
        }

        /// <summary>
        /// A fallback type name to be used when the TargetType is not set or is invalid
        /// </summary>
        public string TypeShortName = null;

        /// <summary>
        /// If true, will accept all enum types or generic types with enum generic parameters
        /// </summary>
        public bool AllEnums = false;

        /// <summary>
        /// The template gameobject
        /// </summary>
        public GameObject Template = null;

        public TypeTemplate() { }

        /// <summary>
        /// Return a 'score' indicating the suitability of this template
        /// to be used to represent the specified object.
        /// The higher this score, the better the match.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public virtual int SuitabilityFor(object target)
        {
            if (target == null) return 0;

            Type targetType = target.GetType();
            if (AllEnums)
            {
                if (targetType.IsEnum) return 7;
                else if (TargetType.IsAssignableFrom(targetType) && targetType.IsGenericType)
                {
                    if (targetType.GetGenericArguments()[0].IsEnum) return 7;
                }
            }
            else
            {
                if (TargetType == null) return 5;
                else if (targetType == TargetType) return 10;
                else if (targetType.Name.EqualsIgnoreCase(TypeShortName)) return 9;
                else if (TargetType.IsAssignableFrom(targetType)) return 8;
            }
            return 0;
        }

        /// <summary>
        /// Get the name of this type including any generic parameters
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private static string TypeNameWithGenericParameters(Type t)
        {
            if (!t.IsGenericType)
                return t.Name;

            StringBuilder sb = new StringBuilder();
            sb.Append(t.Name.Substring(0, t.Name.IndexOf('`')));
            sb.Append('<');
            bool appendComma = false;
            foreach (Type arg in t.GetGenericArguments())
            {
                if (appendComma) sb.Append(',');
                sb.Append(TypeNameWithGenericParameters(arg));
                appendComma = true;
            }
            sb.Append('>');
            return sb.ToString();
        }

    }
}
