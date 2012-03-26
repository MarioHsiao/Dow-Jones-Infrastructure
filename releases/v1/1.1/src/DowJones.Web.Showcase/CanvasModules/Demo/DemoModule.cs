using DowJones.Web.Mvc.UI.Canvas;
using DowJones.Web.Showcase.CanvasModules.Demo.Editor;

namespace DowJones.Web.Showcase.CanvasModules.Demo
{
	public class DemoModule : Module
	{
	    public override string DataServiceUrl
	    {
            get { return dataServiceUrl; }
	    }
        private readonly string dataServiceUrl;


	    public string OnGlobalTimestampUpdated { get; set; }



	    public DemoModule(string dataServiceUrl)
	    {
	        this.dataServiceUrl = dataServiceUrl;
	        Editor = new DemoCanvasModuleEditorModel();
	    }
	}
}