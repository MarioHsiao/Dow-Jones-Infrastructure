// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArticleConversionManager.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using DowJones.Infrastructure;
using DowJones.Session;
using DowJones.Utilities.DTO.Web.Request;
using DowJones.Utilities.Formatters.Globalization;
using DowJones.Utilities.Managers.Search;
using DowJones.Utilities.Managers.Search.Requests;
using DowJones.Utilities.Managers.Search.Responses;
using DowJones.Utilities.Uri;
using Factiva.Gateway.Messages.Archive.V2_0;
using Factiva.Gateway.Messages.Search.V2_0;
using ControlData = Factiva.Gateway.Utils.V1_0.ControlData;
using FreeSearchRequest = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchRequest;
using FreeSearchResponse = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchResponse;

namespace DowJones.Ajax.Article.Converter
{
    public class ArticleConversionManager
    {
        internal const string LogoSiteUri = "http://global.factiva.com/FactivaLogos/";

        private readonly SearchManager manager;
        private readonly string interfaceLanguage;
        private readonly ContentItems contentItems;
        private readonly PostProcessing PostProcessing;
        private readonly Factiva.Gateway.Messages.Archive.V2_0.Article ArticleObject;
        private readonly ControlData ControlData;
        private readonly IPreferences preferences;
        private readonly bool ShowCompanyEntityReference;
        private readonly bool ShowExecutiveEntityReference;
        private readonly bool ShowHighlighting;
        private readonly string CanonicalSearchString;
        private readonly bool EnableELinks;

        /// <summary>
        /// The File Handler Url.
        /// </summary>
        /// ~DowJones.Utilities.Handlers.ArticleControl.ContentHandler.ashx
        private readonly string FILE_HANDLER_URL = "";

        //internal const string LogoSite_Uri = "http://global.factiva.com/FactivaLogos/";

        /// <summary>
        /// The ref accession numbers.
        /// </summary>
        private readonly List<ElinkReferences> _refAccessionNumbers;

        /// <summary>
        /// The un processed ref accession numbers.
        /// </summary>
        private readonly List<ElinkReferences> _unProcessedRefAccessionNumbers;

        public ArticleConversionManager(SearchManager manager,ContentItems contentItem,PostProcessing postProcessing,
            Factiva.Gateway.Messages.Archive.V2_0.Article articleObject, ControlData controlData, IPreferences preferences,
            bool showCompanyEntityReference, bool showExecutiveEntityReference, bool showHighlighting, string canonicalSearchString, 
            bool enableELinks,string imageUrl)
        {
            this.manager = manager;
            this.interfaceLanguage = preferences.InterfaceLanguage;
            this.contentItems = contentItem;
            this.PostProcessing = postProcessing;
            this.ArticleObject = articleObject;
            this.ControlData = controlData;
            this.preferences = preferences;
            this.ShowCompanyEntityReference = showCompanyEntityReference;
            this.ShowExecutiveEntityReference = showExecutiveEntityReference;
            this.ShowHighlighting = showHighlighting;
            this.CanonicalSearchString = canonicalSearchString;
            this.EnableELinks = enableELinks;
            this.FILE_HANDLER_URL = imageUrl ?? "~DowJones.Utilities.Handlers.ArticleControl.ContentHandler.ashx";
            _refAccessionNumbers = new List<ElinkReferences>();
            _unProcessedRefAccessionNumbers = new List<ElinkReferences>();
        }

        public ArticleResultset Process()
        {
            
            ArticleResultset articleResult = new ArticleResultset();
            
            if (ArticleObject != null)
            {
                var tempItem = new List<IRenderItem>();
                articleResult.AccessionNo = ArticleObject.accessionNo;
                var logo = string.Empty;
                //Head Start
                if (ArticleObject.sourceLogo != null && ArticleObject.sourceLogo.image != null)
                    logo = ArticleObject.sourceLogo.image;
                else
                    logo = !String.IsNullOrEmpty(ArticleObject.sourceCode) ? String.Format("{0}Logo.gif", ArticleObject.sourceCode) : null;

                if (!string.IsNullOrEmpty(logo))
                {
                    if ((PostProcessing == PostProcessing.UnSpecified) ||
                        (PostProcessing != PostProcessing.RTF && PostProcessing != PostProcessing.Save))
                    {
                        var urlBuilder = new UrlBuilder(LogoSiteUri + logo);
                        tempItem.Add(new RenderItem { ItemMarkUp = MarkUpType.HeadLogo, ItemValue = urlBuilder.ToString(), ItemText = ArticleObject.sourceCode + " Logo" });
                    }
                }
                if (ArticleObject.contentParts != null && !string.IsNullOrEmpty(ArticleObject.contentParts.contentType) && ArticleObject.contentParts.contentType.ToLower() == "picture")
                {
                    tempItem.Add(new RenderItem { ItemMarkUp = MarkUpType.HeadImage, ItemValue = GetImageUrl(ImageType.Display, ArticleObject.accessionNo, ArticleObject.contentParts.parts) });
                }
                articleResult.ArticleHead = tempItem;

                tempItem = null;
                //Head End

                tempItem = new List<IRenderItem>();
                //Source Start
                if (ArticleObject.sourceCode != null)
                {
                    tempItem.Add(new RenderItem { ItemMarkUp = MarkUpType.EntityLink, ItemEntityData = new EntityLinkData { Code = ArticleObject.sourceCode, Category = Category.source.ToString(), Name = ArticleObject.sourceName } });
                }
                else if (ArticleObject.sourceName != null)
                {
                    tempItem.Add(new RenderItem { ItemMarkUp = MarkUpType.Span, ItemText = ArticleObject.sourceName });
                }
                articleResult.ArticleSource = tempItem;
                tempItem = null;
                
                //Source End

                tempItem = new List<IRenderItem>();
                if (!CheckCodeSN(Codes.PD.ToString()))
                {
                    articleResult.ArticlePublicationDate = GetPublicationDate(ArticleObject.publicationDate, ArticleObject.publicationTime, ArticleObject.publicationTimeSpecified);
                }
                if (!CheckCodeSN(Codes.ET.ToString()))
                {
                    articleResult.ArticlePublicationTime = GetPublicationTime(ArticleObject.publicationDate, ArticleObject.publicationTime, ArticleObject.publicationTimeSpecified);
                }

                //HeadLine Start
                string href = null;
                string contentType = null;
                string headlineText = GetParagraphText(ArticleObject.headline);

                if (ArticleObject.contentParts != null)
                {
                    contentType = ArticleObject.contentParts.contentType.ToLower();

                    if (contentType.Equals(ContentType.webpage.ToString(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        href = ArticleObject.contentParts.primaryReference;
                    }
                }

                if (href == null)
                {
                    tempItem.Add(new RenderItem { ItemMarkUp = MarkUpType.Plain, ItemText = headlineText });
                }
                else if (contentType.Equals(ContentType.webpage.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    tempItem.Add(new RenderItem { ItemMarkUp = MarkUpType.Anchor, ItemValue = href, ItemText = headlineText });
                }
                articleResult.ArticleHeadline = tempItem;
                tempItem=null;
                //Headline End

                //Copyright Start
                articleResult.ArticleCopyright = GetRenderItems(ArticleObject.copyright.Items);
                //Copyright End

                //Correction Start
                
                var listBody = new List<Dictionary<string, List<IRenderItem>>>();
                var dictionaryItem = new Dictionary<string, List<IRenderItem>>(); 
                if (ArticleObject.corrections != null)
                {
                    foreach (Paragraph para in ArticleObject.corrections)
                    {
                        tempItem = new List<IRenderItem>();
                        dictionaryItem = new Dictionary<string, List<IRenderItem>>();

                        tempItem = GetRenderItems(para.Items);
                        
                        if (ParagraphDisplay.Proportional == para.display)
                        {
                            dictionaryItem.Add("display", tempItem);
                        }
                        else
                        {
                            dictionaryItem.Add("pre", tempItem);
                        }
                        listBody.Add(dictionaryItem);
                        tempItem = null;
                        dictionaryItem = null;
                    }
                    articleResult.ArticleCorrection = listBody;
                    listBody= null;
                }
                //Correction End

                //Lead Paragraph Start
                
                if (ArticleObject.leadParagraph != null)
                {
                    listBody = new List<Dictionary<string, List<IRenderItem>>>();
                    foreach (Paragraph para in ArticleObject.leadParagraph)
                    {
                        tempItem = new List<IRenderItem>();
                        dictionaryItem = new Dictionary<string, List<IRenderItem>>();

                        tempItem = GetRenderItems(para.Items);
                        if (ParagraphDisplay.Proportional == para.display)
                        {
                            dictionaryItem.Add("display", tempItem);
                        }
                        else
                        {
                            dictionaryItem.Add("pre", tempItem);
                        }
                        listBody.Add(dictionaryItem);
                        tempItem = null;
                        dictionaryItem = null;
                    }
                    articleResult.ArticleLeadParagraph = listBody;
                    listBody = null;
                }
                //Lead Paragraph End

                //Tail Paragraph Start
                
                
                if (ArticleObject.tailParagraphs != null)
                {
                    listBody = new List<Dictionary<string, List<IRenderItem>>>();
                    foreach (Paragraph para in ArticleObject.tailParagraphs)
                    {
                        tempItem = new List<IRenderItem>();
                        dictionaryItem = new Dictionary<string, List<IRenderItem>>();

                        tempItem = GetRenderItems(para.Items);
                        if (ParagraphDisplay.Proportional == para.display)
                        {
                            dictionaryItem.Add("display", tempItem);
                        }
                        else
                        {
                            dictionaryItem.Add("pre", tempItem);
                        }
                        listBody.Add(dictionaryItem);
                        tempItem = null;
                        dictionaryItem = null;
                    }
                    articleResult.ArticleTailParagraph = listBody;
                    listBody = null;
                }
                //Tail Paragraph End

                //Contact Start
                if(ArticleObject.contact!= null && ArticleObject.contact.Items!=null)
                {
                    articleResult.ArticleContact =  GetRenderItems(ArticleObject.contact.Items);
                }
                //Contact End
                
                //Notes Start
                if (ArticleObject.notes != null && ArticleObject.notes.Items != null)
                {
                    articleResult.ArticleNotes = GetRenderItems(ArticleObject.notes.Items);
                }
                //Notes End

                //Art work Start
                if (ArticleObject.artWork != null && ArticleObject.artWork.Items != null)
                {
                    articleResult.ArticleArtWorks = GetRenderItems(ArticleObject.artWork.Items);
                }
                //Art work End

                ReplaceElinkPlaceholders(articleResult);
            }
            return articleResult;
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
        private void ReplaceElinkPlaceholders(ArticleResultset articleResult)
        {
            var arrAccessionNumbers = new string[_refAccessionNumbers.Count];
            int index = -1;
            foreach (ElinkReferences elink in _refAccessionNumbers)
            {
                bool processed = false;
                foreach (ContentItem item in contentItems.ItemCollection)
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
                ProcessContent(arrAccessionNumbers,articleResult);
            }

            if (_unProcessedRefAccessionNumbers.Count <= 0)
            {
                return;
            }

            // Process unprocessed ANs
            string[] accessNos = ReturnANArray(_unProcessedRefAccessionNumbers);
            ProcessContent(accessNos,articleResult);
        }

        /// <summary>
        /// The render items.
        /// </summary>
        /// <param name="items">
        /// The items.
        /// </param>

        private List<IRenderItem> GetRenderItems(object[] items)
        {
            List<IRenderItem> renderItems = new List<IRenderItem>();
            foreach (object item in items)
            {
                if (item is HighlightedText)
                {
                    var highlight = (HighlightedText)item;
                    if (highlight.text != null && highlight.text.Value != null)
                    {
                        renderItems.Add(new RenderItem { ItemMarkUp = MarkUpType.ArticleHighlight, ItemText = highlight.text.Value.Replace("<", "&lt;").Replace(">", "&gt;") });
                    }
                }
                else if (item is ELink)
                {
                    var entityLink = (ELink)item;
                    RenderEntityLink(entityLink, renderItems);
                }
                else if (item is EntityReference)
                {
                    var entityReference = (EntityReference)item;
                    var companyName = string.Empty;
                    var renderTextStringBuilder = new StringBuilder();
                    if (entityReference.Items != null && entityReference.Items.Length > 0)
                    {
                        foreach (var t in entityReference.Items)
                        {
                            companyName = companyName + t;
                            renderTextStringBuilder.Append(t.Replace("<", "&lt;").Replace(">", "&gt;"));
                        }
                    }

                    if (ShowCompanyEntityReference || ShowExecutiveEntityReference)
                    {
                        if (entityReference.category.Equals(Codes.co.ToString(), StringComparison.InvariantCultureIgnoreCase))
                        {
                            if ((PostProcessing == PostProcessing.UnSpecified) && ShowCompanyEntityReference)
                            {
                                BuildEntityLink(companyName, entityReference.code, Category.company.ToString(), renderItems);
                            }
                            else
                            {
                                renderItems.Add(new RenderItem { ItemMarkUp = MarkUpType.Span, ItemText = renderTextStringBuilder.ToString() });
                            }
                        }
                        else if (entityReference.category.Equals(Codes.pe.ToString(), StringComparison.InvariantCultureIgnoreCase))
                        {
                            if ((PostProcessing == PostProcessing.UnSpecified) && ShowCompanyEntityReference)
                            {
                                BuildEntityLink(companyName, entityReference.code, Category.executive.ToString(), renderItems);
                            }
                            else
                            {
                                renderItems.Add(new RenderItem { ItemMarkUp = MarkUpType.Span, ItemText = renderTextStringBuilder.ToString() });
                            }
                        }
                        else
                        {
                            renderItems.Add(new RenderItem { ItemMarkUp = MarkUpType.Plain, ItemText = renderTextStringBuilder.ToString() });
                        }
                    }
                    else
                    {
                        renderItems.Add(new RenderItem { ItemMarkUp = MarkUpType.Plain, ItemText = renderTextStringBuilder.ToString() });
                    }
                }
                else
                {
                    var text = (Text)item;
                    if (text.Value != null)
                    {
                        string[] splittedParts;
                        if (ShowHighlighting && text.Value.Contains(CanonicalSearchString))
                        {
                            var separator = new string[1];
                            separator[0] = CanonicalSearchString;

                            splittedParts = text.Value.Split(separator, StringSplitOptions.None);
                            if (splittedParts.Length > 0)
                            {
                                for (int i = 0; i <= splittedParts.Length - 2; i++)
                                {
                                    renderItems.Add(new RenderItem { ItemMarkUp = MarkUpType.Plain, ItemText = splittedParts[i] });
                                    renderItems.Add(new RenderItem { ItemMarkUp = MarkUpType.Span, ItemText = CanonicalSearchString, ItemClass = "dj_article_canonicalstring" });
                                }
                                renderItems.Add(new RenderItem { ItemMarkUp = MarkUpType.Plain, ItemText = splittedParts[splittedParts.Length - 1] });
                            }
                        }
                        else
                        {
                            renderItems.Add(new RenderItem { ItemMarkUp = MarkUpType.Plain, ItemText = text.Value.Replace("<", "&lt;").Replace(">", "&gt;") });
                        }
                    }
                }

            }

            return renderItems;
        }

        /// <summary>
        /// The render e link.
        /// </summary>
        /// <param name="elink">The elink.</param>
        private void RenderEntityLink(ELink elink, List<IRenderItem> renderItems)
        {
            if (elink.type.Equals(ElinkType.pro.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                if (PostProcessing != PostProcessing.Save)
                {
                    renderItems.Add(new RenderItem { ItemMarkUp = MarkUpType.Image, ItemValue = GetImageUrl(ImageType.Display, ArticleObject.accessionNo, elink.parts) });
                }
            }
            else if (elink.type.Equals(ElinkType.webpage.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                switch (PostProcessing)
                {
                    case PostProcessing.Print:
                        renderItems.Add(new RenderItem { ItemMarkUp = MarkUpType.PostProcessing, ItemPostProcessData = new PostProcessData { Type = PostProcessing.Print, ElinkValue = (elink.text != null) ? elink.text.Value : elink.reference, ElinkText = elink.reference } });
                        break;
                    case PostProcessing.Save:
                        renderItems.Add(new RenderItem { ItemMarkUp = MarkUpType.PostProcessing, ItemPostProcessData = new PostProcessData { Type = PostProcessing.Save, ElinkValue = (elink.text != null) ? elink.text.Value : elink.reference } });
                        renderItems.Add(new RenderItem { ItemMarkUp = MarkUpType.PostProcessing, ItemPostProcessData = new PostProcessData { Type = PostProcessing.Save, ElinkText = elink.reference } });
                        break;
                    default:
                        if (PostProcessing != PostProcessing.UnSpecified || !EnableELinks)
                        {
                            renderItems.Add(new RenderItem { ItemMarkUp = MarkUpType.Plain, ItemText = (elink.text != null) ? elink.text.Value : elink.reference });
                        }
                        else
                        {
                            renderItems.Add(new RenderItem { ItemMarkUp = MarkUpType.ArticleElink, ItemText = (elink.text != null) ? elink.text.Value : elink.reference, ItemValue = elink.reference });
                        }

                        break;
                }
            }
            else if (elink.type.Equals(ElinkType.company.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                if (PostProcessing != PostProcessing.UnSpecified || !EnableELinks)
                {
                    renderItems.Add(new RenderItem { ItemMarkUp = MarkUpType.Plain, ItemText = elink.text.Value });
                }
                else
                {
                    BuildSpanLink(elink.text.Value, string.Empty, Category.companynews.ToString(), renderItems);
                }
            }
            else if (elink.type.Equals(ElinkType.executive.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                if (PostProcessing != PostProcessing.UnSpecified || !EnableELinks)
                {
                    renderItems.Add(new RenderItem { ItemMarkUp = MarkUpType.Plain, ItemText = elink.text.Value });
                }
                else
                {
                    BuildSpanLink(elink.text.Value, string.Empty, Category.companynews.ToString(), renderItems);
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

                    renderItems.Add(new RenderItem { ItemMarkUp = MarkUpType.Plain, ItemText = "##" + elink.reference + "##" });

                    elinkReference.Reference = elink.reference;
                    elinkReference.Text = elinkText;
                    _refAccessionNumbers.Add(elinkReference);
                }
                else
                {
                    if (elink.text != null)
                    {
                        renderItems.Add(new RenderItem { ItemMarkUp = MarkUpType.Plain, ItemText = elink.text.Value });
                    }
                    else if (elink.reference != null)
                    {
                        renderItems.Add(new RenderItem { ItemMarkUp = MarkUpType.Plain, ItemText = "##" + elink.reference + "##" });
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
        private void BuildEntityLink(string name, string code, string category, List<IRenderItem> renderItem)
        {
            renderItem.Add(new RenderItem { ItemClass = "dj_article_entity", ItemMarkUp = MarkUpType.EntityLink, ItemEntityData = new EntityLinkData { Code = code ?? string.Empty, Name = name ?? string.Empty, Category = category } });
        }

        private void BuildSpanLink(string name, string code, string category, List<IRenderItem> renderItem)
        {
            renderItem.Add(new RenderItem { ItemClass = "dj_article_entity", ItemMarkUp = MarkUpType.SpanAnchor, ItemEntityData = new EntityLinkData { Code = code ?? string.Empty, Name = name ?? string.Empty, Category = category } });
        }


        private bool CheckCodeSN(string code)
        {

            if (code.Equals(Codes.SN.ToString(), StringComparison.InvariantCultureIgnoreCase) &&
                        Equals(PostProcessing.Print, StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }
            else
                return false;
        }


        private string GetPublicationDate(DateTime publicationDate, DateTime publicationTime, bool publicationTimeSpecified)
        {
            var assembler = new DateTimeFormatter(preferences);
            DateTime tempararyPublicationDate = publicationDate;
            if (publicationTimeSpecified)
                tempararyPublicationDate = DateTimeFormatter.Merge(publicationDate, publicationTime);

            return assembler.FormatDate(tempararyPublicationDate);
        }

        private string GetPublicationTime(DateTime publicationDate, DateTime publicationTime, bool publicationTimeSpecified)
        {
            var assembler = new DateTimeFormatter(preferences);
            string publicationTm = string.Empty;
            if (publicationTimeSpecified)
            {
                publicationTm = assembler.FormatTime(DateTimeFormatter.Merge(publicationDate, publicationTime));
            }
            return publicationTm;
        }

        /// <summary>
        /// The get image url.
        /// </summary>
        /// <param name="imageType">The image type.</param>
        /// <param name="accessionNo">The accession no.</param>
        /// <param name="parts">The parts.</param>
        /// <returns>A stromg representing the image url.</returns>
        public virtual string GetImageUrl(ImageType imageType, string accessionNo, Part[] parts)
        {
            string reference = null;
            string mimeType = null;

            if (parts != null && parts.Length > 0)
            {
                foreach (Part part in parts.Where(part => part.type.ToLower() == imageType.ToString().ToLower()))
                {
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

                if (!string.IsNullOrEmpty(ControlData.AccessPointCode))
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "AccessPointCode"), ControlData.AccessPointCode);
                if (!string.IsNullOrEmpty(interfaceLanguage))
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "InterfaceLanguage"), interfaceLanguage);
                if (!string.IsNullOrEmpty(ControlData.ProductID))
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "ProductID"), ControlData.ProductID);
                if (!string.IsNullOrEmpty(ControlData.SessionID))
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "SessionID"), ControlData.SessionID);
                else if (!string.IsNullOrEmpty(ControlData.EncryptedLogin)) // assume this is a lightweight user
                {
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "EncryptedToken"), ControlData.EncryptedLogin);
                }
                else
                {
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "UserID"), ControlData.UserID);
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "Password"), ControlData.UserPassword);
                }

                if (!string.IsNullOrEmpty(ControlData.AccessPointCodeUsage))
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "AccessPointCodeUsage"), ControlData.AccessPointCodeUsage);
                if (!string.IsNullOrEmpty(ControlData.CacheKey))
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "CacheKey"), ControlData.CacheKey);
                if (!string.IsNullOrEmpty(ControlData.ClientCode))
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "ClientCodeType"), ControlData.ClientCode);
                return ub.ToString();
            }

            return null;
        }

        /// <summary>
        /// Gets FileHandlerUrl.
        /// </summary>
        [UrlProperty]
        public string FileHandlerUrl
        {
            get { return FILE_HANDLER_URL; }
        }

        #region public helper method
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
        #endregion



        /// <summary>
        /// The process content.
        /// </summary>
        /// <param name="arrAccessionNumbers">
        /// The arr accession numbers.
        /// </param>
        private void ProcessContent(string[] arrAccessionNumbers, ArticleResultset articleResult)
        {
            var requestDTO = new AccessionNumberSearchRequestDTO();

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
            AccessionNumberSearchResponse response = manager.PerformAccessionNumberSearch<FreeSearchRequest, FreeSearchResponse>(requestDTO);
            foreach (
                AccessionNumberBasedContentItem contentItem in
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
                                    foreach (Dictionary<string, List<IRenderItem>> dc in articleResult.ArticleLeadParagraph)
                                    {
                                        foreach (KeyValuePair<string, List<IRenderItem>> kvp in dc)
                                        {
                                            foreach (IRenderItem ritem in kvp.Value)
                                            {
                                                if (ritem.ItemText.Equals("##" + contentItem.AccessionNumber + "##"))
                                                {
                                                    ritem.ItemMarkUp = MarkUpType.Anchor;
                                                    ritem.ItemText = text;
                                                    ritem.ItemValue = strHref;
                                                }
                                            }
                                        }
                                    }
                                    found = true;
                                    break;
                            }
                        }
                    }

                    if (!found)
                    {
                        foreach (Dictionary<string, List<IRenderItem>> dc in articleResult.ArticleLeadParagraph)
                        {
                            foreach (KeyValuePair<string, List<IRenderItem>> kvp in dc)
                            {
                                foreach (IRenderItem item in kvp.Value)
                                {
                                    if (item.ItemText.Equals("##" + contentItem.AccessionNumber + "##"))
                                    {
                                        item.ItemMarkUp = MarkUpType.Anchor;
                                        item.ItemText = text;
                                        item.ItemValue = contentItem.AccessionNumber;
                                    }
                                }
                            }
                        }
                    }
                }
                else if (contentItem != null)
                {
                    var text = GetText(contentItem);
                    foreach (Dictionary<string, List<IRenderItem>> dc in articleResult.ArticleLeadParagraph)
                    {
                        foreach (KeyValuePair<string, List<IRenderItem>> kvp in dc)
                        {
                            foreach (IRenderItem item in kvp.Value)
                            {
                                if (item.ItemText.Equals("##" + contentItem.AccessionNumber + "##"))
                                {
                                    item.ItemMarkUp = MarkUpType.Anchor;
                                    item.ItemText = text;
                                    item.ItemValue = contentItem.AccessionNumber;
                                }
                            }
                        }
                    }
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
        /// The get handler url.
        /// </summary>
        /// <param name="imageType">The image type.</param>
        /// <param name="accessionNo">The accession no.</param>
        /// <param name="contentItem">The content item.</param>
        /// <returns>A string representing the Handerl Url</returns>
        private string GetHandlerUrl(ImageType imageType, string accessionNo, ContentItem contentItem)
        {
            string reference = contentItem.Ref;
            string mimeType = contentItem.Mimetype;

            if (!string.IsNullOrEmpty(FileHandlerUrl) && !string.IsNullOrEmpty(FileHandlerUrl.Trim()))
            {
                var ub = new UrlBuilder(FileHandlerUrl);
                ub.Append(UrlBuilder.GetParameterName(typeof(ArchiveFileRequestDTO), "AccessionNumber"), accessionNo);
                ub.Append(UrlBuilder.GetParameterName(typeof(ArchiveFileRequestDTO), "Reference"), reference);
                ub.Append(UrlBuilder.GetParameterName(typeof(ArchiveFileRequestDTO), "MimeType"), mimeType);
                ub.Append(UrlBuilder.GetParameterName(typeof(ArchiveFileRequestDTO), "ImageType"), (imageType == ImageType.Display) ? "dispix" : "tnail");
                if (!string.IsNullOrEmpty(ControlData.AccessPointCode))
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "AccessPointCode"), ControlData.AccessPointCode);
                if (!string.IsNullOrEmpty(interfaceLanguage))
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "InterfaceLanguage"), interfaceLanguage);
                if (!string.IsNullOrEmpty(ControlData.ProductID))
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "ProductID"), ControlData.ProductID);
                if (!string.IsNullOrEmpty(ControlData.SessionID))
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "SessionID"), ControlData.SessionID);
                else if (!string.IsNullOrEmpty(ControlData.EncryptedLogin)) // assume this is a lightweight user
                {
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "EncryptedToken"), ControlData.EncryptedLogin);
                }
                else
                {
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "UserID"), ControlData.UserID);
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "Password"), ControlData.UserPassword);
                }

                if (!string.IsNullOrEmpty(ControlData.AccessPointCodeUsage))
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "AccessPointCodeUsage"), ControlData.AccessPointCodeUsage);
                if (!string.IsNullOrEmpty(ControlData.CacheKey))
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "CacheKey"), ControlData.CacheKey);
                if (!string.IsNullOrEmpty(ControlData.ClientCode))
                    ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "ClientCodeType"), ControlData.ClientCode);

                return ub.ToString();
            }
            return null;
        }


    }
}
