using System;
using System.Collections.Generic;
using System.Reflection;
using DowJones.Exceptions;
using DowJones.Extensions;
using DowJones.Session;
using Factiva.Gateway.Messages.Search.V2_0;
using log4net;

namespace DowJones.Managers.Search.CodedNewsQueries.Code
{
    public class AbstractCompanyStructuredQuery : AbstractStructuredQuery, ICompanyQuery
    {
        private static readonly ILog logger =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        protected readonly List<SearchString> ListSearchString = new List<SearchString>();
        private CompanyFilter _CompanyFilter;
        protected new StructuredQuery Query;

        #region ICompanyQuery Members

        public CompanyFilter CompanyFilter
        {
            get { return _CompanyFilter; }
            set { _CompanyFilter = value; }
        }

        public string KeywordFilter { get; set; }

        public override StructuredQuery ExpandQuery
        {
            get
            {
                Query.Dates.After = DATE_RANGE_LAST_1YEAR;
                return Query;
            }
        }

        #endregion

        protected override StructuredQuery BuildQuery()
        {
            if (Query != null)
            {
                return Query;
            }
            ValidateAndUpdate();
            BuildSearchStringList();
            Query = new StructuredQuery();
            Query.SearchCollectionCollection.Add(SearchCollection.Publications);
            Query.Dates = GetDefaultDateOption();

            if (LanguagePreference != null && LanguagePreference.Length > 0)
            {
                ListSearchString.Add(GetLanguageFilter(LanguagePreference));
            }

            if( KeywordFilter.IsNotEmpty() )
            {
                ListSearchString.Add(GetKeywordFilter(new[] { KeywordFilter }));
            }

            //Apply Filter 
            AddFilters(ListSearchString);
            Query.SearchStringCollection.AddRange(ListSearchString.ToArray());
            return Query;
        }

        protected virtual void BuildSearchStringList()
        {
        }

        private void ValidateAndUpdate()
        {
            if (_CompanyFilter == null)
                throw new DowJonesUtilitiesException("[ValidateRequest] Cannot perform company headlines search without company object.", DowJonesUtilitiesException.CodedNewsSearchRequestIncomplete);

            if (String.IsNullOrEmpty(_CompanyFilter.Fcode))
                throw new DowJonesUtilitiesException("[ValidateRequest] Cannot perform company headlines search without company code.", DowJonesUtilitiesException.CodedNewsSearchRequestIncomplete);

            if (_CompanyFilter.Status != null && (_CompanyFilter.NewsSearch != null || (_CompanyFilter.Status.IsNewsCoded)))
                return;
            var screeningCompanies = GetCompaniesFromScreening(new[] {_CompanyFilter.Fcode}, ControlData);
            if (screeningCompanies == null)
            {
                logger.Debug(String.Format("Company[{0}] not found in screening, so try symbology", _CompanyFilter.Fcode));
                _CompanyFilter = GetCompanies(new[] {_CompanyFilter.Fcode});
            }
            else
            {
                _CompanyFilter = screeningCompanies;
            }
        }
    }
}