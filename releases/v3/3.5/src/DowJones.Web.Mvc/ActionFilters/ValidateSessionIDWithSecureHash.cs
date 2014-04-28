using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DowJones.DependencyInjection;
using DowJones.Session;
using Factiva.Gateway.Messages.Membership.Authorization.V1_0;
using Factiva.Gateway.Services.V1_0;

namespace DowJones.Web.Mvc.ActionFilters
{
    public class ValidateSessionIDWithSecureHash : FilterAttribute, IAuthorizationFilter
    {
        protected internal IUserSession Session
        {
            get { return _session ?? ServiceLocator.Resolve<IUserSession>(); }
            set { _session = value; }
        }
        private IUserSession _session;

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsSecureConnection)
            {
                return;
            }
            var sid = Session.SessionId;
            if (string.IsNullOrEmpty(sid))
            {
                return;
            }
            var hash = Session.SecurityHash;
            if (String.IsNullOrEmpty(hash))
            {
                return;
            }

            var cdata = new Factiva.Gateway.Utils.V1_0.ControlData
            {
                SessionID = sid,
                AccessPointCode = Session.AccessPointCode,
                ClientType = Session.ClientTypeCode,
                IPAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"],

            };
            cdata.Add("securityHash", hash);
            var valRequest = new ValidateSessionIdRequest();
            var sResponse = MembershipService.ValidateSessionId(cdata, valRequest);

        }
    }
}
