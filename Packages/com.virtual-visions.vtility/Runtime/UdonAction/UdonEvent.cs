using System.Text;
using UdonSharp;
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

        public static UdonEvent Create(UdonSharpBehaviour target, string eventName)
        {
            DataDictionary dictionary = new DataDictionary();
            dictionary[KEY_TARGET] = target;
            dictionary[KEY_HAS_EVENT] = !string.IsNullOrEmpty(eventName);
            dictionary[KEY_EVENT] = eventName;
            dictionary[KEY_HAS_OUTPUT] = false;
            dictionary[KEY_OUTPUT_NAME] = string.Empty;

            UdonEvent output = (UdonEvent)dictionary;
            output[KEY_HASH] = output.BuildHash();

            return output;
        }

        public static UdonEvent Create(UdonSharpBehaviour target, string eventName, string outputName)
        {
            DataDictionary dictionary = new DataDictionary();
            dictionary[KEY_TARGET] = target;
            dictionary[KEY_HAS_EVENT] = !string.IsNullOrEmpty(eventName);
            dictionary[KEY_EVENT] = eventName;
            dictionary[KEY_HAS_OUTPUT] = true;
            dictionary[KEY_OUTPUT_NAME] = outputName;

            UdonEvent output = (UdonEvent)dictionary;
            output[KEY_HASH] = output.BuildHash();

            return output;
        }
    }

    public static class UdonEventExtensions
    {
        public static UdonEvent UdonEvent(this DataToken token) => (UdonEvent)token.DataDictionary;

        public static UdonSharpBehaviour Target(this UdonEvent udonEvent) =>
            (UdonSharpBehaviour)udonEvent[VTility.UdonEvent.KEY_TARGET].Reference;

        public static bool HasEvent(this UdonEvent udonEvent) =>
            udonEvent[VTility.UdonEvent.KEY_HAS_EVENT].Boolean;

        public static string EventName(this UdonEvent udonEvent) =>
            udonEvent[VTility.UdonEvent.KEY_EVENT].String;

        public static bool HasOutput(this UdonEvent udonEvent) =>
            udonEvent[VTility.UdonEvent.KEY_HAS_OUTPUT].Boolean;

        public static string OutputName(this UdonEvent udonEvent) =>
            udonEvent[VTility.UdonEvent.KEY_OUTPUT_NAME].String;

        public static int Hash(this UdonEvent udonEvent) =>
            udonEvent[VTility.UdonEvent.KEY_HASH].Int;


        public static void _Invoke(this UdonEvent udonEvent)
        {
            UdonBehaviour target = (UdonBehaviour)udonEvent[VTility.UdonEvent.KEY_TARGET].Reference;

            if (udonEvent.HasEvent())
            {
                target.SendCustomEvent(udonEvent.EventName());
            }
        }

        public static void _Invoke<T>(this UdonEvent udonEvent, T outputValue)
        {
            UdonBehaviour target = (UdonBehaviour)udonEvent[VTility.UdonEvent.KEY_TARGET].Reference;

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

            int hash = output.GetHashCode();
            return hash;
        }
    }
}