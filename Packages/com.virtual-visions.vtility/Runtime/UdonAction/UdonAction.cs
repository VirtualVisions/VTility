using System;
using UdonSharp;
using VRC.SDK3.Data;
using VRC.Udon;

namespace VirtualVisions.VTility
{

    public enum UdonAction_Values
    {
        Events,
        OutputValue,
        // ---
        Count
    }
    
    /// <summary>
    /// A subscribable action that can be repeatedly called.
    /// If invoked with a value, the target variable **must** be of type DataToken.
    /// </summary>
    public abstract class UdonAction : DataList
    {
        // Since Udon doesn't natively support fields of this type,
        // it is recommended to use this format for your fields:
        // 
        //    public UdonAction onShow => UdonActions.BackingUdonAction(ref _onShow);
        //    private DataList _onShow;
        //    
        
        
        public static UdonAction Create()
        {
            DataToken[] values = new DataToken[(int)UdonAction_Values.Count];

            values[(int)UdonAction_Values.Events] = new DataDictionary();
            values[(int)UdonAction_Values.OutputValue] = new DataToken();
            
            return (UdonAction)new DataList(values);
        }
        
        
    }

    public static class UdonActionExtensions
    {
        
        public static UdonAction AsUdonAction(this DataToken token) => (UdonAction)token.DataList;
        public static UdonAction AsUdonAction(this DataList list) => (UdonAction)list;
        
        public static UdonAction BackingUdonAction(ref DataList backingValue) =>
            (UdonAction)(backingValue != null ? backingValue : backingValue = UdonAction.Create());
        
        // ---
        
        public static DataDictionary Events(this UdonAction action) => 
            action[(int)UdonAction_Values.Events].DataDictionary;
        
        public static DataToken GetOutput(this UdonAction udonEvent) =>
            udonEvent[(int)UdonAction_Values.OutputValue];

        public static void SetOutput(this UdonAction udonEvent, DataToken token) =>
            udonEvent[(int)UdonAction_Values.OutputValue] = token;
        
        // ---
        
        public static void AddListener(this UdonAction action,
            UdonBehaviour target,
            string eventName,
            bool invokeImmediate = false)
        {
            UdonEvent udonEvent = UdonEvent.Create(target, eventName);
            action.AddListener(udonEvent, invokeImmediate);
        }
        
        public static void AddListener(this UdonAction action,
            UdonBehaviour target,
            string eventName,
            string outputName,
            bool invokeImmediate = false)
        {
            UdonEvent udonEvent = UdonEvent.Create(target, eventName, outputName);
            action.AddListener(udonEvent, invokeImmediate);
        }

        public static void AddListener(this UdonAction action,
            UdonSharpBehaviour target,
            string eventName,
            string outputName,
            bool invokeImmediate = false)
        {
            UdonEvent udonEvent = UdonEvent.Create(target, eventName, outputName);
            action.AddListener(udonEvent, invokeImmediate);
        }

        public static void AddListener(this UdonAction action,
            UdonSharpBehaviour target,
            string eventName,
            bool invokeImmediate = false)
        {
            UdonEvent udonEvent = UdonEvent.Create(target, eventName);
            action.AddListener(udonEvent, invokeImmediate);
        }
        
        public static void AddListener(this UdonAction action,
            UdonEvent udonEvent,
            bool invokeImmediate = false)
        {
            action.Events()[udonEvent.Hash()] = udonEvent;
            if (invokeImmediate) udonEvent._Invoke(action.GetOutput());
        }

        // ---

        public static void RemoveListener(this UdonAction action,
            UdonSharpBehaviour target,
            string eventName)
        {
            UdonEvent comparisonEvent = UdonEvent.Create(target, eventName);
            action.RemoveListener(comparisonEvent);
        }
        public static void RemoveListener(this UdonAction action,
            UdonSharpBehaviour target,
            string eventName,
            string outputName)
        {
            UdonEvent comparisonEvent = UdonEvent.Create(target, eventName, outputName);
            action.RemoveListener(comparisonEvent);
        }

        public static void RemoveListener(this UdonAction action,
            UdonBehaviour target,
            string eventName)
        {
            UdonEvent comparisonEvent = UdonEvent.Create(target, eventName);
            action.RemoveListener(comparisonEvent);
        }
        public static void RemoveListener(this UdonAction action,
            UdonBehaviour target,
            string eventName,
            string outputName)
        {
            UdonEvent comparisonEvent = UdonEvent.Create(target, eventName, outputName);
            action.RemoveListener(comparisonEvent);
        }
        
        public static void RemoveListener(this UdonAction action,
            UdonEvent udonEvent)
        {
            action.Events().Remove(udonEvent.Hash());
        }
        

        public static void _RemoveAllListeners(this UdonAction action)
        {
            action.Events().Clear();
        }
        
        // ---
        
        public static void _Invoke(this UdonAction action, Enum value) => action._Invoke(Convert.ToInt32(value));
        
        public static void _Invoke(this UdonAction action, DataToken value = default)
        {
            DataList events = action.Events().GetValues();
            for (int i = 0; i < events.Count; i++)
            {
                UdonEvent udonEvent = events[i].AsUdonEvent();
                udonEvent._Invoke(value);
            }
        }
    }
}