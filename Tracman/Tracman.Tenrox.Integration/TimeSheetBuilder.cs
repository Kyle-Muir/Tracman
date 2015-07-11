using System;
using System.Globalization;
using Tracman.Core.Domain;
using Tracman.Tenrox.Integration.TimesheetsService;

namespace Tracman.Tenrox.Integration
{
    public class TimeSheetBuilder
    {
        private readonly Timesheet _timesheet;

        public TimeSheetBuilder(Timesheet timesheet)
        {
            if (timesheet == null) throw new ArgumentNullException("timesheet");
            _timesheet = timesheet;
        }

        public TimeSheet Build()
        {
            DateTime startDate = DateTime.ParseExact(_timesheet.StartDate, "M/dd/yyyy", CultureInfo.InvariantCulture);
            DateTime endDate = DateTime.ParseExact(_timesheet.EndDate, "M/dd/yyyy", CultureInfo.InvariantCulture);
            return new TimeSheet(startDate, endDate, _timesheet.UniqueId);
        }
    }
}