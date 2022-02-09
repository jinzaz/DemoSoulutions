using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MemoryCache.Cache
{
    public class SetSize : PageModel
    {
        private Microsoft.Extensions.Caching.Memory.MemoryCache _cache;
        public static readonly string MyKey = "_MyKey";

        public SetSize(MyMemoryCache memoryCache)
        {
            _cache = memoryCache.Cache;
        }

        [TempData]
        public string DateTime_Now { get; set; }

        public IActionResult OnGet()
        {
            if (!_cache.TryGetValue(MyKey, out string CacheEntry))
            {
                // key not in cache, so get data.
                CacheEntry = DateTime.Now.TimeOfDay.ToString();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // set cache entry size by extension method.
                    .SetSize(1)
                    // keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(TimeSpan.FromSeconds(3));
                // set cache entry size via property.
                // cacheEntryOptions.Size = 1;

                // save data in cache
                _cache.Set(MyKey, CacheEntry, cacheEntryOptions);
            }
            DateTime_Now = CacheEntry;

            return RedirectToPage("./Index");
        }
    }
}
