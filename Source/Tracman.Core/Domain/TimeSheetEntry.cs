using System;

namespace Tracman.Core.Domain
{
    public class TimeSheetEntry
    {
        private readonly string _client;
        private readonly string _task;
        private readonly DateTime _date;
        private readonly decimal _hours;
        private readonly string _notes;

        public TimeSheetEntry(string client, string task, DateTime date, decimal hours, string notes)
        {
            if (string.IsNullOrEmpty(client)) throw new ArgumentNullException("client");
            if (string.IsNullOrEmpty(task)) throw new ArgumentNullException("task");
            if (string.IsNullOrEmpty(notes)) throw new ArgumentNullException("notes");
            
            _client = client;
            _task = task;
            _date = date;
            _hours = hours;
            _notes = notes;
        }

        public string Client
        {
            get { return _client; }
        }

        public string Task
        {
            get { return _task; }
        }

        public DateTime Date
        {
            get { return _date; }
        }

        public decimal Hours
        {
            get { return _hours; }
        }

        public string Notes
        {
            get { return _notes; }
        }
    }
}