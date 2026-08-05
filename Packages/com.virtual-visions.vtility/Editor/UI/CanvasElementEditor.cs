using System;
using UdonSharpEditor;
using UnityEditor;
using UnityEngine;

namespace VirtualVisions.VTility.Editor
{
    [CustomEditor(typeof(CanvasElement), true)]
    public class CanvasElementEditor : UnityEditor.Editor
    {

        private CanvasElement _script;
        
        private void OnEnable()
        {
            _script = (CanvasElement)target;
        }

        public override void OnInspectorGUI()
        {
            if (UdonSharpGUI.DrawDefaultUdonSharpBehaviourHeader(target)) return;

            ElementDescriptor descriptor = _script.GetComponent<ElementDescriptor>();
            if (!descriptor)
            {
                if (GUILayout.Button("Add Descriptor"))
                {
                    UdonSharpUndo.AddComponent<ElementDescriptor>(_script.gameObject);
                }
            }
            
            base.OnInspectorGUI();
        }
    }
}