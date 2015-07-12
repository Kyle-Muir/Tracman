using System;
using System.Runtime.Caching;
using Tracman.Core.Domain;

namespace Tracman.Tenrox.Integration
{
    public class TenroxIdentityCache : ITenroxIdentityCache
    {
        private readonly ITenroxAuthenticator _authenticator;
        private readonly MemoryCache _cache;

        public TenroxIdentityCache(ITenroxAuthenticator authenticator, MemoryCache cache)
        {
            if (authenticator == null) throw new ArgumentNullException("authenticator");
            if (cache == null) throw new ArgumentNullException("cache");
            _authenticator = authenticator;
            _cache = cache;
        }

        public TenroxIdentity LoadIdentity(TenroxUser user)
        {
            TenroxIdentity tenroxIdentity = _authenticator.Authenticate(user);
            _cache.Add("TenroxIdentity", tenroxIdentity, new CacheItemPolicy());
            return tenroxIdentity;
        }

        public TenroxIdentity LoadExistingIdentity()
        {
            object cachedObject = _cache.Get("TenroxIdentity");
            if (cachedObject == null)
            {
                throw new ArgumentException("Unable to load tenrox identity from the cache, it has not been initialised.");
            }
            //TODO: validate existing token is still good before returning, e.g. someone has left the app open for days
            return (TenroxIdentity)cachedObject;
        }
    }
}