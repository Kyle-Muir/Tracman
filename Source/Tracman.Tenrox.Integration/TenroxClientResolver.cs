using System;
using System.Linq;
using Tracman.Core;
using Tracman.Core.Domain;
using Client = Tracman.Tenrox.Integration.ClientsService.Client;

namespace Tracman.Tenrox.Integration
{
    public class TenroxClientResolver
    {
        private readonly ITenroxIdentityCache _tenroxIdentityCache;
        private readonly ITenroxClientRepository _clientRepository;

        public TenroxClientResolver(ITenroxIdentityCache tenroxIdentityCache, ITenroxClientRepository clientRepository)
        {
            if (tenroxIdentityCache == null) throw new ArgumentNullException("tenroxIdentityCache");
            if (clientRepository == null) throw new ArgumentNullException("clientRepository");
            _tenroxIdentityCache = tenroxIdentityCache;
            _clientRepository = clientRepository;
        }

        public string Resolve(int clientUniqueId)
        {
            TenroxIdentity identity = _tenroxIdentityCache.LoadExistingIdentity();
            Client[] clients = _clientRepository.LoadAllClients(identity);
            Client matchedClient = clients.FirstOrDefault(c => c.UniqueId == clientUniqueId);
            if (matchedClient == null)
            {
                throw new ArgumentException("Unable to resolve a client for the unique ID of {0}".FormatWith(clientUniqueId));
            }
            return matchedClient.Name;
        }
    }
}