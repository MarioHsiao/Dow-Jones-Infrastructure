using DowJones.Infrastructure;

namespace DowJones.MvcShowcase.BootstrapperTasks
{
	public class ElmahMvc : IBootstrapperTask
    {
		public void Execute()
		{
			Elmah.Mvc.Bootstrap.Initialize();
		}
    }
}