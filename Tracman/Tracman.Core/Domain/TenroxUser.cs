using System;

namespace Tracman.Core.Domain
{
    public class TenroxUser
    {
        private readonly string _userName;
        private readonly string _password;

        public TenroxUser(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName)) { throw new ArgumentNullException("userName"); }
            if (string.IsNullOrEmpty(password)) { throw new ArgumentNullException("password"); }
            _userName = userName;
            _password = password;
        }

        public string UserName
        {
            get { return _userName; }
        }

        public string Password
        {
            get { return _password; }
        }
    }
}