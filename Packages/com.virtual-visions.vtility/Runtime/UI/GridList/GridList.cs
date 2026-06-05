using UnityEngine;

namespace VirtualVisions.VTility
{
    public class GridList : BaseGridList
    {
        
        public override int ItemCount => _contentTransform.childCount;


        protected override void ApplyLayout()
        {
            base.ApplyLayout();
            
            for (int i = 0; i < ItemCount; i++)
            {
                RectTransform child = (RectTransform)_contentTransform.GetChild(i);
                PlaceItem(child, i);
                
                child.gameObject.SetActive(IsIndexVisible(i));
            }
        }
    }
}