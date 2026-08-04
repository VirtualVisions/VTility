
using UdonSharp;
using UnityEngine;
using VirtualVisions.VTility;
using VRC.SDKBase;
using VRC.Udon;

public class CanvasElementTest : UdonSharpBehaviour
{

    public CanvasElement element;

    public void _ShowElement()
    {
        element._ShowElement();
    }

    public void _HideElement()
    {
        element._HideElement();
    }
    
}
