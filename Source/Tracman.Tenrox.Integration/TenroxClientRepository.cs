using System;
using System.ServiceModel;
using Tracman.Core.Domain;
using Tracman.Tenrox.Integration.ClientsService;

namespace Tracman.Tenrox.Integration
{
    public class TenroxClientRepository : ITenroxClientRepository
    {
        private readonly Uri _webServiceEndpoint;
        private readonly ITracmanCache _cache;

        public TenroxClientRepository(Uri webServiceEndpoint, ITracmanCache cache)
        {
            if (webServiceEndpoint == null) throw new ArgumentNullException("webServiceEndpoint");
            if (cache == null) throw new ArgumentNullException("cache");
            _webServiceEndpoint = webServiceEndpoint;
            _cache = cache;
        }

        public Client[] LoadAllClients(TenroxIdentity identity)
        {
            if (identity == null) throw new ArgumentNullException("identity");
            return _cache.TryGetSet("Clients", () =>
            {
                BasicHttpBinding basicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
                basicHttpBinding.MaxReceivedMessageSize = int.MaxValue;
                using (
                    ClientsClient client = new ClientsClient(basicHttpBinding,
                        new EndpointAddress(_webServiceEndpoint))
                    )
                {
                    return client.QueryByAll(identity.UserToken);
                }
            });
        }
    }
}