using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DowJones.Web.Mvc;
using DowJones.Web.Mvc.UI;
using DowJones.Web.Mvc.UI.Components.ArticleTranslator;

namespace DowJones.Web.Showcase.Controllers
{
    public class ArticleTranslatorController : DowJonesControllerBase
    {
        //
        // GET: /ArticleTranslator/

        public ActionResult Index()
        {
            return Components("Index", GetArticleTranslatorModel("OnLangClick"));
        }

        private ContentContainerModel GetArticleTranslatorModel(string clickHandler)
        {
            var articletranslator = new ArticleTranslatorModel();
            articletranslator.Tokens.ArticleTranslatorTitle = "Click to translate";
            articletranslator.SourceLanguage = "en";
            articletranslator.AccessionNo = "MDST000020100506e659000io";
            articletranslator.OnLangClick = clickHandler;
            var targetLa = new List<TargetLanguages>();
            targetLa.Add(new TargetLanguages { contentLanguage = ContentLanguage.en, LangText = "English" });
            targetLa.Add(new TargetLanguages { contentLanguage = ContentLanguage.fr, LangText = "French" });
            targetLa.Add(new TargetLanguages { contentLanguage = ContentLanguage.es, LangText = "Spanish" });
            targetLa.Add(new TargetLanguages { contentLanguage = ContentLanguage.de, LangText = "German" });
            targetLa.Add(new TargetLanguages { contentLanguage = ContentLanguage.ru, LangText = "Rusian" });

            articletranslator.Languages = targetLa.ToArray<TargetLanguages>();

            return new ContentContainerModel(new IViewComponentModel[] {articletranslator});
        }
    }
}
