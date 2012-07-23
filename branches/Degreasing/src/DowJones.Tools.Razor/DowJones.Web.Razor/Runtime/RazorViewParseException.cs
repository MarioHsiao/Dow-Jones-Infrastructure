using System;

namespace DowJones.Web.Razor.Runtime
{
    public class RazorViewParseException : Exception
    {
        public RazorViewParseException(string message)
            : base(message)
        {
        }
    }
}