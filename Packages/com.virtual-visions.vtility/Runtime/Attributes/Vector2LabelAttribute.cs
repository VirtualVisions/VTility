using UnityEngine;

namespace VirtualVisions.VTility
{
    /// <summary>
    /// Enforce custom labels for the X and Y values on a Vector2 field.
    /// </summary>
    public class Vector2LabelAttribute : PropertyAttribute
    {
        public readonly string xLabel;
        public readonly string yLabel;

        public Vector2LabelAttribute(string xLabel, string yLabel)
        {
            this.xLabel = xLabel;
            this.yLabel = yLabel;
        }
    }

}