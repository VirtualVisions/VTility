using JetBrains.Annotations;
using UnityEngine;
using VRC.SDK3.Data;

namespace VirtualVisions.VTility
{
    /// <summary>
    /// Switch between a list of GameObjects, allowing for only one to be active at any time.
    /// </summary>
    public abstract class ObjectSwitcher : DataDictionary
    {
        
        /// Since Udon doesn't natively support fields of this type,
        /// it is recommended to use this format for your fields:
        ///
        ///    public ObjectSwitcher Switcher => (ObjectSwitcher)_switcher;
        ///    private DataDictionary _switcher;
        /// 

        public const string KEY_ACTIVE = "active";
        public const string KEY_OBJECT_LIST = "objectList";
        public const string KEY_ON_OBJECT_SWITCHED = "onObjectSwitched";
        public const string KEY_IS_COMPONENT_LIST = "isComponentList";
        public const string KEY_COMPONENT_LIST = "componentList";

        [PublicAPI]
        public static ObjectSwitcher Create(GameObject[] objs)
        {
            DataDictionary dict = new DataDictionary();
            dict[KEY_OBJECT_LIST] = objs.ToRefList();
            dict[KEY_ACTIVE] = new DataToken((GameObject)null);
            dict[KEY_ON_OBJECT_SWITCHED] = UdonAction.Create();
            dict[KEY_IS_COMPONENT_LIST] = false;
            dict[KEY_COMPONENT_LIST] = new DataList();

            foreach (GameObject obj in objs) obj.SetActive(false);
            
            ObjectSwitcher switcher = (ObjectSwitcher)dict;
            return switcher;
        }

        [PublicAPI]
        public static ObjectSwitcher Create<T>(T[] components) where T: Component
        {
            DataDictionary dict = new DataDictionary();
            dict[KEY_COMPONENT_LIST] = components.ToRefList();
            dict[KEY_ACTIVE] = new DataToken((GameObject)null);
            dict[KEY_ON_OBJECT_SWITCHED] = UdonAction.Create();
            dict[KEY_IS_COMPONENT_LIST] = true;

            DataList objList = new DataList();
            foreach (T comp in components)
            {
                GameObject obj = comp.gameObject;
                obj.SetActive(false);
                objList.Add(obj);
            }
            dict[KEY_OBJECT_LIST] = objList;
            
            ObjectSwitcher switcher = (ObjectSwitcher)dict;
            return switcher;
        }
    }

    public static class ObjectSwitcherExtensions
    {
        public static ObjectSwitcher AsObjSwitcher(this DataToken token) => (ObjectSwitcher)token.DataDictionary;
        public static DataList ObjectList(this ObjectSwitcher switcher) => switcher[ObjectSwitcher.KEY_OBJECT_LIST].DataList;
        public static GameObject Active(this ObjectSwitcher switcher) => (GameObject)switcher[ObjectSwitcher.KEY_ACTIVE].Reference;
        public static UdonAction OnObjectSwitched(this ObjectSwitcher switcher) => switcher[ObjectSwitcher.KEY_ON_OBJECT_SWITCHED].UdonAction();
        public static bool IsComponentList(this ObjectSwitcher switcher) => switcher[ObjectSwitcher.KEY_IS_COMPONENT_LIST].Boolean;
        public static DataList ComponentList(this ObjectSwitcher switcher) => switcher[ObjectSwitcher.KEY_COMPONENT_LIST].DataList;

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
        public static void SwitchTo(this ObjectSwitcher switcher, int index)
        {
            GameObject active = switcher.Active();
            if (active) active.SetActive(false);
            
            GameObject obj = null;
            
            DataList list = switcher.ObjectList();
            if (index >= 0 && index < list.Count)
            {
                obj = list[index].CastReference<GameObject>(); 
                switcher[ObjectSwitcher.KEY_ACTIVE] = obj;
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