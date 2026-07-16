using System.Collections.Generic;
using UnityEngine;

namespace VirtualVisions.VTility
{
    public class UIPalette : MonoBehaviour
    {

        public UIPaletteAsset Palette;

        #region Colors

        public Color GetColor(string colorName)
        {
            if (Palette.GetColor(colorName, out Color color))
            {
                return color;
            }

            return Color.magenta;
        }

        public List<string> GetColorPresetNames()
        {
            if (Palette) return Palette.GetColorPresetNames();
            return new List<string>() { UIPaletteAsset.NONE_FIELD };
        }

        private void OnValidate()
        {
            EditorEventQueue.QueueEvent(ApplyAllChildren);
        }

        public void ApplyAllChildren()
        {
            PaletteComponentBase[] comps = transform.GetComponentsInChildren<PaletteComponentBase>();
            foreach (PaletteComponentBase comp in comps)
            {
                comp.ApplyPalette();
            }
        }
        
        #endregion

        #region Fonts

        public LabelPreset GetLabel(string presetName)
        {
            if (Palette && Palette.GetLabel(presetName, out LabelPreset label))
            {
                return label;
            }

            return LabelPreset.Fallback;
        }

        public List<string> GetLabelPresetNames()
        {
            if (Palette) return Palette.GetLabelPresetNames();
            return new List<string>() { UIPaletteAsset.NONE_FIELD };
        }

        #endregion
        
    }
}