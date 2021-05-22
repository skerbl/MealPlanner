using System;
using System.Collections.Generic;

namespace MPData
{
    public static class Utils
    {
        public static bool IsAscendingOrder<T>(this IEnumerable<T> seq) where T : IComparable<T>
        {
            var predecessor = default(T);
            var hasPredecessor = false;

            foreach (var x in seq)
            {
                if (hasPredecessor && predecessor.CompareTo(x) >= 0) return false;
                predecessor = x;
                hasPredecessor = true;
            }

            return true;
        }
    }
}
