using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace VirtualVisions.VTility.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(PaletteLabel), true)]
    public class PaletteLabelEditor : UnityEditor.Editor
    {

        private PaletteLabel _script;
        private SerializedProperty _paletteName;
        private List<string> _presetNames;

        private void OnEnable()
        {
            _script = (PaletteLabel)target;
            _paletteName = serializedObject.FindProperty(nameof(PaletteLabel.paletteName));

            UIPalette palette = _script.transform.GetComponentInParent<UIPalette>();
            _presetNames = palette ? palette.GetLabelPresetNames() : new List<string> { UIPaletteAsset.NONE_FIELD };
        }

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();
            
            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            List<string> options = _presetNames.ToList();
            string existing = _paletteName.stringValue;
            if (!options.Contains(existing))
            {
                existing = _presetNames[0];
                _paletteName.stringValue = existing;
                serializedObject.ApplyModifiedProperties();
            }

            DropdownField presets = new DropdownField(options, existing);
            presets.label = "Preset";
            presets.AddToClassList("unity-base-field__aligned");
            
            presets.BindProperty(_paletteName);
            root.Add(presets);

            return root;
        }
    }
}
