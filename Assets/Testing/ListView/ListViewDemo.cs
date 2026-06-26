
using System;
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;

namespace VirtualVisions.VTility.Demo
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class ListViewDemo : UdonSharpBehaviour
    {
        public BaseUdonListView list;
        public string[] demoFieldData;

        [ContextMenu("Generate Demo Data")]
        public void _GenerateDemoData()
        {
            int length = UnityEngine.Random.Range(5, 50);
            demoFieldData = new string[length];
            for (int i = 0; i < length; i++)
            {
                demoFieldData[i] = string.Concat(i, ": ", Guid.NewGuid().ToString());
            }
        }
        
        private void OnEnable()
        {
            list.OnBindItem._AddListener(
                this,
                nameof(_OnItemBound),
                nameof(_OnItemBound_Value));
            
            list.SetItemSource(demoFieldData.ToRefList());
        }

        private void OnDisable()
        {
            list.OnBindItem._RemoveListener(
                this,
                nameof(_OnItemBound),
                nameof(_OnItemBound_Value));
        }


        [HideInInspector] public DataList _OnItemBound_Value;
        public void _OnItemBound()
        {
            RectTransform rt = _OnItemBound_Value[0].CastReference<RectTransform>();
            int index = _OnItemBound_Value[1].Int;
            
            TMP_Text label = rt.GetComponentInChildren<TMP_Text>();
            if (label)
            {
                label.text = demoFieldData[index];
            }
        }
    }
}