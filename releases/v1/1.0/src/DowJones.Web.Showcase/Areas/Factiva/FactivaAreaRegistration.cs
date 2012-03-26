using System.Web;
using System.Web.Mvc;

namespace DowJones.Web.Showcase.Areas.Factiva
{
    public class FactivaAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Factiva";
            }
        }

        public bool IsRoutingSupported
        {
            get
            {
                //_isRoutingSupported = _isRoutingSupported ??
                //    new HttpContextWrapper(HttpContext.Current).Request.GetIISVersion().SupportsRouting;
                _isRoutingSupported = _isRoutingSupported ??
                new HttpContextWrapper(HttpContext.Current).GetIISVersion().SupportsRouting;
                
                return _isRoutingSupported.GetValueOrDefault();
            }
            set { _isRoutingSupported = value; }
        }
        private bool? _isRoutingSupported;


        public override void RegisterArea(AreaRegistrationContext context)
        {
            if (IsRoutingSupported)
                MapExtensionlessRoutes(context);
            else
                MapRoutesWithExtensions(context);
        }

        protected void MapExtensionlessRoutes(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Factiva_default",
                "Factiva/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }

        protected void MapRoutesWithExtensions(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Factiva_default_with_aspx",
                "Factiva.aspx/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
