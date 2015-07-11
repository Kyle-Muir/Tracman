using Tracman.Core.Domain;

namespace Tracman.Tenrox.Integration
{
    public interface ITenroxIdentityCache
    {
        TenroxIdentity LoadIdentity(TenroxUser user);
        TenroxIdentity LoadExistingIdentity();
    }
}