
using System;
using UdonSharp;
using UnityEngine;
using VirtualVisions.VTility;
using VRC.SDKBase;
using VRC.Udon;

public class ArrayIndexTest : UdonSharpBehaviour
{

    public int[] myArray = new int[10];

    private void Start()
    {
        if (myArray.TryIndex(2, out int value))
        {
            Debug.Log($"Value {value} found.");
        }
    }
}
