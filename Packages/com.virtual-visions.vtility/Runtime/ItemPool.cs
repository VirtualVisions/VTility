using UnityEngine;
using VRC.SDK3.Data;

namespace VirtualVisions.VTility
{

    public enum ItemPool_Values
    {
        Prefab,
        Parent,
        MaxCount,
        OnItemCreated,
        OnItemSpawned,
        ActiveItems,
        InactiveItems,
        // ---
        Count
    }

    public abstract class ItemPool : DataList
    {
        public static ItemPool Create(
            GameObject prefab,
            Transform parent,
            int maxCount)
        {
            DataToken[] values = new DataToken[(int)ItemPool_Values.Count];
            values[(int)ItemPool_Values.Prefab] = prefab;
            values[(int)ItemPool_Values.Parent] = parent;
            values[(int)ItemPool_Values.MaxCount] = maxCount;
            values[(int)ItemPool_Values.OnItemCreated] = UdonAction.Create();
            values[(int)ItemPool_Values.OnItemSpawned] = UdonAction.Create();
            values[(int)ItemPool_Values.ActiveItems] = new DataList(maxCount);
            values[(int)ItemPool_Values.InactiveItems] = new DataList(maxCount);


            ItemPool result = (ItemPool)new DataList(values);
            return result;
        }
    }

    public static class ItemPools
    {

        public static ItemPool _ItemPool(this DataToken token) => (ItemPool)token.DataList;
        private static GameObject _Prefab(this ItemPool pool) => pool[(int)ItemPool_Values.Prefab].CastReference<GameObject>();
        private static Transform _Parent(this ItemPool pool) => pool[(int)ItemPool_Values.Parent].CastReference<Transform>();
        private static int _MaxCount(this ItemPool pool) => pool[(int)ItemPool_Values.MaxCount].Int;
        public static UdonAction _OnItemCreated(this ItemPool pool) => pool[(int)ItemPool_Values.OnItemCreated].AsUdonAction();
        public static UdonAction _OnItemSpawned(this ItemPool pool) => pool[(int)ItemPool_Values.OnItemSpawned].AsUdonAction();
        private static DataList _ActiveItems(this ItemPool pool) => pool[(int)ItemPool_Values.ActiveItems].DataList;
        private static DataList _InactiveItems(this ItemPool pool) => pool[(int)ItemPool_Values.InactiveItems].DataList;

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