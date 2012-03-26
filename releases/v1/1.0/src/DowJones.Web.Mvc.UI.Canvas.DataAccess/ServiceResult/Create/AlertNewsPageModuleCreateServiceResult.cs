// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AlertNewsPageModuleCreateServiceResult.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using DowJones.Session;
using DowJones.Utilities.Exceptions;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Managers.Common;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Create;
using DALModule = DowJones.Web.Mvc.UI.Models.NewsPages.Modules;
using FactivaModule = Factiva.Gateway.Messages.Assets.Pages.V1_0;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Create
{
    [DataContract(Name = "alertNewsPageModuleCreateServiceResult", Namespace = "")]
    public class AlertNewsPageModuleCreateServiceResult : AbstractServiceResult, ICreateDefinition<AlertsNewsPageModuleCreateRequest>
    {
        [DataMember(Name = "moduleId")]
        public int ModuleId { get; set; }

        public void Create(Factiva.Gateway.Utils.V1_0.ControlData controlData, AlertsNewsPageModuleCreateRequest request, IPreferences preferences)
        {
            var pageAssetsManager = PageAssetsManagerFactory.CreateManager(controlData, preferences.InterfaceLanguage);
            
            ProcessServiceResult(
                MethodBase.GetCurrentMethod(),
                () =>
                    {
                        if (request == null)
                        {
                            throw new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidUpdateRequest);
                        }

                        var manager = new PageListManager(controlData, preferences);                        
                        var alertList = (from alert in request.AlertCollection where alert.IsPrivate && alert.MakePublic select Convert.ToInt32(alert.AlertId)).ToList();
                        manager.MakePersonalAlertsPublic(alertList);

                        var targetModule = SetModuleDefinition(new FactivaModule.AlertsNewspageModule(), request);

                        if (targetModule != null)
                        {
                            pageAssetsManager.AddModuleToEndOfPage(request.PageId, targetModule, null);
                            ModuleId = targetModule.Id;
                            //// PageAssetsManager.DeletePageFromSessionCache(request.CacheKey, CacheScope.Session);
                        }
                    },
                    preferences);
        }

        protected internal FactivaModule.AlertsNewspageModule SetModuleDefinition(FactivaModule.AlertsNewspageModule targetModule, AlertsNewsPageModuleCreateRequest request)
        {
            if (!request.Title.IsNullOrEmpty())
            {
                targetModule.Title = request.Title;
            }

            if (!request.Description.IsNullOrEmpty())
            {
                targetModule.Description = request.Description;
            }

            targetModule.AlertCollection.Clear();

            foreach (var alert in request.AlertCollection)
            {
                targetModule.AlertCollection.Add(new FactivaModule.Alert { AlertID = alert.AlertId });
            }

            return targetModule;
        }
    }
}
