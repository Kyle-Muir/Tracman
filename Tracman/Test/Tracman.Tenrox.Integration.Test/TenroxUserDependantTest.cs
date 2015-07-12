using NUnit.Framework;

namespace Tracman.Tenrox.Integration.Test
{
    public class TenroxUserDependantTest
    {
        protected ITenroxIdentityCache TenroxIdentityCache { get; set; }

        [TestFixtureSetUp]
        public void SetUp()
        {
            TenroxIdentityCache = new TenroxIdentityCacheBuilder().Build();
            TenroxIdentityCache.LoadIdentity(new TenroxUserBuilder().Build());
        }
    }
}