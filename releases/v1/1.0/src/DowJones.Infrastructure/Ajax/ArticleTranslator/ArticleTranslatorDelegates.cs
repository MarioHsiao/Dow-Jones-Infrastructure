using DowJones.Tools.Ajax;
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArticleTranslatorDelegates.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved. 	
// </copyright>
// <summary>
//   Entites for Article translator web service
// </summary>
// <author>
//   Pramod Sankar
// </author>
// <lastModified>
// </lastModified>
// --------------------------------------------------------------------------------------------------------------------
namespace DowJones.Utilities.Ajax.ArticleTranslator
{
    public enum ArticleTranslatorAction
    {
       TranslateArticle,
        UpdateRatings
    }

    /// <summary>
    /// ArticleTranslatorRequestDelegate
    /// <remarks> Request object for ArticleTranslator
    /// </remarks>
    /// </summary>
    public class ArticleTranslatorRequestDelegate : IAjaxRequestDelegate
    {
        public string SourceText{ get;  set;}
        public string SourceLanguage { get; set; }
        public string TargetLanguage { get; set; }
        public string InputFormat { get; set; }
       
    }

    /// <summary>
    /// ArticleTranslatorResponseDelegate
    /// <remarks> Response object for TranslateArticle
    /// </remarks>
    /// </summary>
    public class ArticleTranslatorResponseDelegate : AbstractAjaxResponseDelegate
    {

        public string TranslatedText { get; set; }
    }

    /// <summary>
    /// UpdateRatingsRequestDelegate
    /// <remarks> Request object for UpdateRatings
    /// </remarks>
    /// </summary>
    public class UpdateRatingsRequestDelegate : IAjaxRequestDelegate
    {
        public string SourceLanguage { get; set; }
        public string TargetLanguage { get; set; }
        public string AccessionNumber { get; set; }
        public string WordCount { get; set; }
        public string Rating { get; set; }
        public string SourceName { get; set; }
    }

    /// <summary>
    /// UpdateRatingsResponseDelegate
    /// <remarks> Response object for UpdateRatings
    /// </remarks>
    /// </summary>
    public class UpdateRatingsResponseDelegate : AbstractAjaxResponseDelegate
    {

    }
}
