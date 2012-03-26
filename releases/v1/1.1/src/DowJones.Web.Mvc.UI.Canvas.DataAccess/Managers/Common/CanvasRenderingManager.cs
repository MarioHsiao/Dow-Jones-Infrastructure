
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CanvasManagementDataServiceManager.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the CanvasManagementDataServiceManager type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Reflection;
using DowJones.Session;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Core;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Core;
using Factiva.Gateway.Messages.Cache.SessionCache.V1_0;
using Factiva.Gateway.Utils.V1_0;
using log4net;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Managers.Common
{
    public class CanvasManagementDataServiceManager : BaseManager
    {
        public CanvasManagementDataServiceManager()
        {
            Logger = LogManager.GetLogger(typeof(CanvasManagementDataServiceManager));
        }

        public UpdateCanvasModulesPositionsServiceResult UpdateCanvasModulesPositions(UpdateCanvasModulesPositionsRequest request, ControlData controlData, IPreferences preferences)
        {
            var serviceResult = new UpdateCanvasModulesPositionsServiceResult();

            ProcessRequest(
                MethodBase.GetCurrentMethod(), 
                serviceResult, 
                controlData,
                preferences,
                (response, manager) =>
                    {
                        manager.UpdateModulePositionsOnPage(request.PageId, request.Modules);
                        manager.DeletePageFromSessionCache(request.CacheKey, CacheScope.Session);
                    });

            return serviceResult;
        }

        public DeleteCanvasModuleServiceResponse DeleteCanvasModuleFromCanvas(DeleteCanvasModuleRequest request, ControlData controlData, IPreferences preferences)
        {
            var serviceResult = new DeleteCanvasModuleServiceResponse();

            ProcessRequest(
                MethodBase.GetCurrentMethod(), 
                serviceResult,
                controlData,
                preferences,
                (response, manager) =>
                    {
                        manager.DeleteModules(request.PageId, new List<string>(new[] { request.ModuleId }));
                        manager.DeletePageFromSessionCache(request.CacheKey, CacheScope.Session);
                    });

            return serviceResult;
        }

        public UpdateCanvasModuleStateServiceResult UpdateCanvasModuleState(UpdateCanvasModuleStateRequest request, ControlData controlData, IPreferences preferences)
        {
            var serviceResult = new UpdateCanvasModuleStateServiceResult();

            ProcessRequest(
                MethodBase.GetCurrentMethod(),
                serviceResult, 
                controlData,
                preferences,
                (response, manager) =>
                    {
                        manager.UpdateModuleState(request.PageId, request.ModuleId, request.State);
                        manager.DeletePageFromSessionCache(request.CacheKey, CacheScope.Session);
                    });

            return serviceResult;
        }

        public UpdateCanvasModulesPropertiesServiceResult UpdateCanvasModulesProperties(UpdateCanvasModulesPropertiesRequest request, ControlData controlData, IPreferences preferences)
        {
            var responseDelegate = new UpdateCanvasModulesPropertiesServiceResult();

            ProcessRequest(
                MethodBase.GetCurrentMethod(), 
                responseDelegate,
                controlData,
                preferences,
                (response, manager) =>
                    {
                        manager.UpdateModule(request.PageId, request.ModuleId, request.Properties);
                        manager.DeletePageFromSessionCache(request.CacheKey, CacheScope.Session);
                    });

            return responseDelegate;
        }
    }
}
