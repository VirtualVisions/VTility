using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using VRC.SDK3.Components;

using Object = UnityEngine.Object;

namespace VirtualVisions.VTility.Editor
{
    public static class AttributePostProcessScene
    {

        [PostProcessScene(-9999)]
        public static void OnPostProcessScene()
        {
            MonoBehaviour[] allScripts =
                Object.FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            foreach (MonoBehaviour script in allScripts)
            {
                Type type = script.GetType();
                FieldInfo[] fields =
                    type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                foreach (FieldInfo field in fields)
                {
                    Type fieldType = field.FieldType;
                    
                    // Find Component Handling
                    FindComponentAttribute findComponent = field.GetCustomAttribute<FindComponentAttribute>();
                    if (findComponent != null && typeof(Component).IsAssignableFrom(field.FieldType))
                    {
                        Object foundTarget = script.transform.GetComponent(fieldType);
                        if (foundTarget)
                        {
                            field.SetValue(script, foundTarget);
                            EditorUtility.SetDirty(script);
                        }
                    }
                    
                    // Find Object Handling
                    FindObjectAttribute findObject = field.GetCustomAttribute<FindObjectAttribute>();
                    if (findObject != null && typeof(MonoBehaviour).IsAssignableFrom(field.FieldType))
                    {
                        Object foundTarget = Object.FindObjectOfType(fieldType, true);
                        if (foundTarget)
                        {
                            field.SetValue(script, foundTarget);
                            EditorUtility.SetDirty(script);
                        }
                    }

                    // Find Parent Handling
                    FindParentAttribute findParent = field.GetCustomAttribute<FindParentAttribute>();
                    if (findParent != null && typeof(MonoBehaviour).IsAssignableFrom(field.FieldType))
                    {
                        Object foundTarget = script.transform.GetComponentInParent(fieldType);
                        if (foundTarget)
                        {
                            field.SetValue(script, foundTarget);
                            EditorUtility.SetDirty(script);
                        }
                    }

                    // Find Object Array Handling
                    FindObjectArrayAttribute findObjectArray = field.GetCustomAttribute<FindObjectArrayAttribute>();
                    if (findObjectArray != null && fieldType.IsArray)
                    {
                        Type elementType = fieldType.GetElementType();
                        if (elementType != null && typeof(MonoBehaviour).IsAssignableFrom(elementType))
                        {
                            MonoBehaviour[] foundTargets = (MonoBehaviour[])Object.FindObjectsOfType(elementType, true);

                            // We don't want the PlayerObject version in the list since VRChat turns that into a non-accessible object.
                            List<MonoBehaviour> sanitizedTargets =
                                foundTargets.Where(found => !found.GetComponent<VRCPlayerObject>()).ToList();

                            if (sanitizedTargets.Count != 0)
                            {
                                Array array = Array.CreateInstance(elementType, sanitizedTargets.Count);

                                for (int i = 0; i < array.Length; i++)
                                    array.SetValue(sanitizedTargets[i], i);

                                field.SetValue(script, array);
                                EditorUtility.SetDirty(script);
                            }
                        }
                    }

                    // Find Siblings Handling
                    FindSiblingsAttribute findSiblings = field.GetCustomAttribute<FindSiblingsAttribute>();
                    if (findSiblings != null && fieldType.IsArray && script.transform.parent)
                    {
                        Type elementType = fieldType.GetElementType();
                        if (elementType != null && typeof(Component).IsAssignableFrom(elementType))
                        {
                            Component[] foundTargets = script.transform.parent.GetComponentsInChildren(elementType, true);

                            // We don't want the PlayerObject version in the list since VRChat turns that into a non-accessible object.
                            List<Component> sanitizedTargets =
                                foundTargets.Where(found => !found.GetComponent<VRCPlayerObject>()).ToList();

                            if (sanitizedTargets.Count != 0)
                            {
                                Array array = Array.CreateInstance(elementType, sanitizedTargets.Count);

                                for (int i = 0; i < array.Length; i++)
                                    array.SetValue(sanitizedTargets[i], i);

                                field.SetValue(script, array);
                                EditorUtility.SetDirty(script);
                            }
                        }
                    }

                    // Find Children Handling
                    FindChildrenAttribute findChildren = field.GetCustomAttribute<FindChildrenAttribute>();
                    if (findChildren != null && fieldType.IsArray && script.transform.parent)
                    {
                        Type elementType = fieldType.GetElementType();
                        if (elementType != null && typeof(Component).IsAssignableFrom(elementType))
                        {
                            Component[] foundTargets = script.GetComponentsInChildren(elementType, true);

                            // We don't want the PlayerObject version in the list since VRChat turns that into a non-accessible object.
                            List<Component> sanitizedTargets =
                                foundTargets.Where(found => !found.GetComponent<VRCPlayerObject>()).ToList();

                            if (sanitizedTargets.Count != 0)
                            {
                                Array array = Array.CreateInstance(elementType, sanitizedTargets.Count);

                                for (int i = 0; i < array.Length; i++)
                                    array.SetValue(sanitizedTargets[i], i);

                                field.SetValue(script, array);
                                EditorUtility.SetDirty(script);
                            }
                        }
                    }
                }
            }
        }
    }
}