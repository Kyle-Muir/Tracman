using System;
using FluentAssertions;
using NUnit.Framework;
using Tracman.Core.Domain;

namespace Tracman.Tenrox.Integration.Test
{
    [TestFixture]
    public class TenroxAuthenticatorTests
    {
        [Test]
        public void CanAuthenticateAndGetATokenForAValidUser()
        {
            ITenroxAuthenticator authenticator = new TenroxAuthenticatorBuilder().Build();
            TenroxUser tenroxUser = new TenroxUserBuilder().Build();
            TenroxIdentity token = authenticator.Authenticate(tenroxUser);
            Console.WriteLine(token.Value);
            
            token.Should().NotBeNull();
            token.Value.Should().NotBeEmpty();
        }
    }
}