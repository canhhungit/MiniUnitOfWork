using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtil.Cache
{
    public class CacheRedisService : ICacheService
    {
        private volatile ConnectionMultiplexer _connection;
        private readonly string _connectionString;
        private readonly object _lock = new object();

        public CacheRedisService(string connectionString)
        {
            _connectionString = connectionString;
        }


        private ConnectionMultiplexer GetConnection()
        {
            if (_connection != null && _connection.IsConnected)
            {
                return _connection;
            }

            lock (_lock)
            {
                if (_connection != null && _connection.IsConnected)
                {
                    return _connection;
                }

                if (_connection != null)
                {
                    //Connection disconnected. Disposing connection...
                    _connection.Dispose();
                }

                //Creating new instance of Redis Connection
                _connection = ConnectionMultiplexer.Connect(_connectionString);
            }

            return _connection;
        }
        #region Public Methods


        protected virtual byte[] Serialize(object item)
        {
            var jsonString = JsonConvert.SerializeObject(item, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                NullValueHandling = NullValueHandling.Ignore,
            });
            return Encoding.UTF8.GetBytes(jsonString);
        }
        protected virtual T Deserialize<T>(byte[] serializedObject)
        {
            if (serializedObject == null)
            {
                return default(T);
            }

            var jsonString = Encoding.UTF8.GetString(serializedObject);
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        public IDatabase Database(int? db = null)
        {
            return GetConnection().GetDatabase(db ?? -1); //_settings.DefaultDb);
        }

        public IServer Server(EndPoint endPoint)
        {
            return GetConnection().GetServer(endPoint);
        }

        public EndPoint[] GetEndpoints()
        {
            return GetConnection().GetEndPoints();
        }

        public long Count
        {
            get
            {
                long result = 0;
                foreach (var ep in GetEndpoints())
                {
                    var server = Server(ep);
                    result += server.Keys().Count();
                }
                return result;
            }
        }

        /// <summary>
        /// Inserts a cache entry into the cache by using a key, a value and no expiration.
        /// </summary>
        /// <typeparam name="T">The type of the cache entry value.</typeparam>
        /// <param name="key">A unique identifier for the cache entry to insert.</param>
        /// <param name="value">The data for the cache entry.</param>
        /// <param name="afterItemRemoved">The action to perform after the cache entry has been removed.</param>
        /// <param name="beforeItemRemoved">The action to perform before the cache entry has been removed.</param>
        public void AddOrUpdate<T>(
            string key,
            T value,
            Action<string, T> afterItemRemoved = null,
            Action<string, T> beforeItemRemoved = null) where T : class
        {
            AddOrUpdate(key, value, null, null, afterItemRemoved, beforeItemRemoved);
        }

        /// <summary>
        /// Inserts a cache entry into the cache by using a key, a value and absolute expiration.
        /// </summary>
        /// <typeparam name="T">The type of the cache entry value.</typeparam>
        /// <param name="key">A unique identifier for the cache entry to insert.</param>
        /// <param name="value">The data for the cache entry.</param>
        /// <param name="absoluteExpiration">The absolute expiration time, after which the cache entry will expire.</param>
        /// <param name="afterItemRemoved">The action to perform after the cache entry has been removed.</param>
        /// <param name="beforeItemRemoved">The action to perform before the cache entry has been removed.</param>
        public void AddOrUpdate<T>(
            string key,
            T value,
            DateTimeOffset absoluteExpiration,
            Action<string, T> afterItemRemoved = null,
            Action<string, T> beforeItemRemoved = null) where T : class
        {
            AddOrUpdate(key, value, absoluteExpiration, null, afterItemRemoved, beforeItemRemoved);
        }

        /// <summary>
        /// Inserts a cache entry into the cache by using a key, a value and sliding expiration.
        /// </summary>
        /// <typeparam name="T">The type of the cache entry value.</typeparam>
        /// <param name="key">A unique identifier for the cache entry to insert.</param>
        /// <param name="value">The data for the cache entry.</param>
        /// <param name="slidingExpiration">The sliding expiration time, after which the cache entry will expire.</param>
        /// <param name="afterItemRemoved">The action to perform after the cache entry has been removed.</param>
        /// <param name="beforeItemRemoved">The action to perform before the cache entry has been removed.</param>
        public void AddOrUpdate<T>(
            string key,
            T value,
            TimeSpan slidingExpiration,
            Action<string, T> afterItemRemoved = null,
            Action<string, T> beforeItemRemoved = null) where T : class
        {
            AddOrUpdate(key, value, null, slidingExpiration, afterItemRemoved, beforeItemRemoved);
        }

        /// <summary>
        /// Clears all cache entry items from the cache.
        /// </summary>
        public void Clear()
        {
            foreach (var ep in GetEndpoints())
            {
                var server = Server(ep);
                //we can use the code below (commented)
                //but it requires administration permission - ",allowAdmin=true"
                //server.FlushDatabase();

                //that's why we simply interate through all elements now
                var keys = server.Keys();
                foreach (var key in keys)
                {
                    Database().KeyDelete(key);
                }
            }
        }

        /// <summary>
        /// Determines whether a cache entry exists in the cache with the specified key.
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry.</param>
        /// <returns><c>true</c> if a cache entry exists, otherwise <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">key is null.</exception>
        public bool Contains(string key)
        {
            return Database().KeyExists(key);
        }

        /// <summary>
        /// Determines whether a cache entry exists in the cache with the Pattern key.
        /// </summary>
        /// <param name="pattern">pattern</param>
        /// <returns><c>true</c> if a cache pattern exists, otherwise <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">key is null.</exception>
        public bool ContainsByPattern(string pattern)
        {
            foreach (var ep in GetEndpoints())
            {
                var server = Server(ep);
                var keys = server.Keys(pattern: "*" + pattern + "*");
                if (keys.Count() > 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets an entry from the cache with the specified key.
        /// </summary>
        /// <typeparam name="T">The type of the cache entry to get.</typeparam>
        /// <param name="key">A unique identifier for the cache entry to get.</param>
        /// <returns>A reference to the cache entry that is identified by key, if the entry exists; otherwise, <c>null</c>.</returns>
        /// <exception cref="System.ArgumentNullException">key is null.</exception>
        public T Get<T>(string key) where T : class
        {
            if (Contains(key))
            {
                var rValue = Database().StringGet(key);
                if (!rValue.HasValue)
                {
                    return default(T);
                }

                return Deserialize<T>(rValue);
            }
            else
            {
                return default(T);
            };
        }

        /// <summary>
        /// Gets an entry from the cache with the specified key, or if the entry does not exist, inserts a
        /// cache entry into the cache by using a key, a value and no expiration and returns it.
        /// </summary>
        /// <typeparam name="T">The type of the cache entry value.</typeparam>
        /// <param name="key">A unique identifier for the cache entry to insert.</param>
        /// <param name="value">The data for the cache entry.</param>
        /// <param name="afterItemRemoved">The action to perform after the cache entry has been removed.</param>
        /// <param name="beforeItemRemoved">The action to perform before the cache entry has been removed.</param>
        /// <returns>A reference to the cache entry that is identified by key.</returns>
        public T GetOrAdd<T>(
            string key,
            Func<T> value,
            Action<string, T> afterItemRemoved = null,
            Action<string, T> beforeItemRemoved = null) where T : class
        {
            return GetOrAdd(key, value, null, null, afterItemRemoved, beforeItemRemoved);
        }

        /// <summary>
        /// Gets an entry from the cache with the specified key, or if the entry does not exist, inserts a
        /// cache entry into the cache by using a key, a value and absolute expiration and returns it.
        /// </summary>
        /// <typeparam name="T">The type of the cache entry value.</typeparam>
        /// <param name="key">A unique identifier for the cache entry to insert.</param>
        /// <param name="value">The data for the cache entry.</param>
        /// <param name="absoluteExpiration">The absolute expiration time, after which the cache entry will expire.</param>
        /// <param name="afterItemRemoved">The action to perform after the cache entry has been removed.</param>
        /// <param name="beforeItemRemoved">The action to perform before the cache entry has been removed.</param>
        /// <returns>A reference to the cache entry that is identified by key.</returns>
        public T GetOrAdd<T>(
            string key,
            Func<T> value,
            DateTimeOffset absoluteExpiration,
            Action<string, T> afterItemRemoved = null,
            Action<string, T> beforeItemRemoved = null) where T : class
        {
            return GetOrAdd(key, value, absoluteExpiration, null, afterItemRemoved, beforeItemRemoved);
        }

        /// <summary>
        /// Gets an entry from the cache with the specified key, or if the entry does not exist, inserts a
        /// cache entry into the cache by using a key, a value and sliding expiration and returns it.
        /// </summary>
        /// <typeparam name="T">The type of the cache entry value.</typeparam>
        /// <param name="key">A unique identifier for the cache entry to insert.</param>
        /// <param name="value">The data for the cache entry.</param>
        /// <param name="slidingExpiration">The sliding expiration time, after which the cache entry will expire.</param>
        /// <param name="afterItemRemoved">The action to perform after the cache entry has been removed.</param>
        /// <param name="beforeItemRemoved">The action to perform before the cache entry has been removed.</param>
        /// <returns>A reference to the cache entry that is identified by key.</returns>
        public T GetOrAdd<T>(
            string key,
            Func<T> value,
            TimeSpan slidingExpiration,
            Action<string, T> afterItemRemoved = null,
            Action<string, T> beforeItemRemoved = null) where T : class
        {
            return GetOrAdd(key, value, null, slidingExpiration, afterItemRemoved, beforeItemRemoved);
        }

        /// <summary>
        /// Gets an entry from the cache with the specified key, or if the entry does not exist, inserts a
        /// cache entry into the cache by using a key, a value and no expiration and returns it.
        /// </summary>
        /// <typeparam name="T">The type of the cache entry value.</typeparam>
        /// <param name="key">A unique identifier for the cache entry to insert.</param>
        /// <param name="value">A function that asynchronously gets the data for the cache entry.</param>
        /// <param name="afterItemRemoved">The action to perform after the cache entry has been removed.</param>
        /// <param name="beforeItemRemoved">The action to perform before the cache entry has been removed.</param>
        /// <returns>A reference to the cache entry that is identified by key.</returns>
        public Task<T> GetOrAddAsync<T>(
            string key,
            Func<Task<T>> value,
            Action<string, T> afterItemRemoved = null,
            Action<string, T> beforeItemRemoved = null) where T : class
        {
            return GetOrAddAsync(key, value, null, null, afterItemRemoved, beforeItemRemoved);
        }

        /// <summary>
        /// Gets an entry from the cache with the specified key, or if the entry does not exist, inserts a
        /// cache entry into the cache by using a key, a value and absolute expiration and returns it.
        /// </summary>
        /// <typeparam name="T">The type of the cache entry value.</typeparam>
        /// <param name="key">A unique identifier for the cache entry to insert.</param>
        /// <param name="value">A function that asynchronously gets the data for the cache entry.</param>
        /// <param name="absoluteExpiration">The absolute expiration time, after which the cache entry will expire.</param>
        /// <param name="afterItemRemoved">The action to perform after the cache entry has been removed.</param>
        /// <param name="beforeItemRemoved">The action to perform before the cache entry has been removed.</param>
        /// <returns>A reference to the cache entry that is identified by key.</returns>
        public Task<T> GetOrAddAsync<T>(
            string key,
            Func<Task<T>> value,
            DateTimeOffset absoluteExpiration,
            Action<string, T> afterItemRemoved = null,
            Action<string, T> beforeItemRemoved = null) where T : class
        {
            return GetOrAddAsync(key, value, absoluteExpiration, null, afterItemRemoved, beforeItemRemoved);
        }

        /// <summary>
        /// Gets an entry from the cache with the specified key, or if the entry does not exist, inserts a
        /// cache entry into the cache by using a key, a value and sliding expiration and returns it.
        /// </summary>
        /// <typeparam name="T">The type of the cache entry value.</typeparam>
        /// <param name="key">A unique identifier for the cache entry to insert.</param>
        /// <param name="value">A function that asynchronously gets the data for the cache entry.</param>
        /// <param name="slidingExpiration">The sliding expiration time, after which the cache entry will expire.</param>
        /// <param name="afterItemRemoved">The action to perform after the cache entry has been removed.</param>
        /// <param name="beforeItemRemoved">The action to perform before the cache entry has been removed.</param>
        /// <returns>A reference to the cache entry that is identified by key.</returns>
        public Task<T> GetOrAddAsync<T>(
            string key,
            Func<Task<T>> value,
            TimeSpan slidingExpiration,
            Action<string, T> afterItemRemoved = null,
            Action<string, T> beforeItemRemoved = null) where T : class
        {
            return GetOrAddAsync(key, value, null, slidingExpiration, afterItemRemoved, beforeItemRemoved);
        }

        /// <summary>
        /// Removes a cache entry from the cache with the specified key.
        /// </summary>
        /// <param name="key">A unique identifier for the cache entry to remove.</param>
        /// <exception cref="System.ArgumentNullException">key is null.</exception>
        public void Remove(string key)
        {
            if (Contains(key))
            {
                Database().KeyDelete(key);
            }
        }
        /// <summary>
        /// Removes items by pattern
        /// </summary>
        /// <param name="pattern">pattern</param>
        public void RemoveByPattern(string pattern)
        {
            foreach (var ep in GetEndpoints())
            {
                var server = Server(ep);
                var keys = server.Keys(pattern: "*" + pattern + "*");
                foreach (var key in keys)
                {
                    Database().KeyDelete(key);
                }
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Gets an entry from the cache with the specified key, or if the entry does not exist, inserts a
        /// cache entry into the cache by using a key, a value, a type of expiration and returns it.
        /// </summary>
        /// <typeparam name="T">The type of the cache entry value.</typeparam>
        /// <param name="key">A unique identifier for the cache entry to insert.</param>
        /// <param name="getValue">A function that gets the data for the cache entry.</param>
        /// <param name="absoluteExpiration">The absolute expiration time, after which the cache entry will expire.</param>
        /// <param name="slidingExpiration">The sliding expiration time, after which the cache entry will expire.</param>
        /// <param name="afterItemRemoved">The action to perform after the cache entry has been removed.</param>
        /// <param name="beforeItemRemoved">The action to perform before the cache entry has been removed.</param>
        /// <returns>A reference to the cache entry that is identified by key.</returns>
        private T GetOrAdd<T>(
            string key,
            Func<T> getValue,
            DateTimeOffset? absoluteExpiration,
            TimeSpan? slidingExpiration,
            Action<string, T> afterItemRemoved,
            Action<string, T> beforeItemRemoved) where T : class
        {
            T value = Get<T>(key);

            if (value == null)
            {
                value = getValue();
                AddOrUpdate(
                    key,
                    value,
                    absoluteExpiration,
                    slidingExpiration,
                    afterItemRemoved,
                    beforeItemRemoved);
            }

            return value;
        }

        /// <summary>
        /// Gets an entry from the cache with the specified key, or if the entry does not exist, inserts a
        /// cache entry into the cache by using a key, a value, a type of expiration and returns it.
        /// </summary>
        /// <typeparam name="T">The type of the cache entry value.</typeparam>
        /// <param name="key">A unique identifier for the cache entry to insert.</param>
        /// <param name="getValue">A function that asynchronously gets the data for the cache entry.</param>
        /// <param name="absoluteExpiration">The absolute expiration time, after which the cache entry will expire.</param>
        /// <param name="slidingExpiration">The sliding expiration time, after which the cache entry will expire.</param>
        /// <param name="afterItemRemoved">The action to perform after the cache entry has been removed.</param>
        /// <param name="beforeItemRemoved">The action to perform before the cache entry has been removed.</param>
        /// <returns>A reference to the cache entry that is identified by key.</returns>
        private async Task<T> GetOrAddAsync<T>(
            string key,
            Func<Task<T>> getValue,
            DateTimeOffset? absoluteExpiration,
            TimeSpan? slidingExpiration,
            Action<string, T> afterItemRemoved,
            Action<string, T> beforeItemRemoved) where T : class
        {
            T value = Get<T>(key);

            if (value == null)
            {
                value = await getValue();
                AddOrUpdate(
                    key,
                    value,
                    absoluteExpiration,
                    slidingExpiration,
                    afterItemRemoved,
                    beforeItemRemoved);
            }

            return value;
        }

        /// <summary>
        /// Inserts a cache entry into the cache by using a key, a value and an expiration type.
        /// </summary>
        /// <typeparam name="T">The type of the cache entry value.</typeparam>
        /// <param name="key">A unique identifier for the cache entry to insert.</param>
        /// <param name="value">The data for the cache entry.</param>
        /// <param name="absoluteExpiration">The absolute expiration time, after which the cache entry will expire.</param>
        /// <param name="slidingExpiration">The sliding expiration time, after which the cache entry will expire.</param>
        /// <param name="afterItemRemoved">The action to perform after the cache entry has been removed.</param>
        /// <param name="beforeItemRemoved">The action to perform before the cache entry has been removed.</param>
        private void AddOrUpdate<T>(
            string key,
            T value,
            DateTimeOffset? absoluteExpiration,
            TimeSpan? slidingExpiration,
            Action<string, T> afterItemRemoved,
            Action<string, T> beforeItemRemoved) where T : class
        {

            if (value == null)
            {
                return;
            }

            var entryBytes = Serialize(value);
            if (slidingExpiration.HasValue)
            {
                Database().StringSet(key, entryBytes, slidingExpiration.Value);
            }
            else
            {
                Database().StringSet(key, entryBytes);
            }

        }

        #endregion Private Methods

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
            }
        }
    }
}
