using System.Web;
using DowJones.DependencyInjection;

namespace DowJones.Web
{
    public abstract class HttpHandlerBase : IHttpHandler
    {
        public virtual bool IsReusable
        {
            get { return true; }
        }
        
        public void ProcessRequest(HttpContext context)
        {
            ServiceLocator.Current.Inject(this);
            OnProcessRequest(new HttpContextWrapper(context));
        }

        protected abstract void OnProcessRequest(HttpContextBase context);
    }
}
