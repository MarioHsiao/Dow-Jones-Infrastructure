using System.Collections.Generic;
using System.Linq;
using DowJones.Web.Mvc.UI.Components.Models;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using DowJones.Web.Mvc.UI.Canvas.Editors;

namespace DowJones.Web.Mvc.UI.Canvas.Modules
{
    public class AlertsModule : Module<AlertsNewspageModule>
    {
        private const int MAXALERTSTORETURN = 3;
        private const int MAXHEADLINESTORETURN = 5;
        private const int DEFAULT = 0;
        private const int ACTIVEPAGE = 1;

        public AlertsModule()
        {
            ActivePage = ACTIVEPAGE;
        }

        [ClientProperty("createUpdateAlertModuleUrl")]
        public string CreateUpdateAlertModuleUrl
        {
            get { return Settings.Default.DashboardServiceBaseUrl + Settings.Default.CreateUpdateAlertModuleUrl; }
        }

        [ClientProperty("activePage")]
        public int ActivePage { get; set; }

        [ClientProperty("totalNumberOfAlerts")]
        public int TotalNumberOfAlerts
        {
            get
            {
                if (HasAsset && Asset.AlertCollection != null)
                {
                    return Asset.AlertCollection.Count;
                }
                return DEFAULT;
            }
        }

        [ClientProperty("numberOfPages")]
        public int NumberOfPages
        {
            get { return TotalNumberOfAlerts % MaxAlertsToReturn == DEFAULT ? (TotalNumberOfAlerts / MaxAlertsToReturn) : (TotalNumberOfAlerts / MaxAlertsToReturn) + ACTIVEPAGE; }
        }

        //Alerts
        [ClientProperty("firstAlertToReturn")]
        public int FirstAlertToReturn
        {
            get { return DEFAULT; }
        }

        [ClientProperty("maxAlertsToReturn")]
        public int MaxAlertsToReturn
        {
            get { return MAXALERTSTORETURN; }
        }

        //Headlines
        [ClientProperty("firstHeadlineToReturn")]
        public int FirstHeadlineToReturn
        {
            get { return DEFAULT; }
        }

        [ClientProperty("maxHeadlinesToReturn")]
        public int MaxHeadlinesToReturn
        {
            get { return MAXHEADLINESTORETURN; }
        }

        public IEnumerable<PortalHeadlineListModel> PortalHeadlineLists
        {
            get
            {
                return Enumerable.Repeat(
                     new PortalHeadlineListModel
                     {
                         MaxNumHeadlinesToShow = MaxHeadlinesToReturn,
                         ShowPublicationDateTime = true,
                         DisplayNoResultsToken = true
                     }, MAXALERTSTORETURN);
            }
        }
        [ClientProperty("alertListUrl")]
        public string AlertListUrl
        {
            get { return Settings.Default.AlertsListServiceUrl; }
        }

        public new AlertsModuleEditor Editor
        {
            get
            {
                if ((base.Editor as AlertsModuleEditor) == null)
                    base.Editor = GetDefaultEditor();

                return (AlertsModuleEditor)base.Editor;

            }
            set { base.Editor = value; }
        }

        private AlertsModuleEditor GetDefaultEditor()
        {
            return new AlertsModuleEditor()
            {
                ModuleName = Asset.Title,
                ModuleDescription = Asset.Description,
                AlertListUrl = Settings.Default.DashboardServiceBaseUrl + AlertListUrl,
                ModuleAlerts = GetAllModuleAlerts()
            };
        }

        private IEnumerable<int> GetAllModuleAlerts()
        {
            var alertIds = new List<int>();
            if (TotalNumberOfAlerts > 0)
            {
                alertIds.AddRange(Asset.AlertCollection.Select(alert => int.Parse(alert.AlertID)));
            }
            return alertIds;
        }

    }
}
