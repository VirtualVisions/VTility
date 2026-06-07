namespace VirtualVisions.VTility
{
    public enum SettableBool
    {
        Unset,
        False,
        True
    }
    
    public static class SettableBoolExtensions
    {
        public static bool IsSet(this SettableBool settable) => settable != SettableBool.Unset;
        public static bool IsFalse(this SettableBool settable) => settable == SettableBool.False;
        public static bool IsTrue(this SettableBool settable) => settable == SettableBool.True;
    }
    
}