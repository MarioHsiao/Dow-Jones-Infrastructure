// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArticleTranslator.cs" company="Dow Jones">
//    © 2010 Dow Jones, Inc. All rights reserved. 	
// </copyright>
// <summary>
//   The article model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

namespace DowJones.Web.Mvc.UI.Components.ArticleTranslator
{
    /// <summary>
    /// The article translator model.
    /// </summary>
    /// <remarks></remarks>
    public class ArticleTranslatorModel : ViewComponentModel
    {

        /// <summary>
        /// Gets or sets Tokens.
        /// </summary>
        [ClientTokens]
        public ArticleTranslatorTokens Tokens { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleTranslatorModel"/> class.
        /// </summary>
        public ArticleTranslatorModel()
        {
            Tokens = new ArticleTranslatorTokens();
            Languages = TargetLanguage.All;
        }

        #region ..:: Properties ::..

        /// <summary>
        /// Gets or sets the languages.
        /// </summary>
        /// <value>The languages.</value>
        /// <remarks></remarks>
        public IEnumerable<TargetLanguage> Languages { get; set; }

        /// <summary>
        /// Gets or sets the source language.
        /// </summary>
        /// <value>The source language.</value>
        /// <remarks></remarks>
        public string SourceLanguage { get; set; }

        /// <summary>
        /// Gets or sets Accession number.
        /// </summary>
        public string AccessionNo { get; set; }

        #endregion

        /// <summary>
        /// Gets or sets the on lang click.
        /// </summary>
        /// <value>The on lang click.</value>
        /// <remarks>The OnLangClick event handler name.</remarks>
        [ClientEventHandler("dj.ArticleTranslatorControl.langClick")]
        public string OnLangClick { get; set; }

        /// <summary>
        /// Gets the sorted languages.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public IEnumerable<string> GetSortedLanguages()
        {
            // The format of each element here will be: {translated language name}|{language code}
            IEnumerable<string> translatedlanguages =
                from language in Languages
                let translatedText = 
                    string.IsNullOrEmpty(language.LangText) 
                        ? Tokens.GetLangCodeToken(language.contentLanguage.ToString()) 
                        : Tokens.SetLangTextToken(language.contentLanguage.ToString(), language.LangText) 
                orderby translatedText
                select string.Format("{0}|{1}", translatedText, language.contentLanguage);

            return translatedlanguages.ToArray();
        }
    }
}
