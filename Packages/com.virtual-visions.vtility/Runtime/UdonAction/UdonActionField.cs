using System;

namespace VirtualVisions.VTility
{

    public enum UdonActionTriggerMode
    {
        EventOnly,
        OnStart,
        OnEnable,
        OnDisable,
    }
    
    [Serializable]
    public class UdonActionField
    {
        public UdonActionTriggerMode mode;
        public UdonEventField[] events;
    }

    /// <summary>
    /// Currently unused.
    /// </summary>
    [Serializable]
    public class UdonActionField<T>
    {
        public UdonEventField<T>[] events;
    }


    public enum UdonEventFieldTargetType
    {
        USharp,
        Udon,
    }

    [Serializable]
    public class UdonEventField
    {
        public UdonEventFieldTargetType type;
        public UnityEngine.Object target;
        public string eventName;
    }

    [Serializable]
    public class UdonEventField<T>
    {
        public UdonEventFieldTargetType type;
        public UnityEngine.Object target;
        public string eventName;
        public string variableName;
    }
}