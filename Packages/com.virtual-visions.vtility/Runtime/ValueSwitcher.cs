using System;
using JetBrains.Annotations;
using VRC.SDK3.Data;

namespace VirtualVisions.VTility
{
    public enum ValueSwitcher_Values
    {
        Value,
        ValueList,
        ValueChange,
        // ---
        Count
    }
    
    public abstract class ValueSwitcher : DataList
    {
        /// Since Udon doesn't natively support fields of this type,
        /// it is recommended to use this format for your fields:
        ///
        ///    public ValueSwitcher switcher => ValueSwitcherExtensions.BackingValueSwitcher(ref _switcher, values);
        ///    private DataList _switcher;
        ///
        
        public static ValueSwitcher Create(DataList valueList, DataToken initialValue = default)
        {
            DataToken[] values = new DataToken[(int)ValueSwitcher_Values.Count];
            
            values[(int)ValueSwitcher_Values.Value] = initialValue;
            values[(int)ValueSwitcher_Values.ValueList] = valueList;
            values[(int)ValueSwitcher_Values.ValueChange] = ValueChangeCallback.Create(initialValue);

            DataList objectSwitcher = new DataList(values);
            return (ValueSwitcher)objectSwitcher;
        }
    }

    public static class ValueSwitcherExtensions
    {
        
        public static ValueSwitcher AsValueSwitcher(this DataToken token) => (ValueSwitcher)token.DataList;
        public static ValueSwitcher AsValueSwitcher(this DataList list) => (ValueSwitcher)list;

        public static ValueSwitcher BackingValueSwitcher(ref DataList backingValue, DataList valueList, DataToken initialValue = default) =>
            (ValueSwitcher)(backingValue != null ? backingValue : backingValue = ValueSwitcher.Create(valueList, initialValue));
        
        // ---
        
        public static DataList ValueList(this ValueSwitcher switcher) => switcher[(int)ValueSwitcher_Values.ValueList].DataList;
        public static DataToken Value(this ValueSwitcher switcher) => switcher.ValueChange().Value();
        public static ValueChangeCallback ValueChange(this ValueSwitcher switcher) => switcher[(int)ValueSwitcher_Values.ValueChange].AsValueChangeCallback();


        /// <summary>
        /// Passes the new value that is switched to.
        /// </summary>
        public static UdonAction OnValueSwitched(this ValueSwitcher switcher) =>
            switcher.ValueChange().OnChanged();
        
        [PublicAPI]
        public static void AddValue(this ValueSwitcher switcher, DataToken value)
        {
            switcher.ValueList().Add(value);
        }

        [PublicAPI]
        public static void SwitchTo(this ValueSwitcher switcher, DataToken value)
        {
            DataList list = switcher.ValueList();
            if (!list.Contains(value)) switcher.AddValue(value);
            switcher.SwitchToIndex(list.IndexOf(value));
        }

        [PublicAPI]
        public static void SwitchToIndex(this ValueSwitcher switcher, Enum index) => switcher.SwitchToIndex(Convert.ToInt32(index));

        [PublicAPI]
        public static void SwitchToIndex(this ValueSwitcher switcher, int index)
        {
            switcher.ValueList().TryIndex(index, out DataToken value);
            switcher.ValueChange().SetValue(value);
        }
    }
}