
using System;
using UdonSharp;
using UnityEngine;
using VirtualVisions.VTility;
using VRC.SDK3.Data;

public class ObjSwitcherTest : UdonSharpBehaviour
{

    public Transform[] objs;

    public ObjectSwitcher switcher => (ObjectSwitcher)_switcher;
    private DataList _switcher;


    private void Start()
    {
        _switcher = ObjectSwitchers.Create(objs);
    }

    public override void Interact()
    {
        switcher.SwitchTo(UnityEngine.Random.Range(0, objs.Length));
    }
}
