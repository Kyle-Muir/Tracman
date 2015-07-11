using Tracman.Core.Domain;

namespace Tracman.Tenrox.Integration
{
    public interface ITenroxAuthenticator
    {
        TenroxIdentity Authenticate(TenroxUser tenroxUser);
    }
}