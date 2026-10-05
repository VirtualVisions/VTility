using UnityEngine;
using UnityEngine.UI;
using VRC.SDK3.Data;

namespace VirtualVisions.VTility
{
    [RequireComponent(typeof(Slider))]
    public class SliderElement : UsableElement
    {

        public ValueChangeCallback value => ValueChangeCallbackExtensions.BackingValueChangeCallback(ref _value, _slider.value);
        private DataList _value;

        protected Slider _slider;

        protected override void Init()
        {
            base.Init();
            _slider = GetComponent<Slider>();
        }

        public override void _OnComponentUsed()
        {
            value.SetValue(_slider.value);
            base._OnComponentUsed();
        }
        

#if UNITY_EDITOR && !COMPILER_UDONSHARP
        protected override void _OnValidate()
        {
            _slider = GetComponent<Slider>();
            EditorEventQueue.QueueEvent(() =>
            {
                _slider.AddUdonListener(this, nameof(_OnComponentUsed));
            });
        }
#endif
    }
}