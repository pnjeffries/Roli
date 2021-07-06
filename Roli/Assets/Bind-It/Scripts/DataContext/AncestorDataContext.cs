using UnityEngine;

namespace Binding
{

    /// <summary>
    /// A Component to wrap and return the results of a search for the datacontext of
    /// the parent of the GameObject this behaviour is attached to.
    /// </summary>
    [AddComponentMenu("Binding/Ancestor Data Context")]
    public class AncestorDataContext : MonoBehaviour
    {
        [Tooltip("The number of levels of change of data context to traverse.")]
        public int AncestorLevels = 1;

        /// <summary>
        /// Get the source UnityObject of the ancestor datacontext
        /// </summary>
        public Object Source
        {
            get
            {
                return DataContext.GetAncestorDataContext(this.gameObject)?.Source;
            }
        }

        /// <summary>
        /// Get the source object of the ancestor datacontext
        /// </summary>
        public object SourceObject
        {
            get
            {
                var source = DataContext.GetAncestorDataContext(this.gameObject)?.Source;
                if (source != null && source is DataContextSourceObject sObj) return sObj.Value; 
                return source;
            }
        }
    }
}
