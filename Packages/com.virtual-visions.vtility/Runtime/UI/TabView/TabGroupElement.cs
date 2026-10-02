
using System;
using UnityEngine;
using VRC.SDK3.Data;

namespace VirtualVisions.VTility
{
    public class TabGroupElement : CanvasElement
    {

        [SerializeField] protected TabListElement _tabList;
        [SerializeField] protected CanvasElement[] _pages;
        [SerializeField] protected CanvasElement _currentPage;

        public ValueSwitcher pageSwitcher => ValueSwitcherExtensions.BackingValueSwitcher(ref _pageSwitcher, _pages.ToRefList(), _currentPage);
        private DataList _pageSwitcher;


        protected override void Init()
        {
            base.Init();

            foreach (CanvasElement page in _pages)
            {
                if (!page) continue;
                page._HideElement(true);
            }

            pageSwitcher.OnValueSwitched().AddListener(this, nameof(_OnPageChanged), nameof(_OnPageChanged_Value));

            UdonEvent callback = UdonEvent.Create(this, nameof(_OnTabChanged));
            _tabList.AddListener(callback, true);
        }

        public void _OnTabChanged()
        {
            int index = _tabList.tabIndex.Value().Int;
            pageSwitcher.SwitchToIndex(index);
        }

        [NonSerialized] public DataToken _OnPageChanged_Value;
        public void _OnPageChanged()
        {
            CanvasElement newPage = _OnPageChanged_Value.CastReference<CanvasElement>();
            if (Array.IndexOf(_pages, newPage) == -1) newPage = null;

            if (_currentPage) _currentPage._HideElement();
            _currentPage = newPage;
            if (_currentPage) _currentPage._ShowElement();
        }
    }
}