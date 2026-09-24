using System;
using UdonSharp;
using VRC.SDK3.Data;
using VRC.Udon;

namespace VirtualVisions.VTility
{

    public enum UdonEvent_Values
    {
        Target,
        EventName,
        HasOutput,
        OutputName,
        OutputValue,
        Identifier,
        // ---
        Count
    }

    /// <summary>
    /// A specific event callable via Udon. To be used in tandem with UdonAction.
    /// </summary>
    public abstract class UdonEvent : DataList
    {
        
        // Since Udon doesn't natively support fields of this type,
        // it is recommended to use this format for your fields:
        // 
        //    public UdonEvent onShow => UdonEventExtensions.BackingUdonEvent(ref _onShow, this, nameof(EventCallback), nameof(ValueCallback));
        //    private DataList _onShow;
        //    



        public static UdonEvent Create(UdonSharpBehaviour target, string eventName)
        {
            UdonBehaviour castTarget = (UdonBehaviour)(UnityEngine.Object)target;
            return Create(castTarget, eventName, string.Empty);
        }

        public static UdonEvent Create(UdonSharpBehaviour target, string eventName, string outputName)
        {
            UdonBehaviour castTarget = (UdonBehaviour)(UnityEngine.Object)target;
            return Create(castTarget, eventName, outputName);
        }

        public static UdonEvent Create(UdonBehaviour target, string eventName)
        {
            return Create(target, eventName, string.Empty);
        }

        public static UdonEvent Create(UdonBehaviour target, string eventName, string outputName)
        {
            DataToken[] values = new DataToken[(int)UdonEvent_Values.Count];
            values[(int)UdonEvent_Values.Target] = target;
            values[(int)UdonEvent_Values.EventName] = eventName;
            values[(int)UdonEvent_Values.HasOutput] = !string.IsNullOrEmpty(outputName);
            values[(int)UdonEvent_Values.OutputName] = outputName;
            values[(int)UdonEvent_Values.OutputValue] = new DataToken();

            UdonEvent udonEvent = (UdonEvent)new DataList(values);
            udonEvent[(int)UdonEvent_Values.Identifier] = udonEvent.ContentsToString().GetHashCode();
            return udonEvent;
        }

    }

    public static class UdonEventExtensions
    {

        public static UdonEvent AsUdonEvent(this DataToken token) => (UdonEvent)token.DataList;
        public static UdonEvent AsUdonEvent(this DataList datalist) => (UdonEvent)datalist;

        public static UdonEvent BackingUdonEvent(ref DataList backingValue, UdonSharpBehaviour target, string eventName, string outputName = null) =>
            (UdonEvent)(backingValue != null ? backingValue : backingValue = UdonEvent.Create(target, eventName, outputName));

        // ---

        public static UdonBehaviour Target(this UdonEvent udonEvent) =>
            (UdonBehaviour)udonEvent[(int)UdonEvent_Values.Target].Reference;

        public static string EventName(this UdonEvent udonEvent) =>
            udonEvent[(int)UdonEvent_Values.EventName].String;

        public static bool HasOutput(this UdonEvent udonEvent) =>
            udonEvent[(int)UdonEvent_Values.HasOutput].Boolean;

        public static string OutputName(this UdonEvent udonEvent) =>
            udonEvent[(int)UdonEvent_Values.OutputName].String;

        public static DataToken GetOutput(this UdonEvent udonEvent) =>
            udonEvent[(int)UdonEvent_Values.OutputValue];

        public static void SetOutput(this UdonEvent udonEvent, DataToken token) =>
            udonEvent[(int)UdonEvent_Values.OutputValue] = token;

        public static int Hash(this UdonEvent udonEvent) =>
            udonEvent[(int)UdonEvent_Values.Identifier].Int;

        // ---

        public static void _Invoke(this UdonEvent udonEvent, Enum value) => udonEvent._Invoke(Convert.ToInt32(value));
        
        public static void _Invoke(this UdonEvent udonEvent, DataToken value = default)
        {
            UdonBehaviour target = udonEvent.Target();
            if (udonEvent.HasOutput())
            {
                udonEvent.SetOutput(value);
                target.SetProgramVariable(udonEvent.OutputName(), value);
            }

            target.SendCustomEvent(udonEvent.EventName());
        }
    }
}