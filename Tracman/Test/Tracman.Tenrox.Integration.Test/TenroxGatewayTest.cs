using System;
using FluentAssertions;
using NUnit.Framework;
using Tracman.Core.Domain;

namespace Tracman.Tenrox.Integration.Test
{
    [TestFixture]
    public class TenroxGatewayTest
    {
        [Test]
        public void CanLoadAUsersCurrentTimesheet()
        {
            TenroxGateway tenroxGateway = new TenroxGateway(new TenroxAuthenticatorBuilder().Build(), TenroxConstants.TimesheetsServiceUri);
            TenroxUser tenroxUser = new TenroxUserBuilder().Build();
            TimeSheet timeSheet = tenroxGateway.LoadCurrentTimesheet(tenroxUser);
            timeSheet.Should().NotBeNull();
            DateTime startOfWeek = ClosestSaturday();
            DateTime endOfWeek = startOfWeek.AddDays(6);

            timeSheet.StartDate.Should().Be(startOfWeek);
            timeSheet.EndDate.Should().Be(endOfWeek);
            timeSheet.UniqueId.Should().BePositive();
        }

        private static DateTime ClosestSaturday()
        {
            DateTime today = DateTime.Today;
            int daysUntilTuesday = ((int) DayOfWeek.Saturday - (int) today.DayOfWeek + 7)%7;
            return today.AddDays(daysUntilTuesday);
        }
    }
}