using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Binding
{
    /// <summary>
    /// A struct to temporarily store reflected member info to avoid having to
    /// repeat costly reflection operations
    /// </summary>
    [Serializable]
    public class ReflectionInfo
    {
        public object Obj;
        public MemberInfo Info;
        public string Key;

        /// <summary>
        /// Get the type of the property, field or method
        /// </summary>
        /// <returns></returns>
        public Type Type
        {
            get
            {
                if (Info is PropertyInfo pInfo) return pInfo.PropertyType;
                if (Info is FieldInfo fInfo) return fInfo.FieldType;
                if (Info is MethodInfo mInfo) return mInfo.ReturnType;
                return null;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="info"></param>
        /// <param name="key"></param>
        public ReflectionInfo(object obj, MemberInfo info, string key = null)
        {
            Obj = obj;
            Info = info;
            Key = key;
        }

        /// <summary>
        /// Get the value from the object and memberinfo
        /// </summary>
        /// <returns></returns>
        public object GetValue()
        {
            if (Info is PropertyInfo pInfo)
            {
                if (Key != null)
                    return pInfo.GetValue(Obj, new object[] { Key });
                else return pInfo.GetValue(Obj, null);
            }
            else if (Info is FieldInfo fInfo)
            {
                return fInfo.GetValue(Obj);
            }
            else if (Info is MethodInfo mInfo)
            {
                if (Key != null)
                    return mInfo.Invoke(Obj, new object[] { Key });
                else
                    return mInfo.Invoke(Obj, null);
            }
            return null;
        }

        /// <summary>
        /// Set the value on the object via the memberInfo
        /// </summary>
        /// <param name="value"></param>
        public bool SetValue(object value, Func<object, Type, object, CultureInfo, object> conversion = null)
        {
            if (Info is FieldInfo fInfo)
            {
                if (conversion != null)
                {
                    value = conversion.Invoke(value, fInfo.FieldType, null, CultureInfo.CurrentCulture);
                }
                fInfo.SetValue(Obj, value);
                return true;
            }
            else if (Info is PropertyInfo pInfo)
            {
                if (conversion != null)
                {
                    value = conversion.Invoke(value, pInfo.PropertyType, null, CultureInfo.CurrentCulture);
                }
                pInfo.SetValue(Obj, value);
                return true;
            }
            else if (Info is MethodInfo mInfo)
            {
                var paras = mInfo.GetParameters();
                if (paras.Length != 1) throw new ArgumentException("Target method does not have a single parameter.");
                if (conversion != null)
                {
                    value = conversion.Invoke(value, paras[0].ParameterType, null, CultureInfo.CurrentCulture);
                }
                mInfo.Invoke(Obj, new object[] { value });
            }
            return false;
        }

        /// <summary>
        /// Set the value on the object via the memberInfo
        /// </summary>
        /// <param name="values"></param>
        /// <param name="conversion"></param>
        /// <returns></returns>
        public bool SetValue(object[] values, Func<object[], Type, object, CultureInfo, object> conversion)
        {
            object value = values;
            if (conversion != null)
            {
                value = conversion.Invoke(values, Type, null, CultureInfo.CurrentCulture);
            }
            return SetValue(value);
        }

        /// <summary>
        /// Get a set of reflected member information from the 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static ReflectionInfo GetReflectionInfoFromPath(object obj, string path)
        {
            var tokens = path.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < tokens.Length; i++)
            {
                bool last = i == tokens.Length - 1;
                string token = tokens[i];
                if (obj == null) { return null; }

                Type type = obj.GetType();

                if (token.EndsWith(")")) // Method
                {
                    if (token.EndsWith("()"))
                    {
                        var info = type.GetMethod(token.TrimEnd(')', '('), new Type[] { });
                        if (info == null) return null;
                        else if (last) return new ReflectionInfo(obj, info);
                        else
                            obj = info.Invoke(obj, null);
                    }
                    else
                    {
                        string key = null;
                        token = token.TrimEnd(')');
                        int keyStart = token.LastIndexOf('(');
                        if (keyStart >= 0)
                        {
                            key = token.Substring(keyStart + 1);
                            token = token.Substring(0, keyStart);
                        }
                        MethodInfo info = null;
                        /*if (key.StartsWith("<") && key.EndsWith(">"))
                        {
                            key = key.TrimStart('<').TrimEnd('>');

                        }
                        else
                        {*/
                            info = type.GetMethod(token, new Type[] { key.GetType() });
                        //}
                        if (info == null) return null;
                        else if (last) return new ReflectionInfo(obj, info, key);
                        obj = info.Invoke(obj, new object[] { key });
                    }
                }
                else // Property...
                {
                    PropertyInfo info;
                    string key = null;
                    if (token.EndsWith("]"))
                    {
                        token = token.TrimEnd(']');
                        int keyStart = token.LastIndexOf('[');
                        if (keyStart >= 0)
                        {
                            key = token.Substring(keyStart + 1);
                            token = token.Substring(0, keyStart);
                            if (keyStart == 0) token = "Item";
                        }
                    }
                    if (key != null)
                    {
                        info = type.GetProperty(token, new Type[] { key.GetType() });
                        if (info == null)
                        {
                            //Property accessor isn't indexed, but maybe the object itself is...
                            info = type.GetProperty(token);
                            if (info != null)
                            {
                                obj = info.GetValue(obj, null);
                                if (obj != null)
                                {
                                    info = obj.GetType().GetProperty("Item", new Type[] { key.GetType() });
                                }
                            }
                        }
                    }
                    else info = type.GetProperty(token);
                    if (info == null)
                    {
                        //...or Field?
                        FieldInfo fInfo = type.GetField(token);
                        if (fInfo == null) return null;
                        else if (last) return new ReflectionInfo(obj, fInfo);
                        else
                            obj = fInfo.GetValue(obj);
                    }
                    else if (last)
                    {
                        return new ReflectionInfo(obj, info, key);
                    }
                    else
                    {
                        if (key != null)
                            obj = info.GetValue(obj, new object[] { key });
                        else obj = info.GetValue(obj, null);
                    }
                }
            }
            return null;
        }
    }
}
