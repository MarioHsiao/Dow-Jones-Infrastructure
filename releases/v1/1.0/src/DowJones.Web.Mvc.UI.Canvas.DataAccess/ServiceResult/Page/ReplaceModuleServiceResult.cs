using System.Reflection;
using System.Runtime.Serialization;
using DowJones.Session;
using DowJones.Utilities.Exceptions;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Managers.Common;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Page
{
    [DataContract(Name = "replaceModuleServiceResult", Namespace = "")]
    public class ReplaceModuleServiceResult : AbstractServiceResult, IReplaceModule<IReplaceModuleReqeust>
    {
        [DataMember(Name = "package")]
        public ReplaceModulePackage Package { get; set; }

        public void ReplaceModule(ControlData controlData, IReplaceModuleReqeust request, IPreferences preferences)
        {
            ProcessServiceResult(
               MethodBase.GetCurrentMethod(),
               () =>
                   {
                       if (!request.IsValid())
                       {
                           throw new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidGetRequest);
                       }

                       var pageListManager = new PageListManager(controlData, preferences);
                       var newsPageModule = pageListManager.ReplaceModuleOnPage(request.PageId, request.ModuleIdToAdd, request.ModuleIdToRemove);
                       Package = new ReplaceModulePackage
                                     {
                                         NewsPageModule = newsPageModule
                                     };
                   },
               preferences);
        }
    }
}
