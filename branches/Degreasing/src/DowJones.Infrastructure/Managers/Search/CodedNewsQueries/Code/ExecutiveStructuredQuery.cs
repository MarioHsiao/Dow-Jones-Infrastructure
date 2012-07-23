// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExecutiveStructuredQuery.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the ExecutiveStructuredQuery type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using DowJones.Exceptions;
using DowJones.Session;
using Factiva.Gateway.Messages.Search.V2_0;
using log4net;

namespace DowJones.Managers.Search.CodedNewsQueries.Code
{
    public class ExecutiveStructuredQuery : AbstractStructuredQuery, IExecutiveQuery
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ExecutiveStructuredQuery));
        private ExecutiveFilter executiveFilter;
        private StructuredQuery query;

        #region IExecutiveQuery Members

        public ExecutiveFilter Executives
        {
            get { return executiveFilter; }
            set { executiveFilter = value; }
        }

        public override StructuredQuery ExpandQuery
        {
            get
            {
                query.Dates.After = (QueryType == CodedNewsType.ExecutiveBusinessNews)
                                        ? DATE_RANGE_LAST_2YEAR
                                        : DATE_RANGE_LAST_1YEAR;
                return query;
            }
        }

        #endregion

        protected static SearchString GetDefaultNSFilter()
        {
            var searchString = new SearchString
            {
                Mode = SearchMode.None,
                Id = "Exclude",
                Type = SearchType.Controlled,
                Value = "nnam",
                Scope = "ns",
                Filter = true
            };
            return searchString;
        }

        protected static SearchString GetDefaultRstFilter()
        {
            var searchString = new SearchString
            {
                Mode = SearchMode.None,
                Id = "Exclude",
                Type = SearchType.Controlled,
                Value = "TNFCE",
                Scope = "rst",
                Filter = true
            };
            return searchString;
        }

        protected override StructuredQuery BuildQuery()
        {
            ValidateAndUpdate();
            Logger.Debug("Build query");
            if (query != null)
            {
                return query;
            }

            query = new StructuredQuery();
            query.SearchCollectionCollection.Add(SearchCollection.Publications);
            query.Dates = GetDefaultDateOption();
            
            var listSearchString = new List<SearchString>
                                       {
                                           GetDefaultNSFilter(), 
                                           GetDefaultRstFilter()
                                       };

            // Apply Filter 
            AddFilters(listSearchString);

            if (LanguagePreference != null && LanguagePreference.Length > 0)
            {
                listSearchString.Add(GetLanguageFilter(LanguagePreference));
            }

            SearchString temp;

            var isFirstNameAndLastName = IsFirstNameAndLastName();

            // Name filter
            if (isFirstNameAndLastName)
            {
                temp = GetFirstNameAndLastNameSearchString(Executives.Name.FirstName, Executives.Name.LastName);
                listSearchString.Add(temp);
            }
            else if (!string.IsNullOrEmpty(Executives.Name.FullName)) 
            {
                //// Default to full name
                var fullName = String.Format("\"{0}\"", Executives.Name.FullName);
                temp = GetKeywordFilter(new[] { fullName });
                temp.Id = "FN";
                listSearchString.Add(temp);
            }

            // Company filter
            if (null != Executives.CompanyFilter)
            {
                var companyFilter = Executives.CompanyFilter;
                var companyName = IsValid(companyFilter.NewsSearch) ? companyFilter.NewsSearch : (companyFilter.Descriptor == null ? string.Empty : companyFilter.Descriptor.Value);
                temp = GetCompanyNameFilter(null);

                if (OccurrenceSearch)
                {
                    temp.Value = isFirstNameAndLastName ? string.Format("fds:{0} fds:occur:{0} hlp:\"{1}\"", companyFilter.Fcode, companyName) : string.Format("fds:{0} fds:occur:{0} \"{1}\"", companyFilter.Fcode, companyName);
                }
                else if (companyFilter.Status != null && companyFilter.Status.IsNewsCoded)
                {
                    temp.Value = isFirstNameAndLastName ? string.Format("fds:{0} hlp:\"{1}\"", companyFilter.Fcode, companyName) : string.Format("fds:{0} \"{1}\"", companyFilter.Fcode, companyName);
                }
                else
                {
                    temp.Value = string.Format("{0}", companyName);
                }

                listSearchString.Add(temp);
            }

            if (QueryType == CodedNewsType.ExecutiveBusinessNews)
            {
                query.Dates.After = DATE_RANGE_LAST_2YEAR;
                temp = GetNewsSubjectFilter(new[] { "NFCPEX" });
                listSearchString.Add(temp);
            }

            query.SearchStringCollection.AddRange(listSearchString.ToArray());
            return query;
        }

        protected bool IsFirstNameAndLastName()
        {
            var nm = Executives.Name;
            return IsValid(nm.FirstName) && IsValid(nm.LastName);
        }

        private void ValidateAndUpdate()
        {
            if (executiveFilter == null)
            {
                throw new DowJonesUtilitiesException("[ValidateRequest] Cannot perform executive headlines search without executive object(s).", DowJonesUtilitiesException.CodedNewsSearchRequestIncomplete);
            }

            if (executiveFilter.Name == null || (string.IsNullOrEmpty(executiveFilter.Name.FullName) && 
                (string.IsNullOrEmpty(executiveFilter.Name.FirstName) || 
                string.IsNullOrEmpty(executiveFilter.Name.LastName))))
            {
                throw new DowJonesUtilitiesException("[ValidateRequest] Cannot perform executive headlines search without full executive name or executive first and last names).", DowJonesUtilitiesException.CodedNewsSearchRequestIncomplete);
            }

            if (executiveFilter.CompanyFilter == null || (executiveFilter.CompanyFilter.Status != null && (executiveFilter.CompanyFilter.Status.IsNewsCoded || executiveFilter.CompanyFilter.NewsSearch != null)))
            {
                return;
            }

            if (!String.IsNullOrEmpty(executiveFilter.CompanyFilter.Fcode))
            {
                executiveFilter.CompanyFilter = GetUpdatedCompany(executiveFilter.CompanyFilter.Fcode);
            }
            else if (executiveFilter.CompanyFilter.Status != null)
            {
                executiveFilter.CompanyFilter.Status.IsNewsCoded = false;
            }
        }

        private CompanyFilter GetUpdatedCompany(string fcode)
        {
            CompanyFilter companyFilter;
            var screeningCompanies = GetCompaniesFromScreening(new[] { fcode }, ControlDataManager.Clone(ControlData));
            if (screeningCompanies == null)
            {
                Logger.Debug(String.Format("Company[{0}] not found in screening, so try symbology", fcode));
                companyFilter = GetCompanies(new[] { fcode });
            }
            else
            {
                companyFilter = screeningCompanies;
            }

            return companyFilter;
        }
    }
}