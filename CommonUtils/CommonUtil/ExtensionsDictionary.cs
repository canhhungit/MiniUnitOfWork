using System.Collections.Generic;

namespace CommonUtil {
    public static class ExtensionsDictionary {
        /// <summary>
        /// Returns the stored value or the default type value in case it does not have the key.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TValue GetValueOrDefault<TKey, TValue> ( this Dictionary<TKey, TValue> dictionary, TKey key ) => dictionary.ContainsKey ( key )
                ? dictionary [ key ]
                : default;
    }
}
