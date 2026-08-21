using VRC.SDK3.Data;

namespace VirtualVisions.VTility
{

    public enum FSMState_Values
    {
        OnEnter,
        OnExit,
        OnTick,
        // ---
        Count
    }
    
    public abstract class FSMState : DataList
    {

        public const string KEY_ON_ENTER = "onEnter";
        public const string KEY_ON_EXIT = "onExit";
        public const string KEY_ON_TICK = "onTick";

        public static FSMState Create()
        {
            DataToken[] values = new DataToken[(int)FSMState_Values.Count];
            
            values[(int)FSMState_Values.OnEnter] = UdonAction.Create();
            values[(int)FSMState_Values.OnExit] = UdonAction.Create();
            values[(int)FSMState_Values.OnTick] = UdonAction.Create();

            return (FSMState)new DataList(values);
        }

    }

    public static class FSMStateExtentions
    {

        public static FSMState _FSMState(this DataToken token) => (FSMState)token.DataList;

        public static UdonAction _OnEnter(this FSMState state) => state[(int)FSMState_Values.OnEnter].UdonAction();
        public static UdonAction _OnExit(this FSMState state) => state[(int)FSMState_Values.OnExit].UdonAction();
        public static UdonAction _OnTick(this FSMState state) => state[(int)FSMState_Values.OnTick].UdonAction();

    }
}