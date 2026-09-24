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
    }

    public static class FSMStates
    {
        public static FSMState Create()
        {
            DataToken[] values = new DataToken[(int)FSMState_Values.Count];
            
            values[(int)FSMState_Values.OnEnter] = UdonActions.Create();
            values[(int)FSMState_Values.OnExit] = UdonActions.Create();
            values[(int)FSMState_Values.OnTick] = UdonActions.Create<float>(0);

            return (FSMState)new DataList(values);
        }

        public static FSMState _FSMState(this DataToken token) => (FSMState)token.DataList;

        public static UdonAction _OnEnter(this FSMState state) => state[(int)FSMState_Values.OnEnter].AsUdonAction();
        public static UdonAction _OnExit(this FSMState state) => state[(int)FSMState_Values.OnExit].AsUdonAction();
        public static UdonAction<float> _OnTick(this FSMState state) => state[(int)FSMState_Values.OnTick].AsUdonAction<float>();

    }
}