using System;
using UnityEngine;
using VRC.SDK3.Data;

namespace VirtualVisions.VTility
{
    public abstract class FSMachine : DataDictionary
    {

        public const string KEY_STATES = "states";
        public const string KEY_CURRENT_STATE = "currentState";
        public const string KEY_CURRENT_STATE_VALUE = "currentStateValue";

        public static FSMachine Create()
        {
            DataDictionary dict = new DataDictionary();
            dict[KEY_STATES] = new DataDictionary();
            dict[KEY_CURRENT_STATE] = new DataToken();
            dict[KEY_CURRENT_STATE_VALUE] = -1;

            FSMState emptyState = FSMState.Create();
            dict[KEY_STATES].DataDictionary[-1] = emptyState;
            dict[KEY_CURRENT_STATE_VALUE] = -1;
            dict[KEY_CURRENT_STATE] = emptyState;

            return (FSMachine)dict;
        }
    }

    public static class StateMachineExtensions
    {
        public static FSMachine _StateMachine(this DataToken token) => (FSMachine)token.DataDictionary;


        public static int _CurrentStateValue(this FSMachine machine) =>
            machine[FSMachine.KEY_CURRENT_STATE_VALUE].Int;

        public static FSMState _CurrentState(this FSMachine machine) =>
            machine[FSMachine.KEY_CURRENT_STATE]._FSMState();


        public static FSMState _RegisterState(this FSMachine machine, Enum state) =>
            machine._RegisterState(Convert.ToInt32(state));

        public static FSMState _RegisterState(this FSMachine machine, int state)
        {
            if (machine._States().ContainsKey(state)) return machine._States()[state]._FSMState();

            FSMState newState = FSMState.Create();
            machine._States()[state] = newState;

            return newState;
        }


        public static DataDictionary _States(this FSMachine machine) => machine[FSMachine.KEY_STATES].DataDictionary;


        public static FSMachine _SetState(this FSMachine machine, Enum state) =>
            machine._SetState(Convert.ToInt32(state));

        public static FSMachine _SetState(this FSMachine machine, int state)
        {
            FSMState currentState = machine._CurrentState();
            if (currentState != null)
            {
                currentState._OnExit()._Invoke();
            }

            machine[FSMachine.KEY_CURRENT_STATE_VALUE] = state;

            if (machine._States().TryGetValue(state, TokenType.DataDictionary, out DataToken value))
            {
                FSMState foundState = value._FSMState();
                machine[FSMachine.KEY_CURRENT_STATE] = foundState;
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