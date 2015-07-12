using System;

namespace Tracman.Tenrox.Integration.Test
{
    public static class TenroxConstants
    {
        public static Uri LogonServiceUri = new Uri("https://intergen.tenrox.net/Twebservice/logonas.svc");
        public static Uri TimesheetsServiceUri = new Uri("https://intergen.tenrox.net/Twebservice/timesheets.svc");
        public static Uri AssignmentsServiceUri = new Uri("https://intergen.tenrox.net/Twebservice/sdk/appointments.svc");
        public static Uri ClientsServiceUri = new Uri("https://intergen.tenrox.net/Twebservice/sdk/clients.svc");
        public const int IntergenBusinessSlns = 1340;
    }
}