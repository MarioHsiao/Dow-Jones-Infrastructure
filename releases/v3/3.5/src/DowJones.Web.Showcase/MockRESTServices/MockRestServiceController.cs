using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using DowJones.Web.Mvc;
using ControllerBase = DowJones.Web.Mvc.ControllerBase;

namespace DowJones.Web.Showcase.MockRESTServices
{
    public class MockRestServiceController : ControllerBase
    {
        public static string ServiceResponsesBaseFolder
        {
            get { return serviceResponsesBaseFolder ?? "~/MockRESTServices"; }
            set { serviceResponsesBaseFolder = value; }
        }
        private static string serviceResponsesBaseFolder;


        [Mvc.Routing.Route(
            "api/dashboard/{serviceCategory}/{serviceName}/{version}/{serviceAction}/{responseType}",
            Defaults = "{ controller: 'MockRestService', action: 'ServiceCall', responseType: 'json' }")]
        public virtual ActionResult ServiceCall()
        {
            return DefaultResult();
        }

        [ActionName("ServiceCall")]
        [AcceptVerbs(HttpVerbs.Delete)]
        public virtual ActionResult Delete()
        {
            return DefaultResult();
        }

        [ActionName("ServiceCall")]
        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ActionResult Get()
        {
            return DefaultResult();
        }

        [ActionName("ServiceCall")]
        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult Post()
        {
            return DefaultResult();
        }

        [ActionName("ServiceCall")]
        [AcceptVerbs(HttpVerbs.Put)]
        public virtual ActionResult Put()
        {
            return DefaultResult();
        }


        protected ActionResult DefaultResult()
        {
            var routeValues = RouteData.Values;

            string responseType = routeValues["responseType"] as string ?? "json";
            
            if(responseType.Equals("xml", StringComparison.OrdinalIgnoreCase))
                Response.ContentType = "application/xml";
            else
                Response.ContentType = "application/json";

            // Get the path containing the mock service responses
            // e.g. ~/MockServices/Modules/CompanyOverview
            IEnumerable<string> serviceFolderParts = new [] {
                ServiceResponsesBaseFolder,
                (string)routeValues["serviceCategory"],
                (string)routeValues["serviceName"],
            };
            string relativeServiceFolder = string.Join("/", serviceFolderParts.Where(x => !string.IsNullOrWhiteSpace(x)));
            string absoluteServiceFolder = Server.MapPath(relativeServiceFolder);

            // Get the "action", e.g. "GET data"
            IEnumerable<string> actionParts = new[] {
                Request.HttpMethod,
                (string)routeValues["serviceAction"],
            };
            // Join the parts of the action, e.g. "GET_data"
            string action = string.Join("_", actionParts.Where(x => !string.IsNullOrWhiteSpace(x)));

            // Append the response type, e.g. "GET_data.json"
            string filename = string.Format("{0}.{1}", action, responseType);

            // Get the absolute, server-side file name
            // e.g. C:\Website\MockServices\Modules\CompanyOverview\GET_data.json"
            string absoluteFilename = Path.Combine(absoluteServiceFolder, filename);

            Debug.WriteLine(string.Format("Request for service: {0} ", absoluteFilename));

            // If there is no file defined for this request, just return an empty response
            if (!System.IO.File.Exists(absoluteFilename))
                return Content(string.Empty);

            // Otherwise, parse the stub file and return that
            return BuildResponse(absoluteFilename);
        }

        private ActionResult BuildResponse(string absoluteFilename)
        {
            Regex headerLine = new Regex(@"^([^:{< ""]*):(.*)$", RegexOptions.Singleline);
            var sourceFileLines = System.IO.File.ReadAllLines(absoluteFilename);

            // Parse the headers (in "key: value" format)
            var headers =
                from line in sourceFileLines.Where(line => headerLine.IsMatch(line))
                let match = headerLine.Matches(line)[0]
                let key = match.Groups[1].Value
                let value = match.Groups[2].Value.Trim()
                select new { key, value };

            // Append the headers to the response
            foreach (var header in headers)
            {
                if (header.key.Equals("StatusCode", StringComparison.OrdinalIgnoreCase))
                    Response.StatusCode = int.Parse(header.value);
                else
                    Response.AddHeader(header.key, header.value);
            }

            // The rest of the content is the body of the response
            var contentLines = sourceFileLines.Where(line => !headerLine.IsMatch(line));

            return Content(string.Join("\r\n", contentLines));
        }
    }

}