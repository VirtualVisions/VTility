namespace VirtualVisions.VTility
{
    public static class ArrayExtensions
    {
        public static bool TryIndex<T>(this T[] array, int index, out T value)
        {
            if (index < 0 || array.Length >= index)
            {
                value = default;
                return false;
            }

            value = array[index];
            return true;
        }
    }
}