[assembly: WebActivator.PreApplicationStartMethod(typeof(DowJones.MvcShowcase.App_Start.ElmahMvc), "Start")]
namespace DowJones.MvcShowcase.App_Start
{
    public class ElmahMvc
    {
        public static void Start()
        {
            Elmah.Mvc.Bootstrap.Initialize();
        }
    }
}