using VRC.SDK3.Data;

namespace VirtualVisions.VTility
{
    public static class DataArrayExtensions
    {
        #region References
        
        /// <summary>
        /// Convert an array to a DataList of references.
        /// </summary>
        public static DataList ToRefList<T>(this T[] array)
        {
            DataList list = new DataList();
            foreach (T entry in array) list.Add(new DataToken(entry));
            return list;
        }
        
        /// <summary>
        /// Convert a DataList of references to an array of that given type.
        /// </summary>
        public static T[] ToRefArray<T>(this DataList list)
        {
            T[] array = new T[list.Count];
            for (int i = 0; i < list.Count; i++) array[i] = (T)list[i].Reference;
            return array;
        }

        #endregion

        #region Strings

        public static DataList ToStringList(this string[] array)
        {
            DataList list = new DataList();
            foreach (string entry in array) list.Add(entry);
            return list;
        }

        public static string[] ToStringArray(this DataList list)
        {
            string[] array = new string[list.Count];
            for (int i = 0; i < list.Count; i++) array[i] = list[i].String;
            return array;
        }
        

        #endregion

        #region Ints

        public static DataList ToIntList(this int[] array)
        {
            DataList list = new DataList();
            foreach (int entry in array) list.Add(entry);
            return list;
        }

        public static int[] ToIntArray(this DataList list)
        {
            int[] array = new int[list.Count];
            for (int i = 0; i < list.Count; i++) array[i] = list[i].Int;
            return array;
        }
        

        #endregion
    }
}