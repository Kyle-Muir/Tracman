using System;

namespace Tracman.Core.Domain
{
    public class TenroxClient
    {
        private readonly string _name;
        private readonly int _uniqueId;

        public TenroxClient(string name, int uniqueId)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");
            _name = name;
            _uniqueId = uniqueId;
        }

        public string Name
        {
            get { return _name; }
        }

        public int UniqueId
        {
            get { return _uniqueId; }
        }
    }
}