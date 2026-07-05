using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace VirtualVisions.VTility.Editor
{
    [CustomPropertyDrawer(typeof(TweenAction))]
    public class VRCTweenActionDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            SerializedProperty typeProp = property.FindPropertyRelative(nameof(TweenAction.type));
            SerializedProperty easeProp = property.FindPropertyRelative(nameof(TweenAction.ease));
            SerializedProperty useStartValueProp = property.FindPropertyRelative(nameof(TweenAction.useStartValue));
            
            SerializedProperty pathPointsProp = property.FindPropertyRelative(nameof(TweenAction.pathPoints));
            SerializedProperty colorNameProp = property.FindPropertyRelative(nameof(TweenAction.colorName));
            SerializedProperty floatNameProp = property.FindPropertyRelative(nameof(TweenAction.floatName));

            SerializedProperty transformProp = property.FindPropertyRelative(nameof(TweenAction.transform));
            SerializedProperty gameObjectProp = property.FindPropertyRelative(nameof(TweenAction.gameObject));
            SerializedProperty graphicProp = property.FindPropertyRelative(nameof(TweenAction.graphic));
            SerializedProperty canvasGroupProp = property.FindPropertyRelative(nameof(TweenAction.canvasGroup));
            SerializedProperty sliderProp = property.FindPropertyRelative(nameof(TweenAction.slider));
            SerializedProperty rectTransformProp = property.FindPropertyRelative(nameof(TweenAction.rectTransform));
            SerializedProperty rendererProp = property.FindPropertyRelative(nameof(TweenAction.renderer));
            SerializedProperty lightProp = property.FindPropertyRelative(nameof(TweenAction.light));
            SerializedProperty audioSourceProp = property.FindPropertyRelative(nameof(TweenAction.audioSource));
            SerializedProperty spriteProp = property.FindPropertyRelative(nameof(TweenAction.sprite));

            SerializedProperty startFloatProp = property.FindPropertyRelative(nameof(TweenAction.startFloat));
            SerializedProperty startVector2Prop = property.FindPropertyRelative(nameof(TweenAction.startVector2));
            SerializedProperty startVector3Prop = property.FindPropertyRelative(nameof(TweenAction.startVector3));
            SerializedProperty startColorProp = property.FindPropertyRelative(nameof(TweenAction.startColor));

            SerializedProperty endFloatProp = property.FindPropertyRelative(nameof(TweenAction.endFloat));
            SerializedProperty endVector2Prop = property.FindPropertyRelative(nameof(TweenAction.endVector2));
            SerializedProperty endVector3Prop = property.FindPropertyRelative(nameof(TweenAction.endVector3));
            SerializedProperty endColorProp = property.FindPropertyRelative(nameof(TweenAction.endColor));

            VisualElement root = new VisualElement();

            PropertyField typeField = new PropertyField(typeProp);
            root.Add(typeField);
            PropertyField easeField = new PropertyField(easeProp);
            root.Add(easeField);
            PropertyField useStartValueField = new PropertyField(useStartValueProp);
            root.Add(useStartValueField);
            
            PropertyField transformField = new PropertyField(transformProp);
            root.Add(transformField);
            PropertyField gameObjectField = new PropertyField(gameObjectProp);
            root.Add(gameObjectField);
            PropertyField graphicField = new PropertyField(graphicProp);
            root.Add(graphicField);
            PropertyField canvasGroupField = new PropertyField(canvasGroupProp);
            root.Add(canvasGroupField);
            PropertyField sliderField = new PropertyField(sliderProp);
            root.Add(sliderField);
            PropertyField rectTransformField = new PropertyField(rectTransformProp);
            root.Add(rectTransformField);
            PropertyField rendererField = new PropertyField(rendererProp);
            root.Add(rendererField);
            PropertyField lightField = new PropertyField(lightProp);
            root.Add(lightField);
            PropertyField audioSourceField = new PropertyField(audioSourceProp);
            root.Add(audioSourceField);
            PropertyField spriteField = new PropertyField(spriteProp);
            root.Add(spriteField);
            
            PropertyField pathPointsField = new PropertyField(pathPointsProp);
            root.Add(pathPointsField);
            PropertyField colorNameField = new PropertyField(colorNameProp);
            root.Add(colorNameField);
            PropertyField floatNameField = new PropertyField(floatNameProp);
            root.Add(floatNameField);

            PropertyField startFloatField = new PropertyField(startFloatProp);
            startFloatField.label = "Start Value";
            root.Add(startFloatField);
            PropertyField startVector2Field = new PropertyField(startVector2Prop);
            startVector2Field.label = "Start Value";
            root.Add(startVector2Field);
            PropertyField startVector3Field = new PropertyField(startVector3Prop);
            startVector3Field.label = "Start Value";
            root.Add(startVector3Field);
            PropertyField startColorField = new PropertyField(startColorProp);
            startColorField.label = "Start Value";
            root.Add(startColorField);
            
            PropertyField endFloatField = new PropertyField(endFloatProp);
            endFloatField.label = "End Value";
            root.Add(endFloatField);
            PropertyField endVector2Field = new PropertyField(endVector2Prop);
            endVector2Field.label = "End Value";
            root.Add(endVector2Field);
            PropertyField endVector3Field = new PropertyField(endVector3Prop);
            endVector3Field.label = "End Value";
            root.Add(endVector3Field);
            PropertyField endColorField = new PropertyField(endColorProp);
            endColorField.label = "End Value";
            root.Add(endColorField);

            typeField.RegisterValueChangeCallback(_ => DisplayFields());
            useStartValueField.RegisterValueChangeCallback(_ => DisplayFields());

            DisplayFields();
            return root;

            void DisplayFields()
            {
                transformField.StyleDisplay(false);
                gameObjectField.StyleDisplay(false);
                graphicField.StyleDisplay(false);
                canvasGroupField.StyleDisplay(false);
                sliderField.StyleDisplay(false);
                rectTransformField.StyleDisplay(false);
                rendererField.StyleDisplay(false);
                lightField.StyleDisplay(false);
                audioSourceField.StyleDisplay(false);
                spriteField.StyleDisplay(false);
                
                pathPointsField.StyleDisplay(false);
                colorNameField.StyleDisplay(false);
                floatNameField.StyleDisplay(false);
                
                startFloatField.StyleDisplay(false);
                startVector2Field.StyleDisplay(false);
                startVector3Field.StyleDisplay(false);
                startColorField.StyleDisplay(false);
                
                endFloatField.StyleDisplay(false);
                endVector2Field.StyleDisplay(false);
                endVector3Field.StyleDisplay(false);
                endColorField.StyleDisplay(false);

                bool useStartValue = useStartValueProp.boolValue;
                
                switch ((TweenType)typeProp.enumValueIndex)
                {
                    case TweenType.Position:
                        transformField.StyleDisplay(true);
                        DisplayStartEndFields(startVector3Field, endVector3Field);
                        break;
                    case TweenType.LocalPosition:
                        transformField.StyleDisplay(true);
                        DisplayStartEndFields(startVector3Field, endVector3Field);
                        break;
                    case TweenType.Rotation:
                        transformField.StyleDisplay(true);
                        DisplayStartEndFields(startVector3Field, endVector3Field);
                        break;
                    case TweenType.LocalRotation:
                        transformField.StyleDisplay(true);
                        DisplayStartEndFields(startVector3Field, endVector3Field);
                        break;
                    case TweenType.Scale:
                        transformField.StyleDisplay(true);
                        DisplayStartEndFields(startVector3Field, endVector3Field);
                        break;
                    case TweenType.Path:
                        gameObjectField.StyleDisplay(true);
                        pathPointsField.StyleDisplay(true);
                        break;
                    case TweenType.LocalPath:
                        gameObjectField.StyleDisplay(true);
                        pathPointsField.StyleDisplay(true);
                        break;
                    case TweenType.GraphicColor:
                        graphicField.StyleDisplay(true);
                        DisplayStartEndFields(startColorField, endColorField);
                        break;
                    case TweenType.GraphicFade:
                        graphicField.StyleDisplay(true);
                        DisplayStartEndFields(startFloatField, endFloatField);
                        break;
                    case TweenType.CanvasGroupFade:
                        canvasGroupField.StyleDisplay(true);
                        DisplayStartEndFields(startFloatField, endFloatField);
                        break;
                    case TweenType.SliderValue:
                        sliderField.StyleDisplay(true);
                        DisplayStartEndFields(startFloatField, endFloatField);
                        break;
                    case TweenType.AnchorPosition:
                        rectTransformField.StyleDisplay(true);
                        DisplayStartEndFields(startVector2Field, endVector2Field);
                        break;
                    case TweenType.SizeDelta:
                        rectTransformField.StyleDisplay(true);
                        DisplayStartEndFields(startVector2Field, endVector2Field);
                        break;
                    case TweenType.RendererColor:
                        rendererField.StyleDisplay(true);
                        colorNameField.StyleDisplay(true);
                        DisplayStartEndFields(startColorField, endColorField);
                        break;
                    case TweenType.RendererFloat:
                        rendererField.StyleDisplay(true);
                        floatNameField.StyleDisplay(true);
                        DisplayStartEndFields(startFloatField, endFloatField);
                        break;
                    case TweenType.LightIntensity:
                        lightField.StyleDisplay(true);
                        DisplayStartEndFields(startFloatField, endFloatField);
                        break;
                    case TweenType.LightColor:
                        lightField.StyleDisplay(true);
                        DisplayStartEndFields(startColorField, endColorField);
                        break;
                    case TweenType.Volume:
                        audioSourceField.StyleDisplay(true);
                        DisplayStartEndFields(startFloatField, endFloatField);
                        break;
                    case TweenType.Pitch:
                        audioSourceField.StyleDisplay(true);
                        DisplayStartEndFields(startFloatField, endFloatField);
                        break;
                    case TweenType.SpriteColor:
                        spriteField.StyleDisplay(true);
                        DisplayStartEndFields(startColorField, endColorField);
                        break;
                    case TweenType.SpriteFade:
                        spriteField.StyleDisplay(true);
                        DisplayStartEndFields(startFloatField, endFloatField);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                return;

                void DisplayStartEndFields(PropertyField startField, PropertyField endField)
                {
                    if (useStartValue) startField.StyleDisplay(true);
                    endField.StyleDisplay(true);
                }
            }
        }
    }
}