using UnityEngine;
using UnityEngine.UI;

namespace VirtualVisions.VTility
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Graphic))]
    public class PaletteColor : PaletteComponentBase
    {
        public override void ApplyPalette()
        {
            UIPalette palette = GetComponentInParent<UIPalette>(true);
            Graphic graphic = GetComponent<Graphic>();
            if (palette && graphic && palette.GetColor(paletteName, out Color color))
            {
                graphic.color = color;
            }
        }
    }
}