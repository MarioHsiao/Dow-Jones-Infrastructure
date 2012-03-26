// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArticleControl.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   The article control.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using DowJones.Infrastructure;
using DowJones.Tools.Session;
using DowJones.Utilities.Attributes;
using DowJones.Utilities.DTO.Web.Request;
using DowJones.Utilities.Formatters.Globalization;
using DowJones.Utilities.Handlers.Syndication.Podcast.Core;
using DowJones.Utilities.Managers;
using DowJones.Utilities.Managers.Core;
using DowJones.Utilities.Managers.Search;
using DowJones.Utilities.Managers.Search.Requests;
using DowJones.Utilities.Managers.Search.Responses;
using DowJones.Utilities.Uri;
using DowJones.Web;
using DowJones.Web.Mvc.Extensions;
using DowJones.Web.Mvc.UI.Components.Article;
using DowJones.Web.Mvc.UI.Components.ArticleTranslator;
using DowJones.Web.Mvc.UI.Components.InlineMp3Player;
using DowJones.Web.Mvc.UI.Components.SocialButtons;
using Factiva.Gateway.Messages.Archive.V2_0;
using Factiva.Gateway.Messages.Search.V2_0;
using Factiva.Gateway.Utils.V1_0;
using FreeSearchRequest = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchRequest;
using FreeSearchResponse = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchResponse;

#region ..:: Embedded Resources ::..

[assembly: WebResource(ArticleControl.ScriptFile, KnownMimeTypes.JavaScript)]

#endregion


namespace DowJones.Web.Mvc.UI.Components.Article
{
    #region ..:: Auto-Registered Client Resources ::..

    [ScriptResource(null, ResourceName = ScriptFile, DeclaringType = typeof(ArticleControl))]

    #endregion

    public class ArticleControl : ViewComponentBase<ArticleModel>
    {

        #region ..:: Constants ::..

        internal const string BaseDirectory = "DowJones.Web.Mvc.UI.Components.Article";
        // The JavaScript file for this module
        internal const string ScriptFile = BaseDirectory + ".ArticleControl.js";

        /// <summary>
        /// The File Handler Url.
        /// </summary>
        internal const string FILE_HANDLER_URL = "~DowJones.Utilities.Handlers.ArticleControl.ContentHandler.axd";

        internal const string LogoSite_Uri = "http://global.factiva.com/FactivaLogos/";

        #endregion


        #region ..:: Private Variable(s) ::..

        /// <summary>
        /// The ref accession numbers.
        /// </summary>
        private readonly List<ElinkReferences> _refAccessionNumbers;

        /// <summary>
        /// The un processed ref accession numbers.
        /// </summary>
        private readonly List<ElinkReferences> _unProcessedRefAccessionNumbers;

        #endregion


        #region ..:: Constructor(s) ::..

        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleControl"/> class.
        /// </summary>
        /// <param name="controlData">The control data.</param>
        /// <param name="accessPointCode">The access point code.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        /// <param name="productPrefix">The product prefix.</param>
        //public ArticleControl(ControlData controlData, string accessPointCode, string interfaceLanguage, string productPrefix)
        public ArticleControl()
        {
            _unProcessedRefAccessionNumbers = new List<ElinkReferences>();
            _refAccessionNumbers = new List<ElinkReferences>();

            ReadSpeaker = new ReadSpeaker
            {
                ShowDownloadLink = false,
                Volume = 50,
                AutoStart = false
            };


            //Model.Children = new[] { new InlineMp3PlayerState() };

        }

        #endregion


        #region ..:: Properties ::..



        /// <summary>
        /// Gets or sets ReadSpeaker.
        /// </summary>
        public ReadSpeaker ReadSpeaker { get; set; }

        /// <summary>
        /// Gets or sets SocialButtonsButtons.
        /// </summary>
        public SocialButtonsModel SocialButtons { get; set; }

        /// <summary>
        /// Gets or sets the article translator control.
        /// </summary>
        /// <value>The article translator control.</value>
        public ArticleTranslatorControl ArticleTranslator { get; set; }

        /// <summary>
        /// Gets FileHandlerUrl.
        /// </summary>
        [UrlProperty]
        public string FileHandlerUrl
        {
            get { return FILE_HANDLER_URL; }
        }

        /// <summary>
        /// The client (jQuery) plugin name.
        /// </summary>
        /// <value>dj_ArticleControl</value>
        /// <remarks>
        /// A ClientPluginName name of "dj_ArticleControl"
        /// would render out an initialization script such as:
        /// $('#MyComponent').dj_ArticleControl({ ... });
        /// </remarks>
        public override string ClientPluginName
        {
            get { return "dj_ArticleControl"; }
        }


        #endregion


        #region ..:: Public Methods ::..


        /// <summary>
        /// The get paragraph text.
        /// </summary>
        /// <param name="paragraphs">The paragraphs.</param>
        /// <returns>A string of paragraph text.</returns>
        public static string GetParagraphText(Paragraph[] paragraphs)
        {
            if (paragraphs == null)
            {
                return null;
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
        public static string GetObjectText(object[] items)
        {
            if (items == null)
            {
                return null;
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
                return null;
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

        /// <summary>
        /// The render lead fields.
        /// </summary>
        public void RenderLeadFields(HtmlTextWriter writer)
        {
            string langToken = "langEn";
            DowJones.Utilities.Core.ContentLanguage contentLang = DowJones.Utilities.Core.ContentLanguage.en;
            if (Enum.TryParse<DowJones.Utilities.Core.ContentLanguage>(Model.ArticleObject.baseLanguage, out contentLang))
            {
                langToken = ((AssignedToken)Attribute.GetCustomAttribute(typeof(DowJones.Utilities.Core.ContentLanguage).GetField(contentLang.ToString()), typeof(AssignedToken))).Token;
            }

            if (Model.ArticleObject.columnName != null)
            {
                writer.WriteLine("<div class=\"dj_article_clm\">");
                RenderField(Model.ArticleObject.columnName.Items, Codes.CLM.ToString(), HtmlTextWriterTag.Div, writer);
                writer.WriteLine("</div>");
            }

            if (Model.ArticleObject.sectionName != null)
            {
                RenderField(Model.ArticleObject.sectionName.Items, Codes.SE.ToString(), HtmlTextWriterTag.Div, writer);
            }

            if (Model.ArticleObject.headline != null)
            {
                writer.WriteLine("<h2>");
                RenderArticleTitle(writer);
                writer.WriteLine("</h2>");
            }

            writer.WriteLine("<div class=\"article-details\">");

            if (Model.ArticleObject.byline != null)
            {
                writer.WriteLine("<div class=\"author\">");
                RenderField(Model.ArticleObject.byline.Items, Codes.BY.ToString(), HtmlTextWriterTag.Div, writer);
                writer.WriteLine("</div >");
            }

            if (Model.ArticleObject.credit != null)
            {
                writer.WriteLine("<div class=\"credit\">");
                RenderField(Model.ArticleObject.credit.Items, Codes.CR.ToString(), HtmlTextWriterTag.Div, writer);
                writer.WriteLine("</div >");
            }

            string wordCount = null;
            if (Model.ArticleObject.wordCount > 0)
            {
                wordCount = String.Format("{0} {1}", Model.ArticleObject.wordCount, Model.Tokens.Words);
            }

            writer.WriteLine("<div class=\"length\">");
            RenderField(wordCount, Codes.WC.ToString(), HtmlTextWriterTag.Div, writer);
            writer.WriteLine("</div >");

            var assembler = new DateTimeFormatter(Model.Preferences);
            DateTime tempararyPublicationDate = Model.ArticleObject.publicationDate;
            if (Model.ArticleObject.publicationTimeSpecified)
                tempararyPublicationDate = DateTimeFormatter.Merge(Model.ArticleObject.publicationDate, Model.ArticleObject.publicationTime);

            string publicationDate = assembler.FormatDate(tempararyPublicationDate);
            writer.WriteLine("<div class=\"publicationdate\">");

            RenderField(publicationDate, Codes.PD.ToString(), HtmlTextWriterTag.Span, writer);

            if (Model.ArticleObject.publicationTimeSpecified)
            {
                string publicationTime = assembler.FormatTime(tempararyPublicationDate);
                RenderField(publicationTime, Codes.ET.ToString(), HtmlTextWriterTag.Span, writer);
            }

            writer.WriteLine("</div >");

            if (Model.ShowSourceLinks && Model.PostProcessing == PostProcessing.UnSpecified)
            {
                writer.WriteLine("<div class=\"source\">");
                if (Model.ArticleObject.sourceCode != null)
                {
                    writer.WriteLine(
                        string.Format(
                            "<a href=\"javascript:void(0)\" class=\"dj_article_source\" rel=\"{{fcode:'{0}',category:'{2}'}}\">{1}</a>",
                            Model.ArticleObject.sourceCode,
                            Model.ArticleObject.sourceName,
                            Category.source));
                }
                else if (Model.ArticleObject.sourceName != null)
                {
                    writer.WriteLine(Model.ArticleObject.sourceName);
                }

                writer.WriteLine("</div>");
            }
            else
            {
                RenderField(Model.ArticleObject.sourceName, Codes.SN.ToString(), HtmlTextWriterTag.Div, writer);
            }

            writer.WriteLine("<div class=\"sourcecode\">");
            RenderField(Model.ArticleObject.sourceCode, Codes.SC.ToString(), HtmlTextWriterTag.Div, writer);
            writer.WriteLine("</div>");
            writer.WriteLine("<div class=\"pagename\">");

            RenderField(Model.ArticleObject.publisherGroupName, Codes.NGC.ToString(), HtmlTextWriterTag.Div, writer);
            writer.WriteLine("</div>");
            writer.WriteLine("<div class=\"pagegroupcode\">");
            RenderField(Model.ArticleObject.publisherGroupCode, Codes.GC.ToString(), HtmlTextWriterTag.Div, writer);
            writer.WriteLine("</div>");
            writer.WriteLine("<div class=\"edition\">");
            RenderField(Model.ArticleObject.edition, Codes.ED.ToString(), HtmlTextWriterTag.Div, writer);
            writer.WriteLine("</div>");
            if (Model.ArticleObject.pages != null)
            {
                RenderField(String.Join(",", Model.ArticleObject.pages), Codes.PG.ToString(), HtmlTextWriterTag.Div, writer);
            }

            writer.WriteLine("<div class=\"volume\">");
            RenderField(Model.ArticleObject.volume, Codes.VOL.ToString(), HtmlTextWriterTag.Div, writer);
            writer.WriteLine("</div>");
            writer.WriteLine("<div class=\"language\">");
            RenderField(ResourceTextManager.Instance.GetString(langToken), Codes.LA.ToString(), HtmlTextWriterTag.Div, writer);
            writer.WriteLine("</div>");
            
            writer.WriteLine("<div class=\"copyrights\">");
            RenderField(Model.ArticleObject.copyright.Items, Codes.CY.ToString(), HtmlTextWriterTag.Div, writer);
            writer.WriteLine("</div>");

            writer.WriteLine("</div >");//article-details
        }

        /// <summary>
        /// The render e link.
        /// </summary>
        /// <param name="elink">The elink.</param>
        public void RenderEntityLink(ELink elink, HtmlTextWriter writer)
        {
            if (elink.type.Equals(ElinkType.pro.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                if (Model.PostProcessing != PostProcessing.Save)
                {
                    writer.WriteLine(
                        string.Format(
                            "<img src=\"{0}\"/>",
                            GetImageUrl(ImageType.Display, Model.ArticleObject.accessionNo, elink.parts)));
                }
            }
            else if (elink.type.Equals(ElinkType.webpage.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                switch (Model.PostProcessing)
                {
                    case PostProcessing.Print:
                        writer.WriteLine(
                            string.Format(
                                "<span class=\"dj_article_colorLinks\">{0} [{1}]</span>",
                                (elink.text != null) ? elink.text.Value : elink.reference,
                                elink.reference));
                        break;
                    case PostProcessing.Save:
                        writer.WriteLine((elink.text != null) ? elink.text.Value : elink.reference);
                        writer.WriteLine("[" + elink.reference + "]");
                        break;
                    default:
                        if (Model.PostProcessing != PostProcessing.UnSpecified || !Model.EnableELinks)
                        {
                            writer.WriteLine((elink.text != null) ? elink.text.Value : elink.reference);
                        }
                        else
                        {
                            writer.WriteLine(string.Format(
                                                        "<a class=\"dj_article_elink\" href=\"javascript:void(0)\" rel=\"{{href:'{0}'}}\">{1}</a>",
                                                        elink.reference,
                                                        elink.text != null ? elink.text.Value : elink.reference));
                        }

                        break;
                }
            }
            else if (elink.type.Equals(ElinkType.company.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                if (Model.PostProcessing != PostProcessing.UnSpecified || !Model.EnableELinks)
                {
                    writer.WriteLine(elink.text.Value);
                }
                else
                {
                    var companyNewsLink = BuildEntityLink(
                        elink.text.Value,
                        string.Empty,
                        Category.companynews.ToString());

                    writer.WriteLine(string.Format("<span>{0}</span>", companyNewsLink));
                }
            }
            else if (elink.type.Equals(ElinkType.executive.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                if (Model.PostProcessing != PostProcessing.UnSpecified || !Model.EnableELinks)
                {
                    writer.WriteLine(elink.text.Value);
                }
                else
                {
                    var companyNewsLink = BuildEntityLink(
                        elink.text.Value,
                        string.Empty,
                        Category.companynews.ToString());

                    writer.WriteLine(string.Format("<span>{0}</span>", companyNewsLink));
                }
            }
            else if (elink.type.Equals(ElinkType.doc.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                var elinkText = "Related Information";
                var elinkReference = new ElinkReferences();
                if (elink.reference != null)
                {
                    if (elink.text != null)
                    {
                        elinkText = elink.text.Value;
                    }

                    writer.WriteLine("##" + elink.reference + "##");

                    elinkReference.Reference = elink.reference;
                    elinkReference.Text = elinkText;
                    _refAccessionNumbers.Add(elinkReference);
                }
                else
                {
                    if (elink.text != null)
                    {
                        writer.WriteLine(elink.text.Value);
                    }
                    else if (elink.reference != null)
                    {
                        // RefAccessionNumbers.Add(elink.reference);
                        writer.WriteLine("##" + elink.reference + "##");
                    }
                }
            }
        }

        /// <summary>
        /// The render paragraphs.
        /// </summary>
        /// <param name="paragraphs">
        /// The paragraphs.
        /// </param>
        public void RenderParagraphs(Paragraph[] paragraphs, HtmlTextWriter writer)
        {
            if (paragraphs == null)
            {
                return;
            }

            foreach (Paragraph paragraph in paragraphs)
            {
                writer.WriteLine("<p class=\"dj_article_paragraph\">");
                RenderParagraph(paragraph, writer);
                writer.WriteLine("</p>");
            }
        }

        /// <summary>
        /// The render paragraph.
        /// </summary>
        /// <param name="paragraph">
        /// The paragraph.
        /// </param>
        public void RenderParagraph(Paragraph paragraph, HtmlTextWriter writer)
        {
            if (ParagraphDisplay.Proportional == paragraph.display)
            {
                RenderItems(paragraph.Items, writer);
            }
            else
            {
                writer.WriteLine("<pre class=\"dj_article_paragraph\">");
                RenderItems(paragraph.Items, writer);
                writer.WriteLine("</pre>");
            }
        }

        #endregion


        #region ..:: Protected Methods ::..

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
        /// The render image.
        /// </summary>
        protected void RenderImage(HtmlTextWriter writer)
        {
            if ((Model.ImageType == ImageType.Display) || (Model.ImageType == ImageType.UnSpecified))
            {
                writer.WriteLine(
                    string.Format(
                        "<p class=\"dj_article_paragraph\"><img src=\"{0}\"/></p>",
                        GetImageUrl(ImageType.Display, Model.ArticleObject.accessionNo, Model.ArticleObject.contentParts.parts)));
            }
        }

        /// <summary>
        /// Gets the logo img.
        /// </summary>
        /// <returns>A string representing the img url.</returns>
        protected string GetLogoImg()
        {
            if (Model.ArticleObject.sourceLogo != null && Model.ArticleObject.sourceLogo.image != null)
            {
                return Model.ArticleObject.sourceLogo.image;
            }

            return !String.IsNullOrEmpty(Model.ArticleObject.sourceCode)
                       ? String.Format("{0}Logo.gif", Model.ArticleObject.sourceCode)
                       : null;
        }

        /// <summary>
        /// The get image url.
        /// </summary>
        /// <param name="imageType">The image type.</param>
        /// <param name="accessionNo">The accession no.</param>
        /// <param name="parts">The parts.</param>
        /// <returns>A stromg representing the image url.</returns>
        protected virtual string GetImageUrl(ImageType imageType, string accessionNo, Part[] parts)
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

            if (!string.IsNullOrEmpty(FileHandlerUrl) && !string.IsNullOrEmpty(FileHandlerUrl.Trim()))
            {
                var ub = new UrlBuilder(FileHandlerUrl);
                ub.Append(UrlBuilder.GetParameterName(typeof(ArchiveFileRequestDTO), "AccessionNumber"), accessionNo);
                ub.Append(UrlBuilder.GetParameterName(typeof(ArchiveFileRequestDTO), "Reference"), reference);
                ub.Append(UrlBuilder.GetParameterName(typeof(ArchiveFileRequestDTO), "MimeType"), mimeType);
                ub.Append(UrlBuilder.GetParameterName(typeof(ArchiveFileRequestDTO), "ImageType"), (imageType == ImageType.Display) ? "dispix" : "tnail");

                if (!string.IsNullOrEmpty(Model.ControlData.AccessPointCode))
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "AccessPointCode"), Model.ControlData.AccessPointCode);
                if (!string.IsNullOrEmpty(Model.InterfaceLanguage))
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "InterfaceLanguage"), Model.InterfaceLanguage);
                if (!string.IsNullOrEmpty(Model.ControlData.ProductID))
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "ProductID"), Model.ControlData.ProductID);
                if (!string.IsNullOrEmpty(Model.ControlData.SessionID))
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "SessionID"), Model.ControlData.SessionID);
                else if (!string.IsNullOrEmpty(Model.ControlData.EncryptedLogin)) // assume this is a lightweight user
                {
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "EncryptedToken"), Model.ControlData.EncryptedLogin);
                }
                else
                {
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "UserID"), Model.ControlData.UserID);
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "Password"), Model.ControlData.UserPassword);
                }

                if (!string.IsNullOrEmpty(Model.ControlData.AccessPointCodeUsage))
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "AccessPointCodeUsage"), Model.ControlData.AccessPointCodeUsage);
                if (!string.IsNullOrEmpty(Model.ControlData.CacheKey))
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "CacheKey"), Model.ControlData.CacheKey);
                if (!string.IsNullOrEmpty(Model.ControlData.ClientCode))
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "ClientCodeType"), Model.ControlData.ClientCode);
                return ub.ToString();
            }

            return null;
        }

        /// <summary>
        /// The render items.
        /// </summary>
        /// <param name="items">
        /// The items.
        /// </param>
        protected void RenderItems(object[] items, HtmlTextWriter writer)
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
                        writer.WriteLine(
                            string.Format("<span class=\"dj_article_highlight\">{0}</span>", highlight.text.Value.Replace("<", "&lt;").Replace(">", "&gt;")));
                    }
                }
                else if (item is ELink)
                {
                    var entityLink = (ELink)item;
                    RenderEntityLink(entityLink, writer);
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

                    if (Model.ShowCompanyEntityReference || Model.ShowExecutiveEntityReference)
                    {
                        if (entityReference.category.Equals(Codes.co.ToString(), StringComparison.InvariantCultureIgnoreCase))
                        {
                            if ((Model.PostProcessing == PostProcessing.UnSpecified) && Model.ShowCompanyEntityReference)
                            {
                                var companyLink = BuildEntityLink(companyName, entityReference.code, Category.company.ToString());
                                writer.WriteLine(string.Format("<span >{0}</span>", companyLink));
                            }
                            else
                            {
                                writer.WriteLine(string.Format("<span>{0}</span>", renderTextStringBuilder));
                            }
                        }
                        else if (entityReference.category.Equals(Codes.pe.ToString(), StringComparison.InvariantCultureIgnoreCase))
                        {
                            if ((Model.PostProcessing == PostProcessing.UnSpecified) && Model.ShowExecutiveEntityReference)
                            {
                                var companyLink = BuildEntityLink(companyName, entityReference.code, Category.executive.ToString());
                                writer.WriteLine(string.Format("<span >{0}</span>", companyLink));
                            }
                            else
                            {
                                writer.WriteLine(string.Format("<span>{0}</span>", renderTextStringBuilder));
                            }
                        }
                        else
                        {
                            writer.WriteLine(renderTextStringBuilder.ToString());
                        }
                    }
                    else
                    {
                        writer.WriteLine(renderTextStringBuilder.ToString());
                    }
                }
                else
                {
                    var text = (Text)item;
                    if (text.Value != null)
                    {
                        string[] splittedParts;
                        if (Model.ShowHighlighting && text.Value.Contains(Model.CanonicalSearchString))
                        {
                            var separator = new string[1];
                            separator[0] = Model.CanonicalSearchString;

                            splittedParts = text.Value.Split(separator, StringSplitOptions.None);
                            if (splittedParts.Length > 0)
                            {
                                for (int i = 0; i <= splittedParts.Length - 2; i++)
                                {
                                    writer.WriteLine(splittedParts[i]);

                                    writer.WriteLine(
                                        string.Format("<span class=\"dj_article_canonicalstring\">{0}</span>", Model.CanonicalSearchString));
                                }

                                writer.WriteLine(splittedParts[splittedParts.Length - 1]);
                            }
                        }
                        else
                        {
                            writer.WriteLine(text.Value.Replace("<", "&lt;").Replace(">", "&gt;"));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Renders the field.
        /// </summary>
        /// <param name="items">The field items.</param>
        /// <param name="code">The selected code.</param>
        /// <param name="tag">The html tag.</param>
        protected void RenderField(object[] items, string code, HtmlTextWriterTag tag, HtmlTextWriter writer)
        {
            if (items == null)
            {
                return;
            }

            switch (tag)
            {
                case HtmlTextWriterTag.P:
                    writer.WriteLine("<p class=\"dj_article_paragraph\">");
                    RenderItems(items, writer);
                    writer.WriteLine("</p>");
                    break;
                default:

                    // writer.WriteLine("<div class=\"dj_article_{2}\">");
                    RenderItems(items, writer);

                    // writer.WriteLine("</div>");
                    break;
            }
        }

        /// <summary>
        /// The render field.
        /// </summary>
        /// <param name="text">The text of the render field.</param>
        /// <param name="code">The code associated wtih the field.</param>
        /// <param name="tag">The html tag tag.</param>
        protected void RenderField(string text, string code, HtmlTextWriterTag tag, HtmlTextWriter writer)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            switch (tag)
            {
                case HtmlTextWriterTag.P:
                    writer.WriteLine(string.Format("<p class=\"dj_article_paragraph\">{0}</p>", text));
                    break;
                case HtmlTextWriterTag.Span:
                    writer.WriteLine(string.Format("<span> {0}</span>", text));
                    break;
                default:
                    if (code.Equals(Codes.SN.ToString(), StringComparison.InvariantCultureIgnoreCase) &&
                        Equals(PostProcessing.Print, StringComparison.InvariantCultureIgnoreCase))
                    {
                        writer.WriteLine(string.Format("<div class=\"dj_article_colorLinks\">{0}</div>", text));
                        break;
                    }

                    writer.WriteLine(string.Format("<div class=\"dj_article_{1}\">{0}</div>", text, code));
                    break;
            }
        }

        /// <summary>
        /// The render article title.
        /// </summary>
        protected void RenderArticleTitle(HtmlTextWriter writer)
        {
            string href = null;
            string contentType = null;
            string headlineText = GetParagraphText(Model.ArticleObject.headline);

            if (Model.ArticleObject.contentParts != null)
            {
                contentType = Model.ArticleObject.contentParts.contentType.ToLower();

                if (contentType.Equals(ContentType.webpage.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    href = Model.ArticleObject.contentParts.primaryReference;
                }
            }

            if (href == null)
            {
                writer.WriteLine(headlineText);
            }
            else if (contentType.Equals(ContentType.webpage.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                writer.WriteLine(string.Format("<a href=\"{0}\" rel=\"{{fcode:'{2}'}}\">{1}</a>", href, headlineText, href));
            }
        }

        /// <summary>
        /// The render tail fields.
        /// </summary>
        protected void RenderTailFields(HtmlTextWriter writer)
        {
            if (Model.ArticleObject.contact != null)
            {
                RenderField(Model.ArticleObject.contact.Items, Codes.CT.ToString(), HtmlTextWriterTag.P, writer);
            }

            if (Model.ArticleObject.notes != null)
            {
                RenderField(Model.ArticleObject.notes.Items, Codes.RF.ToString(), HtmlTextWriterTag.P, writer);
            }

            if (Model.ArticleObject.artWork != null)
            {
                RenderField(Model.ArticleObject.artWork.Items, Codes.ART.ToString(), HtmlTextWriterTag.P, writer);
            }
        }

        /// <summary>
        /// The get handler url.
        /// </summary>
        /// <param name="imageType">The image type.</param>
        /// <param name="accessionNo">The accession no.</param>
        /// <param name="contentItem">The content item.</param>
        /// <returns>A string representing the Handerl Url</returns>
        protected virtual string GetHandlerUrl(ImageType imageType, string accessionNo, ContentItem contentItem)
        {
            string reference = contentItem.Ref;
            string mimeType = contentItem.Mimetype;
            //// var cData = ControlDataManager.Clone(SessionData.Instance().SessionBasedControlData);

            if (!string.IsNullOrEmpty(FileHandlerUrl) && !string.IsNullOrEmpty(FileHandlerUrl.Trim()))
            {
                var ub = new UrlBuilder(FileHandlerUrl);
                ub.Append(UrlBuilder.GetParameterName(typeof(ArchiveFileRequestDTO), "AccessionNumber"), accessionNo);
                ub.Append(UrlBuilder.GetParameterName(typeof(ArchiveFileRequestDTO), "Reference"), reference);
                ub.Append(UrlBuilder.GetParameterName(typeof(ArchiveFileRequestDTO), "MimeType"), mimeType);
                ub.Append(UrlBuilder.GetParameterName(typeof(ArchiveFileRequestDTO), "ImageType"), (imageType == ImageType.Display) ? "dispix" : "tnail");
                if (!string.IsNullOrEmpty(Model.ControlData.AccessPointCode))
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "AccessPointCode"), Model.ControlData.AccessPointCode);
                if (!string.IsNullOrEmpty(Model.InterfaceLanguage))
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "InterfaceLanguage"), Model.InterfaceLanguage);
                if (!string.IsNullOrEmpty(Model.ControlData.ProductID))
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "ProductID"), Model.ControlData.ProductID);
                if (!string.IsNullOrEmpty(Model.ControlData.SessionID))
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "SessionID"), Model.ControlData.SessionID);
                else if (!string.IsNullOrEmpty(Model.ControlData.EncryptedLogin)) // assume this is a lightweight user
                {
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "EncryptedToken"), Model.ControlData.EncryptedLogin);
                }
                else
                {
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "UserID"), Model.ControlData.UserID);
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "Password"), Model.ControlData.UserPassword);
                }

                if (!string.IsNullOrEmpty(Model.ControlData.AccessPointCodeUsage))
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "AccessPointCodeUsage"), Model.ControlData.AccessPointCodeUsage);
                if (!string.IsNullOrEmpty(Model.ControlData.CacheKey))
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "CacheKey"), Model.ControlData.CacheKey);
                if (!string.IsNullOrEmpty(Model.ControlData.ClientCode))
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "ClientCodeType"), Model.ControlData.ClientCode);

                return ub.ToString();
            }

            return null;
        }

        #endregion


        #region ..:: Private Methods ::..

        /// <summary>
        /// The build translator control section.This is a plugin for the ArticleTranslation control.
        /// </summary>
        private void BuildTranslatorControlSection(HtmlTextWriter writer)
        {
            var translatorMarkup = Html.DJ().RenderComponent<ArticleTranslatorControl>(Model.TranslatorModel);
            writer.WriteLine(@"<div id=""articleTranslateControlsContainer"" style=""float:left;display:inline;padding-bottom:10px"">");
            writer.WriteLine(translatorMarkup.ToHtmlString());
            writer.WriteLine("</div>");
        }

        /// <summary>
        /// The return an array.
        /// </summary>
        /// <param name="notProcessedRefAccessionNumbers">The un processed ref accession numbers.</param>
        /// <returns>An array of strings</returns>
        private static string[] ReturnANArray(ICollection<ElinkReferences> notProcessedRefAccessionNumbers)
        {
            var arrAccessionNumbers = new string[notProcessedRefAccessionNumbers.Count];
            var index = -1;
            foreach (var elink in notProcessedRefAccessionNumbers)
            {
                index = index + 1;
                arrAccessionNumbers[index] = elink.Reference;
            }

            return arrAccessionNumbers;
        }

        /// <summary>
        /// The replace elink placeholders.
        /// </summary>
        private void ReplaceElinkPlaceholders(HtmlTextWriter writer)
        {
            // Pranab: made 1 single transaction call.
            //GetContent();

            // hrusi: removed GetContent method and brought the method body here
            var arrAccessionNumbers = new string[_refAccessionNumbers.Count];
            int index = -1;
            foreach (ElinkReferences elink in _refAccessionNumbers)
            {
                bool processed = false;
                foreach (ContentItem item in Model.ContentItems.ItemCollection)
                {
                    if (!item.Ref.Equals(elink.Reference, StringComparison.InvariantCultureIgnoreCase) &&
                        !item.Ref.EndsWith(elink.Reference, StringComparison.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }

                    index = index + 1;
                    arrAccessionNumbers[index] = elink.Reference;
                    processed = true;
                    break;
                }

                if (!processed)
                {
                    // Collect unprocessed ANs
                    _unProcessedRefAccessionNumbers.Add(elink);
                }
            }

            if (arrAccessionNumbers.Length > 0)
            {
                ProcessContent(arrAccessionNumbers, writer);
            }

            if (_unProcessedRefAccessionNumbers.Count <= 0)
            {
                return;
            }

            // Process unprocessed ANs
            string[] accessNos = ReturnANArray(_unProcessedRefAccessionNumbers);
            ProcessContent(accessNos, writer);
        }

        /// <summary>
        /// The build header.
        /// </summary>
        /// <returns>A string representing the header.</returns>
        private void BuildHeader(HtmlTextWriter writer)
        {
            if (Model.ArticleObject == null)
            {
                return;
            }

            var logo = GetLogoImg();

            //START THE META DIV
            writer.WriteLine("<div class=\"dj_article_meta\" id=\"articleHead\">");
            if (!string.IsNullOrEmpty(logo))
            {
                // TODO: Settings implementation
                var urlBuilder = new UrlBuilder(LogoSite_Uri + logo);

                if ((Model.PostProcessing == PostProcessing.UnSpecified) ||
                    (Model.PostProcessing != PostProcessing.RTF && Model.PostProcessing != PostProcessing.Save))
                {
                    var altText = Model.ArticleObject.sourceCode + " Logo";
                    writer.WriteLine(
                        String.Format("<div class=\"dj_article_image\" ><img onerror=\"this.style.visibility='hidden'; this.style.display='none'; this.style.height=0;\" onabort=\"this.style.visibility='hidden'; this.style.display='none'; this.style.height=0;\" src=\"{0}\" alt=\"{1}\"/></a></div>", urlBuilder.ToString(), altText));

                }
            }

            if (Model.ArticleObject.contentParts!=null && !string.IsNullOrEmpty(Model.ArticleObject.contentParts.contentType) && Model.ArticleObject.contentParts.contentType.ToLower() == "picture")
            {
                RenderImage(writer);
            }

            // css classes attached inside the method. 
            RenderLeadFields(writer);

            // End the header meta section
            writer.WriteLine("</div>");

        }

        /// <summary>
        /// The build social control section.
        /// </summary>
        private void BuildSocialControlSection(HtmlTextWriter writer)
        {

            var sbtn = Html.DJ().RenderComponent<DowJones.Web.Mvc.UI.Components.SocialButtons.SocialButtonsControl>(Model.SocialButtonModel);
            writer.WriteLine(sbtn.ToHtmlString());
            //// Social networks collection
            ////List<SocialNetworks> socNetworks = SocialButtons.SocialNetworks;
            //List<SocialNetworks> socNetworks = new List<SocialNetworks>
            //                                        {
            //                                            SocialNetworks.Delicious,
            //                                            SocialNetworks.Digg,
            //                                            SocialNetworks.Facebook,
            //                                            SocialNetworks.Furl,
            //                                            SocialNetworks.Google,
            //                                            SocialNetworks.LinkedIn,
            //                                            SocialNetworks.Newsvine,
            //                                            SocialNetworks.Reddit,
            //                                            SocialNetworks.StumbleUpon,
            //                                            SocialNetworks.Technorati,
            //                                            SocialNetworks.Twitter,
            //                                            SocialNetworks.Yahoo
            //                                        };

            //// creating div tag for small socials
            //var smallSocialContainer = new HtmlGenericControl("div")
            //{
            //    ID = "SocialDivContainer"
            //};

            //var socialbuttons = new SocialButtons
            //{
            //    ClientID = "socialButtons1",
            //    Model = new SocialButtonsModel
            //    {
            //        //TargetControlID = "SocialDivContainer",
            //        SocialNetworks = socNetworks,
            //        ImageSize = ImageSize.Small,
            //        Url = "http://dowjones.com",
            //        Title = "Title",
            //        Keywords = "factiva, socials, bank, financials",
            //        Description = "description"
            //        ,ID = "socialButtons"
            //    }
            //};

            //// adding controls to the panel
            //this.AddChild(socialbuttons);
            //socialbuttons.Render(writer);
        }

        /// <summary>
        /// The build speaker control section.
        /// </summary>
        /// <returns>The html strin gof the Readspeaker controls.</returns>
        private void BuildSpeakerControlSection(HtmlTextWriter writer)
        {

            if (Model.ShowReadSpeaker)
            {

                var readSpeakerState = new InlineMp3PlayerModel
                {
                    AttributionToken = Model.Tokens.ReadSpeakerAttribution,
                    ShowDownloadLink = ReadSpeaker.ShowDownloadLink,
                    DownloadToken = Model.Tokens.ReadSpeakerDownload,
                    Mp3PlayerType = MP3PlayerType.DowJones,
                    AutoStart = ReadSpeaker.AutoStart,
                    Volume = ReadSpeaker.Volume,
                    EnableTransparency = true,
                    Mp3FilesUrls = AudioPlayerUrlBuilder(),
                    DownloadUrl = AudioPlayerDownloadUrlBuilder(),
                    AutoReplay = false,
                    ListenToArticleToken = Model.Tokens.ReadSpeakerListenToArticle
                };

                var readSpeakerControl = new InlineMp3PlayerControl
                {
                    ClientID = "spkr",
                    Model = readSpeakerState
                };

                writer.WriteLine(@"<span id=""Mp3PlayerControlsContainer"" style=""display:inline;float:left"">");
                // write out the comment
                writer.WriteLine("<!--{0}-->".FormatWith("© 2006-2008 • www.alsacreations.fr . Use of Dewplayer is subject to a Creative Commons Licence (embedded link to http://creativecommons.org/licenses/by-nd/2.0/fr/deed.en_US)"));
                writer.WriteLine(@"</span>");

                AddChild(readSpeakerControl);
                //RegisterChildControl(readSpeakerControl);
                readSpeakerControl.Render(writer);


            }

        }

        //private void RegisterChildControl(IViewComponent childComponent)
        //{
        //    ComponentRegistry.Register(childComponent);
        //}

        /// <summary>
        /// The build body section.
        /// </summary>
        /// <returns>A string representing the Body section</returns>
        private void BuildBodySection(HtmlTextWriter writer)
        {

            // sTART THE BODY div
            writer.WriteLine("<div class=\"dj_article_articlebody\" id=\"articleBody\">");

            // need to check what is corrects
            RenderParagraphs(Model.ArticleObject.corrections, writer);

            // if (PostProcessing == PostProcessing.RTF)
            writer.WriteLine("<div class=\"dj_article_lp\">");

            // writer.WriteLine("<div class=\"dj_article_lp\">");
            RenderParagraphs(Model.ArticleObject.leadParagraph, writer);

            writer.WriteLine("</div><div class=\"dj_article_tp\">");

            // writer.WriteLine("</div>");
            // writer.WriteLine("<div class=\"dj_article_tp\">");
            RenderParagraphs(Model.ArticleObject.tailParagraphs, writer);

            writer.WriteLine("</div><div class=\"dj_article_tf\">");

            // writer.WriteLine("</div>");
            // writer.WriteLine("<div class=\"dj_article_tf\">");
            RenderTailFields(writer);

            writer.WriteLine("</div>");

            if (Model.ArticleObject.accessionNo != null)
            {
                writer.WriteLine("<div class=\"dj_article_an\">");
                RenderField(
                    String.Format(
                        "{0} {1}",
                        Model.Tokens.Document,
                        Model.ArticleObject.accessionNo),
                    Codes.AN.ToString(),
                    HtmlTextWriterTag.P, writer);

                writer.WriteLine("</div>");
            }

            if (Model.ArticleObject.contentParts!=null && !string.IsNullOrEmpty(Model.ArticleObject.contentParts.contentType) && Model.ArticleObject.contentParts.contentType.ToLower() == ContentType.picture.ToString())
            {
                writer.WriteLine("<div class=\"dj_article_clear\" ></div>");
            }

            // process each accession number in refaccession number 
            // and replace the placeholders
            // GetContent(RefAccessionNumbers.ToArray());
            writer.WriteLine("</div>");
            ReplaceElinkPlaceholders(writer);
        }


        /// <summary>
        /// The process content.
        /// </summary>
        /// <param name="arrAccessionNumbers">
        /// The arr accession numbers.
        /// </param>
        private void ProcessContent(string[] arrAccessionNumbers, HtmlTextWriter writer)
        {
            var requestDTO = new AccessionNumberSearchRequestDTO();
            ControlData controlData = ControlDataManager.Clone(SessionData.Instance().SessionBasedControlData);
            var manager = new SearchManager(controlData, "en");

            requestDTO.AccessionNumbers = arrAccessionNumbers;
            requestDTO.SortBy = SortBy.FIFO;
            requestDTO.MetaDataController.Mode = CodeNavigatorMode.All;
            requestDTO.DescriptorControl.Mode = DescriptorControlMode.All;
            requestDTO.DescriptorControl.Language = "en";
            requestDTO.MetaDataController.ReturnCollectionCounts = true;
            requestDTO.MetaDataController.ReturnKeywordsSet = true;
            requestDTO.MetaDataController.TimeNavigatorMode = TimeNavigatorMode.PublicationDate;

            requestDTO.SearchCollectionCollection.AddRange(new[]
                                                               {
                                                                   SearchCollection.CustomerDoc,
                                                                   SearchCollection.Summary, SearchCollection.Boards,
                                                                   SearchCollection.Multimedia,
                                                                   SearchCollection.Pictures,
                                                                   SearchCollection.Audio, SearchCollection.AlistBlogs,
                                                                   SearchCollection.Blogs, SearchCollection.NewsSites,
                                                                   SearchCollection.Publications,
                                                                   SearchCollection.Video, SearchCollection.WebSites,
                                                                   SearchCollection.Internal
                                                               });

            requestDTO.GetPerformContentSearchRequest<FreeSearchRequest>();
            var response = manager.PerformAccessionNumberSearch<FreeSearchRequest, FreeSearchResponse>(requestDTO);
            foreach (
                var contentItem in 
                    response.AccessionNumberBasedContentItemSet.AccessionNumberBasedContentItemCollection)
            {
                //// AccessionNumberBasedContentItem contentItem = response.AccessionNumberBasedContentItemSet.AccessionNumberBasedContentItemCollection[0];

                if (contentItem != null && contentItem.HasBeenFound && contentItem.ContentHeadline != null)
                {
                    string text = GetText(contentItem);
                    bool found = false;
                    foreach (ContentItem item in contentItem.ContentHeadline.ContentItems.ItemCollection)
                    {
                        if (!string.IsNullOrEmpty(item.Mimetype))
                        {
                            string strHref = GetHandlerUrl(ImageType.Display, contentItem.AccessionNumber, item);

                            // replace the accession number placeholders
                            switch (contentItem.ContentHeadline.ContentItems.ContentType)
                            {
                                case "picture":
                                case "file":
                                case "pdf":
                                case "summary":
                                    writer.InnerWriter.ToString().Replace(
                                        "##" + contentItem.AccessionNumber + "##",
                                        string.Format("<a class=\"dj_article_elink\" href=\"{0}\" rel=\"{{href:'{0}'}}\">{1}</a>", strHref, text));

                                    found = true;
                                    break;
                            }
                        }
                    }

                    if (!found)
                    {
                        writer.InnerWriter.ToString().Replace(
                           "##" + contentItem.AccessionNumber + "##",
                            string.Format("<a class=\"dj_article_elink\" href=\"javascript:void(0);\" rel=\"{{ref:'{0}'}}\">{1}</a>", contentItem.AccessionNumber, text));
                    }
                }
                else if (contentItem != null)
                {
                    var text = GetText(contentItem);
                    writer.InnerWriter.ToString().Replace(
                        "##" + contentItem.AccessionNumber + "##",
                        string.Format("<a class=\"dj_article_elink\" href=\"javascript:void(0);\" rel=\"{{ref:'{0}'}}\">{1}</a>", contentItem.AccessionNumber, text));
                }
            }
        }

        /// <summary>
        /// The get text.
        /// </summary>
        /// <param name="contentItem">The an content item.</param>
        /// <returns>A string representing the content item text value.</returns>
        private string GetText(AccessionNumberBasedContentItem contentItem)
        {
            var text = string.Empty;
            foreach (var elink in _refAccessionNumbers.Where(elink => contentItem.AccessionNumber == elink.Reference))
            {
                text = elink.Text;
                break;
            }

            return text;
        }

        /// <summary>
        /// The audio player url builder.
        /// </summary>
        /// <returns>
        ///  A string representing the url..
        /// </returns>
        private string AudioPlayerUrlBuilder()
        {
            var urlBuilder = new AudioMediaUrlBuilder
            {
                AccessionNumber = Model.ArticleObject.accessionNo,
                //AccountId = Model.AccountID,
                ContentCategory = "Publications",
                ContentLanguage = Model.ArticleObject.baseLanguage.ToLower(),
                IncludeMarketingMessage = true,
                NameSpace = Model.ControlData.ProductID,
                //UserId = Model.UserID
            };

            return urlBuilder.ToString();
        }

        /// <summary>
        /// The audio player download url builder.
        /// </summary>
        /// <returns>
        /// A string representing the url.
        /// </returns>
        private string AudioPlayerDownloadUrlBuilder()
        {
            var urlBuilderforDnld = new AudioMediaUrlBuilder(MediaRedirectionType.UrlToSoundFile)
            {
                AccessionNumber = Model.ArticleObject.accessionNo,
                //AccountId = Model.AccountID,
                ContentCategory = "Publications",
                ContentLanguage = Model.ArticleObject.baseLanguage.ToLower(),
                IncludeMarketingMessage = true,
                NameSpace = Model.ControlData.ProductID,
                //UserId = Model.UserID
            };

            return urlBuilderforDnld.ToString();
        }

        #endregion


        protected override void WriteHtml(HtmlTextWriter writer)
        {
            // Get the article control header section
            BuildHeader(writer);

            // get the artcile cotrol controls section
            writer.Write("<div class=\"dj_article_control\" id=\"control\">");

            if (Model.ShowSocialButtons && Model.SocialButtonModel.SocialNetworks.Count > 0)
            {
                BuildSocialControlSection(writer);
            }


            BuildSpeakerControlSection(writer);

            if (Model.ShowTranslator)
            {
                if (Model.ShowReadSpeaker)
                {
                    writer.Write("<span class=\"emg_translator_pipe\">&nbsp;|&nbsp;</span>");
                }

                BuildTranslatorControlSection(writer);
            }

            // control end section
            writer.Write("</div>");

            // get the article body part
            BuildBodySection(writer);

            base.WriteHtml(writer);
        }


        protected override void WriteContent(HtmlTextWriter writer)
        {
            // do nothing as we're taking full control of HTML in WriteHtml
        }

    }
}
