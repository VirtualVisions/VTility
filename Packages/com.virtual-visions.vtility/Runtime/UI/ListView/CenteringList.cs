using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDK3.Data;

namespace VirtualVisions.VTility
{
    public class CenteringList : UdonListView
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

        
        private float ContainerSize => GetItemPlacement(ItemCount);
        private float ItemHalfSize => _itemSize / 2f;
        private float FullItemSize => _itemSize + _spacingSize;
        
        
        private float GetItemPlacement(int index)
        {
            return ((index * _itemSize) + ((index - 1) * _spacingSize) + ItemHalfSize);
        }

        private Vector2 GetLayoutPosition(int index)
        {
            float selectionMargin = 0;
            if (index < SelectedIndex) selectionMargin = -_selectedItemMargin;
            if (index > SelectedIndex) selectionMargin = _selectedItemMargin;
            
            switch (_direction)
            {
                default:
                case LayoutDirection.Column:
                    return new Vector2(0, -GetItemPlacement(index) - selectionMargin);
                case LayoutDirection.Row:
                    return new Vector2(GetItemPlacement(index) + selectionMargin, 0);
            }
        }

        private void OnEnable()
        {
            _itemContainer.anchoredPosition = -GetLayoutPosition(SelectedIndex);
            RefreshVisibility();
        }

        private void Update()
        {
            if (_blendContainer.IsActive)
            {
                RefreshVisibility();
            }
        }

        public void _TweenToCurrent()
        {
            _blendContainer.TryKill();

            _blendContainer = _itemContainer.TweenAnchorPos(
                -GetLayoutPosition(SelectedIndex),
                _tweenDuration,
                _tweenEase);
        }

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

        public void _SelectPrevious() => _SetIndex(SelectedIndex - 1);
        public void _SelectNext() => _SetIndex(SelectedIndex + 1);


        public override void _SetIndex(int index)
        {
            int lastIndex = SelectedIndex;

            base._SetIndex(index);
            _TweenToCurrent();

            if (lastIndex != -1 && lastIndex != SelectedIndex && !Mathf.Approximately(_selectedItemScale, 1))
            {
                if (_activeItemKeys.TryGetValue(lastIndex, TokenType.Reference, out DataToken lastItem))
                {
                    RectTransform rect = lastItem._CastReference<RectTransform>();

                    _blendLastSelectionPos.TryComplete();
                    _blendLastSelectionScale.TryComplete();

                    _blendLastSelectionPos = rect.TweenAnchorPos(GetLayoutPosition(lastIndex), _tweenDuration, _tweenEase);
                    _blendLastSelectionScale = rect.TweenScale(Vector3.one, _tweenDuration, _tweenEase);
                }

                if (_activeItemKeys.TryGetValue(index, TokenType.Reference, out DataToken nextItem))
                {
                    RectTransform rect = nextItem._CastReference<RectTransform>();

                    _blendNextSelectionPos.TryComplete();
                    _blendNextSelectionScale.TryComplete();

                    _blendNextSelectionPos = rect.TweenAnchorPos(GetLayoutPosition(index), _tweenDuration, _tweenEase);
                    _blendNextSelectionScale = rect.TweenScale(Vector3.one * _selectedItemScale, _tweenDuration, _tweenEase);
                }
            }
        }

        protected override RectTransform CreateItem()
        {
            RectTransform item = base.CreateItem();

            switch (_direction)
            {
                default:
                case LayoutDirection.Column:
                    item.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _itemSize);
                    _itemContainer.sizeDelta = new Vector2(0, ContainerSize);
                    break;
                case LayoutDirection.Row:
                    item.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _itemSize);
                    _itemContainer.sizeDelta = new Vector2(ContainerSize, 0);
                    break;
            }

            return item;
        }

        private void RefreshVisibility()
        {
            for (int i = 0; i < ItemCount; i++)
            {
                bool visible = IsIndexVisible(i, FullItemSize, ItemHalfSize);
                bool isActive = _activeItemKeys.ContainsKey(i);

                if (visible == isActive) continue;
                if (visible)
                {
                    RectTransform item = GetItem(i);
                    item.anchoredPosition = GetLayoutPosition(i);
                }
                else
                {
                    ReleaseItem(i);
                }
            }
        }
    }
}