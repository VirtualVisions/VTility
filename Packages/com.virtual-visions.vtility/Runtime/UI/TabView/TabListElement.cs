using System;
using UnityEngine;
using VRC.SDK3.Data;

namespace VirtualVisions.VTility
{
    public class TabListElement : CanvasElement
    {

        [SerializeField] private int _defaultIndex;
        [SerializeField] protected TabButtonElement[] _tabButtons;
        
        public ValueChangeCallback tabIndex => ValueChangeCallbackExtensions.BackingValueChangeCallback(ref _tabIndex, _defaultIndex);
        private DataList _tabIndex;

        protected override void Init()
        {
            base.Init();

            UdonEvent callback = UdonEvent.Create(this, nameof(_OnButtonPressed), nameof(_OnButtonPressed_Value));
            foreach (TabButtonElement button in _tabButtons)
            {
                if (!button) continue;
                button.onUsed.AddListener(callback);
            }

            tabIndex.OnChanged().AddListener(this, nameof(_OnTabChanged), true);
        }

        /// <summary>
        /// Add a listener into the tabIndex value, while making sure the TabList has initialized first.
        /// </summary>
        public void AddListener(UdonEvent callback, bool invokeImmediate = false)
        {
            if (!_initialized) Init();
            tabIndex.OnChanged().AddListener(callback, invokeImmediate);
        }
        
        [NonSerialized] public DataToken _OnButtonPressed_Value;
        public void _OnButtonPressed()
        {
            TabButtonElement button = _OnButtonPressed_Value.CastReference<TabButtonElement>();
            tabIndex.SetValue(Array.IndexOf(_tabButtons, button));
        }

        public void _OnTabChanged()
        {
            int index = tabIndex.Value().Int;
            for (int i = 0; i < _tabButtons.Length; i++)
            {
                TabButtonElement button = _tabButtons[i];
                if (!button) continue;
                button.Focus(i == index);
            }
        }
        
    }
}