using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace Binding
{

    /// <summary>
    /// Abstract base class for binding scripts, which allow for a property on one object to
    /// be bound to a property or method of a Unity Game Object.
    /// </summary>
    public abstract class DataBindingBase : MonoBehaviour
    {
        /// <summary>
        /// Get or set whether a refresh of the UI should be performed
        /// on the next update.
        /// </summary>
        protected abstract bool UIRefreshRequired { get; set; }

        /// <summary>
        /// Get or set whether a Binding refresh is required
        /// </summary>
        protected abstract bool BindingRefreshRequired { get; set; }

        // Start is called before the first frame update
        void Start()
        {
            InitialiseBinding();
        }

        // Update is called once per frame
        void Update()
        {
            BindingUpdates();
        }

        void OnDestroy()
        {
            CleanupOnDestroy();
        }

        /// <summary>
        /// Initialisation function called on Start
        /// </summary>
        protected virtual void InitialiseBinding()
        {

        }

        /// <summary>
        /// Refresh the bound UI
        /// </summary>
        public virtual void UpdateTargetValue() { }

        /// <summary>
        /// Rebuild the binding chain to establish property change monitoring
        /// </summary>
        protected virtual void RefreshBinding()
        {
        }

        /// <summary>
        /// Notify this data binding that the source object of a parent data
        /// context has been modified
        /// </summary>
        /// <param name="context"></param>
        public virtual void NotifyDataContextUpdated(DataContext context)
        {

        }

        /// <summary>
        /// Is the UI currently locked for editing?
        /// </summary>
        /// <returns></returns>
        protected virtual bool IsLocked()
        {
            return false;
        }

        /// <summary>
        /// Perform final cleanup of event registrations etc. when this component is destroyed.
        /// Subclasses should override to likewise perform any final tidying up that is required.
        /// </summary>
        protected virtual void CleanupOnDestroy()
        {
        }

        /// <summary>
        /// Process necessary updates due to binding changes
        /// </summary>
        protected virtual void BindingUpdates()
        {
            if (BindingRefreshRequired)
            {
                Debug.Log("Binding Refresh Required");
                RefreshBinding();
            }
            if (UIRefreshRequired)
            {
                //Prevent updating if the field is being edited:
                if (!IsLocked())
                {
                    Debug.Log("UI Refresh Required");
                    UIRefreshRequired = false;
                    UpdateTargetValue();
                }
            }
        }
    }
}
