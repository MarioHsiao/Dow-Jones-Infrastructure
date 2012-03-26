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
    [DataContract(Name = "subscribePageServiceResult", Namespace = "")]
    public class SubscribePageServiceResult : AbstractServiceResult, ISubscribePage<IAddPageByIdPageRequest>
    {
        [DataMember(Name = "pageId")]
        public string PageId;

        public void Subscribe(ControlData controlData, IAddPageByIdPageRequest request, IPreferences preferences)
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
                   PageId = manager.SubscribeToPage(request.PageId, request.Position);

                   return;
               },
               preferences);
        }
    }
}
