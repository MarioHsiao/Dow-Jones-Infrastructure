using System.Web;

namespace DowJones.Mocks
{
    public class MockHttpResponse : HttpResponseBase
    {
        private HttpCookieCollection _cookies;

        public override HttpCookieCollection Cookies
        {
            get { return _cookies = _cookies ?? new HttpCookieCollection(); }
        }

    }
}
