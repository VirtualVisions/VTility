using System;
using UnityEngine;

namespace VirtualVisions.VTility
{
    public static class StringExtensions
    {

        /// <summary>
        /// Use FuzzyCompare to determine if a string contains a search key.
        /// </summary>
        public static bool FuzzyContains(this string target, string search, float maxDeltaPercentage = 20)
        {
            if (string.IsNullOrEmpty(target)) return false;
            if (string.IsNullOrEmpty(search)) return true;

            target = target.ToLowerInvariant();
            search = search.ToLowerInvariant();
            
            if (target.IndexOf(search, StringComparison.OrdinalIgnoreCase) != -1) return true;

            int targetLength = target.Length;
            int searchLength = search.Length;

            if (searchLength > targetLength) return target.FuzzyCompare(search, maxDeltaPercentage);

            float percentage = Mathf.Clamp01(maxDeltaPercentage / 100);
            int allowance = Mathf.FloorToInt(searchLength * percentage);
            
            // Scoot the comparison string along to check for contained content comparison.
            int comparisonWindow = targetLength - searchLength;
            for (int i = 0; i <= comparisonWindow; i++)
            {
                string window = target.Substring(i, searchLength);
                if (LevenshteinDistance(window, search) <= allowance) return true;
            }

            return false;
        }

        /// <summary>
        /// Directly compare if a string is within a percentage difference of a target string using a levenshtein distance comparison.
        /// </summary>
        public static bool FuzzyCompare(this string a, string b, float maxDeltaPercentage = 20)
        {
            if (a.Equals(b)) return true;
            
            a = a.ToLowerInvariant();
            b = b.ToLowerInvariant();

            int difference = LevenshteinDistance(a, b);

            int aLength = string.IsNullOrEmpty(a) ? 0 : a.Length;
            int bLength = string.IsNullOrEmpty(b) ? 0 : b.Length;
            int maxLength = Mathf.Max(aLength, bLength);
            
            float percentage = Mathf.Clamp01(maxDeltaPercentage / 100);

            int differenceAllowance = Mathf.FloorToInt(maxLength * percentage);

            return difference <= differenceAllowance;
        }

        /// <summary>
        /// Calculate the difference between two compared strings.
        /// Based largely on this implementation on StackOverflow: https://stackoverflow.com/a/9453762
        /// </summary>
        public static int LevenshteinDistance(string a, string b)
        {
            bool aValid = !string.IsNullOrEmpty(a);
            bool bValid = !string.IsNullOrEmpty(b);
            
            if (!aValid && !bValid) return 0;

            if (!aValid) return b.Length;
            if (!bValid) return a.Length;

            int aLength = a.Length;
            int bLength = b.Length;


            int[] previousRow = new int[bLength + 1];
            int[] currentRow = new int[bLength + 1];

            for (int i = 0; i <= bLength; i++) previousRow[i] = i;

            for (int x = 1; x <= aLength; x++)
            {
                currentRow[0] = x;

                for (int y = 1; y <= bLength; y++)
                {
                    int comparisonCost = b[y - 1] == a[x - 1] ? 0 : 1;
                    currentRow[y] = Math.Min(
                        Math.Min(previousRow[y] + 1, currentRow[y - 1] + 1),
                        previousRow[y - 1] + comparisonCost);
                }

                // Tuple deconstruction doesn't exist in Udon, so we have to toss it around a bit.
                // ReSharper disable once SwapViaDeconstruction
                int[] temp = previousRow;
                previousRow = currentRow;
                currentRow = temp;
            }

            return previousRow[bLength];
        }
    }
}