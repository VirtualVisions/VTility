#if !COMPILER_UDONSHARP

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace VirtualVisions.VTility
{
    [CustomPropertyDrawer(typeof(SubclassDropdownAttribute))]
    public class SubclassDropdownAttributeDrawer : PropertyDrawer
    {

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualTreeAsset uxml = Resources.Load<VisualTreeAsset>("SubclassDropdown");
            VisualElement root = new VisualElement();
            uxml.CloneTree(root);

            Foldout foldout = root.Q<Foldout>("TitleFoldout");
            DropdownField dropdown = root.Q<DropdownField>("TypeDropdown");
            VisualElement container = root.Q<VisualElement>("Container");

            foldout.value = property.isExpanded;
            foldout.text = property.displayName;
            foldout.Q<Label>().AddToClassList("unity-base-field__label");

            foldout.RegisterValueChangedCallback(evt =>
            {
                property.isExpanded = evt.newValue;
                property.serializedObject.ApplyModifiedProperties();
                ApplyFoldout();
            });


            Dictionary<string, string> typeDict = new Dictionary<string, string>();
            SubclassDropdownAttribute subclassDropdown = (SubclassDropdownAttribute)attribute;
            Type baseType = subclassDropdown.baseClass;
            TypeCache.TypeCollection allTypes = TypeCache.GetTypesDerivedFrom(baseType);
            List<Type> types = allTypes.Where(type => !type.IsAbstract).ToList();

            dropdown.RegisterValueChangedCallback(OnDropdownChanged);

            List<string> choices = new List<string>();

            foreach (Type t in types)
            {
                string typeName = GetClassName(t);
                choices.Add(typeName);
                typeDict[typeName] = t.FullName;
            }

            choices.Sort();

            dropdown.choices = choices;
            dropdown.SetValueWithoutNotify(CurrentTypeName());

            ApplyFoldout();
            BuildChildFields();

            return root;

            void BuildChildFields()
            {
                container.Clear();
                string description = GetClassDescription(property.managedReferenceValue?.GetType());
                if (!string.IsNullOrEmpty(description))
                {
                    Label descriptionLabel = new Label(description);
                    descriptionLabel.style.paddingTop = 10;
                    descriptionLabel.style.paddingBottom = 10;
                    container.Add(descriptionLabel);
                }

                IEnumerable<SerializedProperty> properties = EnumerateChildren(property);

                foreach (SerializedProperty prop in properties)
                {
                    PropertyField field = new PropertyField(prop);
                    field.BindProperty(prop);
                    container.Add(field);
                }
            }

            void ApplyFoldout()
            {
                container.style.display = property.isExpanded ? DisplayStyle.Flex : DisplayStyle.None;
            }

            void OnDropdownChanged(ChangeEvent<string> evt)
            {
                string currentType = CurrentTypeName();
                if (currentType != evt.newValue)
                {
                    property.managedReferenceValue = null;

                    if (typeDict.TryGetValue(evt.newValue, out string typeName))
                    {
                        Type foundType = Type.GetType(typeName);
                        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                        {
                            Type type = assembly.GetType(typeName);
                            if (type == null) continue;
                            
                            foundType = type;
                            break;
                        }

                        if (foundType != null)
                        {
                            property.managedReferenceValue = Activator.CreateInstance(foundType);
                            property.serializedObject.ApplyModifiedProperties();
                            property.serializedObject.Update();
                        }
                        else
                        {
                            Debug.LogWarning($"{evt.newValue} is not real type.");
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"Type {evt.newValue} was not found in TypeCache.");
                    }
                }
                else
                {
                    Debug.LogWarning($"Type already is {evt.newValue}");
                }

                BuildChildFields();
            }

            string CurrentTypeName() => GetClassName(property.managedReferenceValue?.GetType());


            static IEnumerable<SerializedProperty> EnumerateChildren(SerializedProperty parent)
            {
                if (parent == null) yield break;


                if (parent.isArray && parent.propertyType != SerializedPropertyType.String)
                {
                    int count = parent.arraySize;
                    for (int i = 0; i < count; i++)
                        yield return parent.GetArrayElementAtIndex(i).Copy();

                    yield break;
                }


                SerializedProperty current = parent.Copy();
                SerializedProperty end = current.GetEndProperty();

                bool hasChild = current.NextVisible(true);
                if (!hasChild) yield break;


                while (!SerializedProperty.EqualContents(current, end) && current.depth > parent.depth)
                {
                    if (current.depth == parent.depth + 1)
                        yield return current.Copy();

                    if (!current.NextVisible(false)) break;
                }
            }
        }

        public static string GetClassName(Type t)
        {
            if (t == null) return string.Empty;
            string typeName = ObjectNames.NicifyVariableName(t.Name);
            if (t.IsDefined(typeof(ClassDescriptionAttribute), true))
            {
                if (Attribute.GetCustomAttribute(t, typeof(ClassDescriptionAttribute)) is ClassDescriptionAttribute description)
                {
                    typeName = description.Title;
                }
            }

            return typeName;
        }

        public static string GetClassDescription(Type t)
        {
            if (t == null) return string.Empty;
            string typeDescription = string.Empty;
            if (t.IsDefined(typeof(ClassDescriptionAttribute), true))
            {
                ClassDescriptionAttribute description =
                    Attribute.GetCustomAttribute(t, typeof(ClassDescriptionAttribute)) as ClassDescriptionAttribute;
                if (description != null)
                {
                    typeDescription = description.Description;
                }
            }

            return typeDescription;
        }
    }
}
#endif