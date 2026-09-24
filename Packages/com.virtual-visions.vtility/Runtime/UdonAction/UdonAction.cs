using UdonSharp;
using VRC.SDK3.Data;
using VRC.Udon;

namespace VirtualVisions.VTility
{

    public enum UdonAction_Values
    {
        Events,
        // ---
        Count
    }
    
    /// <summary>
    /// A subscribable action that can be repeatedly called.
    /// </summary>
    public abstract class UdonAction : DataList
    {
        // Since Udon doesn't natively support fields of this type,
        // it is recommended to use this format for your fields:
        // 
        //    public UdonAction bindItem => (UdonAction)(_bindItem != null ? _bindItem : _bindItem = UdonAction.Create());
        //    private DataList _bindItem;
        //    
        
    }

    public static partial class UdonActions
    {

        public static UdonAction Create()
        {
            DataToken[] values = new DataToken[(int)UdonAction_Values.Count];

            values[(int)UdonAction_Values.Events] = new DataDictionary();
            
            return (UdonAction)new DataList(values);
        }
        
        
        
        
        public static UdonAction AsUdonAction(this DataToken token) => (UdonAction)token.DataList;
        public static UdonAction BackingUdonAction(ref DataList backingValue) =>
            (UdonAction)(backingValue != null ? backingValue : backingValue = UdonActions.Create());
        
        
        public static DataDictionary Events(this UdonAction action) => action[(int)UdonAction_Values.Events].DataDictionary;



        public static void AddListener(this UdonAction action,
            UdonEvent udonEvent,
            bool invokeImmediate = false)
        {
            action.Events()[udonEvent.Hash()] = udonEvent;
            if (invokeImmediate) udonEvent._Invoke();
        }

        public static void AddListener(this UdonAction action,
            UdonBehaviour target,
            string eventName,
            bool invokeImmediate = false)
        {
            UdonEvent udonEvent = UdonEvents.Create(target, eventName);
            action.AddListener(udonEvent);
            if (invokeImmediate) udonEvent._Invoke();
        }

        public static void AddListener(this UdonAction action,
            UdonSharpBehaviour target,
            string eventName,
            bool invokeImmediate = false)
        {
            UdonEvent udonEvent = UdonEvents.Create(target, eventName);
            action.AddListener(udonEvent);
            if (invokeImmediate) udonEvent._Invoke();
        }


        public static void RemoveListener(this UdonAction action,
            UdonEvent udonEvent)
        {
            action.Events().Remove(udonEvent.Hash());
        }

        public static void RemoveListener(this UdonAction action,
            UdonSharpBehaviour target,
            string eventName)
        {
            UdonEvent comparisonEvent = UdonEvents.Create(target, eventName);
            action.RemoveListener(comparisonEvent);
        }

        public static void RemoveListener(this UdonAction action,
            UdonBehaviour target,
            string eventName)
        {
            UdonEvent comparisonEvent = UdonEvents.Create(target, eventName);
            action.RemoveListener(comparisonEvent);
        }


        public static void _RemoveAllListeners(this UdonAction action)
        {
            action.Events().Clear();
        }


        public static void _Invoke(this UdonAction action)
        {
            DataList events = action.Events().GetValues();
            for (int i = 0; i < events.Count; i++)
            {
                UdonEvent udonEvent = events[i].AsUdonEvent();
                udonEvent._Invoke();
            }
        }
    }
}