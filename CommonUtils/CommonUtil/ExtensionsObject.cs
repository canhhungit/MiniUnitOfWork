using System.Collections.Generic;

namespace CommonUtil
{
    public static class ExtensionsObject
    {
        /// <summary>
        /// Transforms an object in a enumerable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static IEnumerable<T> ToEnumerable<T>(this T obj)
        {
            yield return obj;
        }
    }
}
