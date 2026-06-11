
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDKBase;
using VRC.Udon;

namespace VirtualVisions.VTility
{
    public abstract class ItemPool : DataDictionary
    {

        public const string KEY_PREFAB = "prefab";
        public const string KEY_PARENT = "parent";
        public const string KEY_MAX_COUNT = "maxCount";
        
        public const string KEY_ON_ITEM_CREATED = "onItemCreated";
        public const string KEY_ON_ITEM_SPAWNED = "onItemSpawned";
        public const string KEY_ACTIVE_ITEMS = "activeItems";
        public const string KEY_INACTIVE_ITEMS = "inactiveItems";
        
        public static ItemPool Create(
            GameObject prefab,
            Transform parent,
            int maxCount)
        {
            DataDictionary dict = new DataDictionary();
            dict[KEY_PREFAB] = prefab;
            dict[KEY_PARENT] = parent;
            dict[KEY_MAX_COUNT] = maxCount;

            dict[KEY_ON_ITEM_CREATED] = UdonAction.Create();
            dict[KEY_ON_ITEM_SPAWNED] = UdonAction.Create();
            dict[KEY_ACTIVE_ITEMS] = new DataList(maxCount);
            dict[KEY_INACTIVE_ITEMS] = new DataList(maxCount);

            return (ItemPool)dict;
        }
    }

    public static class ItemPoolExtensions
    {
        public static ItemPool _ItemPool(this DataToken token) => (ItemPool)token.DataDictionary;
        private static GameObject _Prefab(this ItemPool pool) => (GameObject)pool[ItemPool.KEY_PREFAB].Reference;
        private static Transform _Parent(this ItemPool pool) => (Transform)pool[ItemPool.KEY_PARENT].Reference;
        private static int _MaxCount(this ItemPool pool) => pool[ItemPool.KEY_MAX_COUNT].Int;
        private static DataList _ActiveItems(this ItemPool pool) => pool[ItemPool.KEY_ACTIVE_ITEMS].DataList;
        private static DataList _InactiveItems(this ItemPool pool) => pool[ItemPool.KEY_INACTIVE_ITEMS].DataList;
        public static UdonAction _OnItemCreated(this ItemPool pool) => pool[ItemPool.KEY_ON_ITEM_CREATED]._UdonAction();
        public static UdonAction _OnItemSpawned(this ItemPool pool) => pool[ItemPool.KEY_ON_ITEM_SPAWNED]._UdonAction();

        public static int _TotalItemCount(this ItemPool pool)
        {
            return pool._ActiveItems().Count + pool._InactiveItems().Count;
        }

        public static GameObject _GetItem(this ItemPool pool)
        {
            GameObject item;
            
            if (pool._InactiveItems().Count > 0)
            {
                item = (GameObject)pool._InactiveItems()[0].Reference;
            }
            else
            {
                item = pool._CreateItem();
            }
            
            if (item)
            {
                pool._ActiveItems().Add(item);
                pool._InactiveItems().Remove(item);
                item.SetActive(true);
                pool._OnItemSpawned()._Invoke(item);
            }
            else
            {
                Debug.LogWarning("No remaining items that can be allocated.");
            }
            return item;
        }

        public static void _ReturnItem(this ItemPool pool, GameObject item)
        {
            if (!pool._ActiveItems().Contains(item))
            {
                Debug.LogWarning($"Item {item} does not exist within pool.");
                return;
            }

            pool._ActiveItems().Remove(item);
            pool._InactiveItems().Add(item);
            item.SetActive(false);
        }

        public static void _ReturnAll(this ItemPool pool)
        {
            DataList activeCopy = new DataList();
            activeCopy.AddRange(pool._ActiveItems());
            
            int activeCount = activeCopy.Count;
            if (activeCount == 0) return;
            for (int i = 0; i < activeCount; i++)
            {
                GameObject item = (GameObject)activeCopy[i].Reference;
                pool._ReturnItem(item);
            }
        }

        private static GameObject _CreateItem(this ItemPool pool)
        {
            if (pool._TotalItemCount() >= pool._MaxCount())
            {
                return null;
            }

            GameObject item = Object.Instantiate(pool._Prefab(), pool._Parent());
            pool._InactiveItems().Add(item);
            pool._OnItemCreated()._Invoke(item);
            return item;
        }
    }
}