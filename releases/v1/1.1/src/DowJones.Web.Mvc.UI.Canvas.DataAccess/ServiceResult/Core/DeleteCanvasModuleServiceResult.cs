using System.Collections.Generic;
using System.Reflection;
using DowJones.Session;
using DowJones.Utilities.Exceptions;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Core;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Core
{
    public class DeleteCanvasModuleServiceResult : AbstractServiceResult, IDeleteModule<DeleteCanvasModuleRequest>
    {
        public void DeleteModule(ControlData controlData, DeleteCanvasModuleRequest request, IPreferences preferences)
        {
            ProcessServiceResult(
               MethodBase.GetCurrentMethod(),
               () =>
               {
                   if (!request.IsValid())
                   {
                       throw new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidUpdateRequest);
                   }

                   var pageAssetsManager = PageAssetsManagerFactory.CreateManager(controlData, preferences.InterfaceLanguage);

                   RecordTransaction(
                       "PageAssetsManager.DeleteModules",
                       null,
                       () => pageAssetsManager.DeleteModules(request.PageId, new List<string>(new[] { request.ModuleId })));
               },
               preferences);
        }
    }
}
