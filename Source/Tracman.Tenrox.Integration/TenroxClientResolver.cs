using System;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using Tracman.Core.Domain;
using Tracman.Tenrox.Integration.AssignmentsService;

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
            using (AssignmentsClient client = new AssignmentsClient(new BasicHttpBinding(BasicHttpSecurityMode.Transport), new EndpointAddress(_webServiceEndpoint)))
            {
                Assignment[] assignments = client.QueryById(identity.UserToken, clientUniqueId.ToString(CultureInfo.CurrentCulture));
                return assignments.First().Name;
            }
        }
    }
}