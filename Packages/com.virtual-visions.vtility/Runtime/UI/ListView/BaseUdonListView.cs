using System;
using JetBrains.Annotations;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;

namespace VirtualVisions.VTility
{

    public enum LayoutDirection
    {
        Column,
        Row,
    }

    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public abstract class BaseUdonListView : UdonSharpBehaviour
    {
        /// <summary>
        /// Called when data is bound to a specific item.
        /// Listeners must contain a [RectTransform: itemObject] [Int: index] [DataToken: itemValue] variable within a DataList.
        /// </summary>
        [PublicAPI]
        public UdonAction OnBindItem =>
            (UdonAction)(_onBindItem != null ? _onBindItem : _onBindItem = UdonAction.Create());

        private DataDictionary _onBindItem;

        /// <summary>
        /// Called when an item is focused in the list.
        /// Listeners must contain a [RectTransform: item] variable.
        /// </summary>
        [PublicAPI]
        public UdonAction OnItemSelected =>
            (UdonAction)(_onItemSelected != null ? _onItemSelected : _onItemSelected = UdonAction.Create());

        private DataDictionary _onItemSelected;

        /// <summary>
        /// Called when an item is selected and used.
        /// Listeners must contain a [RectTransform: item] variable.
        /// </summary>
        [PublicAPI]
        public UdonAction OnItemUsed =>
            (UdonAction)(_onItemUsed != null ? _onItemUsed : _onItemUsed = UdonAction.Create());

        private DataDictionary _onItemUsed;

        /// <summary>
        /// The original source list of items.
        /// </summary>
        public DataList ItemSource { get; protected set; }
        
        /// <summary>
        /// The filtered list of items used by the ListView.
        /// </summary>
        public DataList ListItems { get; } = new DataList();
        
        [field: SerializeField] public int SelectedIndex { get; private set; }
        public int ItemCount => ListItems != null ? ListItems.Count : 0;


        [SerializeField] protected RectTransform _itemPrefab;
        [SerializeField] protected RectTransform _itemContainer;
        [SerializeField] protected RectTransform _viewport;
        [SerializeField] protected LayoutDirection _direction;


        // Key: Int Index, Value: RectTransform Item
        protected DataDictionary _activeItemKeys = new DataDictionary();
        protected DataList _activeItems = new DataList();
        protected DataList _inactiveItems = new DataList();

        protected bool _useFilter;
        protected DataList _filterIndices;


        /// <summary>
        /// Set the focused index within the list.
        /// </summary>
        public virtual void SetIndex(int index)
        {
            SelectedIndex = Mathf.Clamp(index, 0, ListItems.Count - 1);
        }

        /// <summary>
        /// Assign a DataList as the source for the ListView.
        /// This is used to rebuild the list post assignation.
        /// </summary>
        public virtual void SetItemSource(DataList list)
        {
            ItemSource = list;
            ApplyFilter();
        }

        [PublicAPI]
        public void _ClearFilter(bool dontRebuild = false)
        {
            _useFilter = false;
            _filterIndices = null;
            ApplyFilter();

            if (!dontRebuild) RebuildList();
        }

        [PublicAPI]
        public void FilterBy(DataList indices, bool dontRebuild = false)
        {
            if (indices == null || indices.Count == 0)
            {
                _ClearFilter();
            }
            else
            {
                _useFilter = true;
                _filterIndices = indices;
                ApplyFilter();
            }

            if (!dontRebuild) RebuildList();
        }

        private void ApplyFilter()
        {
            ListItems.Clear();

            if (_useFilter)
            {
                for (int i = 0; i < _filterIndices.Count; i++)
                {
                    int index = _filterIndices[i].Int;
                    ListItems.Add(ItemSource[index]);
                }
            }
            else
            {
                ListItems.AddRange(ItemSource);
            }
        }

        /// <summary>
        /// Retrieve an item that represents a given index in the ItemSource list.
        /// If one is not available, a new item will be instantiated.
        /// </summary>
        protected virtual RectTransform GetItem(int index)
        {
            if (_activeItemKeys.TryGetValue(index, TokenType.Reference, out DataToken value))
            {
                return value.CastReference<RectTransform>();
            }

            RectTransform item;
            if (_inactiveItems.Count > 0)
            {
                item = _inactiveItems[0].CastReference<RectTransform>();
            }
            else
            {
                item = CreateItem();
            }

            _inactiveItems.Remove(item);
            _activeItems.Add(item);
            _activeItemKeys[index] = item;
            BindItem(item, index);

            return item;
        }

        protected virtual RectTransform CreateItem()
        {
            RectTransform item = (RectTransform)Instantiate(_itemPrefab.gameObject, _itemContainer).transform;
            
            _inactiveItems.Add(item);
            return item;
        }

        /// <summary>
        /// Run callbacks to represent the value within the assigned list item.
        /// </summary>
        protected void BindItem(RectTransform item, int index)
        {
            item.gameObject.SetActive(true);

            int paramIndex = _useFilter ? _filterIndices[index].Int : index;
            
            DataList bindParams = new DataList();
            
            bindParams.Add(item);
            bindParams.Add(paramIndex);
            bindParams.Add(ListItems[index]);
            
            OnBindItem._Invoke(bindParams);
        }

        /// <summary>
        /// Rebuild the list on re-enable if a list is assigned and has content.
        /// </summary>
        protected virtual void OnEnable()
        {
            if (_itemPrefab.gameObject.activeSelf) _itemPrefab.gameObject.SetActive(false);
            if (ItemCount > 0) RebuildList();
        }

        protected virtual void OnDisable()
        {
            DestroyAllItems();
        }

        /// <summary>
        /// Disable an item that is no longer visible or in use.
        /// </summary>
        /// <param name="index"></param>
        protected void ReleaseItem(int index)
        {
            if (!_activeItemKeys.TryGetValue(index, TokenType.Reference, out DataToken value))
            {
                Debug.LogWarning($"Item index {index} is not currently active.", this);
                return;
            }

            RectTransform item = (RectTransform)value.Reference;
            item.gameObject.SetActive(false);

            _inactiveItems.Add(item);
            _activeItems.Remove(item);
            _activeItemKeys.Remove(index);
        }

        /// <summary>
        /// Check if a point in item-space is visible in the final viewport.
        /// </summary>
        protected bool IsPointVisible(Vector2 point)
        {
            Vector3 worldPoint = _itemContainer.TransformPoint(point);
            Vector3 viewportPoint = _viewport.InverseTransformPoint(worldPoint);

            // Debug.DrawLine(worldPoint, worldPoint + (Vector3)Vector2.one * 0.01f);
            
            return _viewport.rect.Contains(viewportPoint);
        }

        /// <summary>
        /// Destroy all active items before instantiating new ones within the visible region.
        /// </summary>
        protected virtual void RebuildList()
        {
            DestroyAllItems();
            RefreshVisibility();
        }

        protected void DestroyAllItems()
        {
            for (int i = 0; i < _activeItems.Count; i++)
            {
                RectTransform item = _activeItems[i].CastReference<RectTransform>();
                Destroy(item.gameObject);
            }
            for (int i = 0; i < _inactiveItems.Count; i++)
            {
                RectTransform item = _inactiveItems[i].CastReference<RectTransform>();
                Destroy(item.gameObject);
            }
            
            _activeItemKeys.Clear();
            _activeItems.Clear();
            _inactiveItems.Clear();
        }
        
        /// <summary>
        /// Handle visibility checks for individual items.
        /// Largely handled via inherited classes.
        /// </summary>
        protected virtual void RefreshVisibility()
        {
        }
    }
}