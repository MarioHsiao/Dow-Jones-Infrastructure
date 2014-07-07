using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.ActionResults
{/// <summary>
    /// Returns a content negotiated result based on the Accept header.
    /// Minimal implementation that works with JSON and XML content,
    /// can also optionally return a view with HTML.    
    /// </summary>
    /// <example>
    /// // model data only
    /// public ActionResult GetCustomers()
    /// {
    ///      return new NegotiatedResult(repo.Customers.OrderBy( c=> c.Company) )
    /// }
    /// // optional view for HTML
    /// public ActionResult GetCustomers()
    /// {
    ///      return new NegotiatedResult("List", repo.Customers.OrderBy( c=> c.Company) )
    /// }
    /// </example>
    public class NegotiatedResult : ActionResult
    {
        /// <summary>
        /// Data stored to be 'serialized'. Public
        /// so it's potentially accessible in filters.
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// Optional name of the HTML view to be rendered
        /// for HTML responses
        /// </summary>
        public string ViewName { get; set; }

        public static bool FormatOutput { get; set; }

        static NegotiatedResult()
        {
            FormatOutput = HttpContext.Current.IsDebuggingEnabled;
        }

        /// <summary>
        /// Pass in data to serialize
        /// </summary>
        /// <param name="data">Data to serialize</param>        
        public NegotiatedResult(object data)
        {
            Data = data;
        }

        /// <summary>
        /// Pass in data and an optional view for HTML views
        /// </summary>
        /// <param name="data"></param>
        /// <param name="viewName"></param>
        public NegotiatedResult(string viewName, object data)
        {
            Data = data;
            ViewName = viewName;
        }

        private readonly static ViewEngineCollection Engines = new ViewEngineCollection {
            new WebFormViewEngine(),
            new RazorViewEngine(),
        };


        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var response = context.HttpContext.Response;
            var request = context.HttpContext.Request;


            // Look for specific content types            
            if (request != null && request.AcceptTypes != null && request.AcceptTypes.Contains("text/html"))
            {
                response.ContentType = "text/html";

                if (!string.IsNullOrEmpty(ViewName))
                {
                    var viewData = context.Controller.ViewData;
                    viewData.Model = Data;

                    var viewResult = new ViewResult
                    {
                        ViewName = ViewName,
                        MasterName = null,
                        ViewData = viewData,
                        TempData = context.Controller.TempData,
                        ViewEngineCollection = Engines
                    };
                    viewResult.ExecuteResult(context.Controller.ControllerContext);
                }
                else
                    response.Write(Data);
            }
            else if (request != null && request.AcceptTypes != null &&  request.AcceptTypes.Contains("text/plain"))
            {
                response.ContentType = "text/plain";
                response.Write(Data);
            }
            else if (request != null && request.AcceptTypes != null && request.AcceptTypes.Contains("application/json"))
            {
                using (var writer = new JsonTextWriter(response.Output))
                {
                    var settings = new JsonSerializerSettings();
                    if (FormatOutput)
                        settings.Formatting = Newtonsoft.Json.Formatting.Indented;

                    var serializer = JsonSerializer.Create(settings);

                    serializer.Serialize(writer, Data);
                    writer.Flush();
                }
            }
            else if (request != null && request.AcceptTypes != null && request.AcceptTypes.Contains("text/xml"))
            {
                response.ContentType = "text/xml";
                if (Data == null)
                {
                    return;
                }

                using (var writer = new XmlTextWriter(response.OutputStream, new UTF8Encoding()))
                {
                    if (FormatOutput)
                        writer.Formatting = System.Xml.Formatting.Indented;

                    var serializer = new XmlSerializer(Data.GetType());

                    serializer.Serialize(writer, Data);
                    writer.Flush();
                }
            }
            else
            {
                // just write data as a plain string
                response.Write(Data);
            }
        }
    }
}
