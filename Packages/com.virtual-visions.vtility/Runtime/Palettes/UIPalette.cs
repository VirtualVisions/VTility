using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace VirtualVisions.VTility
{
    public class UIPalette : MonoBehaviour
    {

        [FormerlySerializedAs("Palette")] public UIPaletteAsset palette;

        #region Colors

        public bool GetColor(string colorName, out Color result)
        {
            if (palette.GetColor(colorName, out result))
            {
                return true;
            }

            result = Color.magenta;
            return false;

        }

        public List<string> GetColorPresetNames()
        {
            if (palette) return palette.GetColorPresetNames();
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
            if (palette && palette.GetLabel(presetName, out label))
            {
                return true;
            }

            label = default;

            return false;
        }

        public List<string> GetLabelPresetNames()
        {
            if (palette) return palette.GetLabelPresetNames();
            return new List<string>() { UIPaletteAsset.NONE_FIELD };
        }

        #endregion
        
    }
}