using System.Collections.Generic;
using DowJones.Utilities.Search.Core;
using Factiva.Gateway.Messages.Search.V2_0;

namespace DowJones.Utilities.Managers.Search.Base
{
    public class FIISearchList
    {
        public SearchOperator IncludedSearchOperator { get; set; }
        public FIIType ListType { get; set; }
        public List<string> Included { get; set; }
        public List<string> Excluded { get; set; }

        public FIISearchList()
        {
            IncludedSearchOperator = SearchOperator.And;
            ListType = FIIType.Company;
            Included = new List<string>();
            Excluded = new List<string>();
        }

        internal List<SearchString> GenerateSearchString()
        {
            var tList = new List<SearchString>();

            if (Included.Count > 0)
            {
                tList.Add(SearchUtility.GetSearchStringByScopeType(MapIncludedScopeTypeScope(), Included.ToArray()));
            }

            if (Excluded.Count > 0)
            {
                tList.Add(SearchUtility.GetSearchStringByScopeType(MapExcludedScopeTypeScope(), Excluded.ToArray()));
            }
            return tList;
        }

        internal SearchUtility.ScopeType MapExcludedScopeTypeScope()
        {
            switch(ListType)
            {
               case FIIType.Author:
                    return SearchUtility.ScopeType.ExcludedAuthors;
                case FIIType.Company:
                    return SearchUtility.ScopeType.ExcludedCompanies;
                case FIIType.CompanyOccurance:
                    return SearchUtility.ScopeType.ExcludedCompaniesOccurance;
                case FIIType.Industry:
                    return SearchUtility.ScopeType.ExcludedIndustries;
                case FIIType.NewsSubject:
                    return SearchUtility.ScopeType.ExcludedNewSubjects;
                default:
                    return SearchUtility.ScopeType.ExcludedRegions;
            }
        }

        internal SearchUtility.ScopeType MapIncludedScopeTypeScope()
        {
            switch (ListType)
            {
                case FIIType.Author:
                    return (IncludedSearchOperator == SearchOperator.And) ? SearchUtility.ScopeType.AllAuthors : SearchUtility.ScopeType.AnyAuthor;
                case FIIType.Company:
                    return (IncludedSearchOperator == SearchOperator.And) ? SearchUtility.ScopeType.AllCompanies : SearchUtility.ScopeType.AnyCompany;
                case FIIType.CompanyOccurance:
                    return (IncludedSearchOperator == SearchOperator.And) ? SearchUtility.ScopeType.AllCompaniesOcurrance : SearchUtility.ScopeType.AnyCompanyOccurance;
                case FIIType.Industry:
                    return (IncludedSearchOperator == SearchOperator.And) ? SearchUtility.ScopeType.AllIndustries : SearchUtility.ScopeType.AnyIndustry;
                case FIIType.NewsSubject:
                    return (IncludedSearchOperator == SearchOperator.And) ? SearchUtility.ScopeType.AllNewsSubjects : SearchUtility.ScopeType.AnyNewsSubject;
                default:
                    return (IncludedSearchOperator == SearchOperator.And) ? SearchUtility.ScopeType.AllRegions : SearchUtility.ScopeType.AnyRegion;
                    
            }
        }
    }
}
