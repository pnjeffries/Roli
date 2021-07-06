using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace Binding
{

    /// <summary>
    /// A component responsible for defining the data context for binding operations
    /// on a game object.  Source binding paths without an overriding Source object defined
    /// will be relative to this component's SourceObject property.
    /// </summary>
    [AddComponentMenu("Binding/Data Context")]
    public class DataContext : MonoBehaviour
    {
        /// <summary>
        /// The source object for this DataContext
        /// </summary>
        public Object Source = null;

        /// <summary>
        /// Get the source object for this data context.
        /// If the source is null this will iterate up the hierarchy
        /// of game objects until a non-null data context source is
        /// found.
        /// </summary>
        public object SourceObject
        {
            get { return GetSourceObject(); }
        }

        /// <summary>
        /// Get the source object for this data context.
        /// If the source is null this will iterate up the hierarchy
        /// of game objects until a non-null data context source is
        /// found.
        /// </summary>
        public object GetSourceObject()
        {
            if (Source != null)
            {
                if (Source is DataContextSourceObject)
                    return ((DataContextSourceObject)Source).Value;
                else
                    return Source;
            }
            else
            {
                if (transform.parent == null)
                {
                    return null;
                }
                DataContext parentContext = transform.parent.GetComponentInParent<DataContext>();
                return parentContext?.GetSourceObject();
            }
        }

        /// <summary>
        /// Get the DataContext component which defines the current source object
        /// </summary>
        /// <returns></returns>
        public DataContext GetControllingDataContext()
        {
            if (Source != null) return this;

            DataContext parentContext = transform.parent.GetComponentInParent<DataContext>();
            return parentContext?.GetControllingDataContext();
        }

        /// <summary>
        /// Set the source object of this DataContext
        /// </summary>
        /// <param name="source"></param>
        public void SetSourceObject(object source)
        {
            if (source is Object) Source = (Object)source;
            else
            {
                var wrapper = ScriptableObject.CreateInstance<DataContextSourceObject>();
                wrapper.Value = source;
                Source = wrapper;
            }
            // Refresh any dependent bindings
            foreach (DataBindingBase binding in GetComponentsInChildren<DataBindingBase>())
            {
                binding.NotifyDataContextUpdated(this);
            }
        }


        #region Static Methods

        /// <summary>
        /// Set the data context source on a game object.  This will automatically
        /// create a data context component on the specified object if one does 
        /// not already exist.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DataContext SetDataContextOn(GameObject obj, object source)
        {
            var context = obj.GetComponent<DataContext>();
            if (context == null) context = obj.AddComponent<DataContext>();
            context.SetSourceObject(source);
            return context;
        }

        /// <summary>
        /// Get the next DataContext in the object hierarchy above that which applies to the specified object.
        /// Equivalent to WPF's RelativeSource FindAncestor
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DataContext GetAncestorDataContext(GameObject obj, int ancestorLevels = 1)
        {   
            var objDC = obj?.GetComponentInParent<DataContext>();
            if (ancestorLevels <= 0) return objDC;
            if (objDC == null) return null;
            Transform pObj = objDC.transform.parent;
            var gObj = pObj?.gameObject;
            if (gObj != null && ancestorLevels > 1)
            {
                return GetAncestorDataContext(gObj, ancestorLevels - 1);
            }
            else return gObj?.GetComponentInParent<DataContext>();
        }
        #endregion
    }
}

