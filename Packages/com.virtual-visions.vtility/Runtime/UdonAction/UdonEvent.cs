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
        public const string KEY_HAS_EVENT = "hasEvent";
        public const string KEY_EVENT = "event";
        public const string KEY_HAS_OUTPUT = "hasOutput";
        public const string KEY_OUTPUT_NAME = "outputName";
        public const string KEY_HASH = "hash";

        public static UdonEvent _Create(UdonSharpBehaviour target, string eventName)
        {
            DataDictionary dictionary = new DataDictionary();
            dictionary[KEY_TARGET] = target;
            dictionary[KEY_HAS_EVENT] = !string.IsNullOrEmpty(eventName);
            dictionary[KEY_EVENT] = eventName;
            dictionary[KEY_HAS_OUTPUT] = false;
            dictionary[KEY_OUTPUT_NAME] = string.Empty;

            UdonEvent output = (UdonEvent)dictionary;
            output[KEY_HASH] = output._BuildHash();

            return output;
        }

        public static UdonEvent _Create(UdonSharpBehaviour target, string eventName, string outputName)
        {
            DataDictionary dictionary = new DataDictionary();
            dictionary[KEY_TARGET] = target;
            dictionary[KEY_HAS_EVENT] = !string.IsNullOrEmpty(eventName);
            dictionary[KEY_EVENT] = eventName;
            dictionary[KEY_HAS_OUTPUT] = true;
            dictionary[KEY_OUTPUT_NAME] = outputName;

            UdonEvent output = (UdonEvent)dictionary;
            output[KEY_HASH] = output._BuildHash();

            return output;
        }
    }

    public static class UdonEventExtensions
    {
        public static UdonEvent _UdonEvent(this DataToken token) => (UdonEvent)token.DataDictionary;

        public static UdonSharpBehaviour _Target(this UdonEvent udonEvent) =>
            (UdonSharpBehaviour)udonEvent[UdonEvent.KEY_TARGET].Reference;

        public static bool _HasEvent(this UdonEvent udonEvent) =>
            udonEvent[UdonEvent.KEY_HAS_EVENT].Boolean;

        public static string _EventName(this UdonEvent udonEvent) =>
            udonEvent[UdonEvent.KEY_EVENT].String;

        public static bool _HasOutput(this UdonEvent udonEvent) =>
            udonEvent[UdonEvent.KEY_HAS_OUTPUT].Boolean;

        public static string _OutputName(this UdonEvent udonEvent) =>
            udonEvent[UdonEvent.KEY_OUTPUT_NAME].String;

        public static int _Hash(this UdonEvent udonEvent) =>
            udonEvent[UdonEvent.KEY_HASH].Int;


        public static void _Invoke(this UdonEvent udonEvent)
        {
            UdonBehaviour target = (UdonBehaviour)udonEvent[UdonEvent.KEY_TARGET].Reference;

            if (udonEvent._HasEvent())
            {
                target.SendCustomEvent(udonEvent._EventName());
            }
        }

        public static void _Invoke<T>(this UdonEvent udonEvent, T outputValue)
        {
            UdonBehaviour target = (UdonBehaviour)udonEvent[UdonEvent.KEY_TARGET].Reference;

            if (udonEvent._HasOutput())
            {
                target.SetProgramVariable(udonEvent._OutputName(), outputValue);
            }

            if (udonEvent._HasEvent())
            {
                target.SendCustomEvent(udonEvent._EventName());
            }
        }

        public static int _BuildHash(this UdonEvent udonEvent)
        {
            StringBuilder output = new StringBuilder();
            output.Append(udonEvent._Target());
            output.Append(udonEvent._HasEvent());
            if (udonEvent._HasEvent()) output.Append(udonEvent._EventName());
            output.Append(udonEvent._HasOutput());
            if (udonEvent._HasOutput()) output.Append(udonEvent._OutputName());

            int hash = output.GetHashCode();
            return hash;
        }
    }
}