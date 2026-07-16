using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace VirtualVisions.VTility.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(PaletteColor), true)]
    public class PaletteColorEditor : UnityEditor.Editor
    {

        private PaletteColor _script;
        private SerializedProperty _paletteName;
        private List<string> _presetNames;

        private void OnEnable()
        {
            _script = (PaletteColor)target;
            _paletteName = serializedObject.FindProperty(nameof(PaletteColor.paletteName));

            UIPalette palette = _script.transform.GetComponentInParent<UIPalette>();
            _presetNames = palette ? palette.GetColorPresetNames() : new List<string> { UIPaletteAsset.NONE_FIELD };
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