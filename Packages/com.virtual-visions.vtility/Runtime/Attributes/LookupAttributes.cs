using System;
using UnityEngine;

namespace VirtualVisions.VTility
{
    /// <summary>
    /// Runs "GetComponent" on the target object during PostProcessScene.
    /// Field must be Serialized.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class GetComponentAttribute : PropertyAttribute
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
    /// Runs "GetComponentInParent" on the target object during PostProcessScene.
    /// Field must be Serialized.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class GetParentAttribute : PropertyAttribute
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
    public class GetSiblingsAttribute : PropertyAttribute
    {
    }
    
    /// <summary>
    /// Runs "GetComponentsInChildren" during PostProcessScene.
    /// Field must be an array and Serialized.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class GetChildrenAttribute : PropertyAttribute
    {
    }
}