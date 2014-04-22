using System.Linq;
using System.Web.Http;
using Microsoft.Owin.Extensions;
using Owin;

namespace DowJones.SyntaxHost
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional });

            app.UseWebApi(config).UseNancy();

            app.UseNancy();
            app.UseStageMarker(PipelineStage.MapHandler);

            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);
            
        } 
    }
}