using System;
using System.Collections.Generic;
using System.Linq;
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
            TenroxGateway tenroxGateway = new TenroxGatewayBuilder().Build();
            TenroxUser tenroxUser = new TenroxUserBuilder().Build();
            TimeSheet timeSheet = tenroxGateway.LoadCurrentTimesheet(tenroxUser);
            timeSheet.Should().NotBeNull();
            DateTime startOfWeek = ClosestSaturday();
            DateTime endOfWeek = startOfWeek.AddDays(6);

            timeSheet.StartDate.Should().Be(startOfWeek);
            timeSheet.EndDate.Should().Be(endOfWeek);
            timeSheet.UniqueId.Should().BePositive();
            timeSheet.Entries.Count().Should().BeGreaterThan(0, "should have entries for a week...");
        }

        private static DateTime ClosestSaturday()
        {
            DateTime date = DateTime.Today;
            while (date.DayOfWeek != DayOfWeek.Saturday)
            {
                date = date.AddDays(-1);
            }
            return date;
        }
    }
}