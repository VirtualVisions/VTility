
using System;
using UdonSharp;
using UnityEngine;
using VirtualVisions.VTility;
using VRC.SDK3.Data;

public class ObjSwitcherTest : UdonSharpBehaviour
{

    public GameObject[] objs;

    public ObjectSwitcher switcher => (ObjectSwitcher)_switcher;
    private DataList _switcher;


    private void Start()
    {
        _switcher = ObjectSwitcher.Create(objs);
    }

    public override void Interact()
    {
        switcher.SwitchToIndex(UnityEngine.Random.Range(0, objs.Length));
    }
}
