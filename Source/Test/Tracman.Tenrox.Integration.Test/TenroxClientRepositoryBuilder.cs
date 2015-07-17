namespace Tracman.Tenrox.Integration.Test
{
    public class TenroxClientRepositoryBuilder
    {
        public ITenroxClientRepository Build()
        {
            return new TenroxClientRepository(TenroxConstants.ClientsServiceUri, new TracmanCacheBuilder().Build());
        }
    }
}