using System;
using TMPro;

namespace VirtualVisions.VTility
{
    [Serializable]
    public struct LabelPreset
    {
        public string name;
        public float fontSize;
        public TMP_FontAsset font;
        public FontStyles fontStyle;
        
        public static readonly LabelPreset Fallback = new LabelPreset
        {
            name = "Fallback",
            fontSize = 20,
            font = null,
            fontStyle = default
        };
        
    }
}