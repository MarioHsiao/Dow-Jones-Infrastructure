// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AlertNewsPageModuleUpdateServiceResult.cs" company="Dow Jones">
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
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Update;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Update
{
    [DataContract(Name = "alertNewsPageModuleUpdateServiceResult", Namespace = "")]
    public class AlertNewsPageModuleUpdateServiceResult : AbstractServiceResult, IUpdateDefinition<AlertsNewsPageModuleUpdateRequest>
    {
        #region Implementation of IUpdateDefinition<in AlertsNewsPageModuleUpdateRequest>

        public void Update(ControlData controlData, AlertsNewsPageModuleUpdateRequest request, IPreferences preferences)
        {
            var pageAssetsManager = PageAssetsManagerFactory.CreateManager(controlData, preferences.InterfaceLanguage);

            ProcessServiceResult(
                MethodBase.GetCurrentMethod(),
                () =>
                {
                    if (request == null || !request.IsValid())
                    {
                        throw new DowJonesUtilitiesException(DowJonesUtilitiesException.InvalidUpdateRequest);
                    }

                    var manager = new PageListManager(controlData, preferences);
                    var alertList = (from alert in request.AlertCollection where alert.IsPrivate && alert.MakePublic select Convert.ToInt32(alert.AlertId)).ToList();
                    manager.MakePersonalAlertsPublic(alertList);


                    var targetModule = pageAssetsManager.GetModuleById(request.PageId, request.ModuleId) as AlertsNewspageModule;

                    if (targetModule != null)
                    {
                        pageAssetsManager.UpdateModulesOnPage(request.PageId, UpdateModuleDefinition(targetModule, request));
                        //// PageAssetsManager.DeletePageFromSessionCache(request.CacheKey, CacheScope.Session);
                    }
                },
                preferences);
        }

        #endregion

        protected internal AlertsNewspageModule UpdateModuleDefinition(AlertsNewspageModule targetModule, AlertsNewsPageModuleUpdateRequest request)
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
                targetModule.AlertCollection.Add(new Alert { AlertID = alert.AlertId });
            }

            return targetModule;
        }
    }
}
