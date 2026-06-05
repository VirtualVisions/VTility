using System.Text;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.Udon;

namespace VirtualVisions.VTility
{
    /// <summary>
    /// A specific event callable via Udon. To be used in tandem with UdonAction.
    /// </summary>
    public abstract class UdonEvent : DataDictionary
    {
        public const string KEY_TARGET = "target";
        public const string KEY_EVENT = "event";
        public const string KEY_HAS_OUTPUT = "hasOutput";
        public const string KEY_OUTPUT_NAME = "outputName";
        
        public static UdonEvent _Create(UdonSharpBehaviour target, string eventName)
        {
            DataDictionary dictionary = new DataDictionary();
            dictionary[KEY_TARGET] = target;
            dictionary[KEY_EVENT] = eventName;
            dictionary[KEY_HAS_OUTPUT] = false;
            dictionary[KEY_OUTPUT_NAME] = string.Empty;
            return (UdonEvent)dictionary;
        }
        
        public static UdonEvent _Create(UdonSharpBehaviour target, string eventName, string outputName)
        {
            DataDictionary dictionary = new DataDictionary();
            dictionary[KEY_TARGET] = target;
            dictionary[KEY_EVENT] = eventName;
            dictionary[KEY_HAS_OUTPUT] = true;
            dictionary[KEY_OUTPUT_NAME] =outputName;
            return (UdonEvent)dictionary;
        }
    }

    public static class UdonEventExtensions
    {
        public static UdonEvent _UdonEvent(this DataToken token) => (UdonEvent)token.DataDictionary;

        public static UdonSharpBehaviour _Target(this UdonEvent udonEvent) =>
            (UdonSharpBehaviour)udonEvent[UdonEvent.KEY_TARGET].Reference;
        
        public static string _EventName(this UdonEvent udonEvent) =>
            udonEvent[UdonEvent.KEY_EVENT].String;
        
        public static bool _HasOutput(this UdonEvent udonEvent) =>
            udonEvent[UdonEvent.KEY_HAS_OUTPUT].Boolean;
        
        public static string _OutputName(this UdonEvent udonEvent) =>
            udonEvent[UdonEvent.KEY_OUTPUT_NAME].String;
        

        public static void _Invoke(this UdonEvent udonEvent)
        {
            UdonBehaviour target = (UdonBehaviour)udonEvent[UdonEvent.KEY_TARGET].Reference;
            string eventName = udonEvent[UdonEvent.KEY_EVENT].String;
            target.SendCustomEvent(eventName);
        }

        public static void _Invoke<T>(this UdonEvent udonEvent, T outputValue)
        {
            UdonBehaviour target = (UdonBehaviour)udonEvent[UdonEvent.KEY_TARGET].Reference;
            string eventName = udonEvent[UdonEvent.KEY_EVENT].String;
            
            if (udonEvent[UdonEvent.KEY_HAS_OUTPUT].Boolean)
            {
                string outputName = udonEvent[UdonEvent.KEY_OUTPUT_NAME].String;
                target.SetProgramVariable(outputName, outputValue);
            }
            
            target.SendCustomEvent(eventName);
        }

        public static int _GetHash(this UdonEvent udonEvent)
        {
            StringBuilder output = new StringBuilder();
            output.Append(udonEvent._Target());
            output.Append(udonEvent._EventName());
            output.Append(udonEvent._HasOutput());
            if (udonEvent._HasOutput())
            {
                output.Append(udonEvent._OutputName());
            }

            int hash = Animator.StringToHash(output.ToString());
            
            return hash;
        }
    }
}