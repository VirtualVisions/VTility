using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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
        
        [field: SerializeField, FormerlySerializedAs("<Colors>k__BackingField")] public List<ColorPreset> colors = new List<ColorPreset>();
        
        public bool GetColor(string colorName, out Color color)
        {
            foreach (ColorPreset col in colors)
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
            
            foreach (ColorPreset preset in colors)
            {
                names.Add(preset.name);
            }

            return names;
        }

        #endregion

        
        
        #region Fonts

        [field: SerializeField, FormerlySerializedAs("<Labels>k__BackingField")] public List<LabelPreset> labels = new List<LabelPreset>();
        

        public bool GetLabel(string labelName, out LabelPreset label)
        {
            foreach (LabelPreset preset in labels)
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
            
            foreach (LabelPreset preset in labels)
            {
                names.Add(preset.name);
            }

            return names;
        }

        #endregion
    }
}
