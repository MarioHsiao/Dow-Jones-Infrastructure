// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FamilyTreeService.cs" company="">
//   
// </copyright>
// <summary>
//   Family Tree Web Service
// </summary>
// --------------------------------------------------------------------------------------------------------------------


using System;
using System.ComponentModel;
using System.Reflection;
using System.Web.Script.Services;
using System.Web.Services;
using DowJones.Tools.ServiceLayer.WebServices;
using DowJones.Utilities.Ajax.FamilyTree;
using DowJones.Utilities.Exceptions;
using DowJones.Utilities.Loggers;
using DowJones.Utilities.Managers.FamilyTree;
using Factiva.Gateway.Managers;
using Factiva.Gateway.Messages.FCE.DnB.Utilities.V1_0;
using Factiva.Gateway.Messages.FCE.DnB.V1_0;
using Factiva.Gateway.Services.V1_0;
using Factiva.Gateway.Utils.V1_0;
using Factiva.Gateway.V1_0;
using log4net;

namespace DowJones.Tools.WebServices
{
    /// <summary>
    /// Family Tree Web Service
    /// </summary>
    [WebService(Namespace = "DowJones.Utilities.WebServices")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class FamilyTreeService : BaseWebService
    {
        /// <summary>
        /// The m log.
        /// </summary>
        private static readonly ILog MLog = LogManager.GetLogger( typeof (FamilyTreeService ) );

        /// <summary>
        /// The _c data.
        /// </summary>
        private readonly ControlData _cData = ControlDataManager.GetLightWeightUserControlData("joyful", "joyful", "16");

        /// <summary>
        /// The process family tree.
        /// </summary>
        /// <param name="requestDelegate"></param>
        /// <param name="interfaceLanguage">The interface language.</param>
        /// <param name="accessPointCode">The access point code.</param>
        /// <param name="productPrefix">The product prefix.</param>
        /// <returns>The FamilyTreeResponseDelegate Object</returns>
        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public FamilyTreeResponseDelegate ProcessFamilyTree(FamilyTreeRequestDelegate requestDelegate, string interfaceLanguage, string accessPointCode, string productPrefix)
        {
            string dunsNumber = requestDelegate.DunsNumber;
            bool includeBranchLocations = requestDelegate.IncludeBranchLocations;
            
            using (new TransactionLogger(MLog, MethodBase.GetCurrentMethod()))
            {
                var responseDelegate = new FamilyTreeResponseDelegate();
                try
                {
                    if (!string.IsNullOrEmpty(dunsNumber))
                    {
                        // Creating request
                        var getChildrenRequest = new GetChildrenRequest
                                                     {
                                                         code = dunsNumber,
                                                         codeType = CompanyCodeType.DunsNumber,
                                                         includeBranchLocations = includeBranchLocations,
                                                         language = "en"
                                                     };
                        var objResultPage = new ResultPage
                                                {
                                                    firstResult = 1, 
                                                    maxResults = 10000
                                                };

                        getChildrenRequest.resultPage = objResultPage;
                        var objelementsToReturnCollection = new elementsToReturnCollection
                                                                {
                                                                    ElementsToReturn.Address, 
                                                                    ElementsToReturn.CompanyInfo, 
                                                                    ElementsToReturn.Numerics,
                                                                    ElementsToReturn.ParentCompanyInfo, 
                                                                    ElementsToReturn.TaxonomyCodes, 
                                                                    ElementsToReturn.TaxonomyCodesAndDescriptors
                                                                };
                        getChildrenRequest.elementsToReturnCollection = objelementsToReturnCollection;

                        // Getting Response
                        var response = DnbService.GetChildrenEx(_cData, getChildrenRequest);
                        HandleResponse(response);
                        object obj;
                        response.GetResponse(ServiceResponse.ResponseFormat.Object, out obj);
                        var responseObj = obj as GetChildrenResponse;
                        var familyTreeManager = new FamilyTreeManager();
                        responseDelegate.familyTreeDataResult = familyTreeManager.Process(responseObj);
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
            if (r.rc != 0)
            {
                throw new DowJonesUtilitiesException(r.rc);
            }
        }
    }
}