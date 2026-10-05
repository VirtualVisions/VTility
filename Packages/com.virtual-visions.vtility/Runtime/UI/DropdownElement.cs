using TMPro;
using UnityEngine;
using VRC.SDK3.Data;

namespace VirtualVisions.VTility
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class DropdownElement : UsableElement
    {
        
        public ValueChangeCallback value => ValueChangeCallbackExtensions.BackingValueChangeCallback(ref _value, _dropdown.value);
        private DataList _value;

        protected TMP_Dropdown _dropdown;

        protected override void Init()
        {
            base.Init();
            _dropdown = GetComponent<TMP_Dropdown>();
        }

        public override void _OnComponentUsed()
        {
            value.SetValue(_dropdown.value);
            base._OnComponentUsed();
        }
        

#if UNITY_EDITOR && !COMPILER_UDONSHARP
        protected override void _OnValidate()
        {
            _dropdown = GetComponent<TMP_Dropdown>();
            EditorEventQueue.QueueEvent(() =>
            {
                _dropdown.AddUdonListener(this, nameof(_OnComponentUsed));
            });
        }
#endif
    }
}