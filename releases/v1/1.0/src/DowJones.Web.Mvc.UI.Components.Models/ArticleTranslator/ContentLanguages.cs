using DowJones.Utilities.Attributes;

namespace DowJones.Web.Mvc.UI.Components.ArticleTranslator
{

    /// <summary>
    /// Supported content languages.
    /// </summary>
    public enum ContentLanguage
    {
        /// <summary>
        /// Corresponds to German. Editor's Choice Language.
        /// </summary>
        [AutomatedTranslationSupportedLanguage]
        [AssignedToken("langDe")]
        de,
        /// <summary>
        /// Corresponds to English. Editor's Choice Language.
        /// </summary>
        [AutomatedTranslationSupportedLanguage]
        [AssignedToken("langEn")]
        en,
        /// <summary>
        /// Corresponds to Spanish. Editor's Choice Language.
        /// </summary>
        [AutomatedTranslationSupportedLanguage]
        [AssignedToken("langEs")]
        es,
        /// <summary>
        /// Corresponds to French. Editor's Choice Language.
        /// </summary>
        [AutomatedTranslationSupportedLanguage]
        [AssignedToken("langFr")]
        fr,
        /// <summary>
        /// Corresponds to Italian.
        /// </summary>
        [AutomatedTranslationSupportedLanguage]
        [AssignedToken("langIt")]
        it,
        /// <summary>
        /// Corresponds to Japanese.
        /// </summary>
        [AutomatedTranslationSupportedLanguage]
        [AssignedToken("langJa")]
        ja,
        /// <summary>
        /// Corresponds to Russian. Editor's Choice Language.
        /// </summary>
        [AutomatedTranslationSupportedLanguage]
        [AssignedToken("langRu")]
        ru,
        /// <summary>
        /// Corresponds to Chinese - Mainland.
        /// </summary>
        [AutomatedTranslationSupportedLanguage]
        [AssignedToken("langZhcn")]
        zhcn,
        /// <summary>
        /// Corresponds to Chinese - Taiwain.
        /// </summary>
        [AutomatedTranslationSupportedLanguage]
        [AssignedToken("langZhtw")]
        zhtw,
        /// <summary>
        /// Corresponds to Chinese - Taiwain.
        /// </summary>
        [AssignedToken("langKo")]
        ko,
        /// <summary>
        /// Corresponds to Arabic.
        /// </summary>
        [AssignedToken("langAr")]
        ar,
    }
}
