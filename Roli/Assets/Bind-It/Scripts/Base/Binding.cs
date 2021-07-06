using Binding;
using Nucleus.Game;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Binding
{
    /// <summary>
    /// Static utility functions for binding operations
    /// </summary>
    public static class Binding
    {
        /// <summary>
        /// Generate a binding chain for the specified path on this object.
        /// A binding chain is a list of all objects in the chain which implement the
        /// INotifyPropertyChanged interface accompanied by the name of the property
        /// on that object which, when changed, will require the bindings of objects 
        /// further down the chain to be refreshed.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IList<BindingChainLink> GenerateBindingChain(object obj, string path)
        {
            var result = new List<BindingChainLink>();
            string[] subStrings = path.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            object nextObj = obj;
            for (int i = 0; i < subStrings.Length; i++)
            {
                if (nextObj is INotifyPropertyChanged)
                {
                    result.Add(new BindingChainLink((INotifyPropertyChanged)nextObj, subStrings[i]));
                }
                if (i < subStrings.Length - 1)
                {
                    nextObj = GetFromPath(nextObj, subStrings[i]);
                }
            }
            return result;
        }

        /// <summary>
        /// Set the value of a property on this object at the specified path
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="path">The path, consisting of property names and sub-property names
        /// separated by '.' characters.  For example: 'PropertyName.SubPropertyName.SubSubPropertyName' etc.
        /// Parameterless methods may also be invoked by adding '()', i.e.:
        /// 'PropertyName.SubMethodName().SubSubPropertyName'.
        /// Methods and properties on the optional context object may also be invoked in the same way, via a
        /// '*' redirection.  For example: '*.MethodName()'.  When switching to the context
        /// object the SetSourceObject method on it will be called and the current object or property value
        /// passed in.  This allows for complex operations to be performed in order to return a value
        /// provided that functionality is implemented in a suitable context object provided.</param>
        /// <param name="value">The value to be assigned to the property</param>
        public static void SetByPath(object obj, string path, object value)
        {
            object setOn = obj;
            int iLast = path.LastIndexOf('.');
            if (iLast > 0)
            {
                string rootPath = path.Substring(0, iLast);
                setOn = GetFromPath(obj, rootPath);
                path = path.Substring(iLast + 1);
            }
            if (setOn != null)
            {
                PropertyInfo pInfo = setOn.GetType().GetProperty(path);
                if (value != null && value is IConvertible && !pInfo.PropertyType.IsAssignableFrom(value.GetType()))
                {
                    value = Convert.ChangeType(value, pInfo.PropertyType);
                }
                pInfo.SetValue(setOn, value);
            }
        }

        /// <summary>
        /// Get the value of a property on this object at the specified path
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="path">The path, consisting of property names and sub-property names
        /// separated by '.' characters.  For example: 'PropertyName.SubPropertyName.SubSubPropertyName' etc.
        /// Parameterless methods may also be invoked by adding '()', i.e.:
        /// 'PropertyName.SubMethodName().SubSubPropertyName'.
        /// Methods and properties on the optional context object may also be invoked in the same way, via a
        /// '*' redirection.  For example: '*.MethodName()'.  When switching to the context
        /// object the SetSourceObject method on it will be called and the current object or property value
        /// passed in.  This allows for complex operations to be performed in order to return a value
        /// provided that functionality is implemented in a suitable context object provided.</param>
        /// <returns></returns>
        public static object GetFromPath(object obj, string path)
        {
            foreach (string substring in path.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string token = substring;
                if (obj == null) { return null; }

                Type type = obj.GetType();

                if (token.EndsWith(")")) // Method
                {
                    if (token.EndsWith("()"))
                    {
                        MethodInfo info = type.GetMethod(token.TrimEnd(')', '('), new Type[] { });
                        if (info == null) return null;
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
                        MethodInfo info = type.GetMethod(token, new Type[] { key.GetType() });
                        if (info == null) return null;
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
                        // CUSTOM CODE FOR DATA STORES
                        // This is necessary because invoking properties with indexers doesn't
                        // appear to work correctly in WebGL builds.
                        if (obj is GameElement dOwner && token == "Data")
                        {
                            obj = dOwner.Data[key];
                            continue;
                        }
                        // END OF CUSTOM CODE

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
                        else
                            obj = fInfo.GetValue(obj);
                    }
                    else
                    {
                        if (key != null)
                            obj = info.GetValue(obj, new object[] { key });
                        else obj = info.GetValue(obj, null);
                    }
                }
            }
            return obj;
        }

        /// <summary>
        /// Set the value of a property on this object at the specified path
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="path">The path, consisting of property names and sub-property names
        /// separated by '.' characters.  For example: 'PropertyName.SubPropertyName.SubSubPropertyName' etc.
        /// Parameterless methods may also be invoked by adding '()', i.e.:
        /// 'PropertyName.SubMethodName().SubSubPropertyName'.
        /// Methods and properties on the optional context object may also be invoked in the same way, via a
        /// '*' redirection.  For example: '*.MethodName()'.  When switching to the context
        /// object the SetSourceObject method on it will be called and the current object or property value
        /// passed in.  This allows for complex operations to be performed in order to return a value
        /// provided that functionality is implemented in a suitable context object provided.</param>
        /// <param name="value">The value to be assigned to the property</param>
        /// <param name="context">The (optional) string conversion context object.  If supplies this allows
        /// the '*' symbol to be used within property paths in order to access properties and
        /// functions supplied on the context object.</param>
        public static void SetByPath(object obj, string path, object value, Func<object, Type, object, CultureInfo, object> conversion = null)
        {
            object setOn = obj;
            int iLast = path.LastIndexOf('.');
            if (iLast > 0)
            {
                string rootPath = path.Substring(0, iLast);
                setOn = GetFromPath(obj, rootPath);
                path = path.Substring(iLast + 1);
            }
            if (setOn != null)
            {
                Type setType = setOn.GetType();
                FieldInfo fInfo = setType.GetField(path);
                if (fInfo != null)
                {
                    if (conversion != null)
                    {
                        value = conversion.Invoke(value, fInfo.FieldType, null, CultureInfo.CurrentCulture);
                    }
                    fInfo.SetValue(setOn, value);
                }
                else
                {
                    PropertyInfo pInfo = setType.GetProperty(path);
                    if (pInfo != null)
                    {
                        if (conversion != null)
                        {
                            value = conversion.Invoke(value, pInfo.PropertyType, null, CultureInfo.CurrentCulture);
                        }
                        pInfo.SetValue(setOn, value);
                    }
                }
            }
        }
    }
}
