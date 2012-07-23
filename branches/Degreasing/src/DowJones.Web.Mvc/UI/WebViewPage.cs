using System.Web;
using DowJones.DependencyInjection;
using DowJones.Preferences;
using DowJones.Security;
using DowJones.Security.Interfaces;
using DowJones.Session;
using DowJones.Web.Mvc.Extensions;

namespace DowJones.Web.Mvc.UI
{
    public abstract class WebViewPage : WebViewPage<dynamic>
    {
    }

    public abstract class WebViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>
    {

        protected ViewComponentFactory DJ
        {
            get { return Html.DJ(); }
        }

        [Inject("No access to derived constructors")]
        public IUserContext UserContext { get; set; }

        protected IPrinciple Entitlements
        {
            get { return UserContext.Principle; }
        }

        protected IControlData ControlData
        {
            get { return UserContext.ControlData; }
        }

        protected IPreferences Preferences
        {
            get { return UserContext.Preferences; }
        }

        protected ScriptRegistryBuilder ScriptRegistry
        {
            get { return DJ.ScriptRegistry(); }
        }

        protected StylesheetRegistryBuilder StylesheetRegistry
        {
            get { return DJ.StylesheetRegistry(); }
        }


        protected IHtmlString Token(string tokenName)
        {
            return DJ.Token(tokenName);
        }
    }
}
