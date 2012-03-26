// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeclinePageServiceResult.cs" company="Dow Jones">
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

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Page
{
    [DataContract(Name = "declinePageServiceResult", Namespace = "")]
    public class DeclinePageServiceResult : AbstractServiceResult, IDeletePageAssignment<IPageRequest>
    {
        public void DeleteAssignment(ControlData controlData, IPageRequest request, IPreferences preferences)
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
                   manager.RemoveAsssigedPage(request.PageId);

                   return;
               },
               preferences);
        }
    }
}
