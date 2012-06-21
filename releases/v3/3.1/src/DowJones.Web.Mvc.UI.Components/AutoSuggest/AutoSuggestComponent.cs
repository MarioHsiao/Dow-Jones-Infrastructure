namespace DowJones.Web.Mvc.UI.Components.Models
{
    public static class AutoSuggestComponentExtensions
    {

        public static ViewComponentFactory AutoSuggest(this ViewComponentFactory factory)
        {
            factory.ScriptRegistry().Include(ComponentSettings.Default.AutoSuggestHandlerServiceUrl);
            return factory;
        }

    }
}
