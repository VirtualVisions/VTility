using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace VirtualVisions.VTility
{
    [CustomPropertyDrawer(typeof(KeyCodeDisplayAttribute))]
    public class KeyCodeDisplayAttributeDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            if (property.propertyType != SerializedPropertyType.Integer)
            {
                return new Label("KeyCodeDisplay must only be used on Integer fields.");
            }

            EnumField field = new EnumField((KeyCode)property.intValue);
            field.AddToClassList("unity-base-field__aligned");

            field.RegisterValueChangedCallback(evt =>
            {
                property.intValue = Convert.ToInt32(evt.newValue);
                property.serializedObject.ApplyModifiedProperties();
            });

            return field;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Integer)
            {
                EditorGUI.LabelField(position, "KeyCodeDisplay must only be used on Integer fields.");
                return;
            }

            using (EditorGUI.ChangeCheckScope scope = new EditorGUI.ChangeCheckScope())
            {
                Enum newValue = EditorGUI.EnumPopup(position, new GUIContent(label), (KeyCode)property.intValue );

                if (scope.changed)
                {
                    property.intValue = Convert.ToInt32(newValue);
                    property.serializedObject.ApplyModifiedProperties();
                }
            }

        }
    }
}