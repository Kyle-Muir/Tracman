using FluentAssertions;
using NUnit.Framework;

namespace Tracman.Tenrox.Integration.Test
{
    [TestFixture]
    public class TenroxClientResolverTest : TenroxUserDependantTest
    {
        [Test]
        public void CanLoadAClientNameFromAUniqueId()
        {
            const int clientUniqueId = TenroxConstants.IntergenBusinessSlns;
            TenroxClientResolver resolver = new TenroxClientResolverBuilder(TenroxIdentityCache).Build();
            string clientName = resolver.Resolve(clientUniqueId);
            clientName.Should().Be("Intergen Business Solutions Pty (for Intergen NZ)");
        } 
    }
}