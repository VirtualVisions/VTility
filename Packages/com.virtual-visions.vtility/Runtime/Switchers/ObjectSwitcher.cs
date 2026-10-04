using System;
using JetBrains.Annotations;
using UnityEngine;
using VRC.SDK3.Data;

namespace VirtualVisions.VTility
{
    public enum ObjectSwitcher_Values
    {
        Active,
        ObjectList,
        ObjectChange,
        // ---
        Count
    }
    
    public abstract class ObjectSwitcher : DataList
    {
        /// Since Udon doesn't natively support fields of this type,
        /// it is recommended to use this format for your fields:
        ///
        ///    public ObjectSwitcher switcher => ObjectSwitcherExtensions.BackingObjectSwitcher(ref _switcher, values);
        ///    private DataList _switcher;
        ///
        
        public static ObjectSwitcher Create(GameObject[] objectList)
        {
            DataToken[] values = new DataToken[(int)ObjectSwitcher_Values.Count];
            
            DataToken defaultValue = new DataToken((GameObject)null);

            values[(int)ObjectSwitcher_Values.Active] = defaultValue;
            values[(int)ObjectSwitcher_Values.ObjectList] = objectList.ToRefList();
            values[(int)ObjectSwitcher_Values.ObjectChange] = ValueChangeCallback.Create(defaultValue);

            DataList objectSwitcher = new DataList(values);
            return (ObjectSwitcher)objectSwitcher;
        }
    }

    public static class ObjectSwitcherExtensions
    {
        
        public static ObjectSwitcher AsObjectSwitcher(this DataToken token) => (ObjectSwitcher)token.DataList;
        public static ObjectSwitcher AsObjectSwitcher(this DataList list) => (ObjectSwitcher)list;

        public static ObjectSwitcher BackingObjectSwitcher(ref DataList backingValue, GameObject[] objectList) =>
            (ObjectSwitcher)(backingValue != null ? backingValue : backingValue = ObjectSwitcher.Create(objectList));
        
        // ---
        
        public static DataList ObjectList(this ObjectSwitcher switcher) => switcher[(int)ObjectSwitcher_Values.ObjectList].DataList;
        public static GameObject Active(this ObjectSwitcher switcher) => switcher.ObjectChange().Value().AsGameObject();
        public static ValueChangeCallback ObjectChange(this ObjectSwitcher switcher) => switcher[(int)ObjectSwitcher_Values.ObjectChange].AsValueChangeCallback();


        /// <summary>
        /// Passes the new value that is switched to.
        /// </summary>
        public static UdonAction OnObjectSwitched(this ObjectSwitcher switcher) =>
            switcher.ObjectChange().OnChanged();
        
        [PublicAPI]
        public static void AddObject(this ObjectSwitcher switcher, GameObject gameObject)
        {
            switcher.ObjectList().Add(gameObject);
        }

        [PublicAPI]
        public static void SwitchTo(this ObjectSwitcher switcher, GameObject gameObject)
        {
            DataList list = switcher.ObjectList();
            if (!list.Contains(gameObject)) switcher.AddObject(gameObject);
            switcher.SwitchToIndex(list.IndexOf(gameObject));
        }

        [PublicAPI]
        public static void SwitchToIndex(this ObjectSwitcher switcher, Enum index) => switcher.SwitchToIndex(Convert.ToInt32(index));

        [PublicAPI]
        public static void SwitchToIndex(this ObjectSwitcher switcher, int index)
        {
            switcher.ObjectList().TryIndex(index, out DataToken value);
            switcher.ObjectChange().SetValue(value);
        }
    }
    
}