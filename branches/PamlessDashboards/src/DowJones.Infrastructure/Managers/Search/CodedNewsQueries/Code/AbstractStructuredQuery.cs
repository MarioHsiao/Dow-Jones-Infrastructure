using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Exceptions;
using DowJones.Session;
using Factiva.Gateway.Messages.Screening.V1_1;
using Factiva.Gateway.Messages.Search.V2_0;
using Factiva.Gateway.Messages.Symbology.Company.V1_0;
using Factiva.Gateway.Messages.Symbology.NameList.V1_0;
using Factiva.Gateway.Messages.Track.V1_0;
using Factiva.Gateway.Services.V1_0;
using Factiva.Gateway.V1_0;
using Company = Factiva.Gateway.Messages.Screening.V1_1.Company;
using CompanyStatus = Factiva.Gateway.Messages.Symbology.Company.V1_0.CompanyStatus;

namespace DowJones.Managers.Search.CodedNewsQueries.Code
{
    public class AbstractStructuredQuery : IStructuredQuery
    {
        protected const string DATE_RANGE_LAST_1YEAR = "-367";
        protected const string DATE_RANGE_LAST_2YEAR = "-734";
        protected const string DATE_RANGE_LAST_3MONTHS = "-91";

        #region IStructuredQuery Members

        public IControlData ControlData { get; set; }

        public CodedNewsType QueryType { get; set; }

        public NewsFiltersExtended Filters { get; set; }

        public bool OccurrenceSearch { get; set; }

        //public string LanguagePreference
        //{
        //    get { throw new NotImplementedException(); }
        //    set { throw new NotImplementedException(); }
        //}

        public StructuredQuery Query
        {
            get { return BuildQuery(); }
        }

        public virtual StructuredQuery ExpandQuery
        {
            get { throw new NotImplementedException(); }
        }

        public string[] LanguagePreference { get; set; }

        #endregion

        protected static bool IsValid(string str)
        {
            return (!string.IsNullOrEmpty(str));
        }

        protected virtual StructuredQuery BuildQuery()
        {
            return Query;
        }

        protected static SearchString GetCompanyNameFilter(string name)
        {
            return new SearchString
                       {
                           Id = "fdsCat",
                           Mode = SearchMode.Any,
                           Type = SearchType.Free,
                           Value = name
                       };
        }

        protected virtual Dates GetDefaultDateOption()
        {
            return new Dates
                       {
                           After = DATE_RANGE_LAST_3MONTHS,
                           Format = DateFormat.DDMMCCYY
                       };
        }

        protected void AddFilters(List<SearchString> list)
        {
            if (Filters != null)
            {
                var temp = GetKeywordFilter(Filters.keywords);
                if (temp != null)
                {
                    list.Add(temp);
                }
                temp = GetCompanyFilter(GetCompanyCodes(Filters.company), false);
                if (temp != null)
                {
                    list.Add(temp);
                }

                //q208 occur
                temp = GetCompanyFilter(GetCompanyCodes(Filters.OccuresCompany), true);
                if (temp != null)
                {
                    list.Add(temp);
                }

                temp = GetIndustryFilter(GetIndustryCodes(Filters.industry));
                if (temp != null)
                {
                    list.Add(temp);
                }
                temp = GetNewsSubjectFilter(GetNewsSubjectCodes(Filters.newsSubject));
                if (temp != null)
                {
                    list.Add(temp);
                }
                temp = GetRestrictorFilter(GetSourceCodes(Filters.source));
                if (temp != null)
                {
                    list.Add(temp);
                }
            }
        }

        protected static SearchString GetKeywordFilter(string[] items)
        {
            if (items == null || items.Length == 0)
            {
                return null;
            }

            var searchString = new SearchString
                                   {
                                       Id = "keyword",
                                       Type = SearchType.Free,
                                       Value = String.Join(" ", items),
                                       Mode = SearchMode.Simple
                                   };

            return searchString;
        }

        protected static SearchString GetCompanyFilter(string[] codes, bool occurrence)
        {
            return GetCompanyFilter(codes, occurrence, true);
        }

        protected static SearchString GetCompanyFilter(string[] codes, bool occurrence, bool validate)
        {
            if (codes == null || codes.Length == 0)
            {
                return null;
            }
            var searchString = new SearchString
                                   {
                                       Mode = SearchMode.All,
                                       Id = "fdsFilter",
                                       Type = SearchType.Controlled,
                                       Value = String.Join(" ", codes),
                                       Scope = "fds",
                                       Validate = validate
                                   };
            if (occurrence)
            {
                searchString.Scope += ":occur";
            }
            searchString.Filter = true;
            return searchString;
        }

        protected static SearchString GetExcludeSearchString(string scope, string[] codes)
        {
            var searchString = new SearchString
                                   {
                                       Mode = SearchMode.None,
                                       Id = scope + "Exclude",
                                       Type = SearchType.Controlled,
                                       Value = String.Join(" ", codes),
                                       Scope = scope,
                                       Filter = true
                                   };
            return searchString;
        }

        protected static SearchString GetIndustryFilter(string[] codes)
        {
            if (codes == null || codes.Length == 0)
            {
                return null;
            }
            var searchString = new SearchString
                                   {
                                       Mode = SearchMode.All,
                                       Id = "inFilter",
                                       Type = SearchType.Controlled, Value = String.Join(" ", codes), Scope = "in", Filter = true
                                   };
            return searchString;
        }

        protected static SearchString GetNewsSubjectFilter(string[] codes)
        {
            if (codes == null || codes.Length == 0)
            {
                return null;
            }
            var searchString = new SearchString
                                   {
                                       Mode = SearchMode.All, Id = "nsFilter",
                                       Type = SearchType.Controlled,
                                       Value = String.Join(" ", codes),
                                       Scope = "ns", Filter = true
                                   };
            return searchString;
        }

        protected static SearchString GetRestrictorFilter(string[] codes)
        {
            if (codes == null || codes.Length == 0)
            {
                return null;
            }
            var searchString = new SearchString
                                   {
                                       Mode = SearchMode.Any,
                                       Id = "rstFilter",
                                       Type = SearchType.Controlled,
                                       Value = String.Join(" ", codes),
                                       Scope = "rst",
                                       Filter = true
                                   };

            return searchString;
        }

        protected static SearchString GetLanguageFilter(string[] codes)
        {
            var searchString = new SearchString
                                   {
                                       Id = "la",
                                       Scope = "la",
                                       Type = SearchType.Controlled,
                                       Mode = SearchMode.Any,
                                       Filter = true,
                                       Value = String.Join(" ", codes)
                                   };
            return searchString;
        }

        protected static SearchString GetNiweFilter()
        {
            var searchString = new SearchString
                                   {
                                       Mode = SearchMode.None,
                                       Id = "restrictors",
                                       Type = SearchType.Controlled,
                                       Value = "niwe",
                                       Scope = "ns",
                                       Filter = true
                                   };
            return searchString;
        }

        protected static SearchString GetAboutOnlyCompanyFilter(string[] codes)
        {
            if (codes == null || codes.Length == 0)
            {
                return null;
            }

            var searchString = new SearchString
                                   {
                                       Mode = SearchMode.Any,
                                       Id = "fdsAbout",
                                       Type = SearchType.Controlled,
                                       Value = GetAboutString(codes),
                                       Scope = null,
                                       Filter = true
                                   };
            return searchString;
        }

        protected static SearchString GetOccurrenceOnlyCompanyFilter(string[] codes)
        {
            if (codes == null || codes.Length == 0)
            {
                return null;
            }

            var searchString = new SearchString
            {
                Mode = SearchMode.Any,
                Id = "fdsOccur",
                Type = SearchType.Controlled,
                Value = GetOccurString(codes),
                Scope = null,
                Filter = true
            };
            return searchString;
        }

        protected static SearchString GetOccurrenceCompanyFilter(string[] codes)
        {
            if (codes == null || codes.Length == 0)
            {
                return null;
            }

            var searchString = new SearchString
                                   {
                                       Mode = SearchMode.Any,
                                       Id = "fdsOccur",
                                       Type = SearchType.Controlled,
                                       Value = GetOccurOrAboutString(codes),
                                       Scope = null,
                                       Filter = true
                                   };
            return searchString;
        }


        private static string GetOccurOrAboutString(IEnumerable<string> codes)
        {
            var sb = new StringBuilder();
            foreach (var code in codes)
            {
                sb.AppendFormat("fds:{0} fds:occur:{0}", code);
                sb.Append(" ");
            }
            return sb.ToString().Trim();
        }

        private static string GetAboutString(IEnumerable<string> codes)
        {
            var sb = new StringBuilder();
            foreach (var code in codes)
            {
                sb.AppendFormat("fds:{0}", code);
                sb.Append(" ");
            }
            return sb.ToString().Trim();
        }

        private static string GetOccurString(IEnumerable<string> codes)
        {
            var sb = new StringBuilder();
            foreach (var code in codes)
            {
                sb.AppendFormat("fds:occur:{0}", code);
                sb.Append(" ");
            }
            return sb.ToString().Trim();
        }


        protected static string[] GetCompanyCodes(FilterItem[] list)
        {
            if (list != null && list.Length > 0)
            {
                return list.Select(each => each.code).ToArray();
            }
            return new string[0];
        }

        protected static string[] GetIndustryCodes(FilterItem[] list)
        {
            if (list != null && list.Length > 0)
            {
                return list.Select(each => each.code).ToArray();
            }
            return new string[0];
        }

        protected static string[] GetNewsSubjectCodes(FilterItem[] list)
        {
            if (list != null && list.Length > 0)
            {
                return list.Select(each => each.code).ToArray();
            }
            return new string[0];
        }

        protected static string[] GetSourceCodes(FilterSourceItem[] list)
        {
            if (list != null && list.Length > 0)
            {
                return list.Select(each => each.code).ToArray();
            }
            return new string[0];
        }

        protected NameListResult GetNicknames(string firstName)
        {
            NameListResult nameListResult = null;
            if (firstName != null)
            {
                //********Replaced old gateway calls with current gateway calls.********//
                var performNameListSearchRequest = new PerformNameListSearchRequest
                                                       {
                                                           SearchText = firstName,
                                                           SearchScope = NameListSearchScope.NameAndAliases
                                                       };
                var serviceResponse = SymbologyNameListService.PerformNameListSearch(ControlDataManager.Convert(ControlData), performNameListSearchRequest);

                if (serviceResponse.rc != 0)
                {
                    throw new DowJonesUtilitiesException(serviceResponse.rc);
                }

                object responseObj;
                var responseObjRc = serviceResponse.GetResponse(ServiceResponse.ResponseFormat.Object, out responseObj);
                if (responseObjRc == 0)
                {
                    var performNameListSearchResponse = (PerformNameListSearchResponse) responseObj;
                    nameListResult = performNameListSearchResponse.NameListResult;
                }
                else
                {
                    throw new DowJonesUtilitiesException(responseObjRc);
                }
            }
            return nameListResult;
        }


        protected SearchString GetFirstNameAndLastNameSearchString(string firstName, string lastName)
        {
            if (firstName != null)
            {
                var nicknames = GetNicknames(firstName);
                if (nicknames != null && nicknames.NameCollection != null && nicknames.NameCollection.Count > 0)
                {
                    firstName = String.Format("(\"{0}\" or \"{1}\")", firstName, String.Join("\" or \"", nicknames.NameCollection.ToArray()));
                }
                else
                {
                    firstName = String.Format("\"{0}\"", firstName);
                }
            }
            var searchString = new SearchString
                                   {
                                       Mode = SearchMode.Traditional,
                                       Id = "FL",
                                       Type = SearchType.Free,
                                       Value = string.Format("({0}) w/3 (\"{1}\")", firstName, lastName)
                                   };
            return searchString;
        }

        protected CompanyFilter GetCompaniesFromScreening(string[] fcodes, IControlData controlData)
        {
            //********Replaced old gateway calls with current gateway calls.********//

            if (fcodes == null)
                return null;

            string language = null;

            if (LanguagePreference != null && LanguagePreference.Length > 0)
            {
                language = String.Join(" ", LanguagePreference);

                if (language.ToLower().IndexOf("en") == -1)
                    language += ",en";
            }

            var getReportListExRequest = new GetReportListExRequest
                                             {
                                                 FirstResultToReturn = 0,
                                                 MaxResultsToReturn = fcodes.Length,
                                                 ReturnExistenceFlags = true,
                                                 ReturnReplyItem = false,
                                                 ReturnRestrictors = false,
                                                 SortBy = ReportListSortBy.PublishedDate,
                                                 SortOrder = CompanyProfilesSortOrder.Ascending,
                                                 Category = ReportCategory.Company,
                                                 Fcode = fcodes[0],
                                                 LanguageList = language,
                                                 SymbolCodeScheme = SymbolCodeScheme.FII
                                             };

            getReportListExRequest.Subtype.StringCollection.Add(string.Empty);
            getReportListExRequest.Adoctype.StringCollection.Add("core");

            var serviceResponse = ScreeningService.GetReportListEx(ControlDataManager.Convert(controlData), getReportListExRequest);

            if (serviceResponse.rc != 0)
            {
                throw new DowJonesUtilitiesException(serviceResponse.rc);
            }

            CompanyFilter companyFilter = null;
            object responseObj;
            var responseObjRc = serviceResponse.GetResponse(ServiceResponse.ResponseFormat.Object, out responseObj);
            if (responseObjRc == 0)
            {
                var getReportListExResponse = (GetReportListExResponse) responseObj;

                var reportListResult = getReportListExResponse.GetReportListExResult.ReportListResult;

                var cnew = (Company)reportListResult.ReportCategory;
                companyFilter = new CompanyFilter
                                    {
                                        NewsSearch = cnew.NewsSearch,
                                        Status = new CompanyStatus
                                                     {
                                                         IsNewsCoded = cnew.IsNewsCoded
                                                     },
                                        Fcode = cnew.Fcode
                                    };
                if (cnew.Descriptor.DescriptorCollection != null && cnew.Descriptor.DescriptorCollection.Count > 0)
                    companyFilter.Descriptor = cnew.Descriptor.DescriptorCollection[0];
            }

            return companyFilter;
        }

        protected CompanyFilter GetCompanies(string[] fcodes)
        {
            //Replaced old gateway calls with current gateway calls.

            var lstElements = new List<CompanyElements>
                                  {
                                      CompanyElements.Status
                                  };

            var getCompaniesRequest = new GetCompaniesRequest();
            getCompaniesRequest.CodeCollection.AddRange(fcodes);
            getCompaniesRequest.CodeType = CompanyCodeType.FactivaCompany;
            getCompaniesRequest.ElementsToReturnCollection.AddRange(lstElements);

            if (LanguagePreference != null && LanguagePreference.Length > 0)
                getCompaniesRequest.Language = String.Join(" ", LanguagePreference);

            CompanyFilter companyFilter;
            var serviceResponse = SymbologyCompanyService.GetCompanies(ControlDataManager.Convert(ControlData), getCompaniesRequest);
            if (serviceResponse.rc != 0)
                throw new DowJonesUtilitiesException(serviceResponse.rc);

            object responseObj;
            var responseObjRc = serviceResponse.GetResponse(ServiceResponse.ResponseFormat.Object, out responseObj);
            if (responseObjRc == 0)
            {
                var getCompaniesResponse = (GetCompaniesResponse) responseObj;
                var companyResultSet = getCompaniesResponse.CompanyResultSet;

                companyFilter = new CompanyFilter
                                    {
                                        Fcode = companyResultSet.CompanyResultCollection[0].ResultCompany.Code, 
                                        Descriptor = new DescriptorType
                                                         {
                                                             Value = companyResultSet.CompanyResultCollection[0].ResultCompany.Name.Value, 
                                                             Lang = companyResultSet.CompanyResultCollection[0].ResultCompany.Name.Lang
                                                         }
                                    };
                // TODO : alternative value???
                //companyFilter.NewsSearch = companyResultSet.CompanyResultCollection[0].ResultCompany.; 
                //companyFilter.IsNewsCoded = companyResultSet.CompanyResultCollection[0].ResultCompany.; 
            }
            else
            {
                throw new DowJonesUtilitiesException(responseObjRc);
            }

            return companyFilter;
        }
    }
}