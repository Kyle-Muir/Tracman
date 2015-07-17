namespace Tracman.Tenrox.Integration.Test
{
    public class TenroxClientResolverBuilder
    {
        private readonly ITenroxIdentityCache _tenroxIdentityCache;

        public TenroxClientResolverBuilder(ITenroxIdentityCache tenroxIdentityCache)
        {
            _tenroxIdentityCache = tenroxIdentityCache;
        }

        public TenroxClientResolver Build()
        {
            return new TenroxClientResolver(_tenroxIdentityCache, new TenroxClientRepositoryBuilder().Build());
        }
    }
}