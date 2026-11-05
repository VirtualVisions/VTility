using VRC.SDK3.Data;

namespace VirtualVisions.VTility
{

    public enum CallbackValue_Values
    {
        Value,
        InitialValue,
        OnChanged,
        // ---
        Count
    }
    
    public abstract class CallbackValue : DataList
    {
        // Since Udon doesn't natively support fields of this type,
        // it is recommended to use this format for your fields:
        // 
        //    public ValueChangeCallback value => ValueChangeCallbackExtensions.BackingValueChangeCallback(ref _value);
        //    private DataList _value;
        //    

        public static CallbackValue Create(DataToken initialValue)
        {
            DataToken[] values = new DataToken[(int)CallbackValue_Values.Count];
            values[(int)CallbackValue_Values.Value] = initialValue;
            values[(int)CallbackValue_Values.InitialValue] = initialValue;

            UdonAction onChanged = UdonAction.Create();
            onChanged.SetOutput(initialValue);
            values[(int)CallbackValue_Values.OnChanged] = onChanged;
            

            CallbackValue callback = (CallbackValue)new DataList(values);
            return callback;
        }
    }

    public static class CallbackValueExtensions
    {
        
        public static CallbackValue AsCallbackValue(this DataToken token) => (CallbackValue)token.DataList;
        public static CallbackValue AsCallbackValue(this DataList list) => (CallbackValue)list;

        public static CallbackValue BackingValueChangeCallback(ref DataList backingValue, DataToken initialValue) =>
            (CallbackValue)(backingValue != null ? backingValue : backingValue = CallbackValue.Create(initialValue));

        // ---

        public static DataToken Value(this CallbackValue callback) =>
            callback[(int)CallbackValue_Values.Value];
        
        public static DataToken InitialValue(this CallbackValue callback) =>
            callback[(int)CallbackValue_Values.InitialValue];
        
        public static UdonAction OnChanged(this CallbackValue callback) =>
            callback[(int)CallbackValue_Values.OnChanged].AsUdonAction();

        public static void SetValue(this CallbackValue callback, DataToken value, bool ignoreCallback = false)
        {
            callback[(int)CallbackValue_Values.Value] = value;
            if (!ignoreCallback) callback.OnChanged()._Invoke(value);
        }

        /// <summary>
        /// Revert the value to the initial one set during construction.
        /// </summary>
        public static void RevertValue(this CallbackValue callback, bool ignoreCallback = false)
        {
            callback.SetValue(callback.InitialValue(), ignoreCallback);
        }

    }
    
}