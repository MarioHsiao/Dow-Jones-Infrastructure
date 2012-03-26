using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using DowJones.Web.Mvc.UI.Canvas.Editors;

namespace DowJones.Web.Mvc.UI.Canvas.Models
{
    public class CompanyOverviewModule : Module<CompanyOverviewNewspageModule>
    {
        private const int DefaultMaxHeadlinesToShow = 5;

        #region ..:: Client Properties ::..
        
        [ClientProperty("maxResultsToReturn")]
        public int MaxResultsToReturn
        {
            get
            {
                // TODO: Read from config
                return 5;
            }
        }

        [ClientProperty("maxHeadlinesToShow")]
        public int MaxHeadlinesToShow
        {
            get { return DefaultMaxHeadlinesToShow; }
            set { }
        }

        [ClientProperty("suggestServiceUrl")]
        public string SuggestServiceUrl
        {
            get { return Settings.Default.SuggestServiceUrl; }
        }

        public new CompanyOverviewModuleEditor Editor
        {
            get
            {
                if ((base.Editor as CompanyOverviewModuleEditor) == null)
                    base.Editor = GetDefaultEditor();

                return (CompanyOverviewModuleEditor)base.Editor;

 }
            set { base.Editor = value; }
        }

        #endregion

        #region ..:: Client Event Handlers ::..

        /// <summary>
        /// Gets or sets the client side OnHeadlineClick event handler.
        /// </summary>
        /// <value>The OnHeadlineClick event handler name.</value>
        [ClientEventHandler("dj.CompanyOverviewCanvasModule.viewAllRecentArticles")]
        public string OnViewAllRecentArticlesClick { get; set; }

        #endregion

        [ClientProperty("createUpdateCompanyOverviewModuleUrl")]
        public string CreateUpdateCompanyOverviewModuleUrl
        {
            get { return Settings.Default.DashboardServiceBaseUrl + Settings.Default.CreateUpdateCompanyOverviewModuleUrl; }
        }
        

        private CompanyOverviewModuleEditor GetDefaultEditor()
        {
            return new CompanyOverviewModuleEditor()
            {
                ModuleId = ModuleId,
                ModuleName = Asset.Title,
                ModuleDescription = Asset.Description,
                SuggestServiceUrl = SuggestServiceUrl
            };
        }

    }
}