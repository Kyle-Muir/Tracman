using Tracman.Core;
using Tracman.Core.Domain;

namespace Tracman.Tenrox.Integration.Test
{
    public class TenroxUserBuilder
    {
        public TenroxUserBuilder()
        {
            Password = Settings.Load("TenroxPassword").ToString();
            UserName = Settings.Load("TenroxUsername").ToString();
        }

        public TenroxUser Build()
        {
            return new TenroxUser(UserName, Password);
        }

        public string Password { get; set; }

        public string UserName { get; set; }
    }
}