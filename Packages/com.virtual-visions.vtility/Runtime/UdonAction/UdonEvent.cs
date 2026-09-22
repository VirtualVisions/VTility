using System.Text;
using UdonSharp;
using VRC.SDK3.Data;
using VRC.Udon;

namespace VirtualVisions.VTility
{

    public enum UdonEvent_Values
    {
        Target,
        HasEvent,
        Event,
        HasOutput,
        OutputName,
        Hash,
        
        Count
    }
    
    /// <summary>
    /// A specific event callable via Udon. To be used in tandem with UdonAction.
    /// </summary>
    public abstract class UdonEvent : DataList
    {
        
        public static UdonEvent Create(UdonSharpBehaviour target, string eventName)
        {
            UdonBehaviour castTarget = (UdonBehaviour)(UnityEngine.Object)target;
            return Create(castTarget, eventName);
        }

        public static UdonEvent Create(UdonBehaviour target, string eventName)
        {
            DataToken[] values = new DataToken[(int)UdonEvent_Values.Count];
            values.Set(UdonEvent_Values.Target, target);
            values.Set(UdonEvent_Values.HasEvent, !string.IsNullOrEmpty(eventName));
            values.Set(UdonEvent_Values.Event, eventName);
            values.Set(UdonEvent_Values.HasOutput, false);
            values.Set(UdonEvent_Values.OutputName, string.Empty);

            UdonEvent udonList = (UdonEvent)new DataList(values);
            udonList.Set(UdonEvent_Values.Hash, udonList.BuildHash());

            return udonList;
        }

        public static UdonEvent Create(UdonSharpBehaviour target, string eventName, string outputName)
        {
            UdonBehaviour castTarget = (UdonBehaviour)(UnityEngine.Object)target;
            return Create(castTarget, eventName, outputName);
        }
        
        public static UdonEvent Create(UdonBehaviour target, string eventName, string outputName)
        {
            DataToken[] values = new DataToken[(int)UdonEvent_Values.Count];
            values.Set(UdonEvent_Values.Target, target);
            values.Set(UdonEvent_Values.HasEvent, !string.IsNullOrEmpty(eventName));
            values.Set(UdonEvent_Values.Event, eventName);
            values.Set(UdonEvent_Values.HasOutput, true);
            values.Set(UdonEvent_Values.OutputName, outputName);

            UdonEvent udonList = (UdonEvent)new DataList(values);
            udonList.Set(UdonEvent_Values.Hash, udonList.BuildHash());

            return udonList;
        }
    }

    public static class UdonEventExtensions
    {
        public static UdonEvent UdonEvent(this DataToken token) => (UdonEvent)token.DataList;

        public static UdonBehaviour Target(this UdonEvent udonEvent) =>
            (UdonBehaviour)udonEvent.Get(UdonEvent_Values.Target).Reference;

        public static bool HasEvent(this UdonEvent udonEvent) =>
            udonEvent.Get(UdonEvent_Values.HasEvent).Boolean;

        public static string EventName(this UdonEvent udonEvent) =>
            udonEvent.Get(UdonEvent_Values.Event).String;

        public static bool HasOutput(this UdonEvent udonEvent) =>
            udonEvent.Get(UdonEvent_Values.HasOutput).Boolean;

        public static string OutputName(this UdonEvent udonEvent) =>
            udonEvent.Get(UdonEvent_Values.OutputName).String;

        public static int Hash(this UdonEvent udonEvent) =>
            udonEvent.Get(UdonEvent_Values.Hash).Int;


        public static void _Invoke(this UdonEvent udonEvent)
        {
            UdonBehaviour target = (UdonBehaviour)udonEvent.Get(UdonEvent_Values.Target).Reference;

            if (udonEvent.HasEvent())
            {
                target.SendCustomEvent(udonEvent.EventName());
            }
        }

        public static void _Invoke<T>(this UdonEvent udonEvent, T outputValue)
        {
            UdonBehaviour target = (UdonBehaviour)udonEvent.Get(UdonEvent_Values.Target).Reference;

            if (udonEvent.HasOutput())
            {
                target.SetProgramVariable(udonEvent.OutputName(), outputValue);
            }

            if (udonEvent.HasEvent())
            {
                target.SendCustomEvent(udonEvent.EventName());
            }
        }

        public static int BuildHash(this UdonEvent udonEvent)
        {
            StringBuilder output = new StringBuilder();
            output.Append(udonEvent.Target().GetInstanceID());
            output.Append(udonEvent.HasEvent());
            if (udonEvent.HasEvent()) output.Append(udonEvent.EventName());
            output.Append(udonEvent.HasOutput());
            if (udonEvent.HasOutput()) output.Append(udonEvent.OutputName());

            int hash = output.ToString().GetHashCode();
            return hash;
        }
    }
}