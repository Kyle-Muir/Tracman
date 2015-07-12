using System;
using System.Collections.Generic;

namespace Tracman.Core.Domain
{
    public class TimeSheet
    {
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;
        private readonly int _uniqueId;
        private readonly IEnumerable<TimeSheetEntry> _entries;

        public TimeSheet(DateTime startDate, DateTime endDate, int uniqueId, IEnumerable<TimeSheetEntry> entries)
        {
            if (entries == null) throw new ArgumentNullException("entries");
            _startDate = startDate;
            _endDate = endDate;
            _uniqueId = uniqueId;
            _entries = entries;
        }

        public DateTime StartDate
        {
            get { return _startDate; }
        }

        public DateTime EndDate
        {
            get { return _endDate; }
        }

        public int UniqueId
        {
            get { return _uniqueId; }
        }

        public IEnumerable<TimeSheetEntry> Entries
        {
            get { return _entries; }
        }
    }
}