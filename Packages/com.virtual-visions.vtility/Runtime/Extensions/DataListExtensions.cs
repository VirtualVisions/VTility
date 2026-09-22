using System;
using VRC.SDK3.Data;

namespace VirtualVisions.VTility
{
    public static class DataListExtensions
    {
        public static bool TryIndex(this DataList list, int index, out DataToken value)
        {
            if (index < 0 || index >= list.Count)
            {
                value = new DataToken(DataError.IndexOutOfRange);
                return false;
            }

            value = list[index];
            return true;
        }

        public static DataToken Get(this DataList list, Enum key)
        {
            return list[Convert.ToInt32(key)];
        }

        public static DataToken Get(this DataToken[] array, Enum key)
        {
            return array[Convert.ToInt32(key)];
        }

        public static void Set(this DataList list, Enum key, DataToken value)
        {
            list[Convert.ToInt32(key)] = value;
        }

        public static void Set(this DataToken[] array, Enum key, DataToken value)
        {
            array[Convert.ToInt32(key)] = value;
        }
    }
}