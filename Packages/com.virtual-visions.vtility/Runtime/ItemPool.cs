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

    public static class ItemPoolExtensions
    {

        public static ItemPool AsItemPool(this DataToken token) => (ItemPool)token.DataList;
        public static ItemPool AsItemPool(this DataList list) => (ItemPool)list;
        
        private static GameObject Prefab(this ItemPool pool) => pool[(int)ItemPool_Values.Prefab].CastReference<GameObject>();
        private static Transform Parent(this ItemPool pool) => pool[(int)ItemPool_Values.Parent].CastReference<Transform>();
        private static int MaxCount(this ItemPool pool) => pool[(int)ItemPool_Values.MaxCount].Int;
        public static UdonAction OnItemCreated(this ItemPool pool) => pool[(int)ItemPool_Values.OnItemCreated].AsUdonAction();
        public static UdonAction OnItemSpawned(this ItemPool pool) => pool[(int)ItemPool_Values.OnItemSpawned].AsUdonAction();
        private static DataList ActiveItems(this ItemPool pool) => pool[(int)ItemPool_Values.ActiveItems].DataList;
        private static DataList InactiveItems(this ItemPool pool) => pool[(int)ItemPool_Values.InactiveItems].DataList;

        public static int TotalItemCount(this ItemPool pool)
        {
            return pool.ActiveItems().Count + pool.InactiveItems().Count;
        }

        public static GameObject GetItem(this ItemPool pool)
        {
            GameObject item;

            if (pool.InactiveItems().Count > 0)
            {
                item = (GameObject)pool.InactiveItems()[0].Reference;
            }
            else
            {
                item = pool.CreateItem();
            }

            if (item)
            {
                pool.ActiveItems().Add(item);
                pool.InactiveItems().Remove(item);
                item.SetActive(true);
                pool.OnItemSpawned()._Invoke(item);
            }
            else
            {
                Debug.LogWarning("No remaining items that can be allocated.");
            }

            return item;
        }

        public static void ReturnItem(this ItemPool pool, GameObject item)
        {
            if (!pool.ActiveItems().Contains(item))
            {
                Debug.LogWarning($"Item {item} does not exist within pool.");
                return;
            }

            pool.ActiveItems().Remove(item);
            pool.InactiveItems().Add(item);
            item.SetActive(false);
        }

        public static void ReturnAll(this ItemPool pool)
        {
            DataList activeCopy = new DataList();
            activeCopy.AddRange(pool.ActiveItems());

            int activeCount = activeCopy.Count;
            if (activeCount == 0) return;
            for (int i = 0; i < activeCount; i++)
            {
                GameObject item = (GameObject)activeCopy[i].Reference;
                pool.ReturnItem(item);
            }
        }

        private static GameObject CreateItem(this ItemPool pool)
        {
            if (pool.TotalItemCount() >= pool.MaxCount())
            {
                return null;
            }

            GameObject item = Object.Instantiate(pool.Prefab(), pool.Parent());
            pool.InactiveItems().Add(item);
            pool.OnItemCreated()._Invoke(item);
            return item;
        }
    }
}