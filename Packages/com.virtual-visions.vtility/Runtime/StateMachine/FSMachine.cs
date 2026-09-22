using System;
using VRC.SDK3.Data;

namespace VirtualVisions.VTility
{

    public enum FSMachine_Values
    {
        States,
        CurrentState,
        CurrentStateValue,
        // ---
        Count
    }
    
    public abstract class FSMachine : DataList
    {

        public static FSMachine Create()
        {
            DataToken[] values = new DataToken[(int)FSMachine_Values.Count];
            
            values.Set(FSMachine_Values.States, new DataDictionary());

            FSMState emptyState = FSMState.Create();
            values.Get(FSMachine_Values.States).DataDictionary[-1] = emptyState;
            values.Set(FSMachine_Values.CurrentState, emptyState);
            values.Set(FSMachine_Values.CurrentStateValue, -1);

            return (FSMachine)new DataList(values);
        }
    }

    public static class StateMachineExtensions
    {
        public static FSMachine _StateMachine(this DataToken token) => (FSMachine)token.DataList;


        public static int _CurrentStateValue(this FSMachine machine) =>
            machine[(int)FSMachine_Values.CurrentStateValue].Int;

        public static FSMState _CurrentState(this FSMachine machine) =>
            machine[(int)FSMachine_Values.CurrentState]._FSMState();


        public static FSMState _RegisterState(this FSMachine machine, Enum state) =>
            machine._RegisterState(Convert.ToInt32(state));

        public static FSMState _RegisterState(this FSMachine machine, int state)
        {
            if (machine._States().ContainsKey(state)) return machine._States()[state]._FSMState();

            FSMState newState = FSMState.Create();
            machine._States()[state] = newState;

            return newState;
        }


        public static DataDictionary _States(this FSMachine machine) => machine.Get(FSMachine_Values.States).DataDictionary;


        public static FSMachine _SetState(this FSMachine machine, Enum state) =>
            machine._SetState(Convert.ToInt32(state));

        public static FSMachine _SetState(this FSMachine machine, int state)
        {
            FSMState currentState = machine._CurrentState();
            if (currentState != null)
            {
                currentState._OnExit()._Invoke();
            }

            machine[(int)FSMachine_Values.CurrentStateValue] = state;

            if (machine._States().TryGetValue(state, TokenType.DataDictionary, out DataToken value))
            {
                FSMState foundState = value._FSMState();
                machine[(int)FSMachine_Values.CurrentState] = foundState;
            }
            
            machine._CurrentState()._OnEnter()._Invoke();

            return machine;
        }

        public static FSMachine _DoTick(this FSMachine machine, float delta)
        {
            DataToken currentState = machine._CurrentStateValue();
            if (!currentState.IsEmpty)
            {
                machine._CurrentState()._OnTick()._Invoke(delta);
            }

            return machine;
        }

        public static FSMachine _Shutdown(this FSMachine machine)
        {
            machine._SetState(-1);
            return machine;
        }
    }
}