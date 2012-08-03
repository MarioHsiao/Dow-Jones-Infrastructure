namespace $rootnamespace$
{
	public class $ViewComponentModelName$ : DowJones.Web.Mvc.UI.Canvas.Module
	{
		// TODO:  Add properties

        public $ViewComponentModelName$() 
        {
            DataServiceUrl = DowJones.Web.Mvc.UI.Canvas.CanvasSettings.Default.GetDataServiceUrl(GetType(), /* Module Settings */);
        }
	}
}