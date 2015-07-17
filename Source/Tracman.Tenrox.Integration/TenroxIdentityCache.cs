using System;
using Tracman.Core.Domain;

namespace Tracman.Tenrox.Integration
{
    public class TenroxIdentityCache : ITenroxIdentityCache
    {
        private readonly ITenroxAuthenticator _authenticator;
        private readonly ITracmanCache _cache;

        public TenroxIdentityCache(ITenroxAuthenticator authenticator, ITracmanCache cache)
        {
            if (authenticator == null) throw new ArgumentNullException("authenticator");
            if (cache == null) throw new ArgumentNullException("cache");
            _authenticator = authenticator;
            _cache = cache;
        }

        public TenroxIdentity LoadIdentity(TenroxUser user)
        {
            return _cache.TryGetSet("TenroxIdentity", () => _authenticator.Authenticate(user));
        }

        public TenroxIdentity LoadExistingIdentity()
        {
            //TODO: validate existing token is still good before returning, e.g. someone has left the app open for days
            return _cache.Get<TenroxIdentity>("TenroxIdentity");
        }
    }
}