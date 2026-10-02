using JetBrains.Annotations;
using VRC.SDK3.Data;

namespace VirtualVisions.VTility
{
    public abstract class UsableElement : CanvasElement
    {
        /// <summary>
        /// Callback fired this button is pressed.
        /// Passes a reference to itself in the return value.
        /// </summary>
        public UdonAction onUsed => UdonActionExtensions.BackingUdonAction(ref _onUsed);
        private DataList _onUsed;

        
        /// <summary>
        /// Used when the associated component is triggered.
        /// Intended to be used with a Button/Slider/Other's use callback.
        /// </summary>
        [UsedImplicitly]
        public virtual void _OnComponentUsed()
        {
            onUsed._Invoke(this);
        }

    }
}