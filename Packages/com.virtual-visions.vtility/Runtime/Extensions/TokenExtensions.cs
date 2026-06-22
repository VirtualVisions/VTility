using UnityEngine;
using VRC.SDK3.Data;

namespace VirtualVisions.VTility
{
    public static class TokenExtensions
    {
        public static T _CastReference<T>(this DataToken token)
        {
            if (token.TokenType != TokenType.Reference)
            {
                Debug.LogWarning($"Token is not a reference value: {token}");
                return default;
            }

            T reference = (T)token.Reference;
            if (reference == null)
            {
                Debug.LogWarning($"Type of token is not the target type: {token}");
                return default;
            }

            return reference;
        }
    }
}