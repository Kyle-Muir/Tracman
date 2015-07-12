namespace Tracman.Tenrox.Integration.Test
{
    public class TenroxGatewayBuilder
    {
        public TenroxGateway Build()
        {
            return new TenroxGateway(new TenroxAuthenticatorBuilder().Build(), TenroxConstants.TimesheetsServiceUri);
        }
    }
}