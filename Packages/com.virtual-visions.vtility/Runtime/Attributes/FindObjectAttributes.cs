using System;
using UnityEngine;

namespace VirtualVisions.VTility
{
    /// <summary>
    /// Runs "FindObjectOfType" during PostProcessScene.
    /// Field must be Serialized.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class FindComponentAttribute : PropertyAttribute
    {
    }

    /// <summary>
    /// Runs "FindObjectOfType" during PostProcessScene.
    /// Field must be Serialized.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class FindObjectAttribute : PropertyAttribute
    {
    }
    
    /// <summary>
    /// Runs "GetComponent" on the instance's Parent transform during PostProcessScene.
    /// Field must be Serialized.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class FindParentAttribute : PropertyAttribute
    {
    }
    
    /// <summary>
    /// Runs "FindObjectsOfType" during PostProcessScene.
    /// Field must be an array and Serialized.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class FindObjectArrayAttribute : PropertyAttribute
    {
    }
    
    /// <summary>
    /// Runs "GetComponentsInChildren" on the instance's Parent transform during PostProcessScene.
    /// Field must be an array and Serialized.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class FindSiblingsAttribute : PropertyAttribute
    {
    }
    
    /// <summary>
    /// Runs "GetComponentsInChildren" during PostProcessScene.
    /// Field must be an array and Serialized.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class FindChildrenAttribute : PropertyAttribute
    {
    }
}