using TMPro;
using UdonSharp;
#if UNITY_EDITOR && !COMPILER_UDONSHARP
using UdonSharpEditor;
using UnityEditor;
using UnityEditor.Events;
using UnityEngine.Events;
#endif
using UnityEngine;
using UnityEngine.UI;
using VRC.Udon;

namespace VirtualVisions.VTility
{
    public static class UIEventAssignment
    {
        /// <summary>
        /// Append an UdonBehaviour event onto this Button component.
        /// </summary>
        public static void AddUdonListener(this Button button, UdonSharpBehaviour target, string eventName)
        {
#if UNITY_EDITOR && !COMPILER_UDONSHARP
            if (!button || !target) return;
            UdonBehaviour behaviour = UdonSharpEditorUtility.GetBackingUdonBehaviour(target);
            if (!behaviour) return;

            if (HasEvent(button.onClick, behaviour)) return;
            
            UnityEventTools.AddStringPersistentListener(button.onClick, behaviour.SendCustomEvent, eventName);
            EditorUtility.SetDirty(button);
#endif
        }


        /// <summary>
        /// Append an UdonBehaviour event onto this TMP_Dropdown component.
        /// </summary>
        public static void AddUdonListener(this TMP_Dropdown dropdown, UdonSharpBehaviour target, string eventName)
        {
#if UNITY_EDITOR && !COMPILER_UDONSHARP
            if (!dropdown || !target) return;
            UdonBehaviour behaviour = UdonSharpEditorUtility.GetBackingUdonBehaviour(target);
            if (!behaviour) return;

            if (HasEvent(dropdown.onValueChanged, behaviour)) return;

            UnityEventTools.AddStringPersistentListener(dropdown.onValueChanged, behaviour.SendCustomEvent, eventName);
            EditorUtility.SetDirty(dropdown);
#endif
        }


        /// <summary>
        /// Append an UdonBehaviour event onto this Slider component.
        /// </summary>
        public static void AddUdonListener(this Slider slider, UdonSharpBehaviour target, string eventName)
        {
#if UNITY_EDITOR && !COMPILER_UDONSHARP
            if (!slider || !target) return;
            UdonBehaviour behaviour = UdonSharpEditorUtility.GetBackingUdonBehaviour(target);
            if (!behaviour) return;

            if (HasEvent(slider.onValueChanged, behaviour)) return;

            UnityEventTools.AddStringPersistentListener(slider.onValueChanged, behaviour.SendCustomEvent, eventName);
            EditorUtility.SetDirty(slider);
#endif
        }


        /// <summary>
        /// Append an UdonBehaviour event onto this TMP_InputField component.
        /// </summary>
        public static void AddUdonListener(this TMP_InputField inputField, UdonSharpBehaviour target, string eventName)
        {
#if UNITY_EDITOR && !COMPILER_UDONSHARP
            if (!inputField || !target) return;
            UdonBehaviour behaviour = UdonSharpEditorUtility.GetBackingUdonBehaviour(target);
            if (!behaviour) return;

            if (HasEvent(inputField.onValueChanged, behaviour)) return;

            UnityEventTools.AddStringPersistentListener(inputField.onValueChanged, behaviour.SendCustomEvent, eventName);
            EditorUtility.SetDirty(inputField);
#endif
        }


        /// <summary>
        /// Append an UdonBehaviour event onto this Toggle component.
        /// </summary>
        public static void AddUdonListener(this Toggle toggle, UdonSharpBehaviour target, string eventName)
        {
#if UNITY_EDITOR && !COMPILER_UDONSHARP
            if (!toggle || !target) return;
            UdonBehaviour behaviour = UdonSharpEditorUtility.GetBackingUdonBehaviour(target);
            if (!behaviour) return;

            if (HasEvent(toggle.onValueChanged, behaviour)) return;
            
            UnityEventTools.AddStringPersistentListener(toggle.onValueChanged, behaviour.SendCustomEvent, eventName);
            EditorUtility.SetDirty(toggle);
#endif
        }


#if UNITY_EDITOR && !COMPILER_UDONSHARP
        private static bool HasEvent(UnityEventBase evt, UdonBehaviour target)
        {
            for (int i = 0; i < evt.GetPersistentEventCount(); i++)
            {
                Object persistentTarget = evt.GetPersistentTarget(i);
                if (persistentTarget != target) continue;
                
                string persistentMethodName = evt.GetPersistentMethodName(i);
                if (persistentMethodName == nameof(UdonBehaviour.SendCustomEvent)) return true;
            }

            return false;
        }
#endif
    }
}