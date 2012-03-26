// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CopyPageServiceResult.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using System.Runtime.Serialization;
using DowJones.Session;
using DowJones.Utilities.Exceptions;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Managers.Common;
using DowJones.Web.Mvc.UI.Models.NewsPages;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;
using Factiva.Gateway.Services.V1_0;
using Factiva.Gateway.V1_0;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Page
{
    /// <summary>
    /// SnapshotPackage enum
    /// </summary>
    [DataContract(Name = "snapshotPackage", Namespace = "")]
    public enum SnapshotPackage
    {
        [EnumMember]
        NoAccess,
        [EnumMember]
        SubscribeOnly,
        [EnumMember]
        FullAccess,
    }

    [DataContract(Name = "addPageByIdServiceResult", Namespace = "")]
    public class AddPageByIdServiceResult : AbstractServiceResult, IAddPageById<IAddPageByIdPageRequest>
    {
        [DataMember(Name = "pageId")]
        public string PageId;

        public void AddPageById(ControlData controlData, IAddPageByIdPageRequest request, IPreferences preferences)
        {
            ProcessServiceResult(
               MethodBase.GetCurrentMethod(),
               () =>
               {
                   if (!request.IsValid())
                   {
                       throw new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidUpdateRequest);
                   }
                   GetUserAuthorizationsResponse getUserAuthorizationResponse = null;
                   RecordTransaction(
                       "MembershipService.GetUserAuthorizations",
                       MethodBase.GetCurrentMethod().Name,
                       () =>
                       {
                           getUserAuthorizationResponse = GetUserAuthorizationsResponseDetails(controlData);
                       });


                   // Copy only when user has full access and the page to be copied is factiva page
                   var manager = new PageListManager(controlData, preferences);
                   var snapshotPackage = GetSnapshotPackageDetails(getUserAuthorizationResponse);
                   if (snapshotPackage == SnapshotPackage.FullAccess)
                   {
                       // get the page
                       var page = manager.GetUserNewsPage(request.PageId);
                       if (page.PublishStatusScope == PublishStatusScope.Global)
                       {
                           CopyPage(page, request.Position, manager);
                       }
                       else
                       {
                           SubscribeToPage(request.PageId, request.Position, manager);
                       }
                   }
                   else
                   {
                       SubscribeToPage(request.PageId, request.Position, manager);
                   }
                   return;
               },
               preferences);
        }

        void SubscribeToPage(string pageId, int position, PageListManager manager)
        {
            RecordTransaction(
                "PageListManager.SubscribeToPage",
                null,
                () =>
                {
                    PageId = manager.SubscribeToPage(pageId, position);
                });
        }

        void CopyPage(NewsPage newsPage, int position, PageListManager manager)
        {
            RecordTransaction(
                "PageListManager.CreateCustomPage",
                null,
                () =>
                    {
                        newsPage.Position = position;
                        PageId = manager.CreateCustomPage(newsPage);
                    });
        }

        /// <summary>
        /// Gets the snapshot package details.
        /// </summary>
        /// <param name="userAuthorizationsResponse">The user authorizations response.</param>
        /// <returns></returns>
        private static SnapshotPackage? GetSnapshotPackageDetails(GetUserAuthorizationsResponse userAuthorizationsResponse)
        {
            SnapshotPackage? snapShot = null;

            var ac1List = userAuthorizationsResponse.AuthorizationMatrix.PAM.ac1;

            if (ac1List != null && ac1List.Count > 0)
            {
                if (ac1List.Exists(match => string.IsNullOrEmpty(match) || (match.Substring(match.LastIndexOf(':') + 1)) == "-1"))
                    snapShot = SnapshotPackage.NoAccess;
                else if (ac1List.Exists(match => (match.Substring(match.LastIndexOf(':') + 1)) == "0"))
                    snapShot = SnapshotPackage.SubscribeOnly;
                else
                    snapShot = SnapshotPackage.FullAccess;
            }

            return snapShot;
        }


        public static GetUserAuthorizationsResponse GetUserAuthorizationsResponseDetails(ControlData controlData)
        {

            var authorizationRequest = new GetUserAuthorizationsRequest();


            //call the factiva method to set the session time out                        
            ServiceResponse serviceResponse = MembershipService.GetUserAuthorizations(controlData, authorizationRequest);

            if (serviceResponse.rc != 0)
            {
                throw new DowJonesUtilitiesException(serviceResponse.rc);
            }

            object responseObj;
            //Gets the reponse as Factiva message object and cast to the respective response object
            var responseObjRc = serviceResponse.GetResponse(ServiceResponse.ResponseFormat.Object, out responseObj);

            if (responseObjRc != 0)
            {
                throw new DowJonesUtilitiesException(responseObjRc);
            }

            return (GetUserAuthorizationsResponse)responseObj;
        }

    }
}
