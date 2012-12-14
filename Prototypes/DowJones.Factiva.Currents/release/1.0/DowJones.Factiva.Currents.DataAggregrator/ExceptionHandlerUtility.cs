using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace DowJones.Factiva.Currents.Aggregrator
{
    public class ExceptionHandlerUtility
    {
        //private static IResourceTextManager resources = ServiceLocator.Resolve<IResourceTextManager>();

        ///// <summary>
        ///// Return the Webfault Exception for the given error code and Http status code details
        ///// </summary>
        ///// <param name="errorCode"></param>
        ///// <param name="httpStatusCode"></param>
        ///// <returns></returns>
        //public static ServiceFaultException GetWebFaultByServiceException(long errorCode, HttpStatusCode httpStatusCode)
        //{
        //    //System.Resources.ResourceManager textManager = ResourceTextManagerFactory.CreateFromResourceAssembly();
        //    //var userMessage = textManager.
        //    //var userMessage = ResourceTextManager.GetErrorMessage(errorCode.ToString());
        //    resources = resources ?? ServiceLocator.Resolve<IResourceTextManager>();
        //    string userMessage = resources.GetErrorMessage(errorCode.ToString());//Resources.GetErrorMessage(errorCode.ToString());
        //    ErrorResponse result = new ErrorResponse(new Error(errorCode, userMessage));
        //    // Log transaction to PIM

        //    DowJones.API.Common.Utilities.Web.SetResponseHeaders();
        //    PimUtility.PublishToPim(DateTime.Now, result, false);
        //    HttpStatusCode code = httpStatusCode;
        //    if (Utilities.Web.OverrideHttpStatus())
        //        code = HttpStatusCode.OK;
        //    return new ServiceFaultException(result, code);
        //}
    }
}
