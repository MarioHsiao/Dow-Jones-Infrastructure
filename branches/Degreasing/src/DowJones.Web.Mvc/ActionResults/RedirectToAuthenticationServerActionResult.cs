using System.Web.Mvc;
using DowJones.Extensions;

namespace DowJones.Web.Mvc.ActionResults
{
    public class AuthServerLoginRedirectActionResult : ContentResult
    {
        public string InterfaceLanguage { get; set; }
        public string LandingPage { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            var loginForm = new LoginServerForm(context.HttpContext.Request);

            if (InterfaceLanguage.HasValue())
                loginForm.InterfaceLanguage = InterfaceLanguage;

            if (LandingPage.HasValue())
                loginForm.RedirectUrl = LandingPage;

            ContentType = KnownMimeTypes.Html;
            Content = loginForm.ToHtmlString();

            base.ExecuteResult(context);
        }
    }
}
