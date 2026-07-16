using System;
using System.Collections.Generic;
using UnityEngine;

namespace VirtualVisions.VTility
{
    [CreateAssetMenu(menuName = "VTility/Palette", fileName = "Palette")]
    public class UIPaletteAsset : ScriptableObject
    {

        public const string NONE_FIELD = "• None •";
        
        private void OnValidate()
        {
            EditorEventQueue.QueueEvent(ApplyAllPalettes);
        }

        public void ApplyAllPalettes()
        {
            UIPalette[] palettes = FindObjectsOfType<UIPalette>();
            foreach (UIPalette palette in palettes)
            {
                palette.ApplyAllChildren();
            }
        }


        #region Colors
        
        [field: SerializeField] public List<ColorPreset> Colors { get; private set; } = new List<ColorPreset>();
        
        public bool GetColor(string colorName, out Color color)
        {
            foreach (ColorPreset col in Colors)
            {
                if (string.Equals(col.name, colorName))
                {
                    color = col.value;
                    return true;
                }
            }
            
            color = Color.magenta;
            return false;
        }

        public List<string> GetColorPresetNames()
        {
            List<string> names = new List<string>();
            names.Add(NONE_FIELD);
            
            foreach (ColorPreset preset in Colors)
            {
                names.Add(preset.name);
            }

            return names;
        }

        #endregion

        
        
        #region Fonts

        [field: SerializeField] public List<LabelPreset> Labels { get; private set; } = new List<LabelPreset>();
        

        public bool GetLabel(string labelName, out LabelPreset label)
        {
            foreach (LabelPreset preset in Labels)
            {
                if (preset.name.Equals(labelName))
                {
                    label = preset;
                    return true;
                }
            }

            label = LabelPreset.Fallback;
            return false;
        }

        public List<string> GetLabelPresetNames()
        {
            List<string> names = new List<string>();
            names.Add(NONE_FIELD);
            
            foreach (LabelPreset preset in Labels)
            {
                names.Add(preset.name);
            }

            return names;
        }

        #endregion
    }
}
