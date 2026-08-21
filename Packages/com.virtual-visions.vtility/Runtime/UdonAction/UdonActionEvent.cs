using UnityEngine;

namespace VirtualVisions.VTility
{
    [RequireComponent(typeof(UdonActionEventRuntime))]
    public class UdonActionEvent : MonoBehaviour
    {
        public UdonActionTriggerMode mode;
        public UdonEventField[] events;
    }
}