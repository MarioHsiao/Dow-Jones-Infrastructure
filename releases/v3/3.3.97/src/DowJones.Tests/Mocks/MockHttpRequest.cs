using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;

namespace DowJones.Mocks
{
    public class MockHttpRequest : HttpRequestBase
    {
        private HttpCookieCollection _cookies;
        private NameValueCollection _headers;

        public IDictionary<string, string> Values { get; private set; }

        public MockHttpRequest(IDictionary<string, string> values = null)
        {
            Values = values ?? new Dictionary<string, string>();
        }

        public override HttpCookieCollection Cookies
        {
            get { return _cookies = _cookies ?? new HttpCookieCollection(); }
        }

        public override NameValueCollection Headers
        {
            get { return _headers = _headers ?? new NameValueCollection(); }
        }

        public override string this[string key]
        {
            get { return Values.ContainsKey(key) ? Values[key] : null; }
        }
    }
}