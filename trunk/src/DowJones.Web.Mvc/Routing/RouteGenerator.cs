using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using DowJones.DependencyInjection;
using DowJones.Extensions;
using DowJones.Web.Mvc.Infrastructure;
using DowJones.Web.Mvc.Threading;
using Filter = Factiva.Gateway.Filters.V1_0.Filter;

namespace DowJones.Web.Mvc.Routing
{
    public class RouteGenerator : IRouteGenerator
    {
        private readonly RouteCollection _routes;
        private readonly RequestContext _requestContext;
        private readonly IEnumerable<ControllerActionAttributeInfo> _controllerActionAttributes;
        private readonly JavaScriptSerializer _javaScriptSerializer;

        [Inject("Disambiguation: this is the 'real' constructor; the other constructor is for testing")]
        public RouteGenerator(RouteCollection routes, RequestContext requestContext, JavaScriptSerializer javaScriptSerializer, IControllerRegistry controllerRegistry)
            : this(routes, requestContext, javaScriptSerializer, controllerRegistry.ControllerActionAttributes)
        {
            _javaScriptSerializer = javaScriptSerializer;
        }

        public RouteGenerator(RouteCollection routes, RequestContext requestContext, JavaScriptSerializer javaScriptSerializer, IEnumerable<ControllerActionAttributeInfo> controllerActionAttributes)
        {
            _routes = routes;
            _javaScriptSerializer = javaScriptSerializer;
            _requestContext = requestContext;
            _controllerActionAttributes = controllerActionAttributes;
        }

        public virtual IEnumerable<RouteBase> Generate()
        {
            var actionsWithCustomRoutes =
                _controllerActionAttributes.Where(x => x.Attribute is RouteAttribute);

            var customRoutes =
                from action in actionsWithCustomRoutes
                let attribute = action.Attribute as RouteAttribute
                let routeInfo = new RouteInfo(action.Controller, action.Action)
                let defaults = GetDefaults(attribute, routeInfo)
                let constraints = GetConstraints(attribute)
                let routeUrl = GetRouteUrl(routeInfo, attribute, action.Controller, action.Action)
                let handler = GetRouteHandler(action)
                select new Route(routeUrl, defaults, constraints, handler);

            return customRoutes;
        }

        private RouteValueDictionary GetDefaults(RouteAttribute attribute, RouteInfo routeInfo)
        {
            var routeDefaults = routeInfo.Defaults;

            var attributeDefaults = _javaScriptSerializer.Deserialize<IDictionary<string, object>>(attribute.Defaults ?? string.Empty);

            routeDefaults.Merge(attributeDefaults ?? new object());

            if (attribute.Pattern.IsNullOrEmpty())
            {
                routeDefaults.Add("Id", UrlParameter.Optional);
            }
            

            return routeDefaults;
        }

        private RouteValueDictionary GetConstraints(RouteAttribute attribute)
        {
            //var c1 = JsonConvert.DeserializeObject<IDictionary<string, object>>(attribute.Constraints ?? string.Empty);
            var constraints = _javaScriptSerializer.Deserialize<IDictionary<string, object>>(attribute.Constraints ?? string.Empty);

            return new RouteValueDictionary(constraints ?? new object());
        }

        private string GetRouteUrl(RouteInfo routeInfo, RouteAttribute attribute, Type controller, MethodInfo methodInfo)
        {
            var iisVersion = _requestContext.HttpContext.Request.GetIISVersion();

            var pattern = attribute.Pattern;
            if (pattern.IsNullOrEmpty())
            {
                pattern = string.Format("{0}/{{action}}/{{id}}", controller.Name.Replace("Controller",""));
            }

            var routeUrl = routeInfo.ResolveRoute(pattern, _routes, _requestContext, iisVersion);

            return routeUrl;
        }

        private static MvcRouteHandler GetRouteHandler(ControllerActionAttributeInfo actionAttribute)
        {
            if (actionAttribute.Attribute is AspCompatAttribute) 
                return new STAMvcRouteHandler();
            return new MvcRouteHandler();
        }
    }
}
