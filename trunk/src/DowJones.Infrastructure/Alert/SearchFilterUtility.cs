using System.Linq;
using Factiva.Gateway.Messages.Search.V2_0;
using Factiva.Gateway.Messages.Track.V1_0;
using System.Collections.Generic;
using Factiva.Gateway.Services.V1_0;
using Factiva.Gateway.Messages.Symbology.Company.V1_0;
using DowJones.Session;
using DowJones.Exceptions;
using Factiva.Gateway.V1_0;
using Factiva.Gateway.Messages.Symbology.V1_0;

namespace DowJones.Infrastructure.Alert 
{
    internal class SearchFilterUtility
    {

        private SearchFilterUtility()
        {
        }

        public static List<SearchString> ProcessNewsFilters(NewsFilters newsFilter, IControlData controlData)
        {
            var lstSearchString = new List<SearchString>();
            if (newsFilter != null)
            {
                if (newsFilter.company != null || newsFilter.companyExcluded != null)
                {
                    //TODO: make a single call to Symbology
                    if (newsFilter.company != null && newsFilter.company.Length > 0)
                    {
                        var occurCompanies = new List<FilterItem>();
                        var aboutCompanies = new List<FilterItem>();
                        GetCompanies(newsFilter.company, controlData, ref aboutCompanies, ref occurCompanies);
                        if (aboutCompanies.Count > 0)
                            lstSearchString.Add(CreateSearchString("fds", GetFilterValue(aboutCompanies.ToArray()), SearchMode.All, SearchType.Controlled));
                        if (occurCompanies.Count > 0)
                            lstSearchString.Add(CreateSearchString("fds:occur", GetFilterValue(occurCompanies.ToArray()), SearchMode.All, SearchType.Controlled));
                        
                    }
                    if (newsFilter.companyExcluded != null && newsFilter.companyExcluded.Length > 0)
                    {
                        var occurCompanies = new List<FilterItem>();
                        var aboutCompanies = new List<FilterItem>();
                        GetCompanies(newsFilter.companyExcluded, controlData, ref aboutCompanies, ref occurCompanies);
                        if (aboutCompanies.Count > 0)
                            lstSearchString.Add(CreateSearchString("fds", GetFilterValue(aboutCompanies.ToArray()), SearchMode.None, SearchType.Controlled));
                        if (occurCompanies.Count > 0)
                            lstSearchString.Add(CreateSearchString("fds:occur", GetFilterValue(occurCompanies.ToArray()), SearchMode.None, SearchType.Controlled));
                    }
                }
                if (newsFilter.author != null)
                {
                    lstSearchString.Add(CreateSearchString("au", GetFilterValue(newsFilter.author), SearchMode.All, SearchType.Controlled));
                }

                if (newsFilter.region != null)
                {
                    lstSearchString.Add(CreateSearchString("re", GetFilterValue(newsFilter.region), SearchMode.All, SearchType.Controlled));
                }
                if (newsFilter.executive != null)
                {
                    lstSearchString.Add(CreateSearchString("pe", GetFilterValue(newsFilter.executive), SearchMode.All, SearchType.Controlled));
                }
                if (newsFilter.industry != null)
                {
                    lstSearchString.Add(CreateSearchString("in", GetFilterValue(newsFilter.industry), SearchMode.All, SearchType.Controlled));
                }
                if (newsFilter.newsSubject != null)
                {
                    lstSearchString.Add(CreateSearchString("ns", GetFilterValue(newsFilter.newsSubject), SearchMode.All, SearchType.Controlled));
                }
                if (newsFilter.source != null)
                {
                    lstSearchString.Add(CreateSearchString("rst", GetFilterValue(newsFilter.source), SearchMode.Any, SearchType.Controlled));
                }
                if (newsFilter.keywords != null)
                {
                    lstSearchString.Add(CreateSearchString("keyword", string.Join(" ", newsFilter.keywords), SearchMode.Simple, SearchType.Free));
                }
               
                if (newsFilter.authorExcluded != null)
                {
                    lstSearchString.Add(CreateSearchString("au", GetFilterValue(newsFilter.authorExcluded), SearchMode.None, SearchType.Controlled));
                }
                if (newsFilter.regionExcluded != null)
                {
                    lstSearchString.Add(CreateSearchString("re", GetFilterValue(newsFilter.regionExcluded), SearchMode.None, SearchType.Controlled));
                }
                if (newsFilter.executiveExcluded != null)
                {
                    lstSearchString.Add(CreateSearchString("pe", GetFilterValue(newsFilter.executiveExcluded), SearchMode.None, SearchType.Controlled));
                }
                if (newsFilter.industryExcluded != null)
                {
                    lstSearchString.Add(CreateSearchString("in", GetFilterValue(newsFilter.industryExcluded), SearchMode.None, SearchType.Controlled));
                }
                if (newsFilter.newsSubjectExcluded != null)
                {
                    lstSearchString.Add(CreateSearchString("ns", GetFilterValue(newsFilter.newsSubjectExcluded), SearchMode.None, SearchType.Controlled));
                }
                if (newsFilter.sourceExcluded != null)
                {
                    lstSearchString.Add(CreateSearchString("rst", GetFilterValue(newsFilter.sourceExcluded), SearchMode.None, SearchType.Controlled));
                }
                
            }
            return lstSearchString;
        }


        private static void GetCompanies(FilterItem[] companies, IControlData controlData, ref List<FilterItem> aboutCompanies, ref List<FilterItem> occurCompanies)
        {
            if (companies == null || companies.Length <= 0) return;
            var request = new GetCompaniesRequest();
            var listFCodes = new List<string>();
            listFCodes.AddRange(from comp in companies where comp != null && !string.IsNullOrEmpty(comp.code) select comp.code);
            
            request.CodeCollection.AddRange(listFCodes);
          
            request.CodeType = CompanyCodeType.FactivaCompany;
            request.ElementsToReturnCollection = new CompanyElementsToReturnCollection { CompanyElements.Status };
            var serviceResponse = SymbologyCompanyService.GetCompanies(ControlDataManager.Convert(controlData), request);
            if (serviceResponse.rc != 0)
                throw new DowJonesUtilitiesException(serviceResponse.rc);

            object responseObj;
            serviceResponse.GetResponse(ServiceResponse.ResponseFormat.Object, out responseObj);
            var companyResponse = (GetCompaniesResponse)responseObj;

            
            if (companyResponse != null && companyResponse.CompanyResultSet != null)
            {
                foreach (var company in companyResponse.CompanyResultSet.CompanyResultCollection)
                {
                    if (company == null || company.ResultCompany == null || company.ResultCompany.CompanyStatus == null) continue;
                    if (company.ResultCompany.CompanyStatus.IsNewsCoded)
                    {
                        //About company
                        aboutCompanies.Add( new FilterItem { code = company.ResultCompany.Code });
                    }
                    else if (company.ResultCompany.CompanyStatus.IsOccurrenceCoded)
                    {
                        //Occur company
                        occurCompanies.Add(new FilterItem { code = company.ResultCompany.Code });
                    }
                }
            }
        }

        public static SearchString GetFreeText(string freeText)
        {
            return new SearchString
            {
                Id = "FreeText",
                Type = SearchType.Free,
                Value = freeText,
                Mode = SearchMode.Simple
            };
        }

        public static SearchString GetLanguageFilter(string contentLanguage)
        {
            if (!string.IsNullOrEmpty(contentLanguage))
            {
                contentLanguage = contentLanguage.Replace(",", " ");
                var searchString = new SearchString
                {
                    Id = "la",
                    Scope = "la",
                    Type = SearchType.Controlled,
                    Mode = SearchMode.Any,
                    Filter = true,
                    Value = contentLanguage
                };
                return searchString;
            }
            return null;
        }

        private static SearchString CreateSearchString(string id, string value, SearchMode searchMode, SearchType searchType)
        {
            var searchString = new SearchString
            {
                Mode = searchMode,
                Id = id,
                Value = value
            };
            if (id != "keyword")
            {
                searchString.Scope = id;
                searchString.Filter = true;
            }
            searchString.Type = searchType;
            searchString.Filter = true;
            searchString.Validate = false;
            return searchString;
        }

        private static string GetFilterValue(FilterItem[] filterItem)
        {
            var value = "";
            if (filterItem != null && filterItem.Length > 0)
            {
                foreach (var item in filterItem)
                    value += item.code + " ";

                value = value.Substring(0, value.Length - 1);
            }
            return value;
        }

        private static string GetFilterValue(FilterSourceItem[] filterItem)
        {
            var value = "";
            if (filterItem != null && filterItem.Length > 0)
            {
                foreach (var item in filterItem)
                    value += item.code + " ";

                value = value.Substring(0, value.Length - 1);
            }
            return value;
        }
    }
}
