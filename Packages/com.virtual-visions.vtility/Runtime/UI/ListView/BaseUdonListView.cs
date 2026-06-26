using System;
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
        /// Listeners must contain a [RectTransform: item] and [Int: index] variable within a DataList.
        /// </summary>
        public UdonAction OnBindItem =>
            (UdonAction)(_onBindItem != null ? _onBindItem : _onBindItem = UdonAction.Create());

        private DataDictionary _onBindItem;

        /// <summary>
        /// Called when an item is focused in the list.
        /// Listeners must contain a [RectTransform: item] variable.
        /// </summary>
        public UdonAction OnItemSelected =>
            (UdonAction)(_onItemSelected != null ? _onItemSelected : _onItemSelected = UdonAction.Create());

        private DataDictionary _onItemSelected;

        /// <summary>
        /// Called when an item is selected and used.
        /// Listeners must contain a [RectTransform: item] variable.
        /// </summary>
        public UdonAction OnItemUsed =>
            (UdonAction)(_onItemUsed != null ? _onItemUsed : _onItemUsed = UdonAction.Create());

        private DataDictionary _onItemUsed;

        public DataList ItemSource { get; protected set; }
        [field: SerializeField] public int SelectedIndex { get; private set; }


        [SerializeField] protected RectTransform _itemPrefab;
        [SerializeField] protected RectTransform _itemContainer;
        [SerializeField] protected RectTransform _viewport;
        [SerializeField] protected LayoutDirection _direction;


        // Key: Int Index, Value: RectTransform Item
        protected DataDictionary _activeItemKeys = new DataDictionary();
        protected DataList _activeItems = new DataList();
        protected DataList _inactiveItems = new DataList();

        protected int ItemCount => ItemSource != null ? ItemSource.Count : 0;


        public virtual void SetIndex(int index)
        {
            SelectedIndex = Mathf.Clamp(index, 0, ItemSource.Count - 1);
        }

        public virtual void SetItemSource(DataList list)
        {
            ItemSource = list;
        }

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
            RectTransform obj = (RectTransform)Instantiate(_itemPrefab.gameObject, _itemContainer).transform;
            _inactiveItems.Add(obj);
            return obj;
        }

        protected void BindItem(RectTransform item, int index)
        {
            item.gameObject.SetActive(true);

            DataList bindParams = new DataList();
            bindParams.Add(item);
            bindParams.Add(index);
            OnBindItem._Invoke(bindParams);
        }

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

        protected bool IsItemVisible(Vector2 containerPosition, float itemSize)
        {
            Vector2 posStart;
            Vector2 posEnd;

            switch (_direction)
            {
                default:
                case LayoutDirection.Column:
                    Vector2 itemColumnSize = new Vector2(0, itemSize);
                    posStart = containerPosition + itemColumnSize;
                    posEnd = containerPosition - itemColumnSize;
                    break;
                case LayoutDirection.Row:
                    Vector2 itemRowSize = new Vector2(itemSize, 0);
                    posStart = containerPosition + itemRowSize;
                    posEnd = containerPosition - itemRowSize;
                    break;
            }

            Vector3 worldPosStart = _itemContainer.TransformPoint(posStart);
            Vector3 viewportPosStart = _viewport.InverseTransformPoint(worldPosStart);

            Vector3 worldPosEnd = _itemContainer.TransformPoint(posEnd);
            Vector3 viewportPosEnd = _viewport.InverseTransformPoint(worldPosEnd);
            return _viewport.rect.Contains(viewportPosStart) || _viewport.rect.Contains(viewportPosEnd);

        }

        protected virtual void RefreshVisibility()
        {
        }
    }
}