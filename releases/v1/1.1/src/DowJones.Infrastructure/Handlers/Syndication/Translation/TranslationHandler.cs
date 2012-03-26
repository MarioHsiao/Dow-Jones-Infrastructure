using System;
using System.Web;

namespace EMG.Utility.Handlers.Syndication.Translation
{
    public class TranslationHandler : IHttpAsyncHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            throw new System.NotImplementedException();
        }

        public bool IsReusable
        {
            get { throw new System.NotImplementedException(); }
        }

        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            throw new System.NotImplementedException();
        }

        public void EndProcessRequest(IAsyncResult result)
        {
            throw new System.NotImplementedException();
        }
    }
}
