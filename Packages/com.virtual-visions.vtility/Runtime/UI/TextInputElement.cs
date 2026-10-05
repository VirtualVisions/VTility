using TMPro;
using UnityEngine;
using VRC.SDK3.Data;

namespace VirtualVisions.VTility
{
    [RequireComponent(typeof(TMP_InputField))]
    public class TextInputElement : UsableElement
    {

        public ValueChangeCallback value => ValueChangeCallbackExtensions.BackingValueChangeCallback(ref _value, _inputField.text);
        private DataList _value;

        protected TMP_InputField _inputField;

        protected override void Init()
        {
            base.Init();
            _inputField = GetComponent<TMP_InputField>();
        }

        public override void _OnComponentUsed()
        {
            value.SetValue(_inputField.text);
            base._OnComponentUsed();
        }
        
#if UNITY_EDITOR && !COMPILER_UDONSHARP
        protected override void _OnValidate()
        {
            _inputField = GetComponent<TMP_InputField>();
            EditorEventQueue.QueueEvent(() =>
            {
                _inputField.AddUdonListener(this, nameof(_OnComponentUsed));
            });
        }
#endif
    }
}