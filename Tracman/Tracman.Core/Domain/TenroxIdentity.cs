using System;
using Tenrox.Shared.Utilities;

namespace Tracman.Core.Domain
{
    public class TenroxIdentity
    {
        private readonly string _token;
        private readonly int _userId;
        private readonly UserToken _userToken;

        public TenroxIdentity(string token, int userId, UserToken userToken)
        {
            if (userToken == null) throw new ArgumentNullException("userToken");
            if (string.IsNullOrEmpty(token)) throw new ArgumentNullException("token");
            _token = token;
            _userId = userId;
            _userToken = userToken;
        }

        public string Value
        {
            get { return _token; }
        }

        public int UserId
        {
            get { return _userId; }
        }

        public UserToken UserToken
        {
            get { return _userToken; }
        }
    }
}