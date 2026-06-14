
using UdonSharp;
using UnityEngine;
using VirtualVisions.VTility;
using VRC.SDKBase;
using VRC.Udon;

public class StringCompareTest : UdonSharpBehaviour
{

    public string BookTitle;
    public string SearchKey;
    public float MaxDeltaPercent = 20;

    public override void Interact()
    {
        Debug.Log(BookTitle.FuzzyContains(SearchKey, MaxDeltaPercent));
    }
}
