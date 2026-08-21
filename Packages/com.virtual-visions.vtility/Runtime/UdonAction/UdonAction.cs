using UdonSharp;
using VRC.SDK3.Data;
using VRC.Udon;

namespace VirtualVisions.VTility
{

    public enum UdonAction_Values
    {
        Events,
        
        Count
    }
    
    /// <summary>
    /// A subscribable action that can be repeatedly called with passable variables.
    /// </summary>
    public abstract class UdonAction : DataList
    {
        /// Since Udon doesn't natively support fields of this type,
        /// it is recommended to use this format for your fields:
        /// 
        ///    public UdonAction BindItem => (UdonAction)(_bindItem != null ? _bindItem : _bindItem = UdonAction.Create());
        ///    private DataList _bindItem;
        ///    
        

        public static UdonAction Create()
        {
            DataToken[] values = new DataToken[(int)UdonAction_Values.Count];
            
            values[(int)UdonAction_Values.Events] = new DataDictionary();
            
            return (UdonAction)new DataList(values);
        }
    }

    public static class UdonActionExtensions
    {
        public static UdonAction UdonAction(this DataToken token) => (UdonAction)token.DataList;
        public static DataDictionary Events(this UdonAction action) => action[(int)UdonAction_Values.Events].DataDictionary;



        public static void AddListener(this UdonAction action,
            UdonEvent udonEvent)
        {
            action.Events()[udonEvent.Hash()] = udonEvent;
        }

        public static void AddListener(this UdonAction action,
            UdonBehaviour target,
            string eventName)
        {
            action.AddListener(UdonEvent.Create(target, eventName));
        }

        public static void AddListener(this UdonAction action,
            UdonSharpBehaviour target,
            string eventName)
        {
            action.AddListener(UdonEvent.Create(target, eventName));
        }

        public static void AddListener(this UdonAction action,
            UdonBehaviour target,
            string eventName,
            string outputName)
        {
            action.AddListener(UdonEvent.Create(target, eventName, outputName));
        }

        public static void AddListener(this UdonAction action,
            UdonSharpBehaviour target,
            string eventName,
            string outputName)
        {
            action.AddListener(UdonEvent.Create(target, eventName, outputName));
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
            UdonEvent comparisonEvent = UdonEvent.Create(target, eventName);
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
            UdonSharpBehaviour target,
            string eventName,
            string outputName)
        {
            UdonEvent comparisonEvent = UdonEvent.Create(target, eventName, outputName);
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
                UdonEvent udonEvent = events[i].UdonEvent();
                udonEvent._Invoke();
            }
        }

        public static void _Invoke<T>(this UdonAction action,
            T outputValue)
        {
            DataList events = action.Events().GetValues();
            for (int i = 0; i < events.Count; i++)
            {
                UdonEvent udonEvent = events[i].UdonEvent();
                udonEvent._Invoke(outputValue);
            }
        }
    }
}