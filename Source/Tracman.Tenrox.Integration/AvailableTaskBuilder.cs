using System;
using System.Collections.Generic;
using System.Linq;
using Tracman.Core.Domain;
using Tracman.Tenrox.Integration.TimesheetsService;

namespace Tracman.Tenrox.Integration
{
    public class AvailableTaskBuilder
    {
        private readonly AssignmentAttribute[] _timesheetAssignmentAttributes;

        public AvailableTaskBuilder(AssignmentAttribute[] timesheetAssignmentAttributes)
        {
            if (timesheetAssignmentAttributes == null) throw new ArgumentNullException("timesheetAssignmentAttributes");
            _timesheetAssignmentAttributes = timesheetAssignmentAttributes;
        }

        public IEnumerable<AvailableTask> Build()
        {
            return _timesheetAssignmentAttributes.Select(Build);
        }

        private AvailableTask Build(AssignmentAttribute attribute)
        {
            TenroxTask task = new TenroxTask(attribute.AssignmentName, attribute.AssignmentUid);
            TenroxClient client = new TenroxClient(attribute.ClientName, attribute.ClientUid);
            return new AvailableTask(task, client);
        }
    }
}