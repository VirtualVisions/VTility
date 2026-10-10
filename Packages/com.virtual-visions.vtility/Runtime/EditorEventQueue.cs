using System;
#if UNITY_EDITOR && !COMPILER_UDONSHARP
using UnityEditor;
#endif

namespace VirtualVisions.VTility
{
    /// <summary>
    /// Safely queue an event to happen at the next available editor step.
    /// This is the runtime-safe wrapper of EditorApplication.delayCall
    /// </summary>
    public static class EditorEventQueue
    {
        public static void QueueEvent(Action action)
        {
#if UNITY_EDITOR && !COMPILER_UDONSHARP
            EditorApplication.delayCall += () => action?.Invoke();
#endif
        }
    }
}