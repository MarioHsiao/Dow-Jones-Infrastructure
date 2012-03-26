using System.Linq;
using System.Web.Mvc;
using DowJones.Web.Showcase.Models;

namespace DowJones.Web.Showcase.Controllers
{
    public class AjaxController : Controller
    {

        public ActionResult Index()
        {
            var employees = Employee.GetAll();

            return View("List", employees);
        }

        public ActionResult Details(int id)
        {
            Employee employee = Employee.Get(id);

            return PartialView("Details", employee);
        }

        public ActionResult Edit(int id)
        {
            Employee employee = Employee.Get(id);

            return PartialView("Edit", employee);
        }

        [AcceptVerbs(HttpVerbs.Post | HttpVerbs.Put)]
        public ActionResult Update(int id)
        {
            var existingEmployee = Employee.GetAll().Single(x => x.ID == id);
            UpdateModel(existingEmployee);
            existingEmployee.Save();

            if (ModelState.IsValid)
                return RedirectToAction("Details", new { id });
            else
                return RedirectToAction("Edit", new { id });
        }
    }
}
