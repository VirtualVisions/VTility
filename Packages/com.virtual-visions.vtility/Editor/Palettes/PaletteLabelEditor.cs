using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace VirtualVisions.VTility.Editor
{
    [CustomEditor(typeof(PaletteLabel), true)]
    public class PaletteLabelEditor : UnityEditor.Editor
    {

        private PaletteLabel _script;
        private SerializedProperty _paletteName;
        private List<string> _presetNames;

        private void OnEnable()
        {
            _script = (PaletteLabel)target;
            _paletteName = serializedObject.FindProperty(nameof(PaletteColor.paletteName));

            UIPalette palette = _script.transform.GetComponentInParent<UIPalette>();
            _presetNames = palette ? palette.GetLabelPresetNames() : new List<string> { UIPaletteAsset.NONE_FIELD };
        }

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();
            
            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            string current = _paletteName.stringValue;
            List<string> options = _presetNames.ToList();
            if (!options.Contains(current)) options.Insert(0, current);

            DropdownField presets = new DropdownField(options, current);
            presets.label = "Preset";
            presets.AddToClassList("unity-base-field__aligned");
            
            presets.BindProperty(_paletteName);
            root.Add(presets);

            return root;
        }
    }
}
