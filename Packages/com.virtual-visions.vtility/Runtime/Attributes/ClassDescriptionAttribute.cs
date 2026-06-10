using System;
using UnityEngine;

namespace VirtualVisions.VTility
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ClassDescriptionAttribute : PropertyAttribute
    {
        public string Title;
        public string Description;

        public ClassDescriptionAttribute(string title)
        {
            Title = title;
        }

        public ClassDescriptionAttribute(string title, string description)
        {
            Title = title;
            Description = description;
        }
    }
}