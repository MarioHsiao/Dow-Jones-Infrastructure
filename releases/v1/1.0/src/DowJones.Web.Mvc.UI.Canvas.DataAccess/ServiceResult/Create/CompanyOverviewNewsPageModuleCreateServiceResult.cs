// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompanyOverviewNewsPageModuleCreateServiceResult.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Reflection;
using System.Runtime.Serialization;
using DowJones.Utilities.Exceptions;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Interfaces;
using DowJones.Web.Mvc.UI.Canvas.DataAccess.Requests.Create;
using DALModule = DowJones.Web.Mvc.UI.Models.NewsPages.Modules;
using FactivaModule = Factiva.Gateway.Messages.Assets.Pages.V1_0;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.ServiceResult.Create
{
    [DataContract(Name = "companyOverviewNewsPageModuleCreateServiceResult", Namespace = "")]
    public class CompanyOverviewNewsPageModuleCreateServiceResult : AbstractServiceResult, ICreateDefinition<CompanyOverviewNewsPageModuleCreateRequest>
    {
        [DataMember(Name = "moduleId")]
        public int ModuleId { get; set; }

        public void Create(Factiva.Gateway.Utils.V1_0.ControlData controlData, CompanyOverviewNewsPageModuleCreateRequest request, Session.IPreferences preferences)
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

                    var targetModule = SetModuleDefinition(new FactivaModule.CompanyOverviewNewspageModule(), request);

                    if (targetModule != null)
                    {
                        pageAssetsManager.AddModuleToEndOfPage(request.PageId, targetModule, null);
                        ModuleId = targetModule.Id;
                    }
                },
                preferences);
        }

        protected internal FactivaModule.CompanyOverviewNewspageModule SetModuleDefinition(FactivaModule.CompanyOverviewNewspageModule targetModule, CompanyOverviewNewsPageModuleCreateRequest request)
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
            targetModule.FCodeCollection.AddRange(request.FCodeCollection.ToArray());
           
            return targetModule;
        }
    }
}
