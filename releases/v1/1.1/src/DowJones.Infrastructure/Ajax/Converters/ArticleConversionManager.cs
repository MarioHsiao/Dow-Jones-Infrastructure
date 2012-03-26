using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using DowJones.Utilities.Formatters.Globalization;
using Factiva.Gateway.Messages.Archive.V1_0;
using DowJones.Tools.Ajax.Article;
using DowJones.Utilities.Uri;
using System.Web.UI;
using DowJones.Infrastructure;
using DowJones.Utilities.DTO.Web.Request;
using Factiva.Gateway.Utils.V1_0;

namespace DowJones.Tools.Ajax.Converters
{
    public class ArticleConversionManager
    {

        protected static readonly ILog Log = LogManager.GetLogger(typeof(ArticleConversionManager));
        private readonly DateTimeFormatter _datetimeFormatter;
        internal const string LogoSite_Uri = "http://global.factiva.com/FactivaLogos/";

        private readonly string _binaryHandlerUrl;

        public ArticleConversionManager(String binaryHandlerUrl, DateTimeFormatter dateTimeFormatter)
        {
            _binaryHandlerUrl = binaryHandlerUrl;
            _datetimeFormatter = dateTimeFormatter;
        }

        public ArticleDataResult Process(GetArticleResponse response, ControlData controlData)
        {
            Factiva.Gateway.Messages.Archive.V1_0.Article article = null;
            if (response != null && response.articleResponseSet != null && response.articleResponseSet.count > 0)
                article = response.articleResponseSet.article[0];
            return Process(article, controlData);
        }

        public ArticleDataResult Process(Factiva.Gateway.Messages.Archive.V1_0.Article article, ControlData controlData)
        {
            ArticleDataResult result = new ArticleDataResult();

            if (article != null)
            {
                
                result.AccessionNumber = article.accessionNo;
                result.WordCount = article.wordCount;

                StringBuilder html = new StringBuilder();

                BuildHeader(html, article, controlData, result);
                if (article.leadParagraph != null)
                {
                    foreach (var item in article.leadParagraph)
                    {
                        ProcessParagrapgh(html, item, article, controlData);
                    }
                }
                if (article.tailParagraphs != null)
                {
                    foreach (var item in article.tailParagraphs)
                    {
                        ProcessParagrapgh(html, item, article, controlData);
                    }
                }
                if (article.corrections != null)
                {
                    foreach (var item in article.corrections)
                    {
                        ProcessParagrapgh(html, item, article, controlData);
                    }
                }

                result.BodyHtml = html.ToString();
            }
            return result;
        }

        private void ProcessParagrapgh(StringBuilder writer, Paragraph item, Factiva.Gateway.Messages.Archive.V1_0.Article article, ControlData controlData)
        {
            writer.AppendLine("<p class=\"dj_article_paragraph\">");
            RenderParagraph(item, writer, article, controlData);
            writer.AppendLine("</p>");
        }

        /// <summary>
        /// The render paragraph.
        /// </summary>
        /// <param name="paragraph">
        /// The paragraph.
        /// </param>
        public void RenderParagraph(Paragraph paragraph, StringBuilder writer, Factiva.Gateway.Messages.Archive.V1_0.Article article, ControlData controlData)
        {
            if (ParagraphDisplay.Proportional == paragraph.display)
            {
                RenderItems(paragraph.Items, writer, article, controlData);
            }
            else
            {
                writer.AppendLine("<pre class=\"dj_article_paragraph\">");
                RenderItems(paragraph.Items, writer, article, controlData);
                writer.AppendLine("</pre>");
            }
        }

        private void BuildHeader(StringBuilder html, Factiva.Gateway.Messages.Archive.V1_0.Article article, ControlData controlData, ArticleDataResult dataResult)
        {
            var logo = GetLogoImg(article);

            // START THE META DIV
            if (!string.IsNullOrEmpty(logo))
            {
                // TODO: Settings implementation
                var urlBuilder = new UrlBuilder(LogoSite_Uri + logo);
                dataResult.Logo = urlBuilder.ToString();
            }

            // css classes attached inside the method. 

            RenderLeadFields(html, article, controlData, dataResult);

        }

        /// <summary>
        /// The render lead fields.
        /// </summary>
        public void RenderLeadFields(StringBuilder html, Factiva.Gateway.Messages.Archive.V1_0.Article article, ControlData controlData, ArticleDataResult dataResult)
        {

            if (article.headline != null)
            {
                dataResult.Headline = GetParagraphText(article.headline);
            }
            else
            {
                dataResult.Headline = String.Empty;
            }

            dataResult.BaseLanguage = article.baseLanguage;
            dataResult.Volume = article.volume;
            dataResult.PublicationDate = article.publicationDate;
            dataResult.SourceName = article.sourceName;
            dataResult.SourceCode = article.sourceCode;
            dataResult.PublisherGroupName = article.publisherGroupName;
            dataResult.PublisherGroupCode = article.publisherGroupCode;
            dataResult.Pages = article.pages;
            dataResult.Edition = article.edition;


            if (article.columnName != null)
            {
                html.AppendLine("<div class=\"dj_article_clm\">");
                dataResult.ColumnName = RenderItems(article.columnName.Items, article, controlData);
                html.AppendLine("</div>");
            }

            if (article.sectionName != null)
            {
                dataResult.SectionName = RenderItems(article.sectionName.Items, article, controlData);
            }

            if (article.byline != null)
            {
                dataResult.ByLine = RenderItems(article.byline.Items, article, controlData);
            }

            if (article.credit != null)
            {
                dataResult.Credit = RenderItems(article.credit.Items, article, controlData);
            }

            dataResult.Copyright = RenderItems(article.copyright.Items, article, controlData);

        }

        /*
        /// <summary>
        /// The render article title.
        /// </summary>
        protected void RenderArticleTitle(StringBuilder writer, Factiva.Gateway.Messages.Archive.V1_0.Article article)
        {
            string href = null;
            string contentType = null;
            string headlineText = GetParagraphText(article.headline);

            if (article.contentParts != null)
            {
                contentType = article.contentParts.contentType.ToLower();

                if (contentType.Equals(ContentType.webpage.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    href = article.contentParts.primaryReference;
                }
            }

            if (href == null)
            {
                writer.AppendLine(headlineText);
            }
            else if (contentType.Equals(ContentType.webpage.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                writer.AppendLine(string.Format("<a href=\"{0}\" rel=\"{{fcode:'{2}'}}\">{1}</a>", href, headlineText, href));
            }
        }
        */
        /// <summary>
        /// Gets the logo img.
        /// </summary>
        /// <returns>A string representing the img url.</returns>
        protected string GetLogoImg(Factiva.Gateway.Messages.Archive.V1_0.Article article)
        {
            if (article.sourceLogo != null && article.sourceLogo.image != null)
            {
                return article.sourceLogo.image;
            }

            return !String.IsNullOrEmpty(article.sourceCode)
                       ? String.Format("{0}Logo.gif", article.sourceCode)
                       : null;
        }

        /// <summary>
        /// The render field.
        /// </summary>
        /// <param name="text">The text of the render field.</param>
        /// <param name="code">The code associated wtih the field.</param>
        /// <param name="tag">The html tag tag.</param>
        protected void RenderField(string text, string code, HtmlTextWriterTag tag, StringBuilder writer, Factiva.Gateway.Messages.Archive.V1_0.Article article, ControlData controlData)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            switch (tag)
            {
                case HtmlTextWriterTag.P:
                    writer.AppendLine(string.Format("<p class=\"dj_article_paragraph\">{0}</p>", text));
                    break;
                case HtmlTextWriterTag.Span:
                    writer.AppendLine(string.Format("<span> {0}</span>", text));
                    break;
                default:
                    if (code.Equals(Codes.SN.ToString(), StringComparison.InvariantCultureIgnoreCase) &&
                        Equals(PostProcessing.Print, StringComparison.InvariantCultureIgnoreCase))
                    {
                        writer.AppendLine(string.Format("<div class=\"dj_article_colorLinks\">{0}</div>", text));
                        break;
                    }

                    writer.AppendLine(string.Format("<div class=\"dj_article_{1}\">{0}</div>", text, code));
                    break;
            }
        }

        /// <summary>
        /// Renders the field.
        /// </summary>
        /// <param name="items">The field items.</param>
        /// <param name="code">The selected code.</param>
        /// <param name="tag">The html tag.</param>
        protected void RenderField(object[] items, string code, HtmlTextWriterTag tag, StringBuilder writer, Factiva.Gateway.Messages.Archive.V1_0.Article article, ControlData controlData)
        {
            if (items == null)
            {
                return;
            }

            switch (tag)
            {
                case HtmlTextWriterTag.P:
                    writer.AppendLine("<p class=\"dj_article_paragraph\">");
                    RenderItems(items, writer, article, controlData);
                    writer.AppendLine("</p>");
                    break;
                default:

                    // writer.WriteLine("<div class=\"dj_article_{2}\">");
                    RenderItems(items, writer, article, controlData);

                    // writer.WriteLine("</div>");
                    break;
            }
        }

        private string RenderItems(object[] items, Factiva.Gateway.Messages.Archive.V1_0.Article article, ControlData controlData)
        {
            string result = String.Empty;
            if (items != null)
            {
                foreach (object item in items)
                {
                    if (item is HighlightedText)
                    {
                        var highlight = (HighlightedText)item;
                        if (highlight.text != null && highlight.text.Value != null)
                        {
                            result += highlight.text.Value;
                        }
                    }
                    else if (item is ELink)
                    {
                        var entityLink = (ELink)item;
                        result += entityLink.text;
                    }
                    else if (item is EntityReference)
                    {
                        var entityReference = (EntityReference)item;
                        var companyName = string.Empty;
                        var renderTextStringBuilder = new StringBuilder();
                        if (entityReference.Items != null &&
                            entityReference.Items.Length > 0)
                        {
                            foreach (var t in entityReference.Items)
                            {
                                companyName = companyName + t;
                                result += t;
                            }
                        }
                    }
                    else
                    {
                        var text = (Text)item;
                        if (text.Value != null)
                        {
                            result += text.Value;
                        }
                    }
                    result += " ";
                }
            }
            return result.Trim();
        }
        /// <summary>
        /// The render items.
        /// </summary>
        /// <param name="items">
        /// The items.
        /// </param>
        protected void RenderItems(object[] items, StringBuilder writer, Factiva.Gateway.Messages.Archive.V1_0.Article article, ControlData controlData)
        {
            if (items == null)
            {
                return;
            }

            foreach (object item in items)
            {
                if (item is HighlightedText)
                {
                    var highlight = (HighlightedText)item;
                    if (highlight.text != null && highlight.text.Value != null)
                    {
                        writer.AppendLine(
                            string.Format("<div class=\"dj_article_highlight\">{0}</div>", highlight.text.Value.Replace("<", "&lt;").Replace(">", "&gt;")));
                    }
                }
                else if (item is ELink)
                {
                    var entityLink = (ELink)item;
                    RenderEntityLink(entityLink, writer, article, controlData);
                }
                else if (item is EntityReference)
                {
                    var entityReference = (EntityReference)item;
                    var companyName = string.Empty;
                    var renderTextStringBuilder = new StringBuilder();
                    if (entityReference.Items != null &&
                        entityReference.Items.Length > 0)
                    {
                        foreach (var t in entityReference.Items)
                        {
                            companyName = companyName + t;
                            renderTextStringBuilder.Append(
                                t.Replace("<", "&lt;").Replace(">", "&gt;"));
                        }
                    }

                    if (entityReference.category.Equals(Codes.co.ToString(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        var companyLink = BuildEntityLink(companyName, entityReference.code, Category.company.ToString());
                        writer.AppendLine(string.Format("<span >{0}</span>", companyLink));
                    }
                    else if (entityReference.category.Equals(Codes.pe.ToString(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        var companyLink = BuildEntityLink(companyName, entityReference.code, Category.executive.ToString());
                        writer.AppendLine(string.Format("<span >{0}</span>", companyLink));
                    }
                    else
                    {
                        writer.AppendLine(renderTextStringBuilder.ToString());
                    }
                }
                else
                {
                    var text = (Text)item;
                    if (text.Value != null)
                    {
                        writer.AppendLine(text.Value.Replace("<", "&lt;").Replace(">", "&gt;"));
                    }
                }
            }
        }

        /// <summary>
        /// The build entity link.
        /// </summary>
        /// <param name="name">The name of the entitly link.</param>
        /// <param name="code">The code of the entitly link.</param>
        /// <param name="category">The category.</param>
        /// <returns>A string representing build entity link.</returns>
        protected string BuildEntityLink(string name, string code, string category)
        {
            string entityLink = String.Format(
                "<a href=\"javascript:void(0)\" class=\"dj_article_entity\"  rel=\"{{fcode:'{0}',category:'{2}'}}\">{1}</a>",
                code ?? string.Empty,
                name ?? string.Empty,
                category);

            return entityLink;
        }

        /// <summary>
        /// The render e link.
        /// </summary>
        /// <param name="elink">The elink.</param>
        public void RenderEntityLink(ELink elink, StringBuilder writer, Factiva.Gateway.Messages.Archive.V1_0.Article article, ControlData controlData)
        {
            if (elink.type.Equals(ElinkType.pro.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                writer.AppendLine(
                    string.Format(
                        "<img src=\"{0}\"/>",
                        GetImageUrl(ImageType.Display, article.accessionNo, elink.parts, article, controlData)));
            }
            else if (elink.type.Equals(ElinkType.webpage.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                writer.AppendLine(string.Format(
                                            "<a class=\"dj_article_elink\" href=\"javascript:void(0)\" rel=\"{{href:'{0}'}}\">{1}</a>",
                                            elink.reference,
                                            elink.text != null ? elink.text.Value : elink.reference));
            }
            else if (elink.type.Equals(ElinkType.company.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {

                var companyNewsLink = BuildEntityLink(
                    elink.text.Value,
                    string.Empty,
                    Category.companynews.ToString());

                writer.AppendLine(string.Format("<span>{0}</span>", companyNewsLink));
            }
            else if (elink.type.Equals(ElinkType.executive.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                var companyNewsLink = BuildEntityLink(
                    elink.text.Value,
                    string.Empty,
                    Category.companynews.ToString());

                writer.AppendLine(string.Format("<span>{0}</span>", companyNewsLink));
            }
            else if (elink.type.Equals(ElinkType.doc.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                var elinkText = "Related Information";
                if (elink.text != null)
                {
                    elinkText = elink.text.Value;
                }
                else if (elink.reference != null)
                {
                    // RefAccessionNumbers.Add(elink.reference);
                    writer.AppendLine("##" + elink.reference + "##");
                }
            }
        }

        /// <summary>
        /// The get image url.
        /// </summary>
        /// <param name="imageType">The image type.</param>
        /// <param name="accessionNo">The accession no.</param>
        /// <param name="parts">The parts.</param>
        /// <returns>A string representing the image url.</returns>
        protected virtual string GetImageUrl(ImageType imageType, string accessionNo, Part[] parts, Factiva.Gateway.Messages.Archive.V1_0.Article article, ControlData controlData)
        {
            string reference = null;
            string mimeType = null;

            if (parts != null && parts.Length > 0)
            {
                foreach (Part part in parts.Where(part => part.type.ToLower() == imageType.ToString().ToLower()))
                {
                    // reference = part.reference.Replace("probj:archive", "probj_image_jpeg:archive");
                    reference = part.reference;
                    mimeType = part.mimeType;
                    break;
                }
            }

            if (!string.IsNullOrEmpty(_binaryHandlerUrl) && !string.IsNullOrEmpty(_binaryHandlerUrl.Trim()))
            {
                var ub = new UrlBuilder(_binaryHandlerUrl);
                ub.Append(UrlBuilder.GetParameterName(typeof(ArchiveFileRequestDTO), "AccessionNumber"), accessionNo);
                ub.Append(UrlBuilder.GetParameterName(typeof(ArchiveFileRequestDTO), "Reference"), reference);
                ub.Append(UrlBuilder.GetParameterName(typeof(ArchiveFileRequestDTO), "MimeType"), mimeType);
                ub.Append(UrlBuilder.GetParameterName(typeof(ArchiveFileRequestDTO), "ImageType"), (imageType == ImageType.Display) ? "dispix" : "tnail");

                if (!string.IsNullOrEmpty(controlData.AccessPointCode))
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "AccessPointCode"), controlData.AccessPointCode);
                if (!string.IsNullOrEmpty(article.baseLanguage))
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "InterfaceLanguage"), article.baseLanguage);
                if (!string.IsNullOrEmpty(controlData.ProductID))
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "ProductID"), controlData.ProductID);
                if (!string.IsNullOrEmpty(controlData.SessionID))
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "SessionID"), controlData.SessionID);
                else if (!string.IsNullOrEmpty(controlData.EncryptedLogin)) // assume this is a lightweight user
                {
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "EncryptedToken"), controlData.EncryptedLogin);
                }
                else
                {
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "UserID"), controlData.UserID);
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "Password"), controlData.UserPassword);
                }

                if (!string.IsNullOrEmpty(controlData.AccessPointCodeUsage))
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "AccessPointCodeUsage"), controlData.AccessPointCodeUsage);
                if (!string.IsNullOrEmpty(controlData.CacheKey))
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "CacheKey"), controlData.CacheKey);
                if (!string.IsNullOrEmpty(controlData.ClientCode))
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "ClientCodeType"), controlData.ClientCode);
                return ub.ToString();
            }

            return null;
        }

        /// <summary>
        /// The get paragraph text.
        /// </summary>
        /// <param name="paragraphs">The paragraphs.</param>
        /// <returns>A string of paragraph text.</returns>
        public string GetParagraphText(Paragraph[] paragraphs)
        {
            if (paragraphs == null)
            {
                return String.Empty;
            }

            var sb = new StringBuilder();
            foreach (Paragraph paragraph in paragraphs)
            {
                sb.Append(GetObjectText(paragraph.Items));
            }

            return sb.ToString();
        }

        /// <summary>
        /// The get object text.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns>
        /// The string of the text extracted from the items.
        /// </returns>
        public string GetObjectText(object[] items)
        {
            if (items == null)
            {
                return String.Empty;
            }

            var sb = new StringBuilder();
            foreach (object item in items)
            {
                if (item is HighlightedText)
                {
                    var highlight = (HighlightedText)item;
                    if (highlight.text != null && highlight.text.Value != null)
                    {
                        sb.Append(highlight.text.Value.Replace("<", "&lt;").Replace(">", "&gt;"));
                    }
                }
                else if (item is ELink)
                {
                    var elink = (ELink)item;
                    if (elink.text != null && elink.text.Value != null)
                    {
                        sb.Append(elink.text.Value);
                    }
                }
                else if (item is EntityReference)
                {
                    var entityReference = (EntityReference)item;

                    sb.Append(GetEntityReferenceText(entityReference.Items));
                }
                else
                {
                    var text = (Text)item;
                    if (text.Value != null)
                    {
                        sb.Append(text.Value.Replace("<", "&lt;").Replace(">", "&gt;"));
                    }
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// The get entity reference text.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns>
        /// The string representing the entity reference text.
        /// </returns>
        public static string GetEntityReferenceText(object[] items)
        {
            if (items == null)
            {
                return String.Empty;
            }

            var sb = new StringBuilder();
            foreach (var item in items)
            {
                if (item is HighlightedText)
                {
                    var erefHighlight = ((HighlightedText)item).text;
                    if (erefHighlight != null && erefHighlight.Value != null)
                    {
                        sb.Append(erefHighlight.Value.Replace("<", "&lt;").Replace(">", "&gt;"));
                    }
                }
                else if (item is Text)
                {
                    var erefText = (Text)item;
                    if (erefText.Value != null)
                    {
                        sb.Append(erefText.Value.Replace("<", "&lt;").Replace(">", "&gt;"));
                    }
                }
                else if (item is string)
                {
                    sb.Append(item.ToString().Replace("<", "&lt;").Replace(">", "&gt;"));
                }
            }

            return sb.ToString();
        }

    }
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

}
