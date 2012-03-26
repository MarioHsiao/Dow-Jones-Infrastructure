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
                if (newsFilter.author != null)
                {
                    lstSearchString.Add(CreateSearchString("au", GetFilterValue(newsFilter.author), SearchMode.All, SearchType.Controlled));
                }
                if (newsFilter.company != null)
                {
                    var listFCodes = new List<string>();
                    foreach (var comp in newsFilter.company)
                    {
                        if (comp != null && !string.IsNullOrEmpty(comp.code))
                        {
                            listFCodes.Add(comp.code);
                        }
                    }
                    //Seperate out the occur and about compaines
                    var request = new GetCompaniesRequest();
                    request.CodeCollection.AddRange(listFCodes);
                    request.CodeType = CompanyCodeType.FactivaCompany;
                    request.ElementsToReturnCollection = new CompanyElementsToReturnCollection();
                    request.ElementsToReturnCollection.Add(CompanyElements.Status);
                    var serviceResponse = SymbologyCompanyService.GetCompanies(ControlDataManager.Convert(controlData), request);
                    if (serviceResponse.rc != 0)
                        throw new DowJonesUtilitiesException(serviceResponse.rc);

                    var occurCompanies = new List<FilterItem>();
                    var aboutCompanies = new List<FilterItem>();

                    if (serviceResponse != null)
                    {
                        object responseObj;
                        serviceResponse.GetResponse(ServiceResponse.ResponseFormat.Object, out responseObj);
                        var companyResponse = (GetCompaniesResponse)responseObj;
                        if (companyResponse != null && companyResponse.CompanyResultSet != null)
                        {
                            foreach (var company in companyResponse.CompanyResultSet.CompanyResultCollection)
                            {
                                if (company != null && company.ResultCompany != null && company.ResultCompany.CompanyStatus != null)
                                {
                                    if (company.ResultCompany.CompanyStatus.IsNewsCoded)
                                    {
                                        //About company
                                        aboutCompanies.Add(new FilterItem() { code = company.ResultCompany.Code });
                                    }
                                    else if (company.ResultCompany.CompanyStatus.IsOccurrenceCoded)
                                    {
                                        //Occur company
                                        occurCompanies.Add(new FilterItem() { code = company.ResultCompany.Code });
                                    }
                                }
                            }
                        }
                    }
                    if (occurCompanies != null && occurCompanies.Count > 0)
                    {
                        lstSearchString.Add(CreateSearchString("fds:occur", GetFilterValue(occurCompanies.ToArray()), SearchMode.All, SearchType.Controlled));
                    }
                    if (aboutCompanies != null && aboutCompanies.Count > 0)
                    {
                        lstSearchString.Add(CreateSearchString("fds", GetFilterValue(aboutCompanies.ToArray()), SearchMode.All, SearchType.Controlled));
                    }
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
                    lstSearchString.Add(CreateSearchString("sc", GetFilterValue(newsFilter.source), SearchMode.Any, SearchType.Controlled));
                }
                if (newsFilter.keywords != null)
                {
                    lstSearchString.Add(CreateSearchString("keyword", string.Join(" ", newsFilter.keywords), SearchMode.Simple, SearchType.Free));
                }
            }
            return lstSearchString;
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
