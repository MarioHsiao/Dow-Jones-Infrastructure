using DowJones.Attributes;
using DowJones.Converters;
using Newtonsoft.Json;

namespace DowJones.Globalization
{
    /// <summary>
    /// Supported content languages.
    /// </summary>
    [JsonConverter(typeof(GeneralJsonEnumConverter))]
    public enum ContentLanguage
    {
        /// <summary>
        /// Corresponds to Bulgarian.
        /// </summary>
        [AutomatedTranslationSupportedLanguage]
        [AssignedToken("langBg")]
        bg = 0,
        /// <summary>
        /// Corresponds to Catalan.
        /// </summary>
        [AssignedToken("langCa")]
        ca,
        /// <summary>
        /// Corresponds to Czech.
        /// </summary>
        [AssignedToken("langCs")]
        cs,
        /// <summary>
        /// Corresponds to Danish.
        /// </summary>
        [AssignedToken("langDa")]
        da,
        /// <summary>
        /// Corresponds to German. Editor's Choice Language.
        /// </summary>
        [EditorsChoiceLanguage]
        [ReadSpeakerSupportedLanguage]
        [AutomatedTranslationSupportedLanguage]
        [AssignedToken("langDe")]
        de,
        /// <summary>
        /// Corresponds to English. Editor's Choice Language.
        /// </summary>
        [EditorsChoiceLanguage]
        [ReadSpeakerSupportedLanguage]
        [AutomatedTranslationSupportedLanguage]
        [AssignedToken("langEn")]
        en,
        /// <summary>
        /// Corresponds to Spanish. Editor's Choice Language.
        /// </summary>
        [EditorsChoiceLanguage]
        [ReadSpeakerSupportedLanguage]
        [AutomatedTranslationSupportedLanguage]
        [AssignedToken("langEs")]
        es,
        /// <summary>
        /// Corresponds to Farsi. Editor's Choice Language.
        /// </summary>
        [EditorsChoiceLanguage]
        [ReadSpeakerSupportedLanguage]
        [AutomatedTranslationSupportedLanguage]
        [AssignedToken("langFa")]
        fa,
        /// <summary>
        /// Corresponds to Finnish.
        /// </summary>
        [AssignedToken("langFi")]
        fi,
        /// <summary>
        /// Corresponds to French. Editor's Choice Language.
        /// </summary>
        [EditorsChoiceLanguage]
        [ReadSpeakerSupportedLanguage]
        [AutomatedTranslationSupportedLanguage]
        [AssignedToken("langFr")]
        fr,
        /// <summary>
        /// Corresponds to Hungarian.
        /// </summary>
        [AssignedToken("langHu")]
        hu,
        /// <summary>
        /// Corresponds to Italian.
        /// </summary>
        [ReadSpeakerSupportedLanguage]
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
        /// Corresponds to Dutch.
        /// </summary>
        [AssignedToken("langNl")]
        nl,
        /// <summary>
        /// Corresponds to Norwegian.
        /// </summary>
        [AssignedToken("langNo")]
        no,
        /// <summary>
        /// Corresponds to Polish.
        /// </summary>
        [AssignedToken("langPl")]
        pl,
        /// <summary>
        /// Corresponds to Portuguese.
        /// </summary>
        [AssignedToken("langPt")]
        [AutomatedTranslationSupportedLanguage]
        pt,
        /// <summary>
        /// Corresponds to Russian. Editor's Choice Language.
        /// </summary>
        [EditorsChoiceLanguage]
        [AutomatedTranslationSupportedLanguage]
        [AssignedToken("langRu")]
        ru,
        /// <summary>
        /// Corresponds to Slovak.
        /// </summary>
        [AssignedToken("langSk")]
        sk,
        /// <summary>
        /// Corresponds to Swedish.
        /// </summary>	
        [AssignedToken("langSv")]
        sv,
        /// <summary>
        /// Corresponds to Turkish.
        /// </summary>	
        [AssignedToken("langTr")]
        tr,
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
        //Infosys-23/Oct/2010-START:Bahasa changes
        /// <summary>
        /// Corresponds to BahasaIndonesia
        /// </summary>
        [AssignedToken("langId")]
        id,
        /// <summary>
        /// Corresponds to BahasaMalaysia
        /// </summary>
        [AssignedToken("langMs")]
        ms,
        //Infosys-23/Oct/2010-END:Bahasa changes
        /// <summary>
        /// Corresponds to Arabic.
        /// </summary>
        [AssignedToken("langAr")]
        ar,
        /// <summary>
        /// Corresponds to Vietnamese.
        /// </summary>
        [AssignedToken("langVi")]
        vi,

        /// <summary>
        /// Corresponds to Arabic.
        /// </summary>
        [AssignedToken("langTh")]
        th,
    }
}
