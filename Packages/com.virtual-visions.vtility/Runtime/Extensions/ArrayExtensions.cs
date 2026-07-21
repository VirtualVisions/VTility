namespace VirtualVisions.VTility
{
    public static class ArrayExtensions
    {
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