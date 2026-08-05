using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace VirtualVisions.VTility
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class ElementDescriptor : UdonSharpBehaviour
    {
        
        [field: SerializeField] public string Title { get; protected set; }
        [field: SerializeField] public string Subtitle { get; protected set; }
        [field: SerializeField] public Sprite Icon { get; protected set; }
        
        [field: Header("References")]
        [field: SerializeField] public TMP_Text TitleLabel { get; protected set; }
        [field: SerializeField] public TMP_Text SubtitleLabel { get; protected set; }
        [field: SerializeField] public Image IconImage { get; protected set; }

        
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

    }
}