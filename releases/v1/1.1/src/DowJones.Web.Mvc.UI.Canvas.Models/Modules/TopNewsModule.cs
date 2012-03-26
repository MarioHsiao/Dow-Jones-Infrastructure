using Factiva.Gateway.Messages.Assets.Pages.V1_0;
using DowJones.Web.Mvc.UI.Canvas.Editors;


namespace DowJones.Web.Mvc.UI.Canvas.Models
{
    public class TopNewsModule : Module<TopNewsNewspageModule>
    {
        private const int MAX_HEADLINES_TO_RETURN = 5;

        public int NewsItemsPerPage
        {
            //TODO: Get from config
            get { return 3; }
        }

        [ClientProperty("maxHeadlinesToReturn")]
        public int MaxHeadlinesToReturn
        {
            get { return MAX_HEADLINES_TO_RETURN; }
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
                            ModuleType = Mapping.ModuleType.TopNewsNewspageModule.ToString()
                        };

                return (SwapModuleEditor)base.Editor;
            }
            set { base.Editor = value; }
        }
    }
}