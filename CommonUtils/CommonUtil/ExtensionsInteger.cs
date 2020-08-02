using System;
using System.Collections.Generic;

namespace CommonUtil
{
    public static class ExtensionsInteger
    {
        /// <summary>
        /// Gets the next odd integer.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static int GetNextOddNumber(this int number) => number % 2 == 0
                ? number + 1
                : number + 2;
        /// <summary>
        /// Provides an ienumerable of integers between two numbers (both boundaries included).
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static IEnumerable<int> GetIntegersTo(this int start, int end)
        {
            int increment = Math.Sign(end - start);
            yield return start;
            int current = start;
            while (current != end)
            {
                current += increment;
                yield return current;
            }
        }
    }
}
