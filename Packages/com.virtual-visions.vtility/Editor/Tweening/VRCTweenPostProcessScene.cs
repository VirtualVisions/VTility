using UnityEditor.Callbacks;
using UnityEngine;

namespace VirtualVisions.VTility.Editor
{
    public class VRCTweenPostProcessScene
    {
        [PostProcessScene(-10)]
        public static void OnPostProcessScene()
        {
            VRCTweenAction[] tweens = Object.FindObjectsByType<VRCTweenAction>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (VRCTweenAction tween in tweens)
            {
                tween.PopulateUdon();
            }
        }
    }
}