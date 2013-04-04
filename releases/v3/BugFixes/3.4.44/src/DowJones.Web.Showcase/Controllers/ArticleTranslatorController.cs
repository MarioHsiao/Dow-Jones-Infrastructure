using System.Web.Mvc;
using DowJones.Web.Mvc.UI;
using DowJones.Web.Mvc.UI.Components.ArticleTranslator;
using ControllerBase = DowJones.Web.Mvc.ControllerBase;

namespace DowJones.Web.Showcase.Controllers
{
    public class ArticleTranslatorController : ControllerBase
    {
        public ActionResult Index()
        {
            var translator = GetArticleTranslator("OnLangClick");
            return Components("Index", translator);
        }

        private ContentContainerModel GetArticleTranslator(string clickHandler)
        {
            var articletranslator = new ArticleTranslatorModel();
            articletranslator.Tokens.ArticleTranslatorTitle = "Click to translate";
            articletranslator.SourceLanguage = Preferences.InterfaceLanguage;
            articletranslator.AccessionNo = "MDST000020100506e659000io";
            articletranslator.OnLangClick = clickHandler;
      
            return new ContentContainerModel(new IViewComponentModel[] {articletranslator});
        }
    }
}
