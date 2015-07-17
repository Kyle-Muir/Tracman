using System.Runtime.Caching;

namespace Tracman.Tenrox.Integration.Test
{
    public class TracmanCacheBuilder
    {
        public ITracmanCache Build()
        {
            return new TracmanCache(new MemoryCache("TestCache"));
        }
    }
}