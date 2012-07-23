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
        private readonly HttpRequestBase _request;
        private readonly IUserSession _session;

        public ControlDataFactory(IUserSession session, HttpRequestBase request = null)
        {
            _request = request;
            _session = session;
        }

        public override IControlData Create()
        {
            var controlData = new ControlData {
                    AccessPointCode = _session.AccessPointCode,
                    AccessPointCodeUsage = _session.AccessPointCode,
                    Debug = _session.IsDebug,
                    ClientCode = _session.ClientTypeCode,
                    IpAddress = (_request == null) ? null : _request.ServerVariables["REMOTE_ADDR"],
                    UserID = _session.UserId,
                    SessionID = _session.SessionId,
                    ProductID = _session.ProductId,
                    ProxyUserId = _session.IsProxySession ? _session.ProxyUserId : null,
                    ProxyProductId = _session.IsProxySession ? _session.ProxyNamespace : null
                };

            return controlData;
        }
    }
}
