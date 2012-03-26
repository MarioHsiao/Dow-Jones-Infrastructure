// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompanyOverviewNewsPageModuleUpdateServiceResult.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using DowJones.Session;
using DowJones.Utilities.Exceptions;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Update;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using System.Runtime.Serialization;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Update
{
    [DataContract(Name = "companyOverviewNewsPageModuleUpdateServiceResult", Namespace = "")]
    public class CompanyOverviewNewsPageModuleUpdateServiceResult : AbstractServiceResult, IUpdateDefinition<CompanyOverviewNewsPageModuleUpdateRequest>
    {
        #region Implementation of IUpdateDefinition<in CompanyOverviewNewsPageModuleUpdateRequest>

        public void Update(ControlData controlData, CompanyOverviewNewsPageModuleUpdateRequest request, IPreferences preferences)
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

                    var targetModule = pageAssetsManager.GetModuleById(request.PageId, request.ModuleId) as CompanyOverviewNewspageModule;

                    if (targetModule != null)
                    {
                        pageAssetsManager.UpdateModulesOnPage(request.PageId, UpdateModuleDefinition(targetModule, request));
                    }
                },
                preferences);
        }

        #endregion

        protected internal CompanyOverviewNewspageModule UpdateModuleDefinition(CompanyOverviewNewspageModule targetModule, CompanyOverviewNewsPageModuleUpdateRequest request)
        {
            if (!request.Title.IsNullOrEmpty())
            {
                targetModule.Title = request.Title;
            }

            if (!request.Description.IsNullOrEmpty())
            {
                targetModule.Description = request.Description;
            }

            targetModule.FCodeCollection.Clear();

            foreach (var fcode in request.FCodeCollection)
            {
                targetModule.FCodeCollection.Add(fcode);
            }

            return targetModule;
        }
    }
}
