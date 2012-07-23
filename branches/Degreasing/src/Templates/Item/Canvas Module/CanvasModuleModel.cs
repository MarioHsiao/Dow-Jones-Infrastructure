using DowJones.Web.Mvc.UI.Canvas;

namespace $rootnamespace$
{
	public class $ViewComponentModelName$ : Module</* TODO: Factiva Module Type */>
	{

		// TODO:  Add properties

        public $ViewComponentModelName$() 
        {
            DataServiceUrl = CanvasModuleSettings.Default.GetDataServiceUrl(GetType());
        }
	}
}