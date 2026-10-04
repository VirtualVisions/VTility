using UnityEngine;
using VRC.SDK3.Data;

namespace VirtualVisions.VTility
{
    public class TabGroupElement : CanvasElement
    {

        [SerializeField] protected TabListElement _tabList;
        [SerializeField] protected CanvasElement[] _pages;
        [SerializeField] protected CanvasElement _currentPage;

        public ElementSwitcher pageSwitcher => _pageSwitcher.AsElementSwitcher();
        private DataList _pageSwitcher;


        protected override void Init()
        {
            base.Init();
            _pageSwitcher = ElementSwitcher.Create(_pages, _currentPage);

            UdonEvent callback = UdonEvent.Create(this, nameof(_OnTabChanged));
            _tabList.AddListener(callback, true);
        }

        public void _OnTabChanged()
        {
            int index = _tabList.tabIndex.Value().Int;
            pageSwitcher.SwitchToIndex(index);
        }
    }
}