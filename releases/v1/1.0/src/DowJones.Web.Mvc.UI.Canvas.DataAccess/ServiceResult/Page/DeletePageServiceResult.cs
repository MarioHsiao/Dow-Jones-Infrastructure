// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnsubscribePageServiceResult.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using System.Runtime.Serialization;
using DowJones.Session;
using DowJones.Utilities.Exceptions;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Managers.Common;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;
using DowJones.Web.Mvc.UI.Models.NewsPages;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Page
{
    [DataContract(Name = "deletePageServiceResult", Namespace = "")]
    public class DeletePageServiceResult : AbstractServiceResult, IDeletePage<IDeletePageRequest>
    {
        public void Delete(ControlData controlData, IDeletePageRequest request, IPreferences preferences)
        {
            ProcessServiceResult(
               MethodBase.GetCurrentMethod(),
               () =>
               {
                   if (!request.IsValid())
                   {
                       throw new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidUpdateRequest);
                   }
                   var manager = new PageListManager(controlData, preferences);
                   NewsPage newsPage = null;
                   if (request.PageAccessScope == AccessScope.UnSpecified)
                   {
                       RecordTransaction(
                           "PageListManager.GetUserNewsPage",
                           null,
                           () =>
                               {
                                   newsPage = manager.GetUserNewsPage(request.PageId);
                               });
                       if (newsPage != null)
                           request.PageAccessScope = newsPage.AccessScope;
                   }

                   switch (request.PageAccessScope)
                   {
                       case AccessScope.SubscribedByUser:
                           RecordTransaction(
                          "PageListManager.UnSubscribeToPage",
                          null,
                          () =>
                          {
                              manager.UnSubscribeToPage(request.PageId);
                          });
                           
                           break;
                       case AccessScope.AssignedToUser:
                           RecordTransaction(
                               "PageListManager.RemoveAssignedPage",
                               null,
                               () =>
                                   {
                                       manager.RemoveAsssigedPage(request.PageId);
                                   });
                           
                           break;
                       case AccessScope.OwnedByUser:
                           RecordTransaction(
                               "PageListManager.DeleteUserNewsPage",
                               null,
                               () =>
                               {
                                   manager.DeleteUserNewsPage(request.PageId);
                               });
                           
                           break;
                   }
               },
               preferences);
        }
    }
}
