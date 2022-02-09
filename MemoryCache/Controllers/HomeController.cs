using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MemoryCache.Models;
using Microsoft.Extensions.Caching.Memory;
using MemoryCache.Cache;
using System.Threading;
using Microsoft.Extensions.Primitives;

namespace MemoryCache.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private Microsoft.Extensions.Caching.Memory.MemoryCache _cache;

        public HomeController(ILogger<HomeController> logger, MyMemoryCache memoryCache)
        {
            _logger = logger;
            _cache = memoryCache.Cache;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult CacheTryGetValueSet()
        {
            DateTime cacheEntry;
            // Look for cache key.
            if (!_cache.TryGetValue(CacheKeys.Entry, out cacheEntry))
            {
                //key not in cache, so get data.
                cacheEntry = DateTime.Now;

                //set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                // keep in cache for this time, reset time if acessed.
                .SetSlidingExpiration(TimeSpan.FromSeconds(3))
                .SetPriority(CacheItemPriority.NeverRemove);
                

                //save data in cache
                _cache.Set(CacheKeys.Entry,cacheEntry,cacheEntryOptions);
            }
            return View("Cache",cacheEntry);
        }

        public IActionResult CacheGetOrCreate()
        {
            var cacheEntry = _cache.GetOrCreate(CacheKeys.Entry, entry => {
                entry.SlidingExpiration = TimeSpan.FromSeconds(3);
                return DateTime.Now;
            });
            return View("Cache",cacheEntry);
        }

        public async Task<IActionResult> CacheGetOrCreateAsynchronous()
        {
            var cacheEntry = await
                _cache.GetOrCreateAsync(CacheKeys.Entry, entry => {
                    entry.SlidingExpiration = TimeSpan.FromSeconds(3);
                    return Task.FromResult(DateTime.Now);
                });
            return View("Cache",cacheEntry);
        }

        public IActionResult CacheGet()
        {
            var cacheEntry = _cache.Get<DateTime?>(CacheKeys.Entry);
            return View("Cache",cacheEntry);
        }

        public IActionResult CacheGetOrCreateAbs()
        {
            var cacheEntry = _cache.GetOrCreate(CacheKeys.Entry,entry => {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);
                return DateTime.Now;
            });
            return View("Cache",cacheEntry);
        }

        public IActionResult CacheGetOrCreateAbsSliding()
        {
            var cacheEntry = _cache.GetOrCreate(CacheKeys.Entry, entry => {
                entry.SetSlidingExpiration(TimeSpan.FromSeconds(20));
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(20);
                return DateTime.Now;
            });
            return View("Cache",cacheEntry);
        }

        public IActionResult CreateCallbackEntry()
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
            // pin to cache.
            .SetPriority(CacheItemPriority.NeverRemove)
            // add eviction callback
            .RegisterPostEvictionCallback(callback: EvictionCallback, state:this);

            _cache.Set(CacheKeys.CallbackEntry,DateTime.Now, cacheEntryOptions);
            return RedirectToAction("GetCallbackEntry");
        }

        public IActionResult GetCallbackEntry()
        {
            return View("Callback",new CallbackViewModel 
            {
                CachedTime = _cache.Get<DateTime?>(CacheKeys.CallbackEntry),
                Message = _cache.Get<string>(CacheKeys.CallbackMessage)
            });
        }

        public IActionResult RemoveCallbackEntry()
        {
            _cache.Remove(CacheKeys.CallbackEntry);
            return RedirectToAction("GetCallbackEntry");
        }

        private static void EvictionCallback(object key,object value,EvictionReason reason,object state)
        {
            var message = $"Entry was evicted. Reason:{reason}";
            ((HomeController)state)._cache.Set(CacheKeys.CallbackMessage,message);
        }

        public IActionResult CreateDependentEntries()
        {
            var cts = new CancellationTokenSource();
            _cache.Set(CacheKeys.DependentCTS, cts);

            using (var entry = _cache.CreateEntry(CacheKeys.Parent))
            {
                // expire this entry if the dependany entry expires.
                entry.Value = DateTime.Now;
                entry.RegisterPostEvictionCallback(DependentEvictionCallback, this);

                _cache.Set(CacheKeys.Child,
                    DateTime.Now,
                    new CancellationChangeToken(cts.Token));
            }
            return RedirectToAction("GetDependentEntries");
        }

        public IActionResult GetDependentEntries()
        {
            return View("Dependent",new DependentViewModel {
               ParentCachedTime = _cache.Get<DateTime?>(CacheKeys.Parent),
               ChildCachedTime = _cache.Get<DateTime?>(CacheKeys.Child),
               Message = _cache.Get<string>(CacheKeys.DependentMessage)
            });
        }

        public IActionResult RemoveChildEntry()
        {
            _cache.Get<CancellationTokenSource>(CacheKeys.DependentCTS).Cancel();
            return RedirectToAction("GetDependentEntries");
        }

        private static void DependentEvictionCallback(object key, object value, EvictionReason reason,object state)
        {
            var message = $"Parent entry was evicted. Reason:{reason}.";
            ((HomeController)state)._cache.Set(CacheKeys.DependentMessage, message);
        }

        public IActionResult CacheAutoExpiringTryGetValueSet()
        {
            DateTime cacheEntry;

            if (!_cache.TryGetValue(CacheKeys.Entry, out cacheEntry))
            {
                cacheEntry = DateTime.Now;

                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .AddExpirationToken(new CancellationChangeToken(cts.Token));

                _cache.Set(CacheKeys.Entry,cacheEntry,cacheEntryOptions);
                    
            }
            return View("Cache",cacheEntry);
        }

    }
}
