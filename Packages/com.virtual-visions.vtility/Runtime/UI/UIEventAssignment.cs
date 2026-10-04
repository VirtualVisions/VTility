using TMPro;
using UdonSharp;
#if UNITY_EDITOR && !COMPILER_UDONSHARP
using UdonSharpEditor;
using UnityEditor;
using UnityEditor.Events;
#endif
using UnityEngine;
using UnityEngine.UI;
using VRC.Udon;

namespace VirtualVisions.VTility
{
    public static class UIEventAssignment
    {
        /// <summary>
        /// Force a Unity Canvas Button to have a single callback, running SendCustomEvent on a given UdonSharpBehaviour.
        /// </summary>
        public static void AddUdonListener(this Button button, UdonSharpBehaviour target, string eventName)
        {
#if UNITY_EDITOR && !COMPILER_UDONSHARP
            if (!button || !target) return;
            UdonBehaviour behaviour = UdonSharpEditorUtility.GetBackingUdonBehaviour(target);
            if (!behaviour) return;

            SerializedObject serializedObject = new SerializedObject(button);
            serializedObject.Update();
            SerializedProperty persistentCalls = serializedObject.FindProperty("m_OnClick.m_PersistentCalls.m_Calls");

            if (HasEvent(persistentCalls, behaviour, eventName)) return;

            UnityEventTools.AddStringPersistentListener(button.onClick, behaviour.SendCustomEvent, eventName);
            EditorUtility.SetDirty(button);
#endif
        }


        /// <summary>
        /// Force a Unity Canvas Dropdown to have a single callback, running SendCustomEvent on a given UdonSharpBehaviour.
        /// </summary>
        public static void AddUdonListener(this TMP_Dropdown dropdown, UdonSharpBehaviour target, string eventName)
        {
#if UNITY_EDITOR && !COMPILER_UDONSHARP
            if (!dropdown || !target) return;
            UdonBehaviour behaviour = UdonSharpEditorUtility.GetBackingUdonBehaviour(target);
            if (!behaviour) return;

            SerializedObject serializedObject = new SerializedObject(dropdown);
            serializedObject.Update();
            SerializedProperty persistentCalls = serializedObject.FindProperty("m_OnValueChanged.m_PersistentCalls.m_Calls");

            if (HasEvent(persistentCalls, behaviour, eventName)) return;

            UnityEventTools.AddStringPersistentListener(dropdown.onValueChanged, behaviour.SendCustomEvent, eventName);
            EditorUtility.SetDirty(dropdown);
#endif
        }


        /// <summary>
        /// Force a Unity Canvas Slider to have a single callback, running SendCustomEvent on a given UdonSharpBehaviour.
        /// </summary>
        public static void AddUdonListener(this Slider slider, UdonSharpBehaviour target, string eventName)
        {
#if UNITY_EDITOR && !COMPILER_UDONSHARP
            if (!slider || !target) return;
            UdonBehaviour behaviour = UdonSharpEditorUtility.GetBackingUdonBehaviour(target);
            if (!behaviour) return;

            SerializedObject serializedObject = new SerializedObject(slider);
            serializedObject.Update();
            SerializedProperty persistentCalls = serializedObject.FindProperty("m_OnValueChanged.m_PersistentCalls.m_Calls");

            if (HasEvent(persistentCalls, behaviour, eventName)) return;

            UnityEventTools.AddStringPersistentListener(slider.onValueChanged, behaviour.SendCustomEvent, eventName);
            EditorUtility.SetDirty(slider);
#endif
        }


        /// <summary>
        /// Force a Unity Canvas TMP_InputField to have a single callback, running SendCustomEvent on a given UdonSharpBehaviour.
        /// </summary>
        public static void AddUdonListener(this TMP_InputField inputField, UdonSharpBehaviour target, string eventName)
        {
#if UNITY_EDITOR && !COMPILER_UDONSHARP
            if (!inputField || !target) return;
            UdonBehaviour behaviour = UdonSharpEditorUtility.GetBackingUdonBehaviour(target);
            if (!behaviour) return;

            SerializedObject serializedObject = new SerializedObject(inputField);
            serializedObject.Update();
            SerializedProperty persistentCalls = serializedObject.FindProperty("m_OnValueChanged.m_PersistentCalls.m_Calls");

            if (HasEvent(persistentCalls, behaviour, eventName)) return;

            UnityEventTools.AddStringPersistentListener(inputField.onValueChanged, behaviour.SendCustomEvent, eventName);
            EditorUtility.SetDirty(inputField);
#endif
        }


        /// <summary>
        /// Force a Unity Canvas Toggle to have a single callback, running SendCustomEvent on a given UdonSharpBehaviour.
        /// </summary>
        public static void AddUdonListener(this Toggle toggle, UdonSharpBehaviour target, string eventName)
        {
#if UNITY_EDITOR && !COMPILER_UDONSHARP
            if (!toggle || !target) return;
            UdonBehaviour behaviour = UdonSharpEditorUtility.GetBackingUdonBehaviour(target);
            if (!behaviour) return;

            SerializedObject serializedObject = new SerializedObject(toggle);
            serializedObject.Update();
            SerializedProperty persistentCalls = serializedObject.FindProperty("onValueChanged.m_PersistentCalls.m_Calls");

            if (HasEvent(persistentCalls, behaviour, eventName)) return;

            UnityEventTools.AddStringPersistentListener(toggle.onValueChanged, behaviour.SendCustomEvent, eventName);
            EditorUtility.SetDirty(toggle);
#endif
        }


#if UNITY_EDITOR && !COMPILER_UDONSHARP
        private static bool HasEvent(SerializedProperty persistentCalls, UdonBehaviour target, string eventName)
        {
            for (int i = 0; i < persistentCalls.arraySize; i++)
            {
                SerializedProperty call = persistentCalls.GetArrayElementAtIndex(i);
                Object callTarget = call.FindPropertyRelative("m_Target").objectReferenceValue;
                string callMethod = call.FindPropertyRelative("m_MethodName").stringValue;
                string callArgument = call.FindPropertyRelative("m_Arguments.m_StringArgument").stringValue;

                if (callTarget == target &&
                    callMethod == nameof(UdonBehaviour.SendCustomEvent) &&
                    eventName == callArgument)
                {
                    // Event is already included.
                    return true;
                }
            }

            return false;
        }
#endif
    }
}