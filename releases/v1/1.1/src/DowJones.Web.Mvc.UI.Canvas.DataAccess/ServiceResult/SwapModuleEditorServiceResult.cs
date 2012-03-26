// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SwapModuleEditorServiceResult.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using DowJones.Session;
using DowJones.Utilities.Exceptions;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Managers.Common;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Packages;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests;
using DowJones.Web.Mvc.UI.Models.Common;
using DowJones.Web.Mvc.UI.Models.NewsPages;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult
{
    [DataContract(Name = "swapModuleEditorServiceResult", Namespace = "")]
    public class SwapModuleEditorServiceResult : AbstractServiceResult, IPopulate<SwapModuleEditorRequest>
    {
        [DataMember(Name = "package")]
        public SwapModuleEditorPackage Package { get; set; }

        public void Populate(ControlData controlData, SwapModuleEditorRequest request, IPreferences preferences)
        {
            ProcessServiceResult(
               MethodBase.GetCurrentMethod(),
               () =>
               {
                   if (request == null || !request.IsValid())
                   {
                       throw new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidGetRequest);
                   }

                   GetData(request, controlData, preferences);
               },
               preferences);
        }

        private static List<SwapModuleEditorModule> ConvertToSwapModuleEditorCollection(IEnumerable<ModuleIdByMetadata> moduleIdByMetadataList)
        {
            if (moduleIdByMetadataList == null)
            {
                return null;
            }

            return moduleIdByMetadataList.Select(moduleIdByMetadata => new SwapModuleEditorModule
            {
                Id = moduleIdByMetadata.ModuleId.ToString(),
                Name = moduleIdByMetadata.MetaData.MetaDataDescriptor
            }).OrderBy(swapModule => swapModule.Name).ToList();
        }
   
        private void GetData(SwapModuleEditorRequest request, ControlData controlData, IPreferences preferences)
        {
            var pageListManager = new PageListManager(controlData, preferences);
            List<ModuleIdByMetadata> industryModules = null, regionModules = null;

            RecordTransaction(
                "PageAssetsManager.GetModulesByModuleType - industry",
                null,
                manager =>
                {
                    industryModules = pageListManager.GetModulesByModuleType(request.ModuleType, MetaDataType.Industry);
                },
                pageListManager);

            RecordTransaction(
                "PageAssetsManager.GetModulesByModuleType - region",
                null,
                manager =>
                {
                    regionModules = pageListManager.GetModulesByModuleType(request.ModuleType, MetaDataType.Geographic);
                },
                pageListManager);

            Package = new SwapModuleEditorPackage
                          {
                              Industries = ConvertToSwapModuleEditorCollection(industryModules),
                              Regions = ConvertToSwapModuleEditorCollection(regionModules)
                          };
        }
    }
}
