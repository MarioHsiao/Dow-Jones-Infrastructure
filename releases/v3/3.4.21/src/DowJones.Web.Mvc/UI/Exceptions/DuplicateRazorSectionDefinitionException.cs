using System;

namespace DowJones.Web.Mvc.UI.Exceptions
{
    public class DuplicateRazorSectionDefinitionException : Exception
    {
        public DuplicateRazorSectionDefinitionException(string sectionName)
            : base(string.Format("The section '{0}' has already been defined", sectionName))
        {
        }
    }
}