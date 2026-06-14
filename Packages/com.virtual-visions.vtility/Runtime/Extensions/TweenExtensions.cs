using VRC.SDK3.Components;

namespace VirtualVisions.VTility
{
    public static class TweenExtensions
    {
        public static void TryComplete(this VRCTweenHandle tween)
        {
            if (tween.IsActive) tween.Complete();
        }

        public static void TryKill(this VRCTweenHandle tween)
        {
            if (tween.IsActive) tween.Kill();
        }
    }
}