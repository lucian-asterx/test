using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MoviesAPI.Utilities
{
    public interface ICacheProvider<T>
    {
        IEnumerable<T> Get(string key);
        void Add(string key, IEnumerable<T> values);
        void ConfigureCacheLifetime(TimeSpan duration);
        void ForceClear();
    }

    public class CacheProvider<T> : ICacheProvider<T>
    {
        private readonly Dictionary<string, IEnumerable<T>> _internalCacheDictionary;
        private TimeSpan _cacheLifeDuration = new TimeSpan(0, 2, 0);
        private Timer _internalLifeTimer;

        public CacheProvider()
        {
            _internalCacheDictionary = new Dictionary<string, IEnumerable<T>>();
            _internalLifeTimer = new Timer(OnCacheExpire, null, 0, (int)_cacheLifeDuration.TotalMilliseconds);
        }

        public void ConfigureCacheLifetime(TimeSpan duration)
        {
            _cacheLifeDuration = duration;

            ForceClear();

            _internalLifeTimer = new Timer(OnCacheExpire, null, 0, (int)_cacheLifeDuration.TotalMilliseconds);
        }

        private void OnCacheExpire(object state)
        {
            ForceClear();
        }

        public void ForceClear()
        {
            _internalCacheDictionary.Clear();
        }

        public IEnumerable<T> Get(string key)
        {
            var result = Enumerable.Empty<T>();

            if (_internalCacheDictionary.ContainsKey(key))
            {
                result = _internalCacheDictionary[key];
            }
            return result;
        }

        public void Add(string key, IEnumerable<T> values)
        {
            if (!_internalCacheDictionary.ContainsKey(key))
            {
                _internalCacheDictionary.Add(key, values);
            }
        }
    }
}
