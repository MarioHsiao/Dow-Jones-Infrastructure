using System;
using System.Collections.Specialized;
using System.Web;

namespace DowJones.Web.Mvc.Diagnostics.Fakes
{

    public class FakeHttpRequest : HttpRequestBase
    {
        private readonly string _relativeUrl;
        private readonly NameValueCollection _formParams;
        private readonly NameValueCollection _queryStringParams;
        private readonly HttpCookieCollection _cookies;
        private readonly string _httpMethod;


        public FakeHttpRequest(string relativeUrl, string httpMethod, NameValueCollection formParams, NameValueCollection queryStringParams, HttpCookieCollection cookies)
        {
            _relativeUrl = relativeUrl;
            _httpMethod = httpMethod;
            _formParams = formParams;
            _queryStringParams = queryStringParams;
            _cookies = cookies;
        }

        public override NameValueCollection Form
        {
            get
            {
                return _formParams;
            }
        }

        public override NameValueCollection QueryString
        {
            get
            {
                return _queryStringParams;
            }
        }

        public override HttpCookieCollection Cookies
        {
            get
            {
                return _cookies;
            }
        }

        public override string AppRelativeCurrentExecutionFilePath
        {
            get { return _relativeUrl; }
        }

        public override string PathInfo
        {
            get { return String.Empty; }
        }

        public override string HttpMethod
        {
            get
            {
                return _httpMethod;
            }
        }




    }



}
