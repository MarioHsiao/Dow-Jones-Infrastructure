using System.Collections.Generic;
using DowJones.Web.Mvc.UI.Canvas.Editors;
using DowJones.Web.Mvc.UI.Components.Models;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using System.Linq;
using DowJones.Web.Mvc.UI.Canvas.Properties;
using System;

namespace DowJones.Web.Mvc.UI.Canvas.Models
{
    public class SyndicationModule : Module<SyndicationNewspageModule>
    {

        [ClientProperty("maxResultsPerFeed")]
        public int MaxResultsPerFeed
        {
            get
            {
                return SyndicationModuleSettings.Default.MaxResultsPerFeed;
            }
        }


        [ClientProperty("totalFeeds")]
        public int TotalFeeds
        {
            get { return Asset != null && Asset.SyndicationFeedIDCollection != null ? Asset.SyndicationFeedIDCollection.Count : 0; }
        }


        [ClientProperty("feedsPerPage")]
        public int FeedsPerPage
        {
            get { return _feedsPerPage; }
            set
            {
                _feedsPerPage = value;

                PortalHeadlineLists = Enumerable.Repeat(
                   new PortalHeadlineListModel
                   {
                       MaxNumHeadlinesToShow = MaxResultsPerFeed,
                       ShowPublicationDateTime = false,
                       ShowSource = false,
                       DisplayNoResultsToken = true
                   }, value);

            }
        }
        private int _feedsPerPage;

        [ClientProperty("activePage")]
        public int ActivePage { get; set; }

        [ClientProperty("numPages")]
        public int NumPages
        {
            get
            {
                return TotalFeeds % FeedsPerPage == 0
                    ? TotalFeeds / FeedsPerPage
                    : TotalFeeds / FeedsPerPage + 1;
            }
        }

        public new SyndicationModuleEditor Editor
        {
            get
            {
                if ((base.Editor as SyndicationModuleEditor) == null)
                    base.Editor = GetDefaultEditor();

                return (SyndicationModuleEditor)base.Editor;
            }
            set { base.Editor = value; }
        }



        public IEnumerable<PortalHeadlineListModel> PortalHeadlineLists { get; private set; }

        [ClientProperty("dataServiceUrl")]
        public override string DataServiceUrl
        {
            get { return Settings.Default.DashboardServiceBaseUrl + SyndicationModuleSettings.Default.GetDataUrl; }
        }

        [ClientProperty("moduleServiceUrl")]
        public string ModuleServiceUrl
        {
            get { return Settings.Default.DashboardServiceBaseUrl + SyndicationModuleSettings.Default.SyndicationModuleUrl; }
        }


        public SyndicationModule()
        {
            // default active page is the first one (page index is 1 based)
            ActivePage = 1;
            FeedsPerPage = SyndicationModuleSettings.Default.FeedsPerPage;
            CanRefresh = true;
            // doesn't work as initializers run after constructors. so moduleID is 0 when this gets called
            //base.Editor = GetDefaultEditor();

            

        }

        private SyndicationModuleEditor GetDefaultEditor()
        {
            return new SyndicationModuleEditor()
                       {
                           ModuleName = Title,
                           ModuleDescription = Description,
                           CanvasId = CanvasId,
                           ModuleId = ModuleId
                       };
        }
    }


}