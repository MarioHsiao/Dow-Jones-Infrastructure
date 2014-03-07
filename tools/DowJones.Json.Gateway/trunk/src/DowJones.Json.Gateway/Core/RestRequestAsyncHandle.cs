using System.Net;
using RestSharpRestRequestAsyncHandle = RestSharp.RestRequestAsyncHandle;

namespace DowJones.Json.Gateway.Core
{
    public class RestRequestAsyncHandle
    {
        public HttpWebRequest WebRequest;

        public RestRequestAsyncHandle()
        {
        }

        internal RestRequestAsyncHandle(RestSharpRestRequestAsyncHandle restRequestAsyncHandle)
        {
            WebRequest = restRequestAsyncHandle.WebRequest;
        }

        public void Abort()
        {
            if (WebRequest == null)
                return;
            ((WebRequest)WebRequest).Abort();
        }
    }
}
