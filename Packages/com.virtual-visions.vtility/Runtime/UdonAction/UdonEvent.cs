using System.Text;
using UdonSharp;
using VRC.SDK3.Data;
using VRC.Udon;

namespace VirtualVisions.VTility
{

    public enum UdonEvent_Values
    {
        Target,
        Event,
        Hash,
        // ---
        Count
    }
    
    /// <summary>
    /// A specific event callable via Udon. To be used in tandem with UdonAction.
    /// </summary>
    public abstract class UdonEvent : DataList
    {
    }

    public static partial class UdonEvents
    {
        public static UdonEvent Create(UdonSharpBehaviour target, string eventName)
        {
            UdonBehaviour castTarget = (UdonBehaviour)(UnityEngine.Object)target;
            return Create(castTarget, eventName);
        }

        public static UdonEvent Create(UdonBehaviour target, string eventName)
        {
            DataToken[] values = new DataToken[(int)UdonEvent_Values.Count];
            values[(int)UdonEvent_Values.Target] = target;
            values[(int)UdonEvent_Values.Event] = eventName;

            UdonEvent udonList = (UdonEvent)new DataList(values);
            udonList[(int)UdonEvent_Values.Hash] = udonList.BuildHash();

            return udonList;
        }
        
        
        
        public static UdonEvent AsUdonEvent(this DataToken token) => (UdonEvent)token.DataList;

        public static UdonBehaviour Target(this UdonEvent udonEvent) =>
            (UdonBehaviour)udonEvent[(int)UdonEvent_Values.Target].Reference;

        public static string EventName(this UdonEvent udonEvent) =>
            udonEvent[(int)UdonEvent_Values.Event].String;

        public static int Hash(this UdonEvent udonEvent) =>
            udonEvent[(int)UdonEvent_Values.Hash].Int;


        public static void _Invoke(this UdonEvent udonEvent)
        {
            UdonBehaviour target = (UdonBehaviour)udonEvent[(int)UdonEvent_Values.Target].Reference;
            target.SendCustomEvent(udonEvent.EventName());
        }

        public static int BuildHash(this UdonEvent udonEvent)
        {
            StringBuilder output = new StringBuilder();
            output.Append(udonEvent.Target().GetInstanceID());
            output.Append(udonEvent.EventName());

            int hash = output.ToString().GetHashCode();
            return hash;
        }
    }
}