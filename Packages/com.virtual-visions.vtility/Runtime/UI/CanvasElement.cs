using UdonSharp;
using UnityEngine;

namespace VirtualVisions.VTility
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class CanvasElement : UdonSharpBehaviour
    {

        public bool Shown { get; private set; }
        [SerializeField] protected VRCTweenActionUdon _tweenOnShow;
        [SerializeField] protected VRCTweenActionUdon _tweenOnHide;

        protected virtual RectTransform _childContainer => _rectTrans;

        /// <summary>
        /// The descriptor for this element. Automatically populated during build.
        /// If one is not found on this GameObject, this value will be null.
        /// </summary>
        [SerializeField, HideInInspector, FindComponent]
        public ElementDescriptor Descriptor;

        protected bool _initialized;
        protected RectTransform _rectTrans;



        private void Start()
        {
            if (_initialized) Init();
        }


        private void OnEnable() => OnEnabled();

        protected virtual void OnEnabled()
        {
            Shown = true;
        }


        private void OnDisable() => OnDisabled();

        protected virtual void OnDisabled()
        {
            Shown = false;
        }


        protected virtual void Init()
        {
            _initialized = true;
            _rectTrans = (RectTransform)transform;
        }


        /// <summary>
        /// Enable this GameObject. Plays a tween if available.
        /// </summary>
        public void _ShowElement()
        {
            if (_tweenOnShow) _tweenOnShow._RunTween();
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Disable this GameObject. Plays a tween if available, disabling the GameObject after.
        /// </summary>
        public void _HideElement()
        {
            if (_tweenOnHide)
            {
                _tweenOnHide._RunTween();
                _tweenOnHide.AddCompletionCallback(this, nameof(_DisableOnHideTweenComplete));
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// NOTE: Should only be used as the completion callback for the hiding tween. 
        /// Disables this GameObject.
        /// </summary>
        public void _DisableOnHideTweenComplete()
        {
            gameObject.SetActive(false);
        }
    }
}