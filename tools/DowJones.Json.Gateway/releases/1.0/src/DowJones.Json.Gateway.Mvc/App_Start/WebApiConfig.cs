using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DowJones.Json.Gateway.Mvc
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new {id = RouteParameter.Optional}
                );

           /* var jsonformatter = config.Formatters.FirstOrDefault(t => t.GetType() == typeof (JsonMediaTypeFormatter));
            config.Formatters.Remove(jsonformatter );
            config.Formatters.Insert(0, jsonformatter); 
            config.Formatters[0].SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
*/

            // Remove the XML formatter
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            //new MediaTypeMapping(new MediaTypeHeaderValue())
           /* config.Formatters.JsonFormatter.MediaTypeMappings.Add(new QueryStringMapping("type", "json", new MediaTypeHeaderValue("application/json")));

            config.Formatters.XmlFormatter.MediaTypeMappings.Add(new QueryStringMapping("type", "xml", new MediaTypeHeaderValue("application/xml")));
*/
            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();
        }
    }
}