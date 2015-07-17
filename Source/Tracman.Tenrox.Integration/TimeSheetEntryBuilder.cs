using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Tracman.Core.Domain;
using Tracman.Tenrox.Integration.TimesheetsService;

namespace Tracman.Tenrox.Integration
{
    public class TimeSheetEntryBuilder
    {
        private readonly TimesheetEntry[] _timesheetEntries;

        public TimeSheetEntryBuilder(TimesheetEntry[] timesheetEntries)
        {
            if (timesheetEntries == null) throw new ArgumentNullException("timesheetEntries");
            _timesheetEntries = timesheetEntries;
        }

        public IEnumerable<TimeSheetEntry> Build()
        {
            return _timesheetEntries.Select(Build);
        }

        private TimeSheetEntry Build(TimesheetEntry entry)
        {
            string concatNotes = entry.TimeEntryNotes.Aggregate(string.Empty, (current, notes) => current + notes.Description);
            int totalTimeInSeconds = entry.TotalTime;
            decimal totalTimeInHours = totalTimeInSeconds / 3600;
            return new TimeSheetEntry(entry.AssignmentAttributeUid.ToString(CultureInfo.InvariantCulture),
                entry.TaskUid.ToString(CultureInfo.InvariantCulture),
                DateTime.ParseExact(entry.EntryDate, "M/dd/yyyy", CultureInfo.InvariantCulture), totalTimeInHours,
                concatNotes);
        }
    }
}