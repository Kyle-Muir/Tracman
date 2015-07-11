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
            int uniqueId = 72960;
            TenroxClientResolver resolver = new TenroxClientResolverBuilder(TenroxIdentityCache).Build();
            string clientName = resolver.Resolve(uniqueId);
            clientName.Should().Be("Intergen Business Solutions Pty (for Intergen NZ)");
        } 
    }
}