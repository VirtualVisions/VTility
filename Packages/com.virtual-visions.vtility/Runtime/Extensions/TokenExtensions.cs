using System;
using UnityEngine;
using VRC.SDK3.Data;

namespace VirtualVisions.VTility
{
    public static class TokenExtensions
    {
        public static T CastReference<T>(this DataToken token)
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
        
        /// <summary>
        /// Read the token's Integer or Reference value as a cast Enum type.
        /// </summary>
        public static T AsEnum<T>(this DataToken token) where T : Enum
        {
            switch (token.TokenType)
            {
                default:
                case TokenType.Int:
                    return (T)(object)token.Int;
                case TokenType.Byte:
                    return (T)(object)token.Byte;
                case TokenType.Reference:
                    return (T)token.Reference;
            }
        }
    }
}