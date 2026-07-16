using UnityEngine;

namespace VirtualVisions.VTility
{
    public abstract class PaletteComponentBase : MonoBehaviour
    {

        [HideInInspector] public string paletteName;
        
        private void OnValidate()
        {
            EditorEventQueue.QueueEvent(ApplyPalette);
        }
        
        public abstract void ApplyPalette();

    }
}
