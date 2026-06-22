
using UdonSharp;
using UnityEngine;
using VirtualVisions.VTility;
using VRC.SDK3.Data;
using VRC.SDKBase;
using VRC.Udon;

public class TokenTest : UdonSharpBehaviour
{
    public override void Interact()
    {
        DataToken token = new DataToken(this);
        Debug.Log(token._CastReference<TokenTest>());
    }
}
