using System;

namespace DowJones.Web.UI.Exceptions
{
    public class InvalidClientResourceException : Exception
    {
        public string Name { get; set; }
        public ClientResource Resource { get; set; }

        public InvalidClientResourceException(ClientResource resource, string message)
            : base(message)
        {
            Resource = resource;
        }
    }
}
