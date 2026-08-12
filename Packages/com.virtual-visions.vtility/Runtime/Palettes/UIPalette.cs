using System.Collections.Generic;
using UnityEngine;

namespace VirtualVisions.VTility
{
    public class UIPalette : MonoBehaviour
    {

        public UIPaletteAsset Palette;

        #region Colors

        public bool GetColor(string colorName, out Color result)
        {
            if (Palette.GetColor(colorName, out result))
            {
                return true;
            }

            result = Color.magenta;
            return false;

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

        public bool GetLabel(string presetName, out LabelPreset label)
        {
            if (Palette && Palette.GetLabel(presetName, out label))
            {
                return true;
            }

            label = default;

            return false;
        }

        public List<string> GetLabelPresetNames()
        {
            if (Palette) return Palette.GetLabelPresetNames();
            return new List<string>() { UIPaletteAsset.NONE_FIELD };
        }

        #endregion
        
    }
}