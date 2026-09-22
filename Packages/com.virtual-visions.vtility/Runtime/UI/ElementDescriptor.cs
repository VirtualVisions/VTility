using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace VirtualVisions.VTility
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class ElementDescriptor : UdonSharpBehaviour
    {
        
        [field: SerializeField] public string title { get; protected set; }
        [field: SerializeField] public string subtitle { get; protected set; }
        [field: SerializeField] public Sprite icon { get; protected set; }
        
        [field: Header("References")]
        [field: SerializeField] public TMP_Text titleLabel { get; protected set; }
        [field: SerializeField] public TMP_Text subtitleLabel { get; protected set; }
        [field: SerializeField] public Image iconImage { get; protected set; }

        
        private void OnValidate() => OnValidation();

        protected virtual void OnValidation()
        {
            SetTitle(title);
            SetDescription(subtitle);
            SetIcon(icon);
        }

        public void SetTitle(string value)
        {
            title = value;
            if (titleLabel) titleLabel.text = title;
        }

        public void SetDescription(string value)
        {
            subtitle = value;
            if (subtitleLabel) subtitleLabel.text = subtitle;
        }

        public void SetIcon(Sprite value)
        {
            icon = value;
            if (iconImage) iconImage.sprite = icon;
        }

    }
}