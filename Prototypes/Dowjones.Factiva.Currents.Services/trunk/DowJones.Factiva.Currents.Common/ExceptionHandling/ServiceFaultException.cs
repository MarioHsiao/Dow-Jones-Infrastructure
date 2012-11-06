using System.ServiceModel.Web;

namespace DowJones.API.Common.ExceptionHandling
{
    /// <summary>
    /// Summary description for ServiceFaultException
    /// </summary>
    public class ServiceFaultException : WebFaultException<ErrorResponse>
    {

        /// <summary>
        /// Constructor to return WebFaultexception for the HttpStatusCode as OK 
        /// </summary>
        /// <param name="errorResponse"></param>
        public ServiceFaultException(ErrorResponse errorResponse)
            : base(errorResponse, System.Net.HttpStatusCode.OK)
        {
        }

        /// <summary>
        /// Constructor to return WebFaultexception for the given HttpStatusCode
        /// </summary>
        /// <param name="errorResponse"></param>
        /// <param name="httpStatusCode"></param>
        public ServiceFaultException(ErrorResponse errorResponse, System.Net.HttpStatusCode httpStatusCode)
            : base(errorResponse, httpStatusCode)
        {
        }
    }
}
