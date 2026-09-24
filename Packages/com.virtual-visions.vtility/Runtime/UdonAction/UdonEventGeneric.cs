using System.Text;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.Udon;

namespace VirtualVisions.VTility
{

    public enum UdonEventGeneric_Values
    {
        Target,
        Event,
        OutputName,
        Hash,
        // ---
        Count
    }
    
    /// <summary>
    /// A specific event callable via Udon. To be used in tandem with UdonAction.
    /// </summary>
    public abstract class UdonEvent<T> : DataList
    {
    }

    /// <summary>
    /// The utility class for creation and data management of the UdonEvent class.
    /// </summary>
    public static partial class UdonEvents
    {
        public static UdonEvent<T> Create<T>(UdonBehaviour target, string eventName, string outputName)
        {
            DataToken[] values = new DataToken[(int)UdonEventGeneric_Values.Count];
            values[(int)UdonEventGeneric_Values.Target] = target;
            values[(int)UdonEventGeneric_Values.Event] = eventName;
            values[(int)UdonEventGeneric_Values.OutputName] = outputName;

            UdonEvent<T> udonList = (UdonEvent<T>)new DataList(values);
            udonList[(int)UdonEventGeneric_Values.Hash] = udonList.BuildHash();

            return udonList;
        }
        
        public static UdonEvent<T> Create<T>(UdonSharpBehaviour target, string eventName, string outputName)
        {
            DataToken[] values = new DataToken[(int)UdonEventGeneric_Values.Count];
            values[(int)UdonEventGeneric_Values.Target] = (UdonBehaviour)(Component)target;
            values[(int)UdonEventGeneric_Values.Event] = eventName;
            values[(int)UdonEventGeneric_Values.OutputName] = outputName;

            UdonEvent<T> udonList = (UdonEvent<T>)new DataList(values);
            udonList[(int)UdonEventGeneric_Values.Hash] = udonList.BuildHash();

            return udonList;
        }
        
        
        
        public static UdonEvent<T> AsUdonEvent<T>(this DataToken token) => (UdonEvent<T>)token.DataList;

        public static UdonBehaviour Target<T>(this UdonEvent<T> udonEvent) =>
            (UdonBehaviour)udonEvent[(int)UdonEventGeneric_Values.Target].Reference;

        public static string EventName<T>(this UdonEvent<T> udonEvent) =>
            udonEvent[(int)UdonEventGeneric_Values.Event].String;

        public static string OutputName<T>(this UdonEvent<T> udonEvent) =>
            udonEvent[(int)UdonEventGeneric_Values.OutputName].String;

        public static int Hash<T>(this UdonEvent<T> udonEvent) =>
            udonEvent[(int)UdonEventGeneric_Values.Hash].Int;


        public static void _Invoke<T>(this UdonEvent<T> udonEvent, T outputValue)
        {
            UdonBehaviour target = (UdonBehaviour)udonEvent[(int)UdonEventGeneric_Values.Target].Reference;
            target.SetProgramVariable(udonEvent.OutputName(), outputValue);
            target.SendCustomEvent(udonEvent.EventName());
        }

        public static int BuildHash<T>(this UdonEvent<T> udonEvent)
        {
            StringBuilder output = new StringBuilder();
            output.Append(udonEvent.Target().GetInstanceID());
            output.Append(udonEvent.EventName());
            output.Append(udonEvent.OutputName());

            int hash = output.ToString().GetHashCode();
            return hash;
        }
    }
}