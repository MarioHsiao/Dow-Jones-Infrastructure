using DowJones.Documentation.Website.App_Start;

[assembly: WebActivator.PreApplicationStartMethod(typeof(ElmahConfig), "Start")]
namespace DowJones.Documentation.Website.App_Start
{
    public class ElmahConfig
    {
        public static void Start()
        {
            Elmah.Mvc.Bootstrap.Initialize();
        }
    }
}