#if !COMPILER_UDONSHARP

using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace VirtualVisions.VTility
{
    [CustomPropertyDrawer(typeof(Vector2LabelAttribute))]
    public class Vector2LabelAttributeDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            if (property.propertyType != SerializedPropertyType.Vector2)
            {
                return new Label("Vector2Label must only be used on Vector2 fields.");
            }

            Vector2Field field = new Vector2Field();
            field.AddToClassList("unity-base-field__aligned");
            field.BindProperty(property);
            field.label = property.displayName;


            Vector2LabelAttribute v2label = (Vector2LabelAttribute)attribute;

            FloatField xField = field.Q<FloatField>("unity-x-input");
            xField.style.flexGrow = 1;
            Label xLabel = xField.Q<Label>();
            xLabel.text = v2label.xLabel;
            xLabel.style.flexShrink = 0;
            xLabel.style.flexGrow = 1;

            FloatField yField = field.Q<FloatField>("unity-y-input");
            yField.style.flexGrow = 1;
            Label yLabel = yField.Q<Label>();
            yLabel.text = v2label.yLabel;
            yLabel.style.flexShrink = 0;
            yLabel.style.flexGrow = 1;

            return field;
        }
    }
}

#endif