using TMPro;
using UnityEngine;

namespace VirtualVisions.VTility
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(TMP_Text))]
    public class PaletteLabel : PaletteComponentBase
    {
        public override void ApplyPalette()
        {
            UIPalette palette = GetComponentInParent<UIPalette>(true);
            TMP_Text label = GetComponent<TMP_Text>();
            if (palette && label)
            {
                LabelPreset preset = palette.GetLabel(paletteName);
                label.fontSize = preset.fontSize;
                label.font = preset.font;
                label.fontStyle = preset.fontStyle;
            }
        }
    }
}
