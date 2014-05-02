using System;

namespace DowJones.Search
{
    public class ModuleSearchQuery : AbstractSearchQuery
    {
        public string SearchContext { get; set; }

        public override bool IsValid()
        {
            return !String.IsNullOrEmpty(SearchContext);
        }
    }
}