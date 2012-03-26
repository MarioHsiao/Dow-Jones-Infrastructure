using System.Collections.Generic;
using System.Linq;
using DowJones.DependencyInjection;
using DowJones.Session;
using DowJones.Web.Mvc.UI.Components.Models;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using DowJones.Web.Mvc.UI.Canvas.Editors;


namespace DowJones.Web.Mvc.UI.Canvas.Models
{
    public class SourcesModule : Module<SourcesNewspageModule>
    {
        private const int MAXSOURCESTORETURN = 3;
        private const int MAXHEADLINESTORETURN = 5;
        private const int DEFAULT = 0;
        private const int ACTIVEPAGE = 1;

        public SourcesModule()
        {
            ActivePage = ACTIVEPAGE;
        }

        [ClientProperty("activePage")]
        public int ActivePage { get; set; }

        [ClientProperty("totalNumberOfSources")]
        public int TotalNumberOfSources
        {
            get
            {
                if (HasAsset && Asset.SourcesListCollection != null)
                {
                    return GetSourceListByContentLanguages(Asset.SourcesListCollection, Preferences).Count;
                    //return Asset.SourcesListCollection.Count;
                }
                return DEFAULT;
            }
        }

        [ClientProperty("numberOfPages")]
        public int NumberOfPages
        {
            get { return TotalNumberOfSources % MaxSourcesToReturn == DEFAULT ? (TotalNumberOfSources / MaxSourcesToReturn) : (TotalNumberOfSources / MaxSourcesToReturn) + ACTIVEPAGE; }
        }

        //Sources
        [ClientProperty("firstSourceToReturn")]
        public int FirstSourceToReturn
        {
            get { return DEFAULT; }
        }

        [ClientProperty("maxSourcesToReturn")]
        public int MaxSourcesToReturn
        {
            get { return MAXSOURCESTORETURN; }
            
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
                        }, MAXSOURCESTORETURN);
            }
        }

        protected internal static List<string> GetSourceListByContentLanguages(SourceListCollection sourcesCollection, IPreferences preferences)
        {
            
            var temp = new List<string>();

            // Handle the all condition
            if (preferences.ContentLanguages.Count == 0)
            {
                foreach (var sourceList in sourcesCollection)
                {
                    temp.AddRange(sourceList.SourceCodes.SourceCodeCollection);
                }

                return temp;
            }

            foreach (var sourcelist in sourcesCollection.Where(sourcelist => ValidateSource(sourcelist.SourceListId, preferences)))
            {
                temp.AddRange(sourcelist.SourceCodes.SourceCodeCollection);
            }

            return temp;
        }

        protected internal static bool ValidateSource(string sourceLanguage, IPreferences preferences)
        {
            return preferences.ContentLanguages.Any(contentLanguage => contentLanguage.ToLowerInvariant() == sourceLanguage.ToLowerInvariant());
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
                            ModuleType = Mapping.ModuleType.SourcesNewspageModule.ToString()
                        };

                return (SwapModuleEditor)base.Editor;
            }
            set { base.Editor = value; }
        }
    }
}