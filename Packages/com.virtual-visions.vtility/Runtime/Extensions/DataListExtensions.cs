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
    }
}