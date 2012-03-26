using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using DowJones.Web.Mvc.UI.Components.Models;
using DowJones.Web.Mvc.UI.Canvas.Editors;

namespace DowJones.Web.Mvc.UI.Canvas.Models
{
    public class SummaryModule : Module<SummaryNewspageModule>
    {
        private const int DefaultMaxHeadlinesToShow = 8;
        private const int DefaultMaxResultsToReturn = 8;
        private const int DefaultMaxEntitiesToReturn = 10;

        [ClientProperty("maxEntitiesToReturn")]
        public int MaxEntitiesToReturn { get; set; }

        public int MaxHeadlinesToShow { get; set; }

        [ClientProperty("hasMarketDataIndex")]
        public bool HasMarketDataIndex
        {
            get
            {
                return Asset != null
                    && !string.IsNullOrWhiteSpace(Asset.MarketIndex);
            }
        }

        [ClientProperty("maxResultsToReturn")]
        public int MaxResultsToReturn { get; set; }


        #region ..:: Client Event Handlers ::..

        #endregion


        public PortalHeadlineListModel PortalHeadlineList
        {
            get
            {
                return new PortalHeadlineListModel
                           {
                               UseTimeLineLayout = true,
                               MaxNumHeadlinesToShow = MaxHeadlinesToShow,
                               DisplayNoResultsToken = true
                           };
            }
        }

        public SummaryModule()
        {
            MaxEntitiesToReturn = DefaultMaxEntitiesToReturn;
            MaxHeadlinesToShow = DefaultMaxHeadlinesToShow;
            MaxResultsToReturn = DefaultMaxResultsToReturn;
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
                            ModuleType = Mapping.ModuleType.SummaryNewspageModule.ToString()
                        };

                return (SwapModuleEditor)base.Editor;
            }
            set { base.Editor = value; }
        }
    }
}
