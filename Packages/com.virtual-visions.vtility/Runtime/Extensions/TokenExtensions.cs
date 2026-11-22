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
        /// Read the token's Integer, Byte, or Reference value as a cast Enum type.
        /// </summary>
        public static T AsEnum<T>(this DataToken token) where T : Enum
        {
            int enumIndex;
            switch (token.TokenType)
            {
                default:
                case TokenType.Int:
                    enumIndex = token.Int;
                    break;
                case TokenType.Byte:
                    enumIndex = token.Byte;
                    break;
                case TokenType.Double:
                    enumIndex = (int)token.Double;
                    break;
                case TokenType.Float:
                    enumIndex = (int)token.Float;
                    break;
                case TokenType.Reference:
                    return (T)token.Reference;
            }

            // Safe-cast unavailable in UdonSharp.
            // ReSharper disable once PossibleInvalidCastException
            return (T)(object)enumIndex;
        }
    }
}