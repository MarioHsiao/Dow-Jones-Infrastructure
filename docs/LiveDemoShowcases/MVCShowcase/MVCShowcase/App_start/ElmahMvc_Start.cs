using DowJones.MvcShowcase.App_start;

[assembly: WebActivator.PreApplicationStartMethod(typeof(ElmahMvc), "Start")]
namespace DowJones.MvcShowcase.App_start
{
    public class ElmahMvc
    {
        public static void Start()
        {
            Elmah.Mvc.Bootstrap.Initialize();
        }
    }
}