using System;
#if UNITY_EDITOR
using UnityEditor.Callbacks;
#endif
using UnityEngine;

using Object = UnityEngine.Object;

namespace VirtualVisions.VTility
{
    /// <summary>
    /// Destroy an instance of a MonoBehaviour at PostProcessScene(-1).
    /// </summary>
    public class RemoveOnBuildAttribute : Attribute
    {
#if UNITY_EDITOR
        [PostProcessScene(-1)]
        public static void OnPostProcessScene()
        {
            MonoBehaviour[] allScripts =
                Object.FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            foreach (MonoBehaviour script in allScripts)
            {
                Attribute attribute = GetCustomAttribute(script.GetType(), typeof(RemoveOnBuildAttribute));
                if (attribute != null)
                {
                    Object.DestroyImmediate(script);
                }
            }
        }
#endif
    }
}