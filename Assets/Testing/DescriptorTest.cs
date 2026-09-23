
using System;
using UdonSharp;
using UnityEngine;
using VirtualVisions.VTility;
using VRC.SDKBase;
using VRC.Udon;

public class DescriptorTest : UdonSharpBehaviour
{

    public string title;
    public Sprite image;
    public CanvasElement element;
    
    private void Start()
    {
        element.descriptor.SetTitle(title);
        element.descriptor.SetIcon(image);
    }
}
