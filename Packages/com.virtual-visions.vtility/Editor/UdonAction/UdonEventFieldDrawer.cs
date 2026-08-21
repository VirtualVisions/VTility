using System;
using UdonSharp;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using VRC.Udon;

namespace VirtualVisions.VTility.Editor
{
    [CustomPropertyDrawer(typeof(UdonEventField))]
    public class UdonEventFieldDrawer : PropertyDrawer
    {





        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            SerializedProperty propType = property.FindPropertyRelative(nameof(UdonEventField.type));
            SerializedProperty propTarget = property.FindPropertyRelative(nameof(UdonEventField.target));
            SerializedProperty propEventName = property.FindPropertyRelative(nameof(UdonEventField.eventName));

            VisualTreeAsset uxml = Resources.Load<VisualTreeAsset>("UdonEventField");
            VisualElement root = uxml.Instantiate();

            EnumField modeField = root.Q<EnumField>("mode-field");
            ObjectField targetField = root.Q<ObjectField>("target-field");

            VisualElement inputsContainer = root.Q<VisualElement>("inputs");
            VisualElement eventField = root.Q<VisualElement>("event-field");
            TextField eventNameField = root.Q<TextField>("event-name-field");
            VisualElement parameterField = root.Q<VisualElement>("parameter-field");
            TextField parameterNameField = root.Q<TextField>("parameter-name-field");


            modeField.BindProperty(propType);
            targetField.BindProperty(propTarget);
            eventNameField.BindProperty(propEventName);
            
            parameterField.StyleDisplay(false);
            
            modeField.TrackPropertyValue(propType, _ => AssignTargetType());
            targetField.TrackPropertyValue(propTarget, _ => HandleInputVisibility());
            
            AssignTargetType();
            HandleInputVisibility();

            return root;

            void AssignTargetType()
            {
                switch ((UdonEventFieldTargetType)propType.enumValueIndex)
                {
                    case UdonEventFieldTargetType.Udon:
                        targetField.objectType = typeof(UdonBehaviour);
                        break;
                    case UdonEventFieldTargetType.USharp:
                        targetField.objectType = typeof(UdonSharpBehaviour);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            void HandleInputVisibility()
            {
                inputsContainer.SetEnabled(propTarget.objectReferenceValue);
            }
        }
    }
}