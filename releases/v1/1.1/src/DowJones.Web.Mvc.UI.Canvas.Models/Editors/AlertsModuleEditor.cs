using System.Collections.Generic;

namespace DowJones.Web.Mvc.UI.Canvas.Editors
{
    public class AlertsModuleEditor : AbstractEditorModel
    {
        [ClientProperty("alerts")]
        public IEnumerable<string> Alerts { get; set; }

        [ClientProperty("allAlerts")]
        public IEnumerable<string> AllAlerts { get; set; }

        [ClientProperty("alertListUrl")]
        public string AlertListUrl { get; set; }

        [ClientProperty("createUpdateAlertModuleUrl")]
        public string CreateUpdateAlertModuleUrl
        {
            get { return Settings.Default.DashboardServiceBaseUrl + Settings.Default.CreateUpdateAlertModuleUrl; }
        }

        [ClientProperty("moduleAlerts")]
        public IEnumerable<int> ModuleAlerts { get; set; }

        [ClientProperty("moduleName")]
        public string ModuleName { get; set; }

        [ClientProperty("moduleDescription")]
        public string ModuleDescription { get; set; }
    }
}
