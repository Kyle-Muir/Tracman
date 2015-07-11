using System;
using System.Linq;
using System.ServiceModel;
using Tracman.Core.Domain;
using Tracman.Tenrox.Integration.TimesheetsService;

namespace Tracman.Tenrox.Integration
{
    public class TenroxGateway
    {
        private readonly Uri _timesheetsEndpoint;
        private readonly ITenroxAuthenticator _authenticator;

        public TenroxGateway(ITenroxAuthenticator authenticator, Uri timesheetsEndpoint)
        {
            if (authenticator == null) throw new ArgumentNullException("authenticator");
            if (timesheetsEndpoint == null) throw new ArgumentNullException("timesheetsEndpoint");
            _authenticator = authenticator;
            _timesheetsEndpoint = timesheetsEndpoint;
        }

        public TimeSheet LoadCurrentTimesheet(TenroxUser tenroxUser)
        {
            TenroxIdentity token = _authenticator.Authenticate(tenroxUser);
            using (TimesheetsClient client = BuildTimeSheetsClient())
            {
                Timesheets timesheets = client.QueryByUserTyped(token.UserToken, token.UserId, DateTime.UtcNow.ToString("o"), 1, "", "");
                Timesheet currentTimeSheet = timesheets.MyTimesheets.First();
                return new TimeSheetBuilder(currentTimeSheet).Build();
            }
        }

        private TimesheetsClient BuildTimeSheetsClient()
        {
            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            binding.MaxReceivedMessageSize = int.MaxValue;
            return new TimesheetsClient(binding, new EndpointAddress(_timesheetsEndpoint));
        }
    }
}