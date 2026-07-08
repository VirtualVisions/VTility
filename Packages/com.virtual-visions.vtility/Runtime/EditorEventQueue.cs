using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VirtualVisions.VTility
{
    /// <summary>
    /// Safely queue an event to happen at the next available editor step.
    /// </summary>
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    public class EditorEventQueue
    {
        
        private static event Action _queuedEvent;
        
        static EditorEventQueue()
        {
#if UNITY_EDITOR
            EditorApplication.update += Update;
#endif
        }

        private static void Update()
        {
            if (_queuedEvent == null) return;
            try
            {
                _queuedEvent.Invoke();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            _queuedEvent = null;
        }

        public static void QueueEvent(Action action)
        {
            _queuedEvent += action;
        }
    }
}