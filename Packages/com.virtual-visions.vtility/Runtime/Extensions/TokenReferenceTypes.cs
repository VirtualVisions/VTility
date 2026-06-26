using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDKBase;

namespace VirtualVisions.VTility
{
    /// <summary>
    /// Add more when you're feeling it, otherwise simply use CastReference.
    /// </summary>
    public static class TokenReferenceTypes
    {
        /// <summary>
        /// Append an object as a reference within a DataList.
        /// </summary>
        public static void _AddRef<T>(this DataList list, T value) => list.Add(new DataToken(value));


        #region Pre-cast reference types

        public static Vector2 _Vector2Ref(this DataToken token) => token.CastReference<Vector2>();
        public static Vector2Int _Vector2IntRef(this DataToken token) => token.CastReference<Vector2Int>();
        public static Vector3 _Vector3Ref(this DataToken token) => token.CastReference<Vector3>();
        public static Vector3Int _Vector3IntRef(this DataToken token) => token.CastReference<Vector3Int>();
        public static Quaternion _QuaternionRef(this DataToken token) => token.CastReference<Quaternion>();
        public static Color _ColorRef(this DataToken token) => token.CastReference<Color>();
        public static GameObject _GameObjectRef(this DataToken token) => token.CastReference<GameObject>();
        public static Component _ComponentRef(this DataToken token) => token.CastReference<Component>();
        public static VRCUrl _VRCUrlRef(this DataToken token) => token.CastReference<VRCUrl>();
        
        #endregion
    }
}