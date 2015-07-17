using System;
using System.Runtime.Caching;
using Tracman.Core;

namespace Tracman.Tenrox.Integration
{
    public class TracmanCache : ITracmanCache
    {
        private readonly MemoryCache _cache;

        public TracmanCache(MemoryCache cache)
        {
            if (cache == null) throw new ArgumentNullException("cache");
            _cache = cache;
        }

        public T Get<T>(string key)
        {
            object item = _cache.Get(key);
            if (item == null)
            {
                throw new ArgumentException("Unable to load object from the cache with key {0}, it has not been initialised.".FormatWith(key));
            }
            return (T)item;
        }

        private object Get(string key)
        {
            return _cache.Get(key);
        }

        private void Add<T>(string key, T item)
        {
            _cache.Add(key, item, new CacheItemPolicy());
        }

        public T TryGetSet<T>(string key, Func<T> actionToLoadData)
        {
            object result = Get(key);
            if (result == null)
            {
                T item = actionToLoadData.Invoke();
                Add(key, item);
                return item;
            }
            return (T)result;
        }
    }
}