
using System;
using UdonSharp;
using UnityEngine;
using VirtualVisions.VTility;
using VRC.SDK3.Data;

public class ObjSwitcherTest : UdonSharpBehaviour
{

    public Transform[] Objs;

    public ObjectSwitcher Switcher => (ObjectSwitcher)_switcher;
    private DataDictionary _switcher;


    private void Start()
    {
        _switcher = ObjectSwitcher.Create(Objs);
    }

    public override void Interact()
    {
        Switcher._SwitchTo(UnityEngine.Random.Range(0, Objs.Length));
    }
}
