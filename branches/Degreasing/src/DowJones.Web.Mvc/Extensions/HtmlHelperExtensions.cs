using System.Web.Mvc;
using DowJones.DependencyInjection;
using DowJones.Web.Mvc.UI;

namespace DowJones.Web.Mvc.Extensions
{

    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Gets the View Component Factory
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <returns>The Factory</returns>
        public static ViewComponentFactory DJ(this HtmlHelper helper)
        {
            ViewComponentFactory factory = ServiceLocator.Resolve<ViewComponentFactory>();
            
            factory.HtmlHelper = helper;

            return factory;
        }
    }
}