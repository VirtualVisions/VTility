using System;
using UdonSharp;
using UnityEngine;
using VirtualVisions.VTility;
using VRC.SDK3.Data;
using VRC.Udon;

public class UdonActionTester : UdonSharpBehaviour
{

    public UdonAction<float> onValueChanged => UdonActions.BackingUdonAction<float>(ref _onValueChanged);
    private DataList _onValueChanged;


    private void Start()
    {
        UdonEvent<float> ue = UdonEvents.Create<float>(this, nameof(Thing), nameof(thingValue));
        onValueChanged.AddListener(ue);
    }

    public override void Interact()
    {
        onValueChanged._Invoke(UnityEngine.Random.Range(0, 10));
    }

    public float thingValue;

    public void Thing()
    {
        Debug.Log(thingValue);
    }
}