using VRC.SDK3.Data;

namespace VirtualVisions.VTility
{
    public static class DataDictionaryExtensions
    {
        public static void CopyEntry(
            this DataDictionary dict,
            DataDictionary from,
            TokenType type,
            DataToken key)
        {
            if (from.TryGetValue(key, type, out DataToken value)) dict[key] = value;
        }
        
        public static void CopyEntry(
            this DataDictionary dict,
            DataDictionary from,
            TokenType type,
            DataToken key,
            DataToken fallback)
        {
            if (from.TryGetValue(key, type, out DataToken value)) dict[key] = value;
            else dict[key] = fallback;
        }
    }
}