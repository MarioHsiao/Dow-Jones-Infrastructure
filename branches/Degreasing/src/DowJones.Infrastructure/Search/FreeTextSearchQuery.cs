using System;

namespace DowJones.Search
{
    public class FreeTextSearchQuery : AdvancedSearchQuery
    {
        public string FreeText { get; set; }

        public SearchFreeTextArea FreeTextIn { get; set; }

        public override bool IsValid()
        {
            return true;
            //TODO DO PROPER VALIDATION HERE - NP.
        }
    }
}