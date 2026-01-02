using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Application.Services.Cache
{
    public class CacheManagerService : ICacheManagerService
    {
        private readonly IMemoryCache _cache;
        private static CancellationTokenSource _resetCacheToken = new CancellationTokenSource();

        public CacheManagerService(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        public virtual bool IsSet(string key)
        {
            return _cache.TryGetValue(key, out object _);
        }

        public virtual async Task<T> GetOrCreateAsync<T>(string key, Func<T> func, int expireIn = 60)
        {
            return await _cache.GetOrCreateAsync(key, entry =>
            {
                GetFixedEntry<T>(expireIn, entry);
                return Task.FromResult(func());
            });
        }

        public virtual async Task<T> GetOrCreateSlidingAsync<T>(string key, Func<T> func, int expiresIn = 60)
        {
            return await _cache.GetOrCreateAsync(key, entry =>
            {
                GetSlidingEntry<T>(expiresIn, entry);
                return Task.FromResult(func());
            });
        }


        public virtual T GetOrCreate<T>(string key, Func<T> func, int expireIn = 60)
        {
            return _cache.GetOrCreate<T>(key, entry =>
            {
                GetFixedEntry<T>(expireIn, entry);
                return func();
            });
        }

        public virtual T GetOrCreateSliding<T>(string key, Func<T> func, int expiresIn = 60)
        {
            return _cache.GetOrCreate<T>(key, entry =>
            {
                GetSlidingEntry<T>(expiresIn, entry);
                return func();
            });
        }

        public virtual void ClearAll()
        {
            if (_resetCacheToken != null && !_resetCacheToken.IsCancellationRequested && _resetCacheToken.Token.CanBeCanceled)
            {
                _resetCacheToken.Cancel();
                _resetCacheToken.Dispose();
            }
            _resetCacheToken = new CancellationTokenSource();
        }

        public virtual void Remove(string key)
        {
            _cache.Remove(key);
        }

        public virtual async Task<T> Set<T>(string key, T obj, int expireIn = 60)
        {
            return await _cache.GetOrCreateAsync(key, entry =>
            {
                GetSlidingEntry<T>(expireIn, entry);
                return Task.FromResult(obj);
            });
        }

        public T Get<T>(string key)
        {
            return (T)_cache.Get(key);
        }

        public virtual T Set<T>(T obj, string key, int time)
        {
            return _cache.GetOrCreate(key, entry =>
            {
                GetFixedEntry<T>(time, entry);
                return obj;
            });
        }
        public virtual T SetSliding<T>(T obj, string key, int time)
        {
            return _cache.GetOrCreate(key, entry =>
            {
                GetSlidingEntry<T>(time, entry);
                return obj;
            });
        }

        public virtual List<string> GetAllKeys()
        {
            var field = typeof(MemoryCache).GetField("_coherentState", BindingFlags.NonPublic | BindingFlags.Instance);
            var property = field.FieldType.GetProperty("StringEntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance);

            if (property == null)
                property = field.FieldType.GetProperty("EntriesCollection", BindingFlags.Instance | BindingFlags.NonPublic);


            var value = field.GetValue(_cache);
            var dict = property.GetValue(value) as IDictionary;

            var keys = new List<string>();

            foreach (var item in dict.Keys)
            {
                keys.Add(item.ToString());
            }

            return keys;
        }

        private static void GetSlidingEntry<T>(int expireIn, ICacheEntry entry)
        {
            entry.SlidingExpiration = TimeSpan.FromSeconds(expireIn);
            entry.AddExpirationToken(new CancellationChangeToken(_resetCacheToken.Token));
        }

        private static void GetFixedEntry<T>(int expireIn, ICacheEntry entry)
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(expireIn);
            entry.AddExpirationToken(new CancellationChangeToken(_resetCacheToken.Token));
        }
    }
}
