using System.Collections.Generic;
using UdonSharp;
using UdonSharpEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using VRC.Udon;

namespace VirtualVisions.VTility.Editor
{
    public static class UdonActionEventPPS
    {
        [PostProcessScene(-10)]
        public static void OnPostProcessScene()
        {
            UdonActionEvent[] udonActions = Object.FindObjectsByType<UdonActionEvent>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None);

            foreach (UdonActionEvent udonAction in udonActions)
            {
                UdonActionEventRuntime runtime = udonAction.GetComponent<UdonActionEventRuntime>();
                if (!runtime) continue;

                UdonActionTriggerMode mode = udonAction.mode;
                List<UdonBehaviour> targets = new List<UdonBehaviour>();
                List<string> eventNames = new List<string>();

                foreach (UdonEventField udonEvent in udonAction.events)
                {
                    if (!udonEvent.target) continue;
                    
                    UdonBehaviour behaviour;
                    if (udonEvent.target as UdonBehaviour)
                    {
                        behaviour = (UdonBehaviour)udonEvent.target;
                    }
                    else if (udonEvent.target as UdonSharpBehaviour)
                    {
                        behaviour = UdonSharpEditorUtility.GetBackingUdonBehaviour((UdonSharpBehaviour)udonEvent.target);
                    }
                    else
                    {
                        continue;
                    }

                    targets.Add(behaviour);
                    eventNames.Add(udonEvent.eventName);
                }

                runtime.mode = mode;
                runtime.targets = targets.ToArray();
                runtime.eventNames = eventNames.ToArray();
            }
        }
    }
}