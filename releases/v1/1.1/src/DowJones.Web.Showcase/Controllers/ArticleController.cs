using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DowJones.Session;
using DowJones.Web.Mvc;
using DowJones.Web.Mvc.UI;
using DowJones.Web.Mvc.UI.Components.Article;
using DowJones.Web.Mvc.UI.Components.ArticleTranslator;
using DowJones.Web.Mvc.UI.Components.SocialButtons;
using DowJones.Utilities.Managers;
using Factiva.Gateway.Messages.Archive.V2_0;
using Factiva.Gateway.Services.V2_0;
using Factiva.Gateway.Utils.V1_0;
using Factiva.Gateway.V1_0;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;
using DowJones.Web.Showcase.Extensions;
using DowJones.Tools.Ajax.Converters;
using DowJones.Utilities.Formatters.Globalization;
using DowJones.Utilities.Managers.Search;
using DowJones.Ajax.Article;
using DowJones.Infrastructure;

namespace DowJones.Web.Showcase.Controllers
{
    public class ArticleController : DowJonesControllerBase
    {
        private readonly ArticleConversionManager _articleManger;
        private DowJones.Ajax.Article.Converter.ArticleConversionManager articleConverter;

        public ArticleController()
        {
            string uri = "http://minint-qmseck6/newspagessandbox/DowJones.Utilities.Handlers.ArticleControl.ContentHandler.ashx";
            _articleManger = new ArticleConversionManager(uri, new DowJones.Utilities.Formatters.Globalization.DateTimeFormatter("en"));;
        }

        public ActionResult Index(string acn = "X900740020110224e72o0008e")
        {
            return View("Index", GetArticleModel(acn));
        }

        public ActionResult Latest()
        {
            string accessionNo = "WSJO000020101214e6cc004bl";
            Models.ArticleModel model = new Models.ArticleModel{ Article=_articleManger.GetArticle(accessionNo)};            
            return View(model);
        }

        public ActionResult NewRazor(string acn = "X900740020110224e72o0008e")
        {
            return View("NewRazor", GetArticleNew(acn));
        }

        private ArticleModel GetArticleNew(string acn)
        {
            ControlData cd = ControlDataManager.GetLightWeightUserControlData("dacostad", "brian", "16");
            Session["cData"] = cd;
            var targetLa = new List<TargetLanguages>
                               {
                                   new TargetLanguages {contentLanguage = ContentLanguage.en, LangText = "English"},
                                   new TargetLanguages {contentLanguage = ContentLanguage.fr, LangText = "French"}, 
                                   new TargetLanguages {contentLanguage = ContentLanguage.es, LangText = "Spanish"}, 
                                   new TargetLanguages {contentLanguage = ContentLanguage.de, LangText = "German"}, 
                                   new TargetLanguages {contentLanguage = ContentLanguage.ru, LangText = "Rusian"}
                               };

            var articleTranslatorModel = new ArticleTranslatorModel()
            {
                //AccessionNo = "MDST000020100506e659000io",
                //AccessionNo ="WSJO000020101214e6cc004bl",
                AccessionNo = acn, //"X900740020110224e72o0008e",
                SourceLanguage = "en",
                OnLangClick = "onLangTransClick",
                Languages = targetLa.ToArray<TargetLanguages>()
            };
            articleTranslatorModel.Tokens.ArticleTranslatorTitle = "Click to translate";

            List<SocialNetworks> socNetworks = new List<SocialNetworks>
                                                    {
                                                        SocialNetworks.Delicious,
                                                        SocialNetworks.Digg,
                                                        SocialNetworks.Facebook,
                                                        SocialNetworks.Furl,
                                                        SocialNetworks.Google,
                                                        SocialNetworks.LinkedIn,
                                                        SocialNetworks.Newsvine,
                                                        SocialNetworks.Reddit,
                                                        SocialNetworks.StumbleUpon,
                                                        SocialNetworks.Technorati,
                                                        SocialNetworks.Twitter,
                                                        SocialNetworks.Yahoo
                                                    };

            var socialButton = new SocialButtonsModel()
            {
                ImageSize = ImageSize.Small,
                SocialNetworks = socNetworks,
                Target = "_blank",
                Title = "Dow Jones",
                Description = "Financial",
                Url = "http://dowjones.com",
                Keywords = "factiva, socials, bank"
            };
            var basePref = new BasePreferences();
            var art = GetArticle(acn);
            
            var manager = new SearchManager(cd, "en");
            articleConverter = new Ajax.Article.Converter.ArticleConversionManager(
                            manager, null, new PostProcessing(), art, cd, basePref,
                            true, true, true, "T|google T|en N|la O|c O|+ T|article T|file O|, T|report O|, N|fmt O|c O|+ N|pd D|-0366 D| O|d O|+",
                            true, "http://localhost:44355/DowJones.Utilities.Handlers.ArticleControl.ContentHandler.ashx");
        
            ArticleResultset result = articleConverter.Process();

            var article = new ArticleModel(cd, basePref)
            {
                ShowReadSpeaker = true,
                ShowSocialButtons = true,
                ShowTranslator = true,
                TranslatorModel = articleTranslatorModel,
                SocialButtonModel = socialButton,
                ArticleObject = art,
                ArticleDataSet = result,
                ShowHighlighting = true,
                EnableELinks = true,
                ShowSourceLinks = true,
                ShowCompanyEntityReference = true,
                ShowExecutiveEntityReference = true,
                CanonicalSearchString = "T|google T|en N|la O|c O|+ T|article T|file O|, T|report O|, N|fmt O|c O|+ N|pd D|-0366 D| O|d O|+"
            };

            return article;
        }

        private ArticleModel GetArticleModel(string acn)
        {
            ControlData cd = ControlDataManager.GetLightWeightUserControlData("dacostad", "brian", "16");
            Session["cData"] = cd;
            var targetLa = new List<TargetLanguages>
                               {
                                   new TargetLanguages {contentLanguage = ContentLanguage.en, LangText = "English"},
                                   new TargetLanguages {contentLanguage = ContentLanguage.fr, LangText = "French"}, 
                                   new TargetLanguages {contentLanguage = ContentLanguage.es, LangText = "Spanish"}, 
                                   new TargetLanguages {contentLanguage = ContentLanguage.de, LangText = "German"}, 
                                   new TargetLanguages {contentLanguage = ContentLanguage.ru, LangText = "Rusian"}
                               };

            var articleTranslatorModel = new ArticleTranslatorModel()
            {
                //AccessionNo = "MDST000020100506e659000io",
                //AccessionNo ="WSJO000020101214e6cc004bl",
                AccessionNo = acn, //"X900740020110224e72o0008e",
                SourceLanguage = "en",
                OnLangClick = "onLangTransClick",
                Languages = targetLa.ToArray<TargetLanguages>()
            };
            articleTranslatorModel.Tokens.ArticleTranslatorTitle = "Click to translate";

            List<SocialNetworks> socNetworks = new List<SocialNetworks>
                                                    {
                                                        SocialNetworks.Delicious,
                                                        SocialNetworks.Digg,
                                                        SocialNetworks.Facebook,
                                                        SocialNetworks.Furl,
                                                        SocialNetworks.Google,
                                                        SocialNetworks.LinkedIn,
                                                        SocialNetworks.Newsvine,
                                                        SocialNetworks.Reddit,
                                                        SocialNetworks.StumbleUpon,
                                                        SocialNetworks.Technorati,
                                                        SocialNetworks.Twitter,
                                                        SocialNetworks.Yahoo
                                                    };

            var socialButton = new SocialButtonsModel()
            {
                ImageSize= ImageSize.Small,
                SocialNetworks = socNetworks,
                Target="_blank",
                Title = "Dow Jones",
                Description = "Financial",
                Url = "http://dowjones.com",
                Keywords = "factiva, socials, bank"
            };
            var basePref = new BasePreferences();
            basePref.ClockType = ClockType.TwentyFourHours;
            var art = GetArticle(acn);
            var article = new ArticleModel(cd,basePref)
                              {
                                  ShowReadSpeaker = true,
                                  ShowSocialButtons = true,
                                  ShowTranslator = true,
                                  TranslatorModel = articleTranslatorModel,
                                  SocialButtonModel = socialButton,
                                  ArticleObject = art,
                                  ShowHighlighting = true,
                                  EnableELinks = true,
                                  ShowSourceLinks = true,
                                  ShowCompanyEntityReference = true,
                                  ShowExecutiveEntityReference = true,
                                  CanonicalSearchString = "T|google T|en N|la O|c O|+ T|article T|file O|, T|report O|, N|fmt O|c O|+ N|pd D|-0366 D| O|d O|+"
                              };

            return  article;
        }

        /// <summary>
        /// The get article.
        /// </summary>
        /// <param name="accno">
        /// The accno.
        /// </param>
        /// <returns>
        /// </returns>
        protected Factiva.Gateway.Messages.Archive.V2_0.Article GetArticle(string accno)
        {
            var cData = (ControlData)Session["cData"];

            var articleRequest = new GetArticleRequest
            {
                //canonicalSearchString = "T|google T|en N|la O|c O|+ T|article T|file O|, T|report O|, N|fmt O|c O|+ N|pd D|-0366 D| O|d O|+",
                accessionNumbers = new[] { accno },
                responseDataSet = GetBaselinResponseDataSet(),
                //responseDataSet = new Factiva.Gateway.Messages.Archive.V1_0.ResponseDataSet { articleFormat = ArticleFormatType.FULL },
                //usageAggregator = ""
            };
            
            // DJDN000020100310e63a0ljs4 J000000020090814e58e00013

            // Make the call to the backend
            var serviceResponse = ArchiveService.GetArticle(ControlDataManager.Clone(cData), articleRequest);

            if (serviceResponse.rc == 0)
            {
                object responseObj;
                var responseObjRC = serviceResponse.GetResponse(ServiceResponse.ResponseFormat.Object, out responseObj);

                if (responseObjRC == 0)
                {
                    var response = (GetArticleResponse)responseObj;
                    if (response != null &&
                        response.articleResponseSet != null &&
                        response.articleResponseSet.countSpecified &&
                        response.articleResponseSet.count > 0 &&
                        response.articleResponseSet.article[0] != null)
                    {
                        return response.articleResponseSet.article[0];
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// The get baselin response data set.
        /// </summary>
        /// <returns>
        /// </returns>
        private static Factiva.Gateway.Messages.Archive.V2_0.ResponseDataSet GetBaselinResponseDataSet()
        {
            var responseDataSet = new Factiva.Gateway.Messages.Archive.V2_0.ResponseDataSet
            {
                articleFormat = ArticleFormatType.Custom,
                fids = new[]
                                                     {
                                                         DistDocField.LogoImage, DistDocField.SrcName, DistDocField.Ct, 
                                                         DistDocField.Clm, DistDocField.Editor, DistDocField.Rf, 
                                                         DistDocField.Se, DistDocField.PubPage, DistDocField.AccessionNo, 
                                                         DistDocField.Hd, DistDocField.PubVol, DistDocField.In, 
                                                         DistDocField.By, DistDocField.BaseLang, DistDocField.Co, 
                                                         DistDocField.Cr, DistDocField.Art, DistDocField.Re, 
                                                         DistDocField.WordCount, DistDocField.Cy, DistDocField.Ns, 
                                                         DistDocField.PubDate, DistDocField.Cx, DistDocField.AdocTOC, 
                                                         DistDocField.Editor, DistDocField.Lp, DistDocField.SrcCode, 
                                                         DistDocField.Td,DistDocField.PubTime
                                                     }
            };
            return responseDataSet;
        }

    }
}
