using System;
using JetBrains.Annotations;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDK3.Data;

namespace VirtualVisions.VTility
{
    public class CenteringList : BaseUdonListView
    {

        [SerializeField] protected float _itemSize = 50;
        [SerializeField] protected float _spacingSize = 15;
        [SerializeField] protected float _selectedItemMargin = 30;
        [SerializeField] protected float _selectedItemScale = 1.2f;
        [SerializeField] private float _tweenDuration = 0.5f;
        [SerializeField] private VRCTweenEase _tweenEase = VRCTweenEase.OutCubic;

        private VRCTweenHandle _blendContainer;
        
        private VRCTweenHandle _blendNextSelectionPos;
        private VRCTweenHandle _blendNextSelectionScale;
        
        private VRCTweenHandle _blendLastSelectionPos;
        private VRCTweenHandle _blendLastSelectionScale;

        
        private float containerSize => GetItemPlacement(itemCount);
        private float itemHalfSize => _itemSize / 2f;
        private float fullItemSize => _itemSize + _spacingSize;
        
        
        private float GetItemPlacement(int index)
        {
            return ((index * _itemSize) + ((index - 1) * _spacingSize) + itemHalfSize);
        }

        private Vector2 GetLayoutPosition(int index)
        {
            float selectionMargin = 0;
            if (index < selectedIndex) selectionMargin = -_selectedItemMargin;
            if (index > selectedIndex) selectionMargin = _selectedItemMargin;
            
            switch (_direction)
            {
                default:
                case LayoutDirection.Column:
                    return new Vector2(0, -GetItemPlacement(index) - selectionMargin);
                case LayoutDirection.Row:
                    return new Vector2(GetItemPlacement(index) + selectionMargin, 0);
            }
        }

        protected override void OnEnable()
        {
            _itemContainer.anchoredPosition = -GetLayoutPosition(selectedIndex);
            base.OnEnable();
        }

        private void Update()
        {
            if (_blendContainer.IsActive) RefreshVisibility();
        }

        public void _TweenToCurrent()
        {
            _blendContainer.TryKill();

            _blendContainer = _itemContainer.TweenAnchorPos(
                -GetLayoutPosition(selectedIndex),
                _tweenDuration,
                _tweenEase);
        }

        [PublicAPI]
        public void _NavigateNext(Vector2Int direction)
        {
            switch (_direction)
            {
                default:
                case LayoutDirection.Column:
                    if (direction == Vector2Int.up) _SelectPrevious();
                    if (direction == Vector2Int.down) _SelectNext();
                    break;
                case LayoutDirection.Row:
                    if (direction == Vector2Int.right) _SelectNext();
                    if (direction == Vector2Int.left) _SelectPrevious();
                    break;
            }
        }
        
        [PublicAPI]
        public void _SelectPrevious() => SetIndex(selectedIndex - 1);
        [PublicAPI]
        public void _SelectNext() => SetIndex(selectedIndex + 1);


        public override void SetIndex(int index)
        {
            int lastIndex = selectedIndex;
            base.SetIndex(index);
            
            _TweenToCurrent();

            if (lastIndex != -1 && lastIndex != selectedIndex && !Mathf.Approximately(_selectedItemScale, 1))
            {
                if (_activeItemKeys.TryGetValue(lastIndex, TokenType.Reference, out DataToken lastItem))
                {
                    RectTransform rect = lastItem.CastReference<RectTransform>();

                    _blendLastSelectionPos.TryComplete();
                    _blendLastSelectionScale.TryComplete();

                    _blendLastSelectionPos = rect.TweenAnchorPos(GetLayoutPosition(lastIndex), _tweenDuration, _tweenEase);
                    _blendLastSelectionScale = rect.TweenScale(Vector3.one, _tweenDuration, _tweenEase);
                }

                if (_activeItemKeys.TryGetValue(index, TokenType.Reference, out DataToken nextItem))
                {
                    RectTransform rect = nextItem.CastReference<RectTransform>();

                    _blendNextSelectionPos.TryComplete();
                    _blendNextSelectionScale.TryComplete();

                    _blendNextSelectionPos = rect.TweenAnchorPos(GetLayoutPosition(index), _tweenDuration, _tweenEase);
                    _blendNextSelectionScale = rect.TweenScale(Vector3.one * _selectedItemScale, _tweenDuration, _tweenEase);
                }
            }
        }

        public override void SetItemSource(DataList list)
        {
            base.SetItemSource(list);
            RebuildList();
        }

        protected override RectTransform CreateItem()
        {
            RectTransform item = base.CreateItem();

            switch (_direction)
            {
                default:
                case LayoutDirection.Column:
                    item.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _itemSize);
                    _itemContainer.sizeDelta = new Vector2(0, containerSize);
                    break;
                case LayoutDirection.Row:
                    item.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _itemSize);
                    _itemContainer.sizeDelta = new Vector2(containerSize, 0);
                    break;
            }

            return item;
        }

        protected override void RefreshVisibility()
        {
            base.RefreshVisibility();

            Vector2 visibilityCheckOffset;
            switch (_direction)
            {
                default:
                case LayoutDirection.Column:
                    visibilityCheckOffset = Vector2.up * _itemSize;
                    break;
                case LayoutDirection.Row:
                    visibilityCheckOffset = Vector2.right * _itemSize;
                    break;
            }
            
            for (int i = 0; i < itemCount; i++)
            {
                Vector2 layoutPos = GetLayoutPosition(i);
                bool visible = IsPointVisible(layoutPos - visibilityCheckOffset) || IsPointVisible(layoutPos + visibilityCheckOffset);
                bool isActive = _activeItemKeys.ContainsKey(i);

                if (visible == isActive) continue;
                if (visible)
                {
                    RectTransform item = GetItem(i);
                    item.anchoredPosition = layoutPos;
                }
                else
                {
                    ReleaseItem(i);
                }
            }
        }
    }
}