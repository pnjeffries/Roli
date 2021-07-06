using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using UnityEngine;

namespace Binding
{
    /// <summary>
    /// Abstract base class for data binding components which bind to a single source property
    /// </summary>
    public abstract class SingleBindingBase : DataBindingBase
    {
        #region Properties

        /// <summary>
        /// The source object for this binding.  Leave null to inherit the
        ///  source from the Data Context on this or a parent object.
        /// </summary>
        [Tooltip("The source object for this binding.  Leave null to inherit the " +
            "source from the Data Context on this or a parent object.")]
        public UnityEngine.Object Source = null;

        /// <summary>
        /// The path to the source property to be bound to, relative to the source object.  
        /// Leave blank to target the source object itself.
        /// </summary>
        [Tooltip("The path to the source property to be bound to, relative to the source object.  " +
            "Leave blank to bind to the source object itself.")]
        public string Path;

        /// <summary>
        /// Private backing member variable for the BindingChain property
        /// </summary>
        private IList<BindingChainLink> _BindingChain = new List<BindingChainLink>();

        /// <summary>
        /// The binding chain - the sequence of objects and properties that lead 
        /// from the data context to the target of the binding.
        /// </summary>
        protected IList<BindingChainLink> BindingChain
        {
            get { return _BindingChain; }
            set { _BindingChain = value; }
        }

        /// <summary>
        /// Get the source object from the current data context of this binding
        /// </summary>
        public object SourceObject
        {
            get
            {
                object result;
                if (Source != null)
                {
                    if (Source is DataContextSourceObject)
                        result = ((DataContextSourceObject)Source).Value;
                    result = Source;
                }
                else result = GetComponentInParent<DataContext>()?.GetSourceObject();

                return result;
            }
        }

        /// <summary>
        /// Is a refresh of the UI necessary?
        /// If true, this should be performed on the next Binding refresh
        /// </summary>
        private bool _UIRefreshRequired = false;

        /// <summary>
        /// Get or set whether a refresh of the UI should be performed
        /// on the next update.
        /// </summary>
        protected override bool UIRefreshRequired
        {
            get { return _UIRefreshRequired; }
            set { _UIRefreshRequired = value; }
        }

        /// <summary>
        /// Private backing field for BindingRefreshRequired
        /// </summary>
        private int _BindingRefreshIndex = 0;

        /// <summary>
        /// Get or set an integer value indicating whether the
        /// binding chain should be refreshed and if so from
        /// which position.  This is indicated by setting the value
        /// to the index of the item in the chain which should be
        /// refreshed.  If the value is lower than 0, no refresh is
        /// required.
        /// </summary>
        protected int BindingRefreshIndex
        {
            get { return _BindingRefreshIndex; }
            set { _BindingRefreshIndex = value; }
        }

        /// <summary>
        /// Get or set whether a Binding refresh is required
        /// </summary>
        protected override bool BindingRefreshRequired
        {
            get { return _BindingRefreshIndex >= 0; }
            set
            {
                if (value) _BindingRefreshIndex = 0;
                else _BindingRefreshIndex = -1;
            }
        }

        /// <summary>
        /// The cached reflection info for the source member.
        /// </summary>
        private ReflectionInfo _CachedSourceInfo = null;

        #endregion

        #region Methods

        /// <summary>
        /// Get the current value of the bound property
        /// </summary>
        /// <returns></returns>
        public virtual object GetBoundValue()
        {
            if (_CachedSourceInfo == null && SourceObject != null)
            {
                if (string.IsNullOrWhiteSpace(Path)) return SourceObject;

                _CachedSourceInfo = ReflectionInfo.GetReflectionInfoFromPath(SourceObject, Path);
            }
            return _CachedSourceInfo?.GetValue();
        }

        /// <summary>
        /// Get the current value of the bound property as the specified type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="converter"></param>
        /// <returns></returns>
        public T GetBoundValue<T>(IValueConverter converter = null)
        {
            return (T)GetBoundValue(typeof(T), converter);
        }

        /// <summary>
        /// Get the current value of the bound property as the specified type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="converter"></param>
        /// <returns></returns>
        public object GetBoundValue(Type targetType, IValueConverter converter)
        {
            object value = GetBoundValue();
            if (converter != null) value = converter.Convert(value, targetType, this, CultureInfo.CurrentCulture);
            return value;
        }

        /// <summary>
        /// Set the current value of the bound property
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool SetBoundValue(object value, IValueConverter converter = null)
        {
            if (_CachedSourceInfo == null && SourceObject != null)
            {
                if (string.IsNullOrWhiteSpace(Path)) return false;
                _CachedSourceInfo = ReflectionInfo.GetReflectionInfoFromPath(SourceObject, Path);
            }
            if (_CachedSourceInfo != null)
            {
                return _CachedSourceInfo.SetValue(value, converter.ConvertBack);
            }
            return false;
        }

        /// <summary>
        /// Rebuild the binding chain to establish property change monitoring
        /// </summary>
        protected override void RefreshBinding()
        {
            BindingRefreshRequired = false;

            ClearCache();

            // Clear old binding monitoring:
            if (BindingChain != null)
            {
                BindingChain.RemovePropertyChangedHandler(Source_PropertyChanged);
            }

            object context = SourceObject;
            if (context != null && !string.IsNullOrEmpty(Path))
            {
                BindingChain = Binding.GenerateBindingChain(context, Path);
                BindingChain.AddPropertyChangedHandler(Source_PropertyChanged);
                var sb = new StringBuilder();
                foreach (var link in BindingChain)
                {
                    sb.Append(" - ");
                    sb.Append("(");
                    sb.Append(link.Source);
                    sb.Append(")");
                    sb.Append(link.PropertyName);
                }
                Debug.Log("Chain generated: " + sb.ToString());
            }
        }

        /// <summary>
        /// Clear data stored temporarily for the sake of efficiency
        /// </summary>
        protected void ClearCache()
        {
            _CachedSourceInfo = null;
        }

        /// <summary>
        /// Notify this data binding that the source object of a parent data
        /// context has been modified
        /// </summary>
        /// <param name="context"></param>
        public override void NotifyDataContextUpdated(DataContext context)
        {
            if (Source == null &&
                GetComponentInParent<DataContext>()?.GetControllingDataContext() == context)
            {
                BindingRefreshIndex = 0;
                UIRefreshRequired = true;
                ClearCache();
            }
        }

        /// <summary>
        /// Handles propertychanged events on bound source objects
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Source_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Debug.Log("Property '" + e.PropertyName + "' changed.");
            if (BindingChain != null && sender is INotifyPropertyChanged)
            {
                int i = BindingChain.IndexOfSource((INotifyPropertyChanged)sender);
                if (i >= 0)
                {
                    var link = BindingChain[i];
                    if (e.PropertyName.EqualsIgnoreCase(link.PropertyName))
                    {
                        UIRefreshRequired = true;

                        if (i < BindingChain.Count && (BindingRefreshIndex < 0 || i < BindingRefreshIndex))
                            BindingRefreshIndex = i;

                        ClearCache();
                    }
                }
            }
        }

        /// <summary>
        /// Perform final cleanup of event registrations etc. when this component is destroyed.
        /// Subclasses should override to likewise perform any final tidying up that is required.
        /// </summary>
        protected override void CleanupOnDestroy()
        {
            // Clear old binding monitoring:
            if (BindingChain != null)
            {
                BindingChain.RemovePropertyChangedHandler(Source_PropertyChanged);
            }
        }

        #endregion
    }
}
