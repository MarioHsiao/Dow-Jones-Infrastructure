using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.ComponentModel;
using DowJones.Converters;
using DowJones.Infrastructure;
using Factiva.Gateway.Messages.Archive.V1_0;
using MessagesArticle = Factiva.Gateway.Messages.Archive.V1_0.Article;
using System.Text.RegularExpressions;

namespace DowJones.Web.Mvc.UI.Components.TranslateArticle
{
    /// <summary>
    /// translate article model
    /// </summary>
    /// <remarks></remarks>
    public class TranslateArticleModel: ViewComponentModel
    {
        #region ..:: Enumeration(s) ::..

        #region Category enum

        /// <summary>
        /// The category.
        /// </summary>
        public enum Category
        {
            /// <summary>
            /// The company.
            /// </summary>
            company,

            /// <summary>
            /// The executive.
            /// </summary>
            executive,

            /// <summary>
            /// The source.
            /// </summary>
            source,

            /// <summary>
            /// The companynews.
            /// </summary>
            companynews
        }

        #endregion

        #region Codes enum

        /// <summary>
        /// The codes.
        /// </summary>
        public enum Codes
        {
            /// <summary>
            /// The an.
            /// </summary>
            AN,

            /// <summary>
            /// The clm.
            /// </summary>
            CLM,

            /// <summary>
            /// The se.
            /// </summary>
            SE,

            /// <summary>
            /// The by.
            /// </summary>
            BY,

            /// <summary>
            /// The cr.
            /// </summary>
            CR,

            /// <summary>
            /// The wc.
            /// </summary>
            WC,

            /// <summary>
            /// The pd.
            /// </summary>
            PD,

            /// <summary>
            /// The et.
            /// </summary>
            ET,

            /// <summary>
            /// The sn.
            /// </summary>
            SN,

            /// <summary>
            /// The sc.
            /// </summary>
            SC,

            /// <summary>
            /// The ngc.
            /// </summary>
            NGC,

            /// <summary>
            /// The gc.
            /// </summary>
            GC,

            /// <summary>
            /// The ed.
            /// </summary>
            ED,

            /// <summary>
            /// The pg.
            /// </summary>
            PG,

            /// <summary>
            /// The vol.
            /// </summary>
            VOL,

            /// <summary>
            /// The la.
            /// </summary>
            LA,

            /// <summary>
            /// The cy.
            /// </summary>
            CY,

            /// <summary>
            /// The ct.
            /// </summary>
            CT,

            /// <summary>
            /// The rf.
            /// </summary>
            RF,

            /// <summary>
            /// The art.
            /// </summary>
            ART,

            /// <summary>
            /// The co.
            /// </summary>
            co,

            /// <summary>
            /// The pe.
            /// </summary>
            pe
        }

        #endregion

        #region ContentType enum

        /// <summary>
        /// The content type.
        /// </summary>
        public enum ContentType
        {
            /// <summary>
            /// The article.
            /// </summary>
            article,

            /// <summary>
            /// The picture.
            /// </summary>
            picture,

            /// <summary>
            /// The articlewithgraphics.
            /// </summary>
            articlewithgraphics,

            /// <summary>
            /// The webpage.
            /// </summary>
            webpage,

            /// <summary>
            /// The analyst.
            /// </summary>
            analyst
        }

        #endregion

        #region ElinkType enum

        /// <summary>
        /// The elink type.
        /// </summary>
        public enum ElinkType
        {
            /// <summary>
            /// The pro.
            /// </summary>
            pro,

            /// <summary>
            /// The webpage.
            /// </summary>
            webpage,

            /// <summary>
            /// The company.
            /// </summary>
            company,

            /// <summary>
            /// The doc.
            /// </summary>
            doc,

            /// <summary>
            /// The executive.
            /// </summary>
            executive
        }

        #endregion

        #endregion

        #region ..:: Private Variable(s) ::..
        private StringBuilder _htmlBuilder;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslateArticleModel"/> class.
        /// </summary>
        /// <remarks></remarks>
        public TranslateArticleModel()
        {
            Tokens = new TranslatorTokens();
        }

        #region ..:: Properties ::..

        private string _targetLanguage;

        /// <summary>
        /// Gets or sets Tokens.
        /// </summary>
        public TranslatorTokens Tokens { get; set; }

        /// <summary>
        /// Gets or sets PostProcessing.
        /// </summary>
        public PostProcessing PostProcessing { get; set; }

        /// <summary>
        /// Gets or sets SessionId.
        /// </summary>
        [ClientProperty("SessionId")]
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets PageCacheKey.
        /// </summary>
        [ClientProperty("PageCacheKey")]
        public string PageCacheKey { get; set; }

        /// <summary>
        /// Article object.
        /// </summary>
        public MessagesArticle ArticleObject { get; set; }


        /// <summary>
        /// Gets or sets the product prefix.
        /// </summary>
        /// <value>The product prefix.</value>
        /// <remarks></remarks>
        [ClientProperty("ProductPrefix")]
        public string ProductPrefix { get; set; }

        /// <summary>
        /// Gets or sets the access point.
        /// </summary>
        /// <value>The access point.</value>
        /// <remarks></remarks>
        [ClientProperty("AccessPointCode")]
        public string AccessPointCode { get; set; }

        /// <summary>
        /// Gets or sets the interface language.
        /// </summary>
        /// <value>The interface language.</value>
        /// <remarks></remarks>
        [ClientProperty("InterfaceLanguage")]
        public string InterfaceLanguage { get; set; }    

        /// <summary>
        /// TargetLangauage.
        /// </summary>
        [ClientProperty("TargetLanguage")]
        public string TargetLanguage
        {
            get { return _targetLanguage; }
            set
            {
                _targetLanguage = value;
                Tokens.SetLangCodeToken(_targetLanguage);
                Tokens.SetLangTextToken(_targetLanguage);
            }
        }

        /// <summary>
        /// SourceLangauage.
        /// </summary>
        [ClientProperty("SourceLanguage")]
        public string SourceLanguage { get; set; }

        /// <summary>
        /// The path to the web service
        /// </summary>
        /// <value>The service path.</value>
        [UrlProperty]
        [TypeConverter(typeof(WebServicePathConverter))]
        [ClientProperty("TranslationServiceUrl")]
        public string TranslationServiceUrl { get; set; }
 
        #endregion


        #region HelperMethods
        public string RenderArticleTitle()
        {
            _htmlBuilder = new StringBuilder();
            string href = null;
            string contentType = null;
            string headlineText = GetParagraphText(ArticleObject.headline);

            if (ArticleObject.contentParts != null)
            {
                contentType = ArticleObject.contentParts.contentType.ToLower();
                switch (contentType)
                {
                    case "webpage":
                        href = ArticleObject.contentParts.primaryReference;
                        break;
                    default:
                        //href = BaseHeadlines.GetAlternateMediaHref(ContentHeadline);
                        break;
                }
            }

            if (href == null)
                _htmlBuilder.AppendLine(string.Format(@"<span class='dj_translate_headline'>{0}</span>",headlineText));
            else if (contentType == "webpage")
                _htmlBuilder.AppendLine(
                    string.Format(
                        @"<span class='dj_translate_headline'><a href='{0}' onclick='NewWindow('{0}');return false;'>{1}</a></span>",
                        href,
                        headlineText));
            else
                _htmlBuilder.AppendLine(
                    string.Format(
                        @"<img src='../img/{0}.gif'/>&nbsp;<span class='dj_translate_headline'><a href='{1}' onclick='NewWindow('{1}');return false;'>{2}</a></span>",
                        (contentType == "analyst") ? "pdf" : contentType, href, headlineText));

            return _htmlBuilder.ToString();

        }

        public string RenderRemainingArticleTitle()
        {
            var l = new List<Paragraph>();
            if (ArticleObject.leadParagraph != null)
                l.AddRange(ArticleObject.leadParagraph);
            if (ArticleObject.tailParagraphs != null)
                l.AddRange(ArticleObject.tailParagraphs);
            var articleBuilder = new StringBuilder();
            var translationBatchSB = new StringBuilder();
            int numberOfParagraphsInBatch = 0;
            bool isFirstBatch = true;
            Regex regexObj = GetRegexObj();
            for (int i = 0; i < l.Count; i++)
            {
                translationBatchSB.AppendLine(RenderParagraph(l[i]));
                numberOfParagraphsInBatch++;
                int currentWordCount = regexObj.Matches(translationBatchSB.ToString()).Count;

                if (isFirstBatch)
                {
                    if (currentWordCount > 25 || i == l.Count - 1)
                    {
                        numberOfParagraphsInBatch = 0;
                        isFirstBatch = false;
                        articleBuilder.Append(string.Format("{0}</div>", translationBatchSB));
                        translationBatchSB = new StringBuilder();
                        continue;
                    }
                }
                else if (currentWordCount > 200 || numberOfParagraphsInBatch == 3 || i == l.Count - 1)
                {
                    numberOfParagraphsInBatch = 0;
                    articleBuilder.Append(string.Format("<div>{0}</div>", translationBatchSB));
                    translationBatchSB = new StringBuilder();
                }
            }
            return articleBuilder.ToString();
        }

        /// <summary>
        /// Gets the regex obj.
        /// </summary>
        /// <returns></returns>
        private Regex GetRegexObj()
        {
            switch (SourceLanguage)
            {
                case "ja":
                case "zhcn":
                case "zhtw":
                    return new Regex(@"[\S]");
                default:
                    return new Regex(@"[\S]+");
            }
        }

        /// <summary>
        /// Gets the paragraph text.
        /// </summary>
        /// <param name="paragraphs">The paragraphs.</param>
        /// <returns></returns>
        private static string GetParagraphText(IEnumerable<Paragraph> paragraphs)
        {
            var sb = new StringBuilder();
            foreach (Paragraph paragraph in paragraphs)
                sb.Append(GetObjectText(paragraph.Items));

            return sb.ToString();
        }

        /// <summary>
        /// Gets the object text.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        private static string GetObjectText(object[] items)
        {
            if (items == null) return string.Empty;
            string strTextToreturn = string.Empty;
            foreach (object item in items)
            {
                switch (item.GetType().ToString())
                {
                    case "HighlightedText":
                        var highlight = item as HighlightedText;
                        if (highlight != null && highlight.text != null && highlight.text.Value != null)
                            strTextToreturn = highlight.text.Value.Replace("<", "&lt;").Replace(">", "&gt;");
                        break;
                    case "ELink":
                        var elink = item as ELink;
                        if (elink != null && elink.text != null && elink.text.Value != null)
                            strTextToreturn = elink.text.Value;
                        break;
                    // added for inline tagging
                    case "EntityReference":
                        var entityReference = item as EntityReference;
                        if (entityReference != null)
                            strTextToreturn = GetEntityReferenceText(entityReference.Items);
                        break;

                    default:
                        var text = item as Text;
                        if (text != null && text.Value != null)
                            strTextToreturn = text.Value.Replace("<", "&lt;").Replace(">", "&gt;");
                        break;
                }
            }
            return strTextToreturn;
        }

        /// <summary>
        /// Renders the paragraph.
        /// </summary>
        /// <param name="paragraph">The paragraph.</param>
        /// <returns></returns>
        private string RenderParagraph(Paragraph paragraph)
        {
            _htmlBuilder = new StringBuilder();
            _htmlBuilder.AppendLine("<p>");
            if (ParagraphDisplay.Proportional == paragraph.display)
                _htmlBuilder.AppendLine(RenderItems(paragraph.Items));
            else
            {
                _htmlBuilder.AppendLine("<pre>");
                _htmlBuilder.AppendLine(RenderItems(paragraph.Items));
                _htmlBuilder.AppendLine("</pre>");
            }
            _htmlBuilder.AppendLine("</p>");
            return _htmlBuilder.ToString();
        }

        /// <summary>
        /// The render items.
        /// </summary>
        /// <param name="items">
        /// The items.
        /// </param>
        protected string RenderItems(object[] items)
        {
            var htmlBuilder = new StringBuilder();
            if (items == null)
            {
                return string.Empty;
            }

            foreach (object item in items)
            {
                if (item is HighlightedText)
                {
                    var highlight = (HighlightedText)item;
                    if (highlight.text != null && highlight.text.Value != null)
                    {
                        htmlBuilder.AppendLine(string.Format(@"<div class='dj_translate_highlight'>{0}</div>",
                                                              highlight.text.Value.Replace("<", "&lt;").Replace(">",
                                                                                                                "&gt;")));
                    }
                }
                else if (item is ELink)
                {
                    var entityLink = (ELink)item;
                    htmlBuilder.AppendLine(RenderELink(entityLink));
                }
                else if (item is EntityReference)
                {
                    var entityReference = (EntityReference)item;
                    string companyName = string.Empty;
                    var renderTextStringBuilder = new StringBuilder();
                    if (entityReference.Items != null &&
                        entityReference.Items.Length > 0)
                    {
                        foreach (string t in entityReference.Items)
                        {
                            companyName = companyName + t;
                            renderTextStringBuilder.Append(@"<span class='dj_translate_eref_link'>" +
                                                           t.Replace("<", "&lt;").Replace(">", "&gt;") + "</span>");
                        }
                        htmlBuilder.AppendLine(renderTextStringBuilder.ToString());
                    }
                }
                else
                {
                    var text = (Text)item;
                    if (text.Value != null)
                    {
                        htmlBuilder.AppendLine(text.Value.Replace("<", "&lt;").Replace(">", "&gt;"));
                    }
                }
            }
            return htmlBuilder.ToString();
        }

        /// <summary>
        /// The render e link.
        /// </summary>
        /// <param name="elink">The elink.</param>
        private string RenderELink(ELink elink)
        {
            _htmlBuilder = new StringBuilder();
            if (elink.type.Equals(ElinkType.webpage.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                switch (PostProcessing)
                {
                    case PostProcessing.Print:
                        _htmlBuilder.AppendLine(
                            string.Format(
                                @"<span class='dj_article_colorLinks'>{0} [{1}]</span>",
                                (elink.text != null) ? elink.text.Value : elink.reference,
                                elink.reference));
                        break;
                    case PostProcessing.Save:
                        _htmlBuilder.AppendLine((elink.text != null) ? elink.text.Value : elink.reference);
                        _htmlBuilder.AppendLine("[" + elink.reference + "]");
                        break;
                    default:
                        if (PostProcessing != PostProcessing.UnSpecified)
                        {
                            _htmlBuilder.AppendLine((elink.text != null) ? elink.text.Value : elink.reference);
                        }
                        else
                        {
                            _htmlBuilder.AppendLine(string.Format(
                                                        @"<span class='dj_article_elink'>{0}</span>",
                                                        elink.text != null ? elink.text.Value : elink.reference));
                        }

                        break;
                }
            }
            else if (!elink.type.Equals(ElinkType.pro.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                if (PostProcessing != PostProcessing.UnSpecified)
                {
                    _htmlBuilder.AppendLine(elink.text.Value);
                }
                else
                {
                    _htmlBuilder.AppendLine(string.Format("<span>{0}</span>", elink.text.Value));
                }
            }
            return _htmlBuilder.ToString();
        }

        /// <summary>
        /// Gets the entity reference text.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        private static string GetEntityReferenceText(object[] items)
        {
            if (items == null) return string.Empty;
            foreach (object item in items)
            {
                switch (item.GetType().ToString())
                {
                    case "HighlightedText":
                        Text erefHighlight = ((HighlightedText)item).text;
                        if (erefHighlight != null && erefHighlight.Value != null)
                        {
                            return erefHighlight.Value.Replace("<", "&lt;").Replace(">", "&gt;");
                        }
                        break;
                    default:
                        var erefText = (Text)item;
                        if (erefText.Value != null)
                        {
                            return erefText.Value.Replace("<", "&lt;").Replace(">", "&gt;");
                        }
                        break;
                }
            }
            return string.Empty;
        }
        #endregion
    }
}
