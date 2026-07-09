#if UNITY_EDITOR && !COMPILER_UDONSHARP
using UnityEditor;
#endif
using UnityEngine;

namespace VirtualVisions.VTility
{
    public static class GameObjectExtensions
    {
        
        /// <summary>
        /// Safely destroy all child GameObjects of a given Transform.
        /// If done in the editor, records the destruction of each object to the Undo buffer.
        /// </summary>
        public static void DestroyChildren(this Transform transform)
        {
            int count = transform.childCount;
            while (count > 0)
            {
                count--;
#if UNITY_EDITOR && !COMPILER_UDONSHARP
                Undo.DestroyObjectImmediate(transform.GetChild(count).gameObject);
#else
                Object.Destroy(transform.GetChild(count).gameObject);
#endif
            }
        }

        /// <summary>
        /// Safely destroy all child GameObjects of a given GameObject.
        /// If done in the editor, records the destruction of each object to the Undo buffer.
        /// </summary>
        public static void DestroyChildren(this GameObject obj) => DestroyChildren(obj.transform);

    }
}