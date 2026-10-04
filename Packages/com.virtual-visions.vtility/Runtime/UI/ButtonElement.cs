using UnityEngine;
using UnityEngine.UI;

namespace VirtualVisions.VTility
{
    [RequireComponent(typeof(Button))]
    public class ButtonElement : UsableElement
    {

        protected Button _button;

        protected override void Init()
        {
            base.Init();
            _button = GetComponent<Button>();
        }

#if UNITY_EDITOR && !COMPILER_UDONSHARP
        protected override void _OnValidate()
        {
            _button = GetComponent<Button>();
            _button.AddUdonListener(this, nameof(_OnComponentUsed));
        }
#endif
    }
}