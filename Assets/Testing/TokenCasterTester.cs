using System;
using UdonSharp;
using UnityEngine;
using VirtualVisions.VTility;
using VRC.SDK3.Data;

namespace Testing
{
    public class TokenCasterTester : UdonSharpBehaviour
    {
        private void Start()
        {
            DataList list = new DataList();
            list._AddRef(Vector2.one);

            DataToken colorToken = new DataToken(Color.cyan);
            Debug.Log(colorToken._ColorRef());
        }
    }
}