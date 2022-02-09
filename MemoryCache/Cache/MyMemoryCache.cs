using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemoryCache.Cache
{
    public class MyMemoryCache
    {
        public Microsoft.Extensions.Caching.Memory.MemoryCache Cache { get; set; }
        public MyMemoryCache()
        {
            Cache = new Microsoft.Extensions.Caching.Memory.MemoryCache(new MemoryCacheOptions());
        }
    }
}
