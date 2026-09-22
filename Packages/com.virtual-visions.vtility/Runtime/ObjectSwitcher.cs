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
        OnObjectSwitched,
        IsComponentList,
        ComponentList,
        
        Count
    }
    
    /// <summary>
    /// Switch between a list of GameObjects, allowing for only one to be active at any time.
    /// </summary>
    public abstract class ObjectSwitcher : DataList
    {
        
        /// Since Udon doesn't natively support fields of this type,
        /// it is recommended to use this format for your fields:
        ///
        ///    public ObjectSwitcher switcher
        ///    {
        ///        get => (ObjectSwitcher)_switcher;
        ///        set => _switcher = value;
        ///    }
        ///    private DataList _switcher;

        
        [PublicAPI]
        public static ObjectSwitcher Create(GameObject[] objs)
        {
            DataToken[] values = new DataToken[(int)ObjectSwitcher_Values.Count];
            
            values.Set(ObjectSwitcher_Values.Active, (GameObject)null);
            values.Set(ObjectSwitcher_Values.ObjectList, objs.ToRefList());
            values.Set(ObjectSwitcher_Values.OnObjectSwitched, UdonAction.Create());
            values.Set(ObjectSwitcher_Values.IsComponentList, false);
            values.Set(ObjectSwitcher_Values.ComponentList, new DataList());
            
            DataList objectSwitcher = new DataList(values);
            return (ObjectSwitcher)objectSwitcher;
        }

        [PublicAPI]
        public static ObjectSwitcher Create<T>(T[] components) where T: Component
        {
            DataToken[] values = new DataToken[(int)ObjectSwitcher_Values.Count];
            
            values.Set(ObjectSwitcher_Values.ComponentList, components.ToRefList());
            values.Set(ObjectSwitcher_Values.IsComponentList, true);
            values.Set(ObjectSwitcher_Values.Active, new DataToken((GameObject)null));
            values.Set(ObjectSwitcher_Values.OnObjectSwitched, UdonAction.Create());

            DataList objList = new DataList();
            foreach (T comp in components)
            {
                GameObject obj = comp.gameObject;
                obj.SetActive(false);
                objList.Add(obj);
            }
            values.Set(ObjectSwitcher_Values.ObjectList, objList);
            
            DataList objectSwitcher = new DataList(values);
            return (ObjectSwitcher)objectSwitcher;
        }
    }

    public static class ObjectSwitcherExtensions
    {
        public static ObjectSwitcher AsObjSwitcher(this DataToken token) => (ObjectSwitcher)token.DataList;
        public static DataList ObjectList(this ObjectSwitcher switcher) => switcher[(int)ObjectSwitcher_Values.ObjectList].DataList;
        public static GameObject Active(this ObjectSwitcher switcher) => (GameObject)switcher[(int)ObjectSwitcher_Values.Active].Reference;
        /// <summary>
        /// Passes the new object that is switched to.
        /// If this is a GameObject list, the GameObject is passed.
        /// If it's a Component list however, it will be passed cast to Component. 
        /// </summary>
        /// <param name="switcher"></param>
        /// <returns></returns>
        public static UdonAction OnObjectSwitched(this ObjectSwitcher switcher) => 
            switcher.Get(ObjectSwitcher_Values.OnObjectSwitched).UdonAction();
        
        public static bool IsComponentList(this ObjectSwitcher switcher) => 
            switcher.Get(ObjectSwitcher_Values.IsComponentList).Boolean;
        
        public static DataList ComponentList(this ObjectSwitcher switcher) => 
            switcher.Get(ObjectSwitcher_Values.ComponentList).DataList;
        
        
        [PublicAPI]
        public static void AddObject(this ObjectSwitcher switcher, GameObject obj)
        {
            switcher.ObjectList().Add(obj);
            obj.SetActive(false);
        }

        [PublicAPI]
        public static void AddObject<T>(this ObjectSwitcher switcher, T component) where T: Component
        {
            GameObject obj = component.gameObject;
            switcher.ObjectList().Add(obj);
            switcher.ComponentList().Add(component);
            obj.SetActive(false);
        }
        
        [PublicAPI]
        public static void SwitchTo(this ObjectSwitcher switcher, GameObject obj)
        {
            DataList list = switcher.ObjectList();
            if (!list.Contains(obj)) list.Add(obj);
            switcher.SwitchTo(list.IndexOf(obj));
        }

        [PublicAPI]
        public static void SwitchTo(this ObjectSwitcher switcher, Enum index) => switcher.SwitchTo(Convert.ToInt32(index));

        [PublicAPI]
        public static void SwitchTo(this ObjectSwitcher switcher, int index)
        {
            GameObject active = switcher.Active();
            if (active) active.SetActive(false);
            
            GameObject obj = null;
            
            DataList list = switcher.ObjectList();
            if (index >= 0 && index < list.Count)
            {
                obj = list[index].CastReference<GameObject>(); 
                switcher[(int)ObjectSwitcher_Values.Active] = obj;
                obj.SetActive(true);
            }
            
            if (switcher.IsComponentList())
            {
                DataList compList = switcher.ComponentList();
                switcher.OnObjectSwitched()._Invoke(compList[index].CastReference<Component>());
            }
            else
            {
                switcher.OnObjectSwitched()._Invoke(obj);
            }
        }
    }
}