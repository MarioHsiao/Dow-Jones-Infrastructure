using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;

namespace DowJones.Web.Mvc.Diagnostics.Fakes
{
    public class FakeControllerContext : ControllerContext
    {
        public FakeControllerContext(System.Web.Mvc.ControllerBase controller)
            : this(controller, String.Empty, null, null, null, null, null, null)
        {
        }

        public FakeControllerContext(System.Web.Mvc.ControllerBase controller, RouteData routeData)
            : this(controller, String.Empty, null, null, null, null, null, null)
        {
        }


        public FakeControllerContext(System.Web.Mvc.ControllerBase controller, HttpCookieCollection cookies)
            : this(controller, String.Empty, null, null, null, null, cookies, null)
        {
        }

        public FakeControllerContext(System.Web.Mvc.ControllerBase controller, SessionStateItemCollection sessionItems)
            : this(controller, String.Empty, null, null, null, null, null, sessionItems)
        {
        }


        public FakeControllerContext(System.Web.Mvc.ControllerBase controller, NameValueCollection formParams)
            : this(controller, String.Empty, null, null, formParams, null, null, null)
        {
        }


        public FakeControllerContext(System.Web.Mvc.ControllerBase controller, NameValueCollection formParams, NameValueCollection queryStringParams)
            : this(controller, String.Empty, null, null, formParams, queryStringParams, null, null)
        {
        }



        public FakeControllerContext(System.Web.Mvc.ControllerBase controller, string userName)
            : this(controller, String.Empty, userName, null, null, null, null, null)
        {
        }


        public FakeControllerContext(System.Web.Mvc.ControllerBase controller, string userName, string[] roles)
            : this(controller, String.Empty, userName, roles, null, null, null, null)
        {
        }


        public FakeControllerContext
            (
                System.Web.Mvc.ControllerBase controller,
                string relativeUrl,
                string userName,
                string[] roles,
                NameValueCollection formParams,
                NameValueCollection queryStringParams,
                HttpCookieCollection cookies,
                SessionStateItemCollection sessionItems
            )
            : base(new FakeHttpContext(relativeUrl, "GET", new FakePrincipal(new FakeIdentity(userName), roles), formParams, queryStringParams, cookies, sessionItems), new RouteData(), controller)
        { }
 
       
    
    
    }
}