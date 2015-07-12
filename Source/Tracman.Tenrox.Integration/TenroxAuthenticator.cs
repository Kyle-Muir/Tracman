using System;
using System.ServiceModel;
using Tenrox.Shared.Utilities;
using Tracman.Core.Domain;
using Tracman.Tenrox.Integration.TenroxLogonService;

namespace Tracman.Tenrox.Integration
{
    public class TenroxAuthenticator : ITenroxAuthenticator
    {
        private readonly Uri _webserviceEndpoint;

        public TenroxAuthenticator(Uri webserviceEndpoint)
        {
            if (webserviceEndpoint == null) throw new ArgumentNullException("webserviceEndpoint");
            _webserviceEndpoint = webserviceEndpoint;
        }

        public TenroxIdentity Authenticate(TenroxUser tenroxUser)
        {
            UserToken token;
            using (LogonAsClient client = new LogonAsClient(new BasicHttpBinding(BasicHttpSecurityMode.Transport), new EndpointAddress(_webserviceEndpoint)))
            {
                token = client.Authenticate("Intergen", tenroxUser.UserName, tenroxUser.Password, string.Empty, true);
            }
            return new TenroxIdentity(token.AuthenticatedGuid, token.UniqueId, token);
        }
    }
}