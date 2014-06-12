using System.Web.Mvc;
using DowJones.Web.Mvc.ActionResults;

namespace DowJones.Web.Mvc.Extentions
{
    /// <summary>
    /// Extends Controller with Negotiated() ActionResult that does
    /// basic content negotiation based on the Accept header.
    /// </summary>
    public static class NegotiatedResultExtensions
    {
        /// <summary>
        /// Return content-negotiated content of the data based on Accept header.
        /// Supports:
        ///    application/json  - using JSON.NET
        ///    text/xml   - Xml as XmlSerializer XML
        ///    text/html  - as text, or an optional View
        ///    text/plain - as text
        /// </summary>        
        /// <param name="controller"></param>
        /// <param name="data">Data to return</param>
        /// <returns>serialized data</returns>
        /// <example>
        /// public ActionResult GetCustomers()
        /// {
        ///      return this.Negotiated( repo.Customers.OrderBy( c=> c.Company) )
        /// }
        /// </example>
        public static NegotiatedResult Negotiated(this Controller controller, object data)
        {
            return new NegotiatedResult(data);
        }

        /// <summary>
        /// Return content-negotiated content of the data based on Accept header.
        /// Supports:
        ///    application/json  - using JSON.NET
        ///    text/xml   - Xml as XmlSerializer XML
        ///    text/html  - as text, or an optional View
        ///    text/plain - as text
        /// </summary>        
        /// <param name="controller"></param>
        /// <param name="viewName">Name of the View to when Accept is text/html</param>
        /// /// <param name="data">Data to return</param>        
        /// <returns>serialized data</returns>
        /// <example>
        /// public ActionResult GetCustomers()
        /// {
        ///      return this.Negotiated("List", repo.Customers.OrderBy( c=> c.Company) )
        /// }
        /// </example>
        public static NegotiatedResult Negotiated(this Controller controller, string viewName, object data)
        {
            return new NegotiatedResult(viewName, data);
        }
    }
}