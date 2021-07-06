using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Binding
{

    /// <summary>
    /// Class to wrap an object as a Unity Object to make it an acceptable
    /// value for inspector display.
    /// </summary>
    [Serializable]
    public class DataContextSourceObject : ScriptableObject
    {
        /// <summary>
        /// The wrapped object
        /// </summary>
        public object Value;

        /// <summary>
        /// Default constructor
        /// </summary>
        public DataContextSourceObject()
        {

        }

        /// <summary>
        /// Create a new DataContextSourceObject to wrap the specified value
        /// </summary>
        /// <param name="value"></param>
        public DataContextSourceObject(object value) : base()
        {
            Value = value;
        }
    }
}
