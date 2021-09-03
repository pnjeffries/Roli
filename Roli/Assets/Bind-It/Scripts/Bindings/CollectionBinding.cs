using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;

namespace Binding
{
    /// <summary>
    /// A special type of binding which binds to a collection and
    /// can be used to generate and remove objects as items are added and
    /// removed from the bound collection
    /// </summary>
    [AddComponentMenu("Binding/Collection Binding")]
    public class CollectionBinding : SingleBindingBase
    {
        #region Properties

        /// <summary>
        /// The collection of templates to be used to generate items of different types
        /// </summary>
        [HideInInspector]
        public TypeTemplate[] ItemTemplates = new TypeTemplate[0];

        /// <summary>
        /// Should the items be grouped?
        /// </summary>
        [Tooltip("Should the items be grouped?")]
        public bool Group = false;

        /// <summary>
        /// The path of the property on each item which should be used for grouping
        /// </summary>
        [Tooltip("The path to the property on each item which is to be used for grouping.")]
        public string GroupPath = null;

        /// <summary>
        /// The template to use
        /// </summary>
        [Tooltip("The template prefab object to be used when generating group containers.")]
        public GameObject GroupTemplate = null;


        /// <summary>
        /// Cached reference to the latest bound collection
        /// </summary>
        private INotifyCollectionChanged _Collection = null;

        private List<object> _AddedItems = null;

        /// <summary>
        /// Items which have been recently added.
        /// Null if all added items have been processed.
        /// </summary>
        public List<object> AddedItems
        {
            get { return _AddedItems; }
            set { _AddedItems = value; }
        }

        private IList<object> _RemovedItems = null;

        /// <summary>
        /// Items which have been recently removed.
        /// Null if all removed items have been processed.
        /// </summary>
        public IList<object> RemovedItems
        {
            get { return _RemovedItems; }
            set { _RemovedItems = value; }
        }

        /// <summary>
        /// Get a boolean value indicating whether it is necessary to refresh
        /// items generated as a result of this binding
        /// </summary>
        public bool ItemsRefreshRequired
        {
            get
            {
                return (_AddedItems != null && _AddedItems.Count > 0) ||
                    (_RemovedItems != null && _RemovedItems.Count > 0);
            }
        }

        /// <summary>
        /// A dictionary which maps source objects to their unity representation
        /// </summary>
        protected IDictionary<object, GameObject> _ItemRepresentationMap = new Dictionary<object, GameObject>();

        /// <summary>
        /// A dictionary which maps group key values to their unity representation
        /// </summary>
        protected IDictionary<object, GameObject> _GroupRepresentationMap = new Dictionary<object, GameObject>();

        #endregion

        #region Methods

        protected override void InitialiseBinding()
        {
            base.InitialiseBinding();
            RefreshCollection();
            BindingUpdates();
        }

        private void OnEnable()
        {
            BindingUpdates();
        }

        /// <summary>
        /// Fully refresh the collection by adding all items currently in the collection
        /// to the AddedItems list.
        /// Call TriggerBindingRefresh after this to attempt to recreate all objects.
        /// </summary>
        public void RefreshCollection()
        {
            // Initialise to starting items in collection (if any)
            var sourceCollection = GetBoundValue();
            if (sourceCollection != null && sourceCollection is IEnumerable)
            {
                var allObjects = new List<object>();
                foreach (var obj in (IEnumerable)sourceCollection)
                {
                    allObjects.Add(obj);
                }
                AddedItems = allObjects;
            }
        }

        /// <summary>
        /// Rebuild the binding chain to establish property change monitoring,
        /// additionally setting up collection change event listening on the
        /// source object (if it is a collection)
        /// </summary>
        protected override void RefreshBinding()
        {
            base.RefreshBinding();

            object newValue = GetBoundValue();
            if (newValue != _Collection)
            {
                // Remove collectionchanged event watcher:
                if (_Collection != null)
                {
                    _Collection.CollectionChanged -= CollectionChanged;
                    if (_Collection is IEnumerable)
                    {
                        // Remove items from the old collection:
                        var enumerable = (IEnumerable)_Collection;
                        if (RemovedItems == null) RemovedItems = new List<object>();
                        foreach (var item in enumerable) RemovedItems.Add(item);
                    }
                }

                // Add collectionchanged event watcher:
                if (newValue != null && newValue is INotifyCollectionChanged)
                {
                    var newCollection = (INotifyCollectionChanged)newValue;
                    newCollection.CollectionChanged += CollectionChanged;
                    _Collection = newCollection;
                    if (newCollection is IEnumerable)
                    {
                        // Add items from the new collection:
                        var enumerable = (IEnumerable)newCollection;
                        AddedItems = new List<object>();
                        foreach (var item in enumerable) AddedItems.Add(item);
                    }
                }
                else _Collection = null;
            }
        }

        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                if (AddedItems == null) AddedItems = new List<object>(e.NewItems.Count);
                foreach (var item in e.NewItems) AddedItems.Add(item);
            }
            if (e.OldItems != null)
            {
                if (RemovedItems == null) RemovedItems = new List<object>(e.OldItems.Count);
                foreach (var item in e.OldItems) RemovedItems.Add(item);
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                // Clear action - remove everything!
                if (RemovedItems == null) RemovedItems = new List<object>(_ItemRepresentationMap.Count);
                foreach (var item in _ItemRepresentationMap.Keys) RemovedItems.Add(item);
            }
        }

        public override void NotifyDataContextUpdated(DataContext context)
        {
            base.NotifyDataContextUpdated(context);

            RefreshCollection();
        }

        protected override void BindingUpdates()
        {
            base.BindingUpdates();

            // Update child items
            if (ItemsRefreshRequired) RefreshChildren();
        }

        /// <summary>
        /// Get the template prefab for the specified object
        /// </summary>
        /// <param name="forObject"></param>
        /// <returns></returns>
        protected virtual GameObject GetTemplate(object forObject)
        {
            int highestScore = 0;
            TypeTemplate bestMatch = null;
            foreach (var template in ItemTemplates)
            {
                if (template != null)
                {
                    int score = template.SuitabilityFor(forObject);
                    if (score > highestScore)
                    {
                        highestScore = score;
                        bestMatch = template;
                    }
                }
            }
            if (bestMatch != null) return bestMatch.Template;//transform.gameObject;

            return null;
        }

        /// <summary>
        /// Create or destroy unity objects to represent the items added to or removed
        /// from the bound collection.
        /// </summary>
        public void RefreshChildren()
        {
            bool refit = false;

            if (RemovedItems != null)
            {
                // Destroy item representations
                foreach (object item in RemovedItems)
                {
                    if (DestroyItemRepresentation(item)) refit = true;
                }
            }

            if (AddedItems != null)
            {
                foreach (object item in AddedItems)
                {
                    if (CreateItemRepresentation(item)) refit = true;
                }
                AddedItems = null;
            }

            if (RemovedItems != null)
            {
                // Destroy empty groups:
                DestroyEmptyGroupRepresentations();

                RemovedItems = null;
            }

            if (refit)
            {
                if (transform is RectTransform)
                {
                    //Force rebuild of any content size fitters:
                    var fitters = GetComponentsInParent<ContentSizeFitter>();
                    // Cycle through in reverse order:
                    for (int i = 0; i < fitters.Length; i++)
                    {
                        var fitter = fitters[i];
                        var rect = fitter.GetComponent<RectTransform>();
                        LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
                    }
                }
            }
        }

        /// <summary>
        /// Create an item representation for the specified item
        /// </summary>
        /// <param name="item"></param>
        /// <returns>True if a new item representation was added</returns>
        private bool CreateItemRepresentation(object item)
        {
            if (!_ItemRepresentationMap.ContainsKey(item))
            {

                // Create representation from template:
                var template = GetTemplate(item);
                if (template != null)
                {
                    Transform addTo = this.transform;
                    if (Group)
                    {
                        object grouper = Binding.GetFromPath(item, GroupPath);
                        if (grouper != null)
                        {

                            var container = CreateOrGetGroupContainer(grouper);
                            if (container != null) addTo = GetGroupContainerItemTransform(container);
                        }
                    }
                    var representation = Instantiate(template, addTo);

                    DataContext.SetDataContextOn(representation, item);
                    _ItemRepresentationMap.Add(item, representation);

                    Debug.Log("Created object to represent '" + item.ToString() + "'.  Now " + addTo.childCount + " items.");

                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Create a group container representation    
        /// /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        private GameObject CreateOrGetGroupContainer(object group)
        {
            if (_GroupRepresentationMap.ContainsKey(group))
            {
                return _GroupRepresentationMap[group];
            }
            else
            {
                var template = GroupTemplate;
                if (template != null)
                {
                    var representation = Instantiate(template, this.transform);
                    DataContext.SetDataContextOn(representation, group);
                    _GroupRepresentationMap.Add(group, representation);
                    return representation;
                }
            }
            return null;
        }

        /// <summary>
        /// Get the transform that will act as the parent of items added to the specified group representation
        /// </summary>
        /// <param name="groupRep"></param>
        /// <returns></returns>
        private Transform GetGroupContainerItemTransform(GameObject groupRep)
        {
            var container = groupRep.GetComponent<GroupContainer>();
            if (container?.ContentsContainer == null) return groupRep.transform;
            return container.ContentsContainer.transform;
        }

        /// <summary>
        /// Delete the current item representation of the specified item
        /// </summary>
        /// <param name="item"></param>
        private bool DestroyItemRepresentation(object item)
        {
            if (_ItemRepresentationMap.ContainsKey(item))
            {
                var representation = _ItemRepresentationMap[item];
                if (representation != null)
                {
                    Destroy(representation);
                    representation.transform.SetParent(null);
                }
                _ItemRepresentationMap.Remove(item);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Delete the current representaion of the specifed group
        /// </summary>
        /// <param name="group"></param>
        private void DestroyGroupRepresentation(object group)
        {
            if (_GroupRepresentationMap.ContainsKey(group))
            {
                var representation = _GroupRepresentationMap[group];
                Destroy(representation);
                _GroupRepresentationMap.Remove(group);
            }
        }

        /// <summary>
        /// Test all current group representations and destroy any which are now empty
        /// </summary>
        private void DestroyEmptyGroupRepresentations()
        {
            // Iterate through group representations and find any which are empty
            IList deleteKeys = null;
            foreach (var kvp in _GroupRepresentationMap)
            {
                var transform = GetGroupContainerItemTransform(kvp.Value);
                if (transform.childCount == 0)
                {
                    if (deleteKeys == null) deleteKeys = new List<object>();
                    deleteKeys.Add(kvp.Key);
                }
            }

            // Destroy identified emptry group representations
            if (deleteKeys != null)
            {
                foreach (var key in deleteKeys) DestroyGroupRepresentation(key);
            }
        }

        private void OnValidate()
        {
            if (ItemTemplates.Length == 0)
            {
                AddItemTemplate(null);
            }
        }

        /// <summary>
        /// Create a new template for the specified target type in this collection binding
        /// </summary>
        /// <param name="forType"></param>
        public void AddItemTemplate(Type forType)
        {
            var template = new TypeTemplate();
            template.TargetType = forType;
            var tempList = new List<TypeTemplate>(ItemTemplates);
            tempList.Add(template);
            ItemTemplates = tempList.ToArray();
        }

        /// <summary>
        /// Create a new template for any enum type (possibly within a generic container)
        /// </summary>
        /// <param name="containerType">The generic container for the enum value.</param>
        public void AddEnumsTemplate(Type containerType = null)
        {
            var template = new TypeTemplate();
            template.AllEnums = true;
            template.TargetType = containerType;
            var tempList = new List<TypeTemplate>(ItemTemplates);
            tempList.Add(template);
            ItemTemplates = tempList.ToArray();
        }

        /// <summary>
        /// Clear any item templates that do not have items assigned
        /// </summary>
        public void ClearUnassignedItemTemplates()
        {
            var tempList = new List<TypeTemplate>(ItemTemplates);
            tempList.RemoveIf(i => i.Template == null);
            ItemTemplates = tempList.ToArray();
        }

        protected override void CleanupOnDestroy()
        {
            base.CleanupOnDestroy();
            if (_Collection != null)
            {
                _Collection.CollectionChanged -= CollectionChanged;
            }
        }

        #endregion
    }
}