using System.Web.Mvc;
using DowJones.Web.Mvc.ActionFilters;
using $rootnamespace$.Controllers;

namespace $rootnamespace$.Controllers
{
	[RequireAuthentication]
    public class HomeController : AbstractController
    {

        public ActionResult Index()
        {
            return View();
        }

    }
}
