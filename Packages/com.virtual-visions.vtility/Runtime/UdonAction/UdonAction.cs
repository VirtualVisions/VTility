using UdonSharp;
using VRC.SDK3.Data;

namespace VirtualVisions.VTility
{
    /// <summary>
    /// A subscribable action that can be repeatedly called with passable variables.
    /// </summary>
    public abstract class UdonAction : DataDictionary
    {
        /// Since Udon doesn't natively support fields of this type,
        /// it is recommended to use this format for your fields:
        /// 
        ///    public UdonAction BindItem
        ///    {
        ///        get => (UdonAction)(_bindItem != null ? _bindItem : _bindItem = UdonAction.Create());
        ///    }
        ///    private DataDictionary _bindItem;
        ///    
        
        
        public const string KEY_EVENTS = "events";
        
        
        public static UdonAction Create()
        {
            DataDictionary dictionary = new DataDictionary();
            dictionary[KEY_EVENTS] = new DataDictionary();
            return (UdonAction)dictionary;
        }
    }

    public static class UdonActionExtensions
    {
        public static UdonAction _UdonAction(this DataToken token) => (UdonAction)token.DataDictionary;
        public static DataDictionary _Events(this UdonAction action) => action[UdonAction.KEY_EVENTS].DataDictionary;
        
        
        
        public static void _AddListener(this UdonAction action, UdonEvent udonEvent)
        {
            action._Events()[udonEvent._GetHash()] = udonEvent;
        }
        public static void _AddListener(this UdonAction action, UdonSharpBehaviour target, string eventName)
        {
            action._AddListener(UdonEvent._Create(target, eventName));
        }
        
        public static void _AddListener(this UdonAction action, UdonSharpBehaviour target, string eventName, string outputName)
        {
            action._AddListener(UdonEvent._Create(target, eventName, outputName));
        }



        public static void _RemoveListener(this UdonAction action, UdonEvent udonEvent)
        {
            action._Events().Remove(udonEvent._GetHash());
        }
        
        public static void _RemoveListener(this UdonAction action, UdonSharpBehaviour target, string eventName)
        {
            UdonEvent comparisonEvent = UdonEvent._Create(target, eventName);
            action._RemoveListener(comparisonEvent);
        }

        public static void _RemoveListener(this UdonAction action, UdonSharpBehaviour target, string eventName, string outputName)
        {
            UdonEvent comparisonEvent = UdonEvent._Create(target, eventName, outputName);
            action._RemoveListener(comparisonEvent);
        }
        
        
        
        public static void _Invoke(this UdonAction action)
        {
            DataList events = action._Events().GetValues();
            for (int i = 0; i < events.Count; i++)
            {
                UdonEvent udonEvent = events[i]._UdonEvent();
                udonEvent._Invoke();
            }
        }

        public static void _Invoke<T>(this UdonAction action, T outputValue)
        {
            DataList events = action._Events().GetValues();
            for (int i = 0; i < events.Count; i++)
            {
                UdonEvent udonEvent = events[i]._UdonEvent();
                udonEvent._Invoke(outputValue);
            }
        }
    }
}