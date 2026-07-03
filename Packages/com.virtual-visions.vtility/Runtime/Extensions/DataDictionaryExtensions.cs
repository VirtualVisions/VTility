using System;
using UnityEngine;
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
            if (from
                .TryGetValue(key, type, out DataToken value)) dict[key] = value;
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

        /// <summary>
        /// If a number of any type is found, it is either copied or cast to the correct type.
        /// </summary>
        public static void CopyNumber(
            this DataDictionary dict,
            DataDictionary from,
            TokenType type,
            DataToken key,
            DataToken fallback)
        {
            if (from.TryGetValue(key, out DataToken value))
            {
                if (value.TokenType == type) dict[key] = value;
                else
                {
                    switch (type)
                    {
                        case TokenType.SByte:
                            dict[key] = (sbyte)value;
                            break;
                        case TokenType.Byte:
                            dict[key] = (byte)value;
                            break;
                        case TokenType.Short:
                            dict[key] = (short)value;
                            break;
                        case TokenType.UShort:
                            dict[key] = (ushort)value;
                            break;
                        case TokenType.Int:
                            dict[key] = (int)value;
                            break;
                        case TokenType.UInt:
                            dict[key] = (uint)value;
                            break;
                        case TokenType.Long:
                            dict[key] = (long)value;
                            break;
                        case TokenType.ULong:
                            dict[key] = (ulong)value;
                            break;
                        case TokenType.Float:
                            dict[key] = (float)value;
                            break;
                        case TokenType.Double:
                            dict[key] = (double)value;
                            break;
                        default:
                            Debug.LogWarning($"Target type {type} is not a number, entry {key} was not copied.");
                            break;
                    }
                }
            }
            else dict[key] = fallback;
        }
    }
}