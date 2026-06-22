using UnityEngine.UIElements;

namespace VirtualVisions.VTility.Editor
{
    public static class ElementStyleExtensions
    {
        
        public static bool StyleDisplay(this VisualElement element)
        {
            return element.style.display == DisplayStyle.Flex;
        }

        public static void StyleDisplay(this VisualElement element, bool value)
        {
            element.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
        }

        public static bool StyleVisible(this VisualElement element)
        {
            return element.style.visibility == Visibility.Visible;
        }

        public static void StyleVisible(this VisualElement element, bool value)
        {
            element.style.visibility = value ? Visibility.Visible : Visibility.Hidden;
        }
        
    }
}