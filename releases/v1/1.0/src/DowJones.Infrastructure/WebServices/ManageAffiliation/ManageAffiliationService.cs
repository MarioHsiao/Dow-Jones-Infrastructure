using System;
using System.ComponentModel;
using System.Reflection;
using System.Web.Script.Services;
using System.Web.Services;
using DowJones.Tools.ServiceLayer.WebServices;
using DowJones.Tools.Session;
using DowJones.Utilities.Ajax.ManageAffiliation;
using DowJones.Utilities.Exceptions;
using DowJones.Utilities.Loggers;
using Factiva.Gateway.Messages.RelationshipMapping.V1_0;
using Factiva.Gateway.Messages.Screening.V1_1;
using Factiva.Gateway.Services.V1_0;
using Factiva.Gateway.V1_0;
using log4net;

namespace DowJones.Tools.WebServices
{
    [WebService(Namespace = "DowJones.Utilities.WebServices")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class ManageAffiliationService : BaseWebService
    {
        private static readonly ILog m_Log = LogManager.GetLogger(typeof(ManageAffiliationService));
        [WebMethod]
        [ScriptMethod]
        public ManageAffiliationResponseDelegate Process(ManageAffiliationRequestDelegate requestDelegate, string interfaceLanguage, string accessPointCode, string productPrefix)
        {
            using (new TransactionLogger(m_Log, MethodBase.GetCurrentMethod()))
            {
                var sessionData = new SessionData(accessPointCode, interfaceLanguage, 0, true, productPrefix, string.Empty);
                var responseDelegate = new ManageAffiliationResponseDelegate();
                try
                {
                    switch (requestDelegate.Action)
                    {
                        case ManageAffiliationAction.Add:
                            #region Add new affiliation
                            var affItem = new Affiliation
                            {
                                IsMyself = false,
                                IsPublic = !requestDelegate.IsPrivate,
                                Strength = requestDelegate.Strength * 20,
                                LastName = requestDelegate.LastName,
                                FirstName = requestDelegate.FirstName,
                                DjPersonCode = requestDelegate.ExecCode
                            };

                            var addRequest = new AddAffiliationRequest
                            {
                                AffiliationCollection = new AffiliationCollection { affItem }
                            };
                            HandleResponse(RelationshipMappingService.AddAffiliation(sessionData.SessionBasedControlData, addRequest));
                            break;
                            #endregion
                        case ManageAffiliationAction.Update:
                            #region Update affiliation
                            // First, get affiliation by the given id
                            var _req = new GetAffiliationByIdRequest
                            {
                                AffiliateIDCollection = new AffiliateIDCollection { requestDelegate.AffId },
                                IncludeBiography = IncludeBiography.IncludeAll
                            };
                            var serviceResponse = RelationshipMappingService.GetAffiliationById(sessionData.SessionBasedControlData, _req);
                            HandleResponse(serviceResponse);

                            object obj;
                            serviceResponse.GetResponse(ServiceResponse.ResponseFormat.Object, out obj);
                            var responseObj = (GetAffiliationByIdResponse)obj;

                            // Now update the affiliation
                            var request =
                                new UpdateAffiliationRequest
                                {
                                    AffiliationCollection =
                                        new AffiliationCollection
                                            {
                                                responseObj.AffiliationResponse.AffiliationResultSet.AffiliationResultCollection[0].Affiliation
                                            }
                                };
                            request.AffiliationCollection[0].Strength = requestDelegate.Strength * 20;
                            request.AffiliationCollection[0].IsPublic = !requestDelegate.IsPrivate;
                            HandleResponse(RelationshipMappingService.UpdateAffiliation(sessionData.SessionBasedControlData, request));
                            break;
                            #endregion
                        case ManageAffiliationAction.GetAffiliationId:
                            #region Get affiliation id, if exists, based on given executive code
                            // First, get all currently affiliated executives
                            var getAffiliationListResponse =
                                RelationshipMappingService.GetAffiliationList(sessionData.SessionBasedControlData,
                                                                              new GetAffiliationListRequest
                                                                              {
                                                                                  IncludeBiography = IncludeBiography.IncludeNone,
                                                                                  FiltersCollection = new FiltersCollection { AffiliationListFilter.Personal },
                                                                                  MaxResults = 500
                                                                              });
                            HandleResponse(getAffiliationListResponse);

                            object obj1;
                            getAffiliationListResponse.GetResponse(ServiceResponse.ResponseFormat.Object, out obj1);
                            var responseObj1 = (GetAffiliationListResponse)obj1;

                            foreach (var aff in responseObj1.AffiliationListResponse.AffiliationListResultSet.AffiliationCollection)
                            {
                                if (aff.DjPersonCode != requestDelegate.ExecCode)
                                    continue;
                                responseDelegate.AffId = aff.Id;
                                responseDelegate.Strength = aff.Strength / 20;
                                responseDelegate.FirstName = aff.FirstName;
                                responseDelegate.LastName = aff.LastName;
                                responseDelegate.IsPrivate = !aff.IsPublic;
                                break;
                            }

                            #region If no affiliation found, get first/last name by exec code
                            if (string.IsNullOrEmpty(responseDelegate.AffId))
                            {
                                var req = new GetReportListExRequest
                                {
                                    Fcode = requestDelegate.ExecCode,
                                    Adoctype = new ArrayOfString(),
                                };
                                req.Adoctype.StringCollection.Add("core");
                                req.Category = ReportCategory.Executive;
                                req.SymbolCodeScheme = SymbolCodeScheme.FII;
                                req.FirstResultToReturn = 0;
                                req.MaxResultsToReturn = 1;
                                req.LanguageList = "en";
                                req.SortBy = ReportListSortBy.PublishedDate;
                                req.SortOrder = CompanyProfilesSortOrder.Descending;
                                req.ReturnReplyItem = true;
                                req.ReturnRestrictors = false;
                                req.ReturnExistenceFlags = true;
                                var getReportListExResponse =
                                    ScreeningService.GetReportListEx(sessionData.SessionBasedControlData, req);
                                HandleResponse(getReportListExResponse);
                                object obj2;
                                getReportListExResponse.GetResponse(ServiceResponse.ResponseFormat.Object, out obj2);
                                var responseObj2 = (GetReportListExResponse)obj2;
                                if (responseObj2 != null && responseObj2.GetReportListExResult != null)
                                {
                                    var executive =
                                        (Executive)responseObj2.GetReportListExResult.ReportListResult.ReportCategory;
                                    if (string.IsNullOrEmpty(executive.FirstName) && string.IsNullOrEmpty(executive.LastName))
                                    {
                                        if (string.IsNullOrEmpty(executive.CompleteName))
                                        {
                                            //NN: Throw a better exception, with error code?
                                            throw new Exception("Executive with given id does not have any names provided.");
                                        }

                                        responseDelegate.LastName = executive.CompleteName;
                                    }
                                    else
                                    {
                                        responseDelegate.FirstName = executive.FirstName ?? "";
                                        responseDelegate.LastName = executive.LastName ?? "";
                                    }
                                }
                            }
                            #endregion
                            break;
                            #endregion
                    }
                    return responseDelegate;
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
            if (r.rc != 0)
                throw new DowJonesUtilitiesException(r.rc);
        }
    }
}
