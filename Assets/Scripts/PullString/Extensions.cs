//
// Assets/Scripts/PullString/Extensions.cs
//
// Some convenience extensions to C# classes
//
// Copyright (c) 2016 PullString, Inc.
//
// The following source code is licensed under the MIT license.
// See the LICENSE file, or https://opensource.org/licenses/MIT.
//

using System.Collections.Generic;

namespace PullString
{
    internal static class Extensions
    {
        /// <summary>
        /// Extension method for generic dictionaries to return a default value when a key is not found instead of
        /// throwing a KeyNotFoundException
        /// </summary>
        /// <returns>The value for the key or the default value for TValue</returns>
        public static object GetValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key)
        {
            if (dict.ContainsKey(key))
            {
                return dict[key];
            }
            return default(TValue);
        }

        /// <summary>
        /// Check if an object is any sort of C# numerif value.
        /// </summary>
        /// <returns>true if the object is deemed numeric.</returns>
        public static bool IsNumeric(this object val)
        {
            return val is sbyte ||
            val is byte ||
            val is short ||
            val is ushort ||
            val is int ||
            val is uint ||
            val is long ||
            val is ulong ||
            val is float ||
            val is double ||
            val is decimal;
        }
    }
}
