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

        public static Vector2 AsVector2(this DataToken token) => token.CastReference<Vector2>();
        public static Vector2Int AsVector2Int(this DataToken token) => token.CastReference<Vector2Int>();
        public static Vector3 AsVector3(this DataToken token) => token.CastReference<Vector3>();
        public static Vector3Int AsVector3Int(this DataToken token) => token.CastReference<Vector3Int>();
        public static Quaternion AsQuaternion(this DataToken token) => token.CastReference<Quaternion>();
        public static Color AsColor(this DataToken token) => token.CastReference<Color>();
        public static GameObject AsGameObject(this DataToken token) => token.CastReference<GameObject>();
        public static Component AsComponent(this DataToken token) => token.CastReference<Component>();
        public static VRCUrl AsVRCUrl(this DataToken token) => token.CastReference<VRCUrl>();
        
        #endregion
    }
}