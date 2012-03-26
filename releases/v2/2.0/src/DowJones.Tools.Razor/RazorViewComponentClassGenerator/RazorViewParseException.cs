using System;

namespace DowJones.Web.Mvc.Razor
{
    public class RazorViewParseException : Exception
    {
        public RazorViewParseException(string message)
            : base(message)
        {
        }
    }
}