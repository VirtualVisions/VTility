using System;
using VRC.Udon;

namespace VirtualVisions.VTility
{
    /// <summary>
    /// Currently unused.
    /// </summary>
    [Serializable]
    public class UdonActionField
    {
        public UdonEventField[] events;

        public void Invoke()
        {
            foreach (UdonEventField evt in events)
            {
                evt.Invoke();
            }
        }
    }

    /// <summary>
    /// Currently unused.
    /// </summary>
    [Serializable]
    public class UdonActionField<T>
    {
        public UdonEventField<T>[] events;

        public void Invoke(T value)
        {
            foreach (UdonEventField<T> evt in events)
            {
                evt.Invoke(value);
            }
        }
    }
    

    [Serializable]
    public class UdonEventField
    {
        public UdonBehaviour target;
        public string eventName;
        
        public void Invoke()
        {
            target.SendCustomEvent(eventName);
        }
    }

    [Serializable]
    public class UdonEventField<T>
    {
        public UdonBehaviour target;
        public string eventName;
        public string variableName;
        
        public void Invoke(T value)
        {
            target.SetProgramVariable(variableName, value);
            target.SendCustomEvent(eventName);
        }
    }
}