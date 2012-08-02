using System;
using System.Web;
using DowJones.Infrastructure;
using DowJones.Session;

namespace DowJones.Assemblers.Session
{
    [Obsolete("Use ControlDataFactory instead (class renamed)")]
    public class UserSessionControlDataFactory : ControlDataFactory
    {
        public UserSessionControlDataFactory(IUserSession session, HttpRequestBase request) 
            : base(session, request)
        {
        }
    }

    public class ControlDataFactory : Factory<IControlData>
    {
        protected readonly HttpRequestBase Request;
        protected readonly IUserSession Session;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlDataFactory" /> class.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="request">The request.</param>
        public ControlDataFactory(IUserSession session, HttpRequestBase request = null)
        {
            Request = request;
            Session = session;
        }

        public override IControlData Create()
        {
            var controlData = new ControlData {
                    AccessPointCode = Session.AccessPointCode,
                    AccessPointCodeUsage = Session.AccessPointCode,
                    Debug = Session.IsDebug,
                    ClientType = Session.ClientTypeCode,
                    IpAddress = (Request == null) ? null : Request.ServerVariables["REMOTE_ADDR"],
                    UserID = Session.UserId,
                    SessionID = Session.SessionId,
                    ProductID = Session.ProductId,
                    ProxyUserId = Session.IsProxySession ? Session.ProxyUserId : null,
                    ProxyProductId = Session.IsProxySession ? Session.ProxyNamespace : null
                };

            return controlData;
        }
    }
}
