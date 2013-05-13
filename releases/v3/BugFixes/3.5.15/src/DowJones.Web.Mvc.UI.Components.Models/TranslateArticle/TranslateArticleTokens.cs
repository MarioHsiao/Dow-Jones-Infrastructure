
using System.Text;
using DowJones.Infrastructure;
using DowJones.Token;
using DowJones.Web.Mvc.Infrastructure;

namespace DowJones.Web.Mvc.UI.Components.TranslateArticle
{
    /// <summary>
    /// The TranslatorTokens.
    /// </summary>
    public class TranslatorTokens : AbstractTokenBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TranslatorTokens"/> class.
        /// </summary>
        public TranslatorTokens()
        {
            IncorrectDataModel = GetTokenByName("incorrectdatamodelsupplied");
            //Langcode = GetTokenByName(_langcode);
            //LangText = GetTokenByName(_langText);
            ArticleTranslateDisclaimer = GetTokenByName("articleTranslateDisclaimer");
            ClickForFullDisclaimer = GetTokenByName("clickForFullDisclaimer");
            ArticleTranslateDisclaimerFull = GetTokenByName("articleTranslateDisclaimerFull");
            TranslatedByPre = GetTokenByName("translatedByPre");
            TranslatedByPost = GetTokenByName("translatedByPost");
            TranslatedByPostPost = GetTokenByName("translatedByPostPost");
            FCPBeta = GetTokenByName("fcpBeta");
            TranslationInProgress = GetTokenByName("translationInProgress");
            LargeArticleTranslationMsg = GetTokenByName("largeArticleTranslationMsg");
            TranslateMore = GetTokenByName("translateMore");
            TranslateAll = GetTokenByName("translateAll");
            TranslationSomewhatDifficult = GetTokenByName("translationSomewhatDifficult");
            TranslationVeryDifficult = GetTokenByName("translationVeryDifficult");
            TranslationGeneralUnderstanding = GetTokenByName("translationGeneralUnderstanding");
            TranslationSomewhatEasy = GetTokenByName("translationSomewhatEasy");
            TranslationVeryEasy = GetTokenByName("translationVeryEasy");
            RateTranslationReadability = GetTokenByName("rateTranslationReadability");
        }

        /// <summary>
        /// SetLangCodeToken
        /// <param name="token">Token
        /// </param>
        /// </summary>
        public void SetLangCodeToken(string token)
        {
            Langcode = GetTokenByName(string.Format("{0}", token));
            //return Langcode;
        }

        /// <summary>
        /// SetLangTextToken
        /// <param name="token">Token
        /// </param>
        /// </summary>
        public void SetLangTextToken(string token)
        {
            LangText = GetTokenByName(string.Format("lang{0}", token));
            //return LangText;
        }

        #region << Accessors >>

        private string _langcode;

        private string _langText;

        /// <summary>
        /// Gets or sets Langcode.
        /// </summary>
        public string Langcode
        {
            get { return _langcode; }
            set { _langcode = value; }
        }

        /// <summary>
        /// Gets or sets LangText.
        /// </summary>
        public string LangText
        {
            get { return _langText; }
            set { _langText = value; }
        }

        /// <summary>
        /// Gets or sets IncorrectDataModel.
        /// </summary>
        public string IncorrectDataModel { get; set; }

        /// <summary>
        /// Gets or sets ArticleTranslateDisclaimer.
        /// </summary>
        public string ArticleTranslateDisclaimer { get; set; }

        /// <summary>
        /// Gets or sets ClickForFullDisclaimer.
        /// </summary>
        public string ClickForFullDisclaimer { get; set; }

        /// <summary>
        /// Gets or sets ArticleTranslateDisclaimerFull.
        /// </summary>
        public string ArticleTranslateDisclaimerFull { get; set; }

        /// <summary>
        /// Gets or sets FCPBeta.
        /// </summary>
        public string FCPBeta { get; set; }

        /// <summary>
        /// Gets or sets TranslatedByPostPost.
        /// </summary>
        public string TranslatedByPostPost { get; set; }

        /// <summary>
        /// Gets or sets _translatedByPost.
        /// </summary>
        public string TranslatedByPost { get; set; }

        /// <summary>
        /// Gets or sets _translatedByPre.
        /// </summary>
        public string TranslatedByPre { get; set; }

        /// <summary>
        /// Gets or sets TranslateAll.
        /// </summary>
        public string TranslateAll { get; set; }

        /// <summary>
        /// Gets or sets TranslateMore.
        /// </summary>
        public string TranslateMore { get; set; }

        /// <summary>
        /// Gets or sets LargeArticleTranslationMsg.
        /// </summary>
        public string LargeArticleTranslationMsg { get; set; }

        /// <summary>
        /// Gets or sets TranslationInProgress.
        /// </summary>
        public string TranslationInProgress { get; set; }

        /// <summary>
        /// Gets or sets TranslationVeryDifficult.
        /// </summary>
        public string TranslationVeryDifficult { get; set; }

        /// <summary>
        /// Gets or sets TranslationSomewhatDifficult.
        /// </summary>
        public string TranslationSomewhatDifficult { get; set; }

        /// <summary>
        /// Gets or sets TranslationGeneralUnderstandingt.
        /// </summary>
        public string TranslationGeneralUnderstanding { get; set; }

        /// <summary>
        /// Gets or sets TranslationSomewhatEasy.
        /// </summary>
        public string TranslationSomewhatEasy { get; set; }

        /// <summary>
        /// Gets or sets TranslationVeryEasy.
        /// </summary>
        public string TranslationVeryEasy { get; set; }

        /// <summary>
        /// Gets or sets RateTranslationReadability.
        /// </summary>
        public string RateTranslationReadability { get; set; }

        #endregion
    }
}
