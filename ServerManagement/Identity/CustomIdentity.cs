using System.Security.Principal;

namespace ServerManagement.Identity
{
    public class CustomIdentity : IIdentity
    {
        public CustomIdentity(string name, string role)
        {
            Name = name;
            Role = role;
        }

        public string Name { get; private set; }
        public string Role { get; private set; }

        #region IIdentity Members
        public string AuthenticationType { get { return "Custom authentication"; } }

        public bool IsAuthenticated { get { return !string.IsNullOrEmpty(Name); } }
        #endregion
    }
}
