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
    }
}