using System.Web.Mvc;

namespace DowJones.Prod.X.Web.App_Start
{
    public class ViewEnginesRegistration
    {
        public static void CleanupOtherViewEngines()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
        }
    }
}