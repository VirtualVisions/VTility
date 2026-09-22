using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.Udon;

namespace VirtualVisions.VTility
{
    [AddComponentMenu("")]
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class UdonActionEventRuntime : UdonSharpBehaviour
    {
        [HideInInspector] public UdonActionTriggerMode mode;
        [HideInInspector] public UdonBehaviour[] targets;
        [HideInInspector] public string[] eventNames;

        public UdonAction action => (UdonAction)(_action != null ? _action : _action = UdonAction.Create());
        private DataList _action;
        
        private bool _initialized;
        
        
        private void Start()
        {
            if (mode == UdonActionTriggerMode.OnStart) _Invoke();
        }

        private void OnEnable()
        {
            if (mode == UdonActionTriggerMode.OnEnable) _Invoke();
        }

        private void OnDisable()
        {
            if (mode == UdonActionTriggerMode.OnDisable) _Invoke();
        }

        private void Init()
        {
            _initialized = true;

            for (int i = 0; i < targets.Length; i++)
            {
                UdonBehaviour target = targets[i];
                string eventName = eventNames[i];

                action.AddListener(target, eventName);
            }
        }


        public void _Invoke()
        {
            if (!_initialized) Init();

            action._Invoke();
        }
    }
}