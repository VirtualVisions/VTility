
using System;
using JetBrains.Annotations;
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
        public bool useFilter;
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

        [PublicAPI]
        public void _BuildFilter()
        {
            useFilter = true;
            DataList indices = new DataList();
            for (int i = 0; i < demoFieldData.Length; i++)
            {
                if (UnityEngine.Random.Range(0, 2) == 1)
                {
                    indices.Add(i);
                }
            }
            
            list.FilterBy(indices);
        }

        [PublicAPI]
        public void _ClearFilter()
        {
            useFilter = false;
            list._ClearFilter();
        }
        
        
        private void OnEnable()
        {
            list.OnBindItem._AddListener(
                this,
                nameof(_OnItemBound),
                nameof(_OnItemBound_Value));
            
            list.SetItemSource(demoFieldData.ToStringList());
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
            string item = _OnItemBound_Value[2].String;
            
            TMP_Text label = rt.GetComponentInChildren<TMP_Text>();
            if (label)
            {
                label.text = string.Concat(index, " - ", item);
            }
        }
    }
}