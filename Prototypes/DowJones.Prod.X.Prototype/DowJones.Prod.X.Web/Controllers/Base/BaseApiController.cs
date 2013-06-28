using System;
using System.Globalization;
using System.Net;
using System.Runtime.InteropServices;
using System.Web.Mvc;
using System.Web.SessionState;
using DowJones.DependencyInjection;
using DowJones.Exceptions;
using DowJones.Globalization;
using DowJones.Preferences;
using DowJones.Prod.X.Common.Exceptions;
using DowJones.Prod.X.Common.Extentions;
using DowJones.Prod.X.Web.Models.Api;

namespace DowJones.Prod.X.Web.Controllers.Base
{
    [SessionState(SessionStateBehavior.Disabled)]
    public abstract class BaseApiController : DowJones.Web.Mvc.ControllerBase
    {
        [Inject("")]
        public IResourceTextManager ResourceTextManager { get; set; }

        [Inject("")]
        public IPreferences NewsletterPreferences { get; set; }

        public T Process<T>(Action action) where T : BasicApiResult, new()
        {
            var packetResult = new T
                                   {
                                       ResourceTextManager = ResourceTextManager
                                   };
            try
            {
                if (Principle.HasAccessToProductX())
                {
                    action.Invoke();
                }
                else
                {
                    throw new DowJonesUtilitiesException(ApplicationExceptionConstants.NoAccessToNewsletters);
                }
            }
            catch (DowJonesUtilitiesException djux)
            {
                packetResult.ReturnCode = djux.ReturnCode;
            }
            catch (AggregateException aex)
            {
                var djex = ExtractAggregateException(aex);
                packetResult.ReturnCode = djex.ReturnCode;
            }
            catch (COMException comEx)
            {
                var t = new DowJonesUtilitiesException(comEx, comEx.ErrorCode);
                packetResult.ReturnCode = t.ReturnCode;
            }
            catch (WebException webEx)
            {
                // Note this error may be hit as a result of a base PageProxyUrl...  This will cause some issues.
                var t = new DowJonesUtilitiesException(webEx, ApplicationExceptionConstants.GeneralWebException);
                packetResult.ReturnCode = t.ReturnCode;
            }
            catch (Exception ex)
            {
                var t = new DowJonesUtilitiesException(ex, -1);
                packetResult.ReturnCode = t.ReturnCode;
            }

            if (packetResult.ReturnCode != 0)
            {
                packetResult.ErrorMessage = ResourceTextManager.GetErrorMessage(packetResult.ReturnCode.ToString(CultureInfo.InvariantCulture));
            }
            return packetResult;
        }

        private static DowJonesUtilitiesException ExtractAggregateException(AggregateException aggregateException)
        {
            // just extract the first non-aggregate exception
            // return it if it is DowJonesUtilitiesException
            // or return generic -1 exception
            foreach (var exception in aggregateException.InnerExceptions)
            {
                var exception1 = exception as AggregateException;
                if (exception1 != null)
                {
                    return ExtractAggregateException(exception1);
                }

                var utilitiesException = exception as DowJonesUtilitiesException;
                if (utilitiesException != null)
                {
                    return utilitiesException;
                }
            }

            return new DowJonesUtilitiesException(aggregateException, -1);
        }
    }
}