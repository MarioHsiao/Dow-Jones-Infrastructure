using System.Collections.Generic;
using System.Linq;
using DowJones.Web.Mvc.UI.Canvas.Editors;
using DowJones.Web.Mvc.UI.Components.Models;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;

namespace DowJones.Web.Mvc.UI.Canvas.Modules
{
    public class CustomTopicsModule : Module<CustomTopicsNewspageModule>
    {

        private const int MAXTOPICSTORETURN = 3;
        private const int MAXHEADLINESTORETURN = 5;
        private const int DEFAULT = 0;
        private const int ACTIVEPAGE = 1;

        public CustomTopicsModule()
        {
            ActivePage = ACTIVEPAGE;
        }

        [ClientProperty("activePage")]
        public int ActivePage { get; set; }

        [ClientProperty("totalNumberOfTopics")]
        public int TotalNumberOfTopics
        {
            get
            {
                if (HasAsset && Asset.CustomTopicCollection != null)
                {
                    return Asset.CustomTopicCollection.Count;
                }
                return DEFAULT;
            }
        }

        [ClientProperty("numberOfPages")]
        public int NumberOfPages
        {
            get { return TotalNumberOfTopics % MaxTopicsToReturn == DEFAULT ? (TotalNumberOfTopics / MaxTopicsToReturn) : (TotalNumberOfTopics / MaxTopicsToReturn) + ACTIVEPAGE; }
        }

        //CustomTopic
        [ClientProperty("firstTopicToReturn")]
        public int FirstTopicToReturn
        {
            get { return DEFAULT; }
        }

        [ClientProperty("maxTopicsToReturn")]
        public int MaxTopicsToReturn
        {
            get { return MAXTOPICSTORETURN; }
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
                     }, MAXTOPICSTORETURN);
            }
        }

        public new SwapModuleEditor Editor
        {
            get
            {
                if ((base.Editor as SwapModuleEditor) == null)
                    base.Editor =
                        new SwapModuleEditor
                        {
                            ModuleId = ModuleId,
                            ModuleType = Mapping.ModuleType.CustomTopicsNewspageModule.ToString()
                        };

                return (SwapModuleEditor)base.Editor;
            }
            set { base.Editor = value; }
        }
    }
}