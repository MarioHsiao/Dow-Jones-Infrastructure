using System.Collections.Generic;
using DowJones.Web.Mvc.UI.Canvas.Properties;

namespace DowJones.Web.Mvc.UI.Canvas.Editors
{
    public class SyndicationModuleEditor : AbstractEditorModel
    {
        [ClientProperty("moduleName")]
        public string ModuleName { get; set; }

        [ClientProperty("moduleId")]
        public int ModuleId { get; set; }

        [ClientProperty("canvasId")]
        public string CanvasId { get; set; }

        [ClientProperty("maximumFeeds")]
        public int MaximumFeeds
        {
            get { return SyndicationModuleSettings.Default.MaximumFeeds; }
        }

        [ClientProperty("moduleDescription")]
        public  string ModuleDescription { get; set; }


        public override string WebServiceUrl
        {
            get
            {
                return Settings.Default.DashboardServiceBaseUrl + SyndicationModuleSettings.Default.ValidateSyndicationUrl;
            }
        }

        [ClientProperty("moduleServiceUrl")]
        public string ModuleServiceUrl
        {
            get { return Settings.Default.DashboardServiceBaseUrl + SyndicationModuleSettings.Default.SyndicationModuleUrl; }
        }


    }

}
