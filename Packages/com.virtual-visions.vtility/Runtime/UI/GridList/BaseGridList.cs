using UdonSharp;
using UnityEngine;

namespace VirtualVisions.VTility
{
    [RequireComponent(typeof(RectTransform))]
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public abstract class BaseGridList : UdonSharpBehaviour
    {
        
        public virtual int ItemCount => 0;
        
        
        [Header("Settings")]
        [Min(1)] public int columnCount = 2;
        [Min(1)] public float rowHeight = 100;
        [Min(0)] public Vector2 padding = new Vector2(10, 10);
        [SerializeField] protected RectTransform _viewportTransform;
        [SerializeField] protected RectTransform _contentTransform;

        [field: Header("Readout")]
        [field: SerializeField] public float ContainerHeight { get; private set; }
        [field: SerializeField] public int RowCount { get; private set; }




#if UNITY_EDITOR && !COMPILER_UDONSHARP
        private void OnValidate() => EditorEventQueue.QueueEvent(ApplyLayout);
#endif
        
        private void OnTransformChildrenChanged()
        {
            // Debug.Log("Children Changed", this);
            ApplyLayout();
        }

        private void OnRectTransformDimensionsChange()
        {
            // Debug.Log("Rect Transform Changed", this);
            ApplyLayout();
        }

        public void _RefreshLayout() => ApplyLayout();
        
        
        protected virtual void ApplyLayout()
        {
            if (!_contentTransform || !_viewportTransform) return;
            
            Rect rect = _contentTransform.rect;
            ContainerHeight = rect.size.y;
            
            RowCount = Mathf.CeilToInt(ItemCount / (float)columnCount);
            ContainerHeight = RowCount * rowHeight;
            _contentTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ContainerHeight);
        }
        
        /// <summary>
        /// Position a transform at the corresponding index's position. 
        /// </summary>
        protected virtual void PlaceItem(RectTransform item, int index)
        {
            Vector2 position = GetIndexPosition(index);
            
            item.pivot = new Vector2(0, 1);
            item.anchorMin = new Vector2(0, 1);
            item.anchorMax = new Vector2(0, 1);
            item.anchoredPosition = position;
            item.sizeDelta = GetItemSize();
        }

        /// <summary>
        /// Check if a index is visible within the viewport.
        /// </summary>
        protected bool IsIndexVisible(int index)
        {
            Vector2 position = GetIndexPosition(index);
            Vector2 size = GetItemSize();

            Vector3[] itemCorners = new Vector3[4];
            itemCorners[0] = _contentTransform.TransformPoint(position);
            itemCorners[1] = _contentTransform.TransformPoint(position + new Vector2(size.x, 0));
            itemCorners[2] = _contentTransform.TransformPoint(position + new Vector2(size.x, -size.y));
            itemCorners[3] = _contentTransform.TransformPoint(position + new Vector2(0, -size.y));

            return AreCornersVisible(itemCorners);
        }

        /// <summary>
        /// Check if a rect transform is visible within the viewport.
        /// </summary>
        protected bool IsTransformVisible(RectTransform item)
        {
            Vector3[] itemCorners = new Vector3[4];
            item.GetWorldCorners(itemCorners);
            
            return AreCornersVisible(itemCorners);
        }

        /// <summary>
        /// Compare corner bounds against the viewport bounds for visibility.
        /// </summary>
        private bool AreCornersVisible(Vector3[] corners)
        {
            Vector3[] viewportCorners = new Vector3[4];
            _viewportTransform.GetWorldCorners(viewportCorners);

            Bounds itemBounds = new Bounds(corners[0], Vector3.zero);
            foreach (Vector3 corner in corners) itemBounds.Encapsulate(corner);

            Bounds viewportBounds = new Bounds(viewportCorners[0], Vector3.zero);
            foreach (Vector3 corner in viewportCorners) viewportBounds.Encapsulate(corner);

            return itemBounds.Intersects(viewportBounds);
        }

        /// <summary>
        /// Get the local position of an item based on it's index within the grid.
        /// </summary>
        protected Vector2 GetIndexPosition(int index)
        {
            Rect rect = _contentTransform.rect;
            float itemWidth = rect.width / columnCount;
            int rowIndex = index % columnCount;
            int columnIndex = index / columnCount;
            
            return new Vector2(rowIndex * itemWidth, -columnIndex * rowHeight);
        }

        /// <summary>
        /// Calculate the padded size of an item when placed on the grid.
        /// </summary>
        protected Vector2 GetItemSize()
        {
            Rect rect = _contentTransform.rect;
            float itemWidth = rect.width / columnCount;

            float paddedWidth = itemWidth - padding.x;
            float paddedHeight = rowHeight - padding.y;

            return new Vector2(paddedWidth, paddedHeight);
        }
    }
}