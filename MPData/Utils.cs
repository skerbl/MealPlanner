using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

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

        public static bool IsValidFilename(string testName)
        {
            string strTheseAreInvalidFileNameChars = new string(Path.GetInvalidFileNameChars());
            Regex regInvalidFileName = new Regex("[" + Regex.Escape(strTheseAreInvalidFileNameChars) + "]");

            if (regInvalidFileName.IsMatch(testName))
            {
                return false;
            }

            return true;
        }
    }
}
