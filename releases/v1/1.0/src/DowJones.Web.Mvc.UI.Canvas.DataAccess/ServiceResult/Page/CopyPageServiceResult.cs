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
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;
using System.Collections.Generic;
using DowJones.Web.Mvc.UI.Models.NewsPages;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Page
{
    [DataContract(Name = "copyPageServiceResult", Namespace = "")]
    public class CopyPageServiceResult : AbstractServiceResult, ICopyPageAssignment<IAddPageByIdPageRequest>
    {
        [DataMember(Name="pageId")]
        public string PageId;

        public void CopyPage(ControlData controlData, IAddPageByIdPageRequest request, IPreferences preferences)
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
                   PageId = manager.CopyPage(request.PageId, request.Position);

                   return;
               },
               preferences);
        }
    }
}
