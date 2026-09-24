using UdonSharp;
using VRC.SDK3.Data;
using VRC.Udon;

namespace VirtualVisions.VTility
{
    public enum UdonActionGeneric_Values
    {
        Events,
        Value,
        // ---
        Count
    }
    
    /// <summary>
    /// A subscribable action that can be repeatedly called with passable variables.
    /// </summary>
    public abstract class UdonAction<T> : DataList
    {
        // Since Udon doesn't natively support fields of this type,
        // it is recommended to use this format for your fields:
        // 
        //    public UdonAction<float> onValueChanged => UdonActions.BackingUdonAction<float>(ref _onValueChanged);
        //    private DataList _onValueChanged;
    }

    public static partial class UdonActions
    {
        
        public static UdonAction<T> Create<T>(T defaultValue)
        {
            DataToken[] values = new DataToken[(int)UdonActionGeneric_Values.Count];
            
            values[(int)UdonActionGeneric_Values.Events] = new DataDictionary();
            values[(int)UdonActionGeneric_Values.Value] = new DataToken(defaultValue);
            
            return (UdonAction<T>)new DataList(values);
        }
        
        
        
        
        public static UdonAction<T> AsUdonAction<T>(this DataToken token) => (UdonAction<T>)token.DataList;
        public static UdonAction<T> BackingUdonAction<T>(ref DataList backingValue, T defaultValue) =>
            (UdonAction<T>)(backingValue != null ? backingValue : backingValue = Create(defaultValue));
        
        
        public static DataDictionary Events<T>(this UdonAction<T> action) => action[(int)UdonActionGeneric_Values.Events].DataDictionary;
        public static T Value<T>(this UdonAction<T> action) => (T)(object)action[(int)UdonActionGeneric_Values.Value];
        public static void SetValue<T>(this UdonAction<T> action, T value) => action[(int)UdonActionGeneric_Values.Value] = new DataToken(value);



        public static void AddListener<T>(this UdonAction<T> action,
            UdonBehaviour target,
            string eventName,
            string outputName,
            bool invokeImmediate = false)
        {
            UdonEvent<T> udonEvent = UdonEvents.Create<T>(target, eventName, outputName);
            action.AddListener(udonEvent);
            if (invokeImmediate && action.Value() != null) udonEvent._Invoke(action.Value());
        }

        public static void AddListener<T>(this UdonAction<T> action,
            UdonSharpBehaviour target,
            string eventName,
            string outputName,
            bool invokeImmediate = false)
        {
            UdonEvent<T> udonEvent = UdonEvents.Create<T>(target, eventName, outputName);
            action.AddListener(udonEvent);
            if (invokeImmediate && action.Value() != null) udonEvent._Invoke(action.Value());
        }

        public static void AddListener<T>(this UdonAction<T> action,
            UdonEvent<T> udonEvent,
            bool invokeImmediate = false)
        {
            action.Events()[udonEvent.Hash()] = udonEvent;
            if (invokeImmediate && action.Value() != null) udonEvent._Invoke(action.Value());
        }

        
        public static void RemoveListener<T>(this UdonAction<T> action,
            UdonBehaviour target,
            string eventName,
            string outputName)
        {
            UdonEvent<T> comparisonEvent = UdonEvents.Create<T>(target, eventName, outputName);
            action.RemoveListener(comparisonEvent);
        }

        public static void RemoveListener<T>(this UdonAction<T> action,
            UdonSharpBehaviour target,
            string eventName,
            string outputName)
        {
            UdonEvent<T> comparisonEvent = UdonEvents.Create<T>(target, eventName, outputName);
            action.RemoveListener(comparisonEvent);
        }
        
        public static void RemoveListener<T>(this UdonAction<T> action,
            UdonEvent<T> udonEvent)
        {
            action.Events().Remove(udonEvent.Hash());
        }


        public static void _RemoveAllListeners<T>(this UdonAction<T> action)
        {
            action.Events().Clear();
        }

        
        public static void _Invoke<T>(this UdonAction<T> action,
            T outputValue)
        {
            action.SetValue(outputValue);
            
            DataList events = action.Events().GetValues();
            for (int i = 0; i < events.Count; i++)
            {
                UdonEvent<T> udonEvent = events[i].AsUdonEvent<T>();
                udonEvent._Invoke(outputValue);
            }
        }
    }
}