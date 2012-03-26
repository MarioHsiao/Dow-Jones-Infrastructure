// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SyndicationNewsPageModuleUpdateServiceResult.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using System.Runtime.Serialization;
using DowJones.Session;
using DowJones.Utilities.Exceptions;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Update;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Update
{
    [DataContract(Name = "syndicationNewsPageModuleUpdateServiceResult", Namespace = "")]
    public class SyndicationNewsPageModuleUpdateServiceResult : AbstractServiceResult, IUpdateDefinition<SyndicationNewsPageModuleUpdateRequest>
    {
        #region Implementation of IUpdateDefinition

        public void Update(ControlData controlData, SyndicationNewsPageModuleUpdateRequest request, IPreferences preferences)
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

                    var targetModule = pageAssetsManager.GetModuleById(request.PageId, request.ModuleId) as SyndicationNewspageModule;

                    if (targetModule != null)
                    {
                        pageAssetsManager.UpdateModulesOnPage(request.PageId, UpdateModuleDefinition(targetModule, request));
                    }
                },
                preferences);
        }

        #endregion

        protected internal SyndicationNewspageModule UpdateModuleDefinition(SyndicationNewspageModule targetModule, SyndicationNewsPageModuleUpdateRequest request)
        {
            if (!request.Title.IsNullOrEmpty())
            {
                targetModule.Title = request.Title;
            }

            if (!request.Description.IsNullOrEmpty())
            {
                targetModule.Description = request.Description;
            }

            targetModule.SyndicationFeedIDCollection.Clear();

            foreach (var syndicationID in request.SyndicationIdCollection)
            {
                targetModule.SyndicationFeedIDCollection.Add(syndicationID);
            }

            return targetModule;
        }
    }
}
