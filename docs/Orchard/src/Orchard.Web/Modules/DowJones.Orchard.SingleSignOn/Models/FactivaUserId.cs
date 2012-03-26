using System.Diagnostics.Contracts;

namespace DowJones.Orchard.SingleSignOn
{
    internal class FactivaUserId
    {
        private readonly string _username;
        private readonly string _namespace;

        public string Username
        {
            get { return _username; }
        }

        public string Namespace
        {
            get { return _namespace; }
        }

        public string OrchardUsername
        {
            get { return string.Format("{0}@{1}.dowjones.com", _username, _namespace); }
        }

        public FactivaUserId(string username, string @namespace)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(username));
            Contract.Requires(!string.IsNullOrWhiteSpace(@namespace));

            _username = username;
            _namespace = @namespace;
        }

        public static implicit operator string(FactivaUserId userId)
        {
            return userId.OrchardUsername;
        }
    }
}