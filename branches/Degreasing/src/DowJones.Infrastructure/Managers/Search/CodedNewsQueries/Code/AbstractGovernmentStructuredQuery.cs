using System.Collections.Generic;
using DowJones.Managers.Search.CodedNewsQueries.Government;
using Factiva.Gateway.Messages.Search.V2_0;

namespace DowJones.Managers.Search.CodedNewsQueries.Code
{
    public class AbstractGovernmentStructuredQuery : AbstractStructuredQuery, IGovernmentQuery
    {
        protected List<SearchString> ListSearchString = new List<SearchString>();
        private StructuredQuery query;

        #region IGovernmentQuery Members

        public GovernmentOfficial Official { get; set; }

        public Organization Organization { get; set; }

        public override StructuredQuery ExpandQuery
        {
            get
            {
                query.Dates.After = DATE_RANGE_LAST_2YEAR;
                return query;
            }
        }

        #endregion

        protected override StructuredQuery BuildQuery()
        {
            if (query != null)
            {
                return query;
            }
            BuildSearchStringList();
            query = new StructuredQuery();

            //Apply Filter 
            AddFilters(ListSearchString);
            query.SearchCollectionCollection.Add(SearchCollection.Publications);
            query.Dates = GetDefaultDateOption();
            query.Dates.After = DATE_RANGE_LAST_3MONTHS;

            if (LanguagePreference != null && LanguagePreference.Length > 0)
                ListSearchString.Add(GetLanguageFilter(LanguagePreference));

            query.SearchStringCollection.AddRange(ListSearchString.ToArray());
            return query;
        }

        protected virtual void BuildSearchStringList()
        {
        }
    }
}