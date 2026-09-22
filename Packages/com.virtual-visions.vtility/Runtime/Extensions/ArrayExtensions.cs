namespace VirtualVisions.VTility
{
    public static class ArrayExtensions
    {
        /// <summary>
        /// Try to extract an index out of an array.
        /// If the index is not within the array bounds, this method will fail and return null.
        /// </summary>
        public static bool TryIndex<T>(this T[] array, int index, out T value)
        {
            if (index < 0 || index >= array.Length)
            {
                value = default;
                return false;
            }

            value = array[index];
            return true;
        }
    }
}