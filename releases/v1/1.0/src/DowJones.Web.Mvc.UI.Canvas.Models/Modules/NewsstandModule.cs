using System.Collections.Generic;
using System.Linq;
using DowJones.Web.Mvc.UI.Components.Models;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using DowJones.Web.Mvc.UI.Canvas.Editors;

namespace DowJones.Web.Mvc.UI.Canvas.Models
{
    public class NewsstandModule : Module<NewsstandNewspageModule>
    {

        private const int MAXHEADLINESECTIONSTORETURN = 3;
        private const int MAXHEADLINESTORETURN = 5;
        private const int DEFAULT = 0;
        
        [ClientProperty("totalNumberOfHeadlineSections")]
        public int TotalNumberOfHeadlineSections
        {
            get
            {
                if (HasAsset && Asset.NewsstandListCollection != null)
                {
                    return Asset.NewsstandListCollection.Count;
                }
                return DEFAULT;
            }
        }

        //Sources
        [ClientProperty("maxHeadlineSectionsToReturn")]
        public int MaxHeadlineSectionsToReturn
        {
            get { return MAXHEADLINESECTIONSTORETURN; }

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
                            DisplayNoResultsToken = true,
                            ShowSource = false
                        }, MAXHEADLINESECTIONSTORETURN);
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
                            ModuleType = Mapping.ModuleType.NewsstandNewspageModule.ToString()
                        };

                return (SwapModuleEditor)base.Editor;
            }
            set { base.Editor = value; }
        }

    }    
}