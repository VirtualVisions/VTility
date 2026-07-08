using System;
using UnityEngine;
using VirtualVisions.VTility;

namespace VowTest
{
    public class SubclassExample : MonoBehaviour
    {
        [SerializeReference] [SubclassDropdown(typeof(MyClass))]
        public MyClass[] tests;
    }


    [Serializable]
    public abstract class MyClass
    {

    }

    [Serializable]
    public class MySubClass : MyClass
    {
        public string text;
    }
}