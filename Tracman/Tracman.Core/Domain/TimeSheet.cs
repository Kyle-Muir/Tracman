using System;

namespace Tracman.Core.Domain
{
    public class TimeSheet
    {
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;
        private readonly int _uniqueId;

        public TimeSheet(DateTime startDate, DateTime endDate, int uniqueId)
        {
            _startDate = startDate;
            _endDate = endDate;
            _uniqueId = uniqueId;
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
    }
}