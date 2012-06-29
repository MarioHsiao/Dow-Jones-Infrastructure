namespace DowJones.Web.Mvc.UI.Components.AutoSuggest
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
