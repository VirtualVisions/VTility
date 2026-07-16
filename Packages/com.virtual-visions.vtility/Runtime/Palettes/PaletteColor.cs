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
            if (palette && graphic) graphic.color = palette.GetColor(paletteName);
        }
    }
}