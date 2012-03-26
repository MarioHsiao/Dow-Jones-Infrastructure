using DowJones.Session;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;
using DataAccessModel= DowJones.Web.Mvc.UI.Models.NewsPages.Modules;
using FactivaDataModel = Factiva.Gateway.Messages.Assets.Pages.V1_0;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Assemblers.NewsPages
{
    public class ModuleExAssembler:AbstractPageModuleAssembler,
       IAssembler<AlertsNewspageModule, DataAccessModel.AlertsNewspageModule>,
        IAssembler<CompanyOverviewNewspageModule, DataAccessModel.CompanyOverviewNewspageModule>,
        IAssembler<SyndicationNewspageModule, DataAccessModel.SyndicationNewspageModule>
    {
        public ModuleExAssembler(ControlData controlData, IPreferences preferences) : base(controlData, preferences) {}

        public AlertsNewspageModule Convert(DataAccessModel.AlertsNewspageModule source)
        {
            var module = Initialize(new AlertsNewspageModule(), source) as AlertsNewspageModule;
            if (module != null && source != null)
            {
                //module.HeadlineCount = source.HeadlineCount;
                if (source.AlertIDCollection != null && source.AlertIDCollection.Count > 0)
                {
                    foreach (var alertId in source.AlertIDCollection)
                    {
                        module.AlertCollection.Add(new Alert { AlertID = alertId });
                    }

                }
                // module.TimePeriod = source.TimePeriod;
            }
            return module;
        }

        public CompanyOverviewNewspageModule Convert(DataAccessModel.CompanyOverviewNewspageModule source)
        {
            var module = Initialize(new CompanyOverviewNewspageModule(), source) as CompanyOverviewNewspageModule;
            if (module != null && source != null)
            {
                module.FCodeCollection.AddRange(source.Fcode != null ? source.Fcode.ToArray() : new string[0]);
            }
            return module;
        }

        public SyndicationNewspageModule Convert(DataAccessModel.SyndicationNewspageModule source)
        {
            //DataAccessModel.SyndicationNewspageModule module 
            var module = Initialize(new SyndicationNewspageModule(), source) as SyndicationNewspageModule;
            if (module != null)
            {
                //module.HeadlineCount = source.HeadlineCount;
                foreach (var feed in source.SyndicationFeedIdCollection)
                {
                    module.SyndicationFeedIDCollection.Add(feed);
                }
                // this.SyndicationFeedUriCollection.AddRange(_localMolule.SyndicationFeedCollection);
            }

            return module;
        }
    }
}
