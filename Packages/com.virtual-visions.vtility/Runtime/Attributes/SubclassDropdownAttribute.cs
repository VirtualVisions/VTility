using System;
using UnityEngine;

namespace VirtualVisions.VTility
{
    /// <summary>
    /// Display a dropdown of all non-abstract subclasses of the given field.
    /// This must be used in conjunction with the 'SerializeReference' attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class SubclassDropdownAttribute : PropertyAttribute
    {
        public readonly Type baseClass;

        public SubclassDropdownAttribute(Type baseClass)
        {
            this.baseClass = baseClass;
        }
    }
}