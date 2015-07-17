using Tracman.Core.Domain;
using Tracman.Tenrox.Integration.ClientsService;

namespace Tracman.Tenrox.Integration
{
    public interface ITenroxClientRepository
    {
        Client[] LoadAllClients(TenroxIdentity identity);
    }
}