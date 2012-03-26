using System.Web.Mvc;
using DowJones.Web.Mvc;
using DowJones.Web.Mvc.UI.Controls.Content;

namespace $safeprojectname$.Controllers
{
    public class HomeController : DowJonesControllerBase  // derives from DowJonesControllerBase
    {

        public ActionResult Index()
        {
            // "Log" property provided by the base class 
            // and injected via dependency injection
            Log.Debug("Index action called");

            // Create a couple of content models to be rendered by the view.
            // NOTE: This is just a demo - in a real scenario this HTML would come from a service!
            var content1 = new ContentComponentModel { Html = "<h2>Content Component Demo</h2>" };
            var content2 = new ContentComponentModel { Html = "Hello, World!" };

            // Use the .Components() method to pass the 
            // component states to the view
            return Components("Index", content1, content2);
        }

    }
}
