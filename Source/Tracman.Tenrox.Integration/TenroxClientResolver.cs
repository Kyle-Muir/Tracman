using System;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using Tracman.Core.Domain;
using Tracman.Tenrox.Integration.ClientsService;
using Client = Tracman.Tenrox.Integration.ClientsService.Client;

namespace Tracman.Tenrox.Integration
{
    public class TenroxClientResolver
    {
        private readonly ITenroxIdentityCache _tenroxIdentityCache;
        private readonly Uri _webServiceEndpoint;

        public TenroxClientResolver(ITenroxIdentityCache tenroxIdentityCache, Uri webServiceEndpoint)
        {
            if (tenroxIdentityCache == null) throw new ArgumentNullException("tenroxIdentityCache");
            if (webServiceEndpoint == null) throw new ArgumentNullException("webServiceEndpoint");
            _tenroxIdentityCache = tenroxIdentityCache;
            _webServiceEndpoint = webServiceEndpoint;
        }

        public string Resolve(int clientUniqueId)
        {
            TenroxIdentity identity = _tenroxIdentityCache.LoadExistingIdentity();
            BasicHttpBinding basicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            basicHttpBinding.MaxReceivedMessageSize = int.MaxValue;
            using (ClientsClient client = new ClientsClient(basicHttpBinding, new EndpointAddress(_webServiceEndpoint)))
            {
                var clients = client.QueryByAll(identity.UserToken);
                Client whatIAmLookingFor = clients.First(c => c.Name == "Intergen Business Solutions Pty (for Intergen NZ)");
                return whatIAmLookingFor.Name;
            }
        }
    }
}