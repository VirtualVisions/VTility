using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;

namespace VirtualVisions.VTility
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class CanvasElement : UdonSharpBehaviour
    {

        /// <summary>
        /// Whether the element is actively displayed and active.
        /// </summary>
        public bool isShown { get; private set; }
        
        /// <summary>
        /// Parent used for storing child elements.
        /// </summary>
        public virtual RectTransform childContainer => _rectTrans;
        
        /// <summary>
        /// Callback fired immediately upon the GameObject running OnEnable.
        /// </summary>
        public UdonAction onShow => UdonActionExtensions.BackingUdonAction(ref _onShow);
        private DataList _onShow;
        
        /// <summary>
        /// Callback fired immediately upon the GameObject running OnDisable.
        /// </summary>
        public UdonAction onHidden => UdonActionExtensions.BackingUdonAction(ref _onHidden);
        private DataList _onHidden;

        /// <summary>
        /// The descriptor for this element. Automatically populated during build.
        /// If one is not found on this GameObject, this value will be null.
        /// </summary>
        /// Note: For some U# compatability reason, GetComponent cannot be used on a {get; private set;} field.
        [GetComponent, SerializeField, HideInInspector] public ElementDescriptor descriptor;
        
        
        [SerializeField] protected VRCTweenActionUdon _tweenOnShow;
        [SerializeField] protected VRCTweenActionUdon _tweenOnHide;


        protected bool _initialized;
        protected RectTransform _rectTrans;



        private void Start()
        {
            if (!_initialized) Init();
        }


        private void OnEnable() => OnEnabled();

        protected virtual void OnEnabled()
        {
            isShown = true;
            onShow._Invoke();
        }


        private void OnDisable() => OnDisabled();

        protected virtual void OnDisabled()
        {
            isShown = false;
            onHidden._Invoke();
        }


        protected virtual void Init()
        {
            _initialized = true;
            _rectTrans = (RectTransform)transform;
        }


        /// <summary>
        /// Reparent an item to the ChildContainer of this Element.
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(RectTransform item)
        {
            item.SetParent(childContainer);
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