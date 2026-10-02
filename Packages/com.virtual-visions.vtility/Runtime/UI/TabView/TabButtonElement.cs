using UnityEngine;
using UnityEngine.UI;

namespace VirtualVisions.VTility
{
    public class TabButtonElement : ButtonElement
    {

        [SerializeField] private Color _inactiveColor = Color.clear;

        private ColorBlock _hiddenColor;
        private ColorBlock _baseColor;
        
        protected override void Init()
        {
            base.Init();

            _baseColor = _button.colors;
            _hiddenColor = _baseColor;
            _hiddenColor.normalColor = _inactiveColor;
        }

        public void Focus(bool value)
        {
            if (!_initialized) Init();

            _button.colors = value ? _baseColor : _hiddenColor;
        }
        
    }
}