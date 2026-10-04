using UnityEngine;
using UnityEngine.UI;
using VRC.SDK3.Data;

namespace VirtualVisions.VTility
{
    [RequireComponent(typeof(Toggle))]
    public class ToggleElement : UsableElement
    {

        public ValueChangeCallback value => ValueChangeCallbackExtensions.BackingValueChangeCallback(ref _value, _toggle.isOn);
        private DataList _value;

        protected Toggle _toggle;

        protected override void Init()
        {
            base.Init();
            _toggle = GetComponent<Toggle>();
        }

        public override void _OnComponentUsed()
        {
            value.SetValue(_toggle.isOn);
            base._OnComponentUsed();
        }

#if UNITY_EDITOR && !COMPILER_UDONSHARP
        protected override void _OnValidate()
        {
            _toggle = GetComponent<Toggle>();
            _toggle.AddUdonListener(this, nameof(_OnComponentUsed));
        }
#endif
    }
}