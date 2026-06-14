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
        public const string KEY_LIST = "list";

        public static ObjectSwitcher Create(GameObject[] objs)
        {
            DataDictionary dict = new DataDictionary();
            dict[KEY_LIST] = objs._ToReferenceArray();
            dict[KEY_ACTIVE] = new DataToken((GameObject)null);

            foreach (GameObject obj in objs) obj.SetActive(false);
            
            ObjectSwitcher switcher = (ObjectSwitcher)dict;
            return switcher;
        }
    }

    public static class ObjectSwitcherExtensions
    {
        public static ObjectSwitcher _ObjSwitcher(this DataToken token) => (ObjectSwitcher)token.DataDictionary;
        public static DataList _List(this ObjectSwitcher switcher) => switcher[ObjectSwitcher.KEY_LIST].DataList;
        public static GameObject _Active(this ObjectSwitcher switcher) => (GameObject)switcher[ObjectSwitcher.KEY_ACTIVE].Reference;

        public static void _AddObject(this ObjectSwitcher switcher, GameObject obj)
        {
            switcher._List().Add(obj);
            obj.SetActive(false);
        }
        
        public static void _SwitchTo(this ObjectSwitcher switcher, GameObject obj)
        {
            DataList list = switcher._List();
            if (!list.Contains(obj)) list.Add(obj);

            GameObject active = switcher._Active();
            if (active) active.SetActive(false);
            
            obj.SetActive(true);
            switcher[ObjectSwitcher.KEY_ACTIVE] = obj;
        }
        
        public static void _SwitchTo(this ObjectSwitcher switcher, int index)
        {
            DataList list = switcher._List();
            if (index < 0 || index >= list.Count) return;
            
            GameObject obj = list[index].CastReference<GameObject>();
            
            GameObject active = switcher._Active();
            if (active) active.SetActive(false);
            
            obj.SetActive(true);
            switcher[ObjectSwitcher.KEY_ACTIVE] = obj;
        }
    }
}