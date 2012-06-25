[assembly: WebActivator.PreApplicationStartMethod(typeof(DowJones.Documentation.Website.App_Start.ElmahMvc), "Start")]
namespace DowJones.Documentation.Website.App_Start
{
    public class ElmahMvc
    {
        public static void Start()
        {
            Elmah.Mvc.Bootstrap.Initialize();
        }
    }
}