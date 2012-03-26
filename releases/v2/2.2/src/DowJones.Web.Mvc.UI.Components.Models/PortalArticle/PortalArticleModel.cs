using System;
using System.Collections.Generic;
using System.Text;
using DowJones.Infrastructure;
using Factiva.Gateway.Messages.Archive.V1_0;
using MessagesArticle = Factiva.Gateway.Messages.Archive.V1_0.Article;

namespace DowJones.Web.Mvc.UI.Components.PortalArticle
{
    public class PortalArticleModel : ViewComponentModel
    {
        private StringBuilder _htmlBuilder;

        #region enums
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

        /// <summary>
        /// Get or Set Article object
        /// </summary>
        public MessagesArticle ArticleObject { get; set; }

        /// <summary>
        /// Get or Set source language of the article
        /// </summary>
        public string SourceLanguage { get; set; }

        public PostProcessing PostProcessing { get; set; }

        /// <summary>
        /// Retrives Title from the article object
        /// </summary>
        /// <returns></returns>
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
                _htmlBuilder.AppendLine(string.Format(@"<span class='dj_translate_headline'>{0}</span>", headlineText));
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

        /// <summary>
        /// Creates the paragraphs of the article
        /// </summary>
        /// <returns></returns>
        public string RenderArticleParagraph()
        {
            var l = new List<Paragraph>();
            if (ArticleObject.leadParagraph != null)
                l.AddRange(ArticleObject.leadParagraph);
            if (ArticleObject.tailParagraphs != null)
                l.AddRange(ArticleObject.tailParagraphs);

            var articleBuilder = new StringBuilder();
            var translationBatchSB = new StringBuilder();
            
            for (int i = 0; i < l.Count; i++)
            {
                translationBatchSB.AppendLine(RenderParagraph(l[i]));
                
                articleBuilder.Append(string.Format("{0}", translationBatchSB));
                translationBatchSB = new StringBuilder();
            }
            return articleBuilder.ToString();
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
                        htmlBuilder.AppendLine(string.Format(@"<span class='dj_translate_highlight'>{0}</span>",
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
            //_htmlBuilder.AppendLine(string.Format("<span>{0}</span>", elink.text.Value));
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

    }
}
