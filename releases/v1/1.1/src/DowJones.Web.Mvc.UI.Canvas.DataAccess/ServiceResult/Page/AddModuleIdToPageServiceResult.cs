using System.Reflection;
using System.Runtime.Serialization;
using DowJones.Session;
using DowJones.Utilities.Exceptions;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Managers.Common;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;
using System.Collections.Generic;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Page
{
    [DataContract(Name = "addModuleIdToPageServiceResult", Namespace = "")]
    public class AddModuleIdToPageServiceResult : AbstractServiceResult, IAddModuleToPage<IAddModuleToPageRequest>
    {
        public void AddModuleToPage(ControlData controlData, IAddModuleToPageRequest request, IPreferences preferences)
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
                   var moduleIdToAdd = new List<KeyValuePair<int, int>>();
                   moduleIdToAdd.Add(new KeyValuePair<int, int>(int.Parse(request.ModuleId), 0));
                   manager.AddModuleIdsToPage(request.PageId, moduleIdToAdd);

                   return;
               },
               preferences);
        }
    }
}
