using System.Web;
using System.Web.Mvc;

namespace DowJones.Web.Mvc.Diagnostics
{
    public class DiagnosticsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Diagnostics";
            }
        }

        public bool IsRoutingSupported
        {
            get
            {
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
                "Diagnostics_default",
                "Diagnostics/{controller}/{action}/{id}",
                new { controller="Homepage", action = "Index", id = UrlParameter.Optional }
            );
        }

        protected void MapRoutesWithExtensions(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Diagnostics_default_with_aspx",
                "Diagnostics.aspx/{controller}/{action}/{id}",
                new { controller = "Homepage", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
