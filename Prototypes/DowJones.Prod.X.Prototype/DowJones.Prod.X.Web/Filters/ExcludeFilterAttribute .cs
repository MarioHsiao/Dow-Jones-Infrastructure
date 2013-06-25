using System;
using System.Web.Mvc;

namespace DowJones.Prod.X.Web.Filters
{
    public class ExcludeFilterAttribute : FilterAttribute
    {
        private readonly Type filterType;

        public ExcludeFilterAttribute(Type filterType)
        {
            this.filterType = filterType;
        }

        public Type FilterType
        {
            get
            {
                return filterType;
            }
        }
    } 
}