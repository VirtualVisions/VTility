using JetBrains.Annotations;
using UnityEngine;
using VRC.SDK3.Data;

namespace VirtualVisions.VTility
{
    public class UdonListView : BaseUdonListView
    {

        [SerializeField] protected float _itemSize = 50;
        [SerializeField] protected float _spacingSize = 15;
        [SerializeField] protected int _groupCount = 1;
        [SerializeField] protected float _groupItemSize = 80;
        [SerializeField] protected float _groupSpacingSize = 10;

        private Vector2 _contentPos;

        private float ContainerSize => GetItemPlacement(ItemCount + (_groupCount - 1));
        private float FullItemSize => _itemSize + _spacingSize;
        private float FullGroupSize => (_groupItemSize * _groupCount) + (_groupSpacingSize * (_groupCount - 1));


        private float GetItemPlacement(int index)
        {
            int groupIndex = index / _groupCount;
            return (groupIndex * (_itemSize + _spacingSize));
        }

        private int GetIndexInGroup(int index)
        {
            return index % _groupCount;
        }

        private Vector2 GetPositionInGroup(int index)
        {
            int groupIndex = GetIndexInGroup(index);
            float position = (_groupItemSize + _groupSpacingSize) * groupIndex;

            switch (_direction)
            {
                default:
                case LayoutDirection.Column:
                    return new Vector2(position, 0);
                case LayoutDirection.Row:
                    return new Vector2(0, -position);
            }
        }

        private Vector2 GetLayoutPosition(int index)
        {
            switch (_direction)
            {
                default:
                case LayoutDirection.Column:
                    return new Vector2(0, -GetItemPlacement(index));
                case LayoutDirection.Row:
                    return new Vector2(GetItemPlacement(index), 0);
            }
        }


        private void Update()
        {
            Vector2 containerPos = _itemContainer.anchoredPosition;
            if (_contentPos != containerPos)
            {
                _contentPos = containerPos;
                if (ItemCount > 0) RefreshVisibility();
            }
        }

        [PublicAPI]
        public void SetGroupCount(int count)
        {
            _groupCount = Mathf.Max(1, count);
            RebuildList();
        }

        public override void SetItemSource(DataList list)
        {
            base.SetItemSource(list);
            RebuildList();
        }

        protected override RectTransform CreateItem()
        {
            RectTransform item = base.CreateItem();

            item.anchorMin = Vector2.up;
            item.anchorMax = Vector2.up;
            item.pivot = Vector2.up;

            switch (_direction)
            {
                default:
                case LayoutDirection.Column:
                    item.sizeDelta = new Vector2(_groupItemSize, _itemSize);
                    break;
                case LayoutDirection.Row:
                    item.sizeDelta = new Vector2(_itemSize, _groupItemSize);
                    break;
            }

            return item;
        }

        protected override void RebuildList()
        {
            // Resize the item container for the new ListItems.
            _itemContainer.anchorMin = Vector2.up;
            _itemContainer.anchorMax = Vector2.up;
            _itemContainer.pivot = Vector2.up;

            switch (_direction)
            {
                default:
                case LayoutDirection.Column:
                    _itemContainer.sizeDelta = new Vector2(FullGroupSize, ContainerSize);
                    break;
                case LayoutDirection.Row:
                    _itemContainer.sizeDelta = new Vector2(ContainerSize, FullGroupSize);
                    break;
            }
            
            base.RebuildList();
        }

        protected override void RefreshVisibility()
        {
            base.RefreshVisibility();
            
            float width;
            float height;
            
            switch (_direction)
            {
                default:
                case LayoutDirection.Column:
                    width = _groupItemSize;
                    height = _itemSize;
                    break;
                case LayoutDirection.Row:
                    width = _itemSize;
                    height = _groupItemSize;
                    break;
            }

            // We enforce a top-right pivot, so we good
            Vector2 cornerTopLeft = Vector2.zero;
            Vector2 cornerTopRight = new Vector2(width, 0);
            Vector2 cornerBottomLeft = new Vector2(0, -height);
            Vector2 cornerBottomRight = new Vector2(width, -height);
            Vector2 itemCenter = new Vector2(width, -height) * 0.5f;
            
            for (int i = 0; i < ItemCount; i++)
            {
                Vector2 itemPosition = GetLayoutPosition(i) + GetPositionInGroup(i);

                bool visible = IsPointVisible(itemPosition + cornerTopLeft) ||
                               IsPointVisible(itemPosition + cornerTopRight) ||
                               IsPointVisible(itemPosition + cornerBottomLeft) ||
                               IsPointVisible(itemPosition + cornerBottomRight) ||
                               IsPointVisible(itemPosition + itemCenter);
                
                bool isActive = _activeItemKeys.ContainsKey(i);
                
                if (visible == isActive) continue;
                if (visible)
                {
                    RectTransform item = GetItem(i);
                    item.anchoredPosition = itemPosition;
                }
                else
                {
                    ReleaseItem(i);
                }
            }
        }
    }
}