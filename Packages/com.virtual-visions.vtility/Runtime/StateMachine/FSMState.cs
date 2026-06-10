using VRC.SDK3.Data;

namespace VirtualVisions.VTility
{
    public abstract class FSMState : DataDictionary
    {

        public const string KEY_ON_ENTER = "onEnter";
        public const string KEY_ON_EXIT = "onExit";
        public const string KEY_ON_TICK = "onTick";

        public static FSMState Create()
        {
            DataDictionary dict = new DataDictionary();
            dict[KEY_ON_ENTER] = UdonAction.Create();
            dict[KEY_ON_EXIT] = UdonAction.Create();
            dict[KEY_ON_TICK] = UdonAction.Create();
            
            return (FSMState)dict;
        }

    }

    public static class FSMStateExtentions
    {

        public static FSMState _FSMState(this DataToken token) => (FSMState)token.DataDictionary;

        public static UdonAction _OnEnter(this FSMState state) => state[FSMState.KEY_ON_ENTER]._UdonAction();
        public static UdonAction _OnExit(this FSMState state) => state[FSMState.KEY_ON_EXIT]._UdonAction();
        public static UdonAction _OnTick(this FSMState state) => state[FSMState.KEY_ON_TICK]._UdonAction();

    }
}