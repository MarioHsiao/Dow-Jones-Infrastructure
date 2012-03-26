// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArticleTokens.cs" company="Dow Jones">
//    © 2010 Dow Jones, Inc. All rights reserved. 	
// </copyright>
// <summary>
//   The article tokens.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


using DowJones.Infrastructure;
namespace DowJones.Web.Mvc.UI.Components.Article
{
    /// <summary>
    /// The article tokens.
    /// </summary>
    public class ArticleTokens : AbstractTokenBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleTokens"/> class.
        /// </summary>
        public ArticleTokens()
        {
            Document = GetTokenByName("document");
            Words = GetTokenByName("words");
            Language = string.Empty;
            ReadSpeakerAttribution = GetTokenByName("attribution");
            ReadSpeakerDownload = GetTokenByName("download");
            ReadSpeakerListenToArticle = GetTokenByName("listentoarticle");
            IncorrectDataModel = GetTokenByName("incorrectdatamodelsupplied");
            Language = GetTokenByName("langEn");
            ArticleTranslatorTitle = GetTokenByName("articleTranslatorTitle");
        }

        #region << Accessors >>

        /// <summary>
        /// Gets or sets Document.
        /// </summary>
        public string Document { get; set; }

        /// <summary>
        /// Gets or sets Document.
        /// </summary>
        public string ArticleTranslatorTitle { get; set; }

        /// <summary>
        /// Gets or sets Words.
        /// </summary>
        public string Words { get; set; }

        /// <summary>
        /// Gets or sets Language.
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets ReadSpeakerAttribution.
        /// </summary>
        public string ReadSpeakerAttribution { get; set; }

        /// <summary>
        /// Gets or sets ReadSpeakerDownload.
        /// </summary>
        public string ReadSpeakerDownload { get; set; }

        /// <summary>
        /// Gets or sets ReadSpeakerListenToArticle.
        /// </summary>
        public string ReadSpeakerListenToArticle { get; set; }

        /// <summary>
        /// Gets or sets IncorrectDataModel.
        /// </summary>
        public string IncorrectDataModel { get; set; }

        #endregion
    }
}