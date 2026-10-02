using VRC.SDK3.Data;

namespace VirtualVisions.VTility
{

    public enum ValueChangeCallback_Values
    {
        Value,
        InitialValue,
        OnChanged,
        
        // ---
        Count
    }
    
    public abstract class ValueChangeCallback : DataList
    {
        // Since Udon doesn't natively support fields of this type,
        // it is recommended to use this format for your fields:
        // 
        //    public ValueChangeCallback value => ValueChangeCallbackExtensions.BackingValueChangeCallback(ref _value);
        //    private DataList _value;
        //    

        public static ValueChangeCallback Create(DataToken initialValue)
        {
            DataToken[] values = new DataToken[(int)ValueChangeCallback_Values.Count];
            values[(int)ValueChangeCallback_Values.Value] = initialValue;
            values[(int)ValueChangeCallback_Values.InitialValue] = initialValue;
            values[(int)ValueChangeCallback_Values.OnChanged] = UdonAction.Create();

            ValueChangeCallback callback = (ValueChangeCallback)new DataList(values);
            return callback;
        }
    }

    public static class ValueChangeCallbackExtensions
    {
        
        public static ValueChangeCallback AsValueChangeCallback(this DataToken token) => (ValueChangeCallback)token.DataList;
        public static ValueChangeCallback AsValueChangeCallback(this DataList list) => (ValueChangeCallback)list;

        public static ValueChangeCallback BackingValueChangeCallback(ref DataList backingValue, DataToken initialValue) =>
            (ValueChangeCallback)(backingValue != null ? backingValue : backingValue = VTility.ValueChangeCallback.Create(initialValue));

        // ---

        public static DataToken Value(this ValueChangeCallback callback) =>
            callback[(int)ValueChangeCallback_Values.Value];
        
        public static DataToken InitialValue(this ValueChangeCallback callback) =>
            callback[(int)ValueChangeCallback_Values.InitialValue];
        
        public static UdonAction OnChanged(this ValueChangeCallback callback) =>
            callback[(int)ValueChangeCallback_Values.OnChanged].AsUdonAction();

        public static void SetValue(this ValueChangeCallback callback, DataToken value, bool ignoreCallback = false)
        {
            callback[(int)ValueChangeCallback_Values.Value] = value;
            if (!ignoreCallback) callback.OnChanged()._Invoke(value);
        }

        /// <summary>
        /// Revert the value to the initial one set during construction.
        /// </summary>
        public static void RevertValue(this ValueChangeCallback callback, bool ignoreCallback = false)
        {
            callback.SetValue(callback.InitialValue(), ignoreCallback);
        }

    }
    
}