using UnityEditor;

namespace VirtualVisions.VTility.Editor
{
    /// <summary>
    /// I want these icons for editor professionalism, but not for actually in the scene.
    /// This does a one-time disable so that users can manually enable them.
    /// </summary>
    [InitializeOnLoad]
    public class SceneIconHider
    {

        private const string KEY_HAS_HIDDEN = "VirtualVisions/VTility/HasHiddenIcons";
        
        static SceneIconHider()
        {
            if (EditorPrefs.HasKey(KEY_HAS_HIDDEN)) return;

            EditorPrefs.SetBool(KEY_HAS_HIDDEN, true);
            
            GizmoUtility.SetIconEnabled(typeof(ButtonElement), false);
            GizmoUtility.SetIconEnabled(typeof(SliderElement), false);
            GizmoUtility.SetIconEnabled(typeof(ToggleElement), false);
            GizmoUtility.SetIconEnabled(typeof(DropdownElement), false);
            GizmoUtility.SetIconEnabled(typeof(TextInputElement), false);
        }
    }
}