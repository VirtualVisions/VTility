using VRC.SDK3.Data;

namespace VirtualVisions.VTility
{
    public static class DataArrayExtensions
    {
        /// <summary>
        /// Convert a datalist of references to an array of that given type.
        /// </summary>
        public static T[] _FromReferenceList<T>(this DataList list)
        {
            T[] array = new T[list.Count];
            for (int i = 0; i < list.Count; i++) array[i] = (T)list[i].Reference;
            return array;
        }

        /// <summary>
        /// Convert an array to a datalist of references.
        /// </summary>
        public static DataList _ToReferenceArray<T>(this T[] array)
        {
            DataList list = new DataList();
            foreach (T entry in array) list.Add(new DataToken(entry));
            return list;
        }
    }
}