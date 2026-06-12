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
    public abstract class UdonListView : UdonSharpBehaviour
    {
        /// <summary>
        /// Called when data is bound to a specific item.
        /// Listeners must contain a [RectTransform: item] and [Int: index] variable within a DataList.
        /// </summary>
        public UdonAction OnBindItem => (UdonAction)(_onBindItem != null ? _onBindItem : _onBindItem = UdonAction.Create());
        private DataDictionary _onBindItem;

        /// <summary>
        /// Called when an item is focused in the list.
        /// Listeners must contain a [RectTransform: item] variable.
        /// </summary>
        public UdonAction OnItemSelected => (UdonAction)(_onItemSelected != null ? _onItemSelected : _onItemSelected = UdonAction.Create());
        private DataDictionary _onItemSelected;

        /// <summary>
        /// Called when an item is selected and used.
        /// Listeners must contain a [RectTransform: item] variable.
        /// </summary>
        public UdonAction OnItemUsed => (UdonAction)(_onItemUsed != null ? _onItemUsed : _onItemUsed = UdonAction.Create());
        private DataDictionary _onItemUsed;

        public DataList ItemSource { get; protected set; }
        public int SelectedIndex { get; private set; }


        [SerializeField] protected RectTransform _itemPrefab;
        [SerializeField] protected RectTransform _itemContainer;
        [SerializeField] protected RectTransform _viewport;
        [SerializeField] protected LayoutDirection _direction;
        
        protected DataDictionary _activeItemKeys = new DataDictionary();
        protected DataList _activeItems = new DataList();
        protected DataList _inactiveItems = new DataList();
        
        protected int ItemCount => ItemSource != null ? ItemSource.Count : 0;

        
        public virtual void _SetIndex(int index)
        {
            SelectedIndex = Mathf.Clamp(index, 0, ItemSource.Count - 1);
        }

        public virtual void _SetItemSource(DataList list)
        {
            ItemSource = list;
        }

        protected virtual RectTransform GetItem(int index)
        {
            if (_activeItemKeys.TryGetValue(index, TokenType.Reference, out DataToken value))
            {
                return (RectTransform)value.Reference;
            }

            RectTransform item;
            if (_inactiveItems.Count > 0)
            {
                item = (RectTransform)_inactiveItems[0].Reference;
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

        protected bool IsIndexVisible(int index, float itemSize, float offset = 0)
        {
            // The extra add/subtract at the end of these two is for
            // partial visibility when an item is coming into view.
            float itemStart = (index * itemSize) + itemSize + offset;
            float itemEnd = ((index + 1) * itemSize) - itemSize - offset;

            float containerOffset;
            switch (_direction)
            {
                default:
                case LayoutDirection.Column:
                    containerOffset = _itemContainer.anchoredPosition.y;
                    break;
                case LayoutDirection.Row:
                    containerOffset = -_itemContainer.anchoredPosition.x;
                    break;
            }
            
            float itemStartViewport = itemStart - containerOffset;
            float itemEndViewport = itemEnd - containerOffset;

            float viewportStart;
            float viewportEnd;
            
            switch (_direction)
            {
                default:
                case LayoutDirection.Column:
                    viewportStart = _viewport.rect.yMin;
                    viewportEnd = _viewport.rect.yMax;
                    break;
                case LayoutDirection.Row:
                    viewportStart = _viewport.rect.xMin;
                    viewportEnd = _viewport.rect.xMax;
                    break;
            }

            return itemStartViewport > viewportStart &&
                   itemEndViewport < viewportEnd;
        }

    }
}