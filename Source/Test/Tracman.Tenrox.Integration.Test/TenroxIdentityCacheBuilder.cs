namespace Tracman.Tenrox.Integration.Test
{
    public class TenroxIdentityCacheBuilder
    {
        public ITenroxIdentityCache Build()
        {
            return new TenroxIdentityCache(new TenroxAuthenticatorBuilder().Build(), new TracmanCacheBuilder().Build());
        }
    }
}