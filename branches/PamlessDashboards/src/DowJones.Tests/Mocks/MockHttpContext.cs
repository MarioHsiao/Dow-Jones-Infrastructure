using System.Web;

namespace DowJones.Mocks
{
    public class MockHttpContext : HttpContextBase
    {
        private readonly HttpRequestBase _request;
        private readonly HttpResponseBase _response;

        public MockHttpContext(HttpRequestBase request, HttpResponseBase response)
        {
            _request = request;
            _response = response;
        }

        public override HttpRequestBase Request
        {
            get { return _request; }
        }

        public override HttpResponseBase Response
        {
            get { return _response; }
        }
    }
}
