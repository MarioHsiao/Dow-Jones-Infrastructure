using System.Collections.Specialized;
using System.Security.Principal;
using System.Web;
using System.Web.SessionState;

namespace DowJones.Web.Mvc.Diagnostics.Fakes
{
    public class FakeHttpContext : HttpContextBase
    {
        private readonly string _relativeUrl;
        private readonly FakePrincipal _principal;
        private readonly NameValueCollection _formParams;
        private readonly NameValueCollection _queryStringParams;
        private readonly HttpCookieCollection _cookies;
        private readonly SessionStateItemCollection _sessionItems;
        private readonly HttpRequestBase _httpRequest;
        private readonly HttpResponseBase _httpResponse;
        private readonly string _httpMethod;

        public FakeHttpContext(string relativeUrl):this(relativeUrl, "GET", null, null, null, null, null)
        {
        }


        public FakeHttpContext(string relativeUrl, string httpMethod)
            : this(relativeUrl, httpMethod, null, null, null, null, null)
        {
        }


        public FakeHttpContext(string relativeUrl, string httpMethod, FakePrincipal principal, NameValueCollection formParams, NameValueCollection queryStringParams, HttpCookieCollection cookies, SessionStateItemCollection sessionItems )
        {
            _relativeUrl = relativeUrl;
            _httpMethod = httpMethod;
            _principal = principal;
            _formParams = formParams;
            _queryStringParams = queryStringParams;
            _cookies = cookies;
            _sessionItems = sessionItems;

            _httpRequest = new FakeHttpRequest(_relativeUrl, _httpMethod, _formParams, _queryStringParams, _cookies);
            _httpResponse = new FakeHttpResponse();
        }

        public override HttpRequestBase Request
        {
            get
            {
                return _httpRequest;
            }
        }

        public override HttpResponseBase Response
        {
            get
            {
                return _httpResponse;
            }
        }


        public override IPrincipal User
        {
            get
            {
                return _principal;
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        public override HttpSessionStateBase Session
        {
            get
            {
                return new FakeHttpSessionState(_sessionItems);
            }
        }



    }


}
