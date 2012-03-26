using System.Web.Script.Services;
using System.Web.Services;

namespace DowJones.Web.Mvc.UI.Canvas.WebServices
{
    [ScriptService]
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class FactivaDataService : WebService 
    {
        [ScriptMethod]
        [WebMethod]
        public string HelloWorld() {
            return "Hello World";
        }
    }
}
