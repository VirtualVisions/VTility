using System;
using UnityEngine;
using VRC.SDK3.Data;

namespace VirtualVisions.VTility
{
    public static class DataDictionaryExtensions
    {
        public static DataToken CopyEntry(
            this DataDictionary dict,
            DataDictionary from,
            TokenType type,
            DataToken key)
        {
            if (from.TryGetValue(key, type, out DataToken value))
            {
                dict[key] = value;
                return value;
            }

            return new DataToken(DataError.KeyDoesNotExist);
        }

        public static DataToken CopyEntry(
            this DataDictionary dict,
            DataDictionary from,
            TokenType type,
            DataToken key,
            DataToken fallback)
        {
            DataToken result;
            
            if (from.TryGetValue(key, type, out DataToken value)) result = value;
            else result = fallback;
            dict[key] = result;

            return result;
        }

        /// <summary>
        /// If a number of any type is found, it is either copied or cast to the correct type.
        /// </summary>
        public static DataToken CopyNumber(
            this DataDictionary dict,
            DataDictionary from,
            TokenType type,
            DataToken key,
            DataToken fallback)
        {
            DataToken result;

            if (from.TryGetValue(key, out DataToken value))
            {
                if (value.TokenType == type) result = value;
                else
                {
                    switch (type)
                    {
                        case TokenType.SByte:
                            result = (sbyte)value;
                            break;
                        case TokenType.Byte:
                            result = (byte)value;
                            break;
                        case TokenType.Short:
                            result = (short)value;
                            break;
                        case TokenType.UShort:
                            result = (ushort)value;
                            break;
                        case TokenType.Int:
                            result = (int)value;
                            break;
                        case TokenType.UInt:
                            result = (uint)value;
                            break;
                        case TokenType.Long:
                            result = (long)value;
                            break;
                        case TokenType.ULong:
                            result = (ulong)value;
                            break;
                        case TokenType.Float:
                            result = (float)value;
                            break;
                        case TokenType.Double:
                            result = (double)value;
                            break;
                        default:
                            Debug.LogWarning($"Target type {type} is not a number, entry {key} was not copied.");
                            result = new DataToken(DataError.TypeUnsupported);
                            break;
                    }
                }
            }
            else
            {
                result = fallback;
            }

            dict[key] = result;

            return result;
        }
    }
}