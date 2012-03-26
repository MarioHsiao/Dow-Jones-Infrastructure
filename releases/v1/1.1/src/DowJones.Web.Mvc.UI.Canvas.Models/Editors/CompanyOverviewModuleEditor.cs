using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DowJones.Web.Mvc.UI.Canvas.Editors
{
    public class CompanyOverviewModuleEditor : AbstractEditorModel
    {
        [ClientProperty("moduleId")]
        public virtual int ModuleId { get; set; }

        [ClientProperty("moduleName")]
        public string ModuleName { get; set; }

        [ClientProperty("moduleDescription")]
        public string ModuleDescription { get; set; }

        [ClientProperty("suggestServiceUrl")]
        public virtual string SuggestServiceUrl { get; set; }

        [ClientProperty("createUpdateCompanyOverviewModuleUrl")]
        public virtual string CreateUpdateCompanyOverviewModuleUrl
        {
            get
            {
                return Settings.Default.DashboardServiceBaseUrl + Settings.Default.CreateUpdateCompanyOverviewModuleUrl;
            }
        
        }
    }
}
