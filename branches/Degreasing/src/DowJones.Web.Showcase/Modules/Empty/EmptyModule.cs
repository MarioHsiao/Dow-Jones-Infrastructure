using DowJones.Web.Mvc.UI;
using Module = DowJones.Web.Mvc.UI.Canvas.Module;

namespace DowJones.Web.Showcase.Modules.Empty
{
    public class EmptyModule : Module
	{
        [ClientProperty("contentUrl")]
        public string ContentUrl { get; set; }
    }
}