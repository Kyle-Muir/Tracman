using System;

namespace Tracman.Core.Domain
{
    public class AvailableTask
    {
        private readonly TenroxTask _task;
        private readonly TenroxClient _client;

        public AvailableTask(TenroxTask task, TenroxClient client)
        {
            if (task == null) throw new ArgumentNullException("task");
            if (client == null) throw new ArgumentNullException("client");
            _task = task;
            _client = client;
        }

        public TenroxTask Task
        {
            get { return _task; }
        }

        public TenroxClient Client
        {
            get { return _client; }
        }
    }
}