using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Web.Script.Services;
using System.Web.Services;
using DowJones.Tools.ServiceLayer.WebServices;
using DowJones.Tools.Session;
using DowJones.Utilities.Ajax.CompanySearch;
using DowJones.Utilities.Exceptions;
using DowJones.Utilities.Loggers;
using Factiva.Gateway.Messages.Screening.V1_1;
using Factiva.Gateway.Services.V1_0;
using Factiva.Gateway.V1_0;
using log4net;
using Company=DowJones.Utilities.Ajax.CompanySearch.Company;

namespace DowJones.Tools.WebServices
{
    [WebService(Namespace = "DowJones.Utilities.WebServices")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class CompanySearchService : BaseWebService
    {
        private static readonly ILog m_Log = LogManager.GetLogger(typeof(ManageAffiliationService));
        [WebMethod]
        [ScriptMethod]
        public CompanySearchResponseDelegate Process(CompanySearchRequestDelegate requestDelegate, string interfaceLanguage, string accessPointCode, string productPrefix)
        {
            using (new TransactionLogger(m_Log, MethodBase.GetCurrentMethod()))
            {
                var sessionData = new SessionData(accessPointCode, interfaceLanguage, 0, true, productPrefix, string.Empty);
                var responseDelegate = new CompanySearchResponseDelegate();
                try
                {
                    if (!string.IsNullOrEmpty(requestDelegate.SearchString))
                    {
                        // Creating request
                        var request =
                            new GetCompanyScreeningListExRequest
                            {
                                GetHitCounts = false,
                                Currency = "USD",
                                FirstResultToReturn = 0,
                                MaxResultsToReturn = 100,
                                SortOrder = CompanyProfilesSortOrder.Ascending,
                                SortBy = CompanyProfilesSortBy.CompanyName,
                                DisplayOption = ScreeningListDisplayOptions.Standard,
                                Language = interfaceLanguage,
                                CriteriaGroups = new CompanyCriteriaGroups { CriteriaGroupCollection = new List<CompanyCriteriaGroup>() }
                            };
                        request.ContentSets.ContentSetsCollection.Add(ContentSet.Full);
                        var companyCriteriaGroup = new CompanyCriteriaGroup
                        {
                            GroupName = "CompanyNameCriteria",
                            CriteriaOperator = CriteriaOperator.OR
                        };

                        var searchString = new ScreeningStringSearchWithAlias
                                           {
                                               SearchOperator = StringSearchOperatorWithAlias.Contains,
                                               Value = requestDelegate.SearchString
                                           };

                        var companyNameCriteria = new CompanyNameCriteria();
                        companyNameCriteria.CompanyNameCollection.Add(searchString);

                        companyCriteriaGroup.CriteriaCollection.Add(companyNameCriteria);
                        request.CriteriaGroups.CriteriaGroupCollection.Add(companyCriteriaGroup);


                        var response = ScreeningService.GetCompanyScreeningListEx(sessionData.SessionBasedControlData, request);
                        HandleResponse(response);
                        object obj;
                        response.GetResponse(ServiceResponse.ResponseFormat.Object, out obj);
                        var responseObj = obj as GetCompanyScreeningListExResponse;
                        if (responseObj != null &&
                            responseObj.CompanyScreeningListResult != null &&
                            responseObj.CompanyScreeningListResult.Companies != null &&
                            responseObj.CompanyScreeningListResult.Companies.CompanyCollection != null)
                        {
                            foreach (var c in responseObj.CompanyScreeningListResult.Companies.CompanyCollection)
                            {
                                responseDelegate.Companies.Add(new Company { code = c.Fcode, name = c.CompanyName });
                            }

                        }
                    }
                }
                catch (DowJonesUtilitiesException rEx)
                {
                    UpdateAjaxDelegate(rEx, responseDelegate);
                }
                catch (Exception exception)
                {
                    UpdateAjaxDelegate(new DowJonesUtilitiesException(exception, -1), responseDelegate);
                }
                return responseDelegate;
            }
        }

        private static void HandleResponse(ServiceResponse r)
        {
            // 410152 is nothing found
            if (r.rc != 0 && r.rc != 410152)
            {
                throw new DowJonesUtilitiesException(r.rc);
            }
        }
    }
}
