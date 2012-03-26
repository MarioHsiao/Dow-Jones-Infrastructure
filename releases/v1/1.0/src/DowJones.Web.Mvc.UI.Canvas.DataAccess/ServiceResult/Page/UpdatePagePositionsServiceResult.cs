// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdatePagePositionsServiceResult.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using System.Runtime.Serialization;
using DowJones.Utilities.Exceptions;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Managers.Common;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Page;


namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Page
{
    [DataContract(Name = "updatePagePositionsServiceResult", Namespace = "")]
    public class UpdatePagePositionsServiceResult : AbstractServiceResult, IUpdatePagePositions<UpdatePagePositionsRequest>
    {
        #region IUpdatePagePositions<UpdatePagePositionsRequest> Members

        public void Update(Factiva.Gateway.Utils.V1_0.ControlData controlData, UpdatePagePositionsRequest request, Session.IPreferences preferences)
        {
            ProcessServiceResult(
                MethodBase.GetCurrentMethod(),
                () =>
                {
                    if (!request.IsValid())
                    {
                        throw new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidUpdatePositionsRequest);
                    }

                    var manager = new PageListManager(controlData, preferences);
                    manager.UpdatePagePositions(request.PagePositions);
                    return;
                },
                preferences);
        }

        #endregion
    }
}
