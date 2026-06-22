using VRC.SDK3.Data;

namespace VirtualVisions.VTility
{
    public static class DataStack
    {
        public static DataToken _PushStack(this DataList stack, DataToken token)
        {
            stack.Add(token);
            return token;
        }

        public static DataToken _PopStack(this DataList stack)
        {
            DataToken result = new DataToken(DataError.IndexOutOfRange);
            int stackEnd = stack.Count - 1;
            if (stackEnd >= 0)
            {
                result = stack[stackEnd];
                stack.RemoveAt(stackEnd);
            }

            return result;
        }

        public static bool _TryPopStack(this DataList stack, out DataToken result)
        {
            result = new DataToken(DataError.IndexOutOfRange);
            int stackEnd = stack.Count - 1;
            if (stackEnd >= 0)
            {
                result = stack[stackEnd];
                stack.RemoveAt(stackEnd);
                return true;
            }

            return false;
        }

        public static DataToken _PeekStack(this DataList stack)
        {
            DataToken result = new DataToken(DataError.IndexOutOfRange);
            int stackEnd = stack.Count - 1;
            if (stackEnd >= 0) result = stack[stackEnd];
            return result;
        }
    }
}