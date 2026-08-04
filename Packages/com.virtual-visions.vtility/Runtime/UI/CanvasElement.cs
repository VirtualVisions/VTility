using System;
using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace VirtualVisions.VTility
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class CanvasElement : UdonSharpBehaviour
    {

        [field: Header("Descriptor")]
        [field: SerializeField] public string Title { get; protected set; }
        [field: SerializeField] public string Subtitle { get; protected set; }
        [field: SerializeField] public Sprite Icon { get; protected set; }
        
        [field: Header("References")]
        [field: SerializeField] public TMP_Text TitleLabel { get; protected set; }
        [field: SerializeField] public TMP_Text SubtitleLabel { get; protected set; }
        [field: SerializeField] public Image IconImage { get; protected set; }
        
        [SerializeField] protected RectTransform _childContainer;
        [SerializeField] protected VRCTweenActionUdon _tweenOnShow;
        [SerializeField] protected VRCTweenActionUdon _tweenOnHide;


        protected bool _initialized;
        protected RectTransform _rectTrans;


        private void Start()
        {
            if (_initialized) Init();
        }


        protected virtual void Init()
        {
            _initialized = true;
            _rectTrans = (RectTransform)transform;
            if (!_childContainer) _childContainer = _rectTrans;
        }

        private void OnValidate() => OnValidation();

        protected virtual void OnValidation()
        {
            SetTitle(Title);
            SetDescription(Subtitle);
            SetIcon(Icon);
        }

        public void SetTitle(string value)
        {
            Title = value;
            if (TitleLabel) TitleLabel.text = Title;
        }

        public void SetDescription(string value)
        {
            Subtitle = value;
            if (SubtitleLabel) SubtitleLabel.text = Subtitle;
        }

        public void SetIcon(Sprite value)
        {
            Icon = value;
            if (IconImage) IconImage.sprite = Icon;
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
                gameObject.SetActive(true);
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