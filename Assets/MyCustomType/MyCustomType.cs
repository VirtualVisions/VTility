using UnityEngine;
using VRC.SDK3.Data;

namespace Monocle
{
    public class MyCustomType<T> : DataList
    {
        public static MyCustomType<T> Create()
        {
            DataToken[] values = new DataToken[1];

            values[0] = new DataToken((T)default);
            
            DataList thing = new DataList(values);
            return (MyCustomType<T>)thing;
        }
    }

    public static class MyCustomTypeExtensions
    {
        public static T Get<T>(this MyCustomType<T> thing)
        {
            return (T)(object)thing[0];
        }
        
        public static void Set<T>(this MyCustomType<T> thing, T value)
        {
            thing[0] = new DataToken(value);
        }
        
        public static void Log<T>(this MyCustomType<T> thing)
        {
            Debug.Log(thing[0]);
        }
    }
}
