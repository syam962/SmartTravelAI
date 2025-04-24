using Microsoft.Extensions.Caching.Memory;
using System;

namespace Travel.Web.Infrastructure
{
    public interface ICacheService
    {
        List<T> Get<T>(string cacheKey);
        void Set<T>(string cacheKey, T item);
        void AddToExistingCache<T>(string cacheKey, T item);
    }

    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public List<T> Get<T>(string cacheKey)
        {
            _memoryCache.TryGetValue(cacheKey, out List<T> cacheEntry);
            return cacheEntry;
        }

        public void Set<T>(string cacheKey, T item)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                SlidingExpiration = TimeSpan.FromMinutes(10),
            };

            _memoryCache.Set(cacheKey, item, cacheEntryOptions);
        }

        public void AddToExistingCache<T>(string cacheKey, T item)
        {
            if (_memoryCache.TryGetValue(cacheKey, out List<T> existingItems))
            {
                existingItems.Add(item);
                _memoryCache.Set(cacheKey, existingItems);
            }
            else
            {
                Set(cacheKey, new List<T> { item });
            }
        }
    }

}
