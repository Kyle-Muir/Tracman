namespace Tracman.Tenrox.Integration.Test
{
    public class TenroxAuthenticatorBuilder
    {
        public ITenroxAuthenticator Build()
        {
            return new TenroxAuthenticator(TenroxConstants.LogonServiceUri);
        }
    }
}