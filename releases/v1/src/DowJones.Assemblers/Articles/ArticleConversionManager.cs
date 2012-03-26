// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArticleConversionManager.cs" company="Dow Jones">
//   Dow Jones 
// </copyright>
// <summary>
//   Defines the ArticleConversionManager type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using DowJones.Ajax;
using DowJones.Ajax.Article;
using DowJones.Articles;
using DowJones.DTO.Web.Request;
using DowJones.Extensions;
using DowJones.Formatters.Globalization.DateTime;
using DowJones.Globalization;
using DowJones.Infrastructure;
using DowJones.Managers.Search;
using DowJones.Managers.Search.Requests;
using DowJones.Managers.Search.Responses;
using DowJones.Preferences;
using DowJones.Session;
using DowJones.Url;
using Factiva.Gateway.Messages.Archive.V2_0;
using Factiva.Gateway.Messages.Search.V2_0;
using log4net;
using Sgml;
using ContentCategory = DowJones.Ajax.ContentCategory;
using PerformContentSearchRequest = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchRequest;
using PerformContentSearchResponse = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchResponse;

namespace DowJones.Assemblers.Articles
{
    public class ArticleConversionManager
    {
        public const string DefaultFileHandlerUrl = "~/DowJones.Web.Handlers.Article.ContentHandler.ashx";
        internal const string LogoSiteUri = "http://global.factiva.com/FactivaLogos/";
        private static readonly ILog log = LogManager.GetLogger(typeof (ArticleConversionManager));

        private readonly IControlData _controlData;
        private readonly DateTimeFormatter _dateTimeFormatter;
        private readonly SearchManager _manager;
        private readonly PostProcessing _postProcessing;
        private readonly IPreferences _preferences;
        private readonly List<ElinkReferences> _refAccessionNumbers;
        private readonly IResourceTextManager _resourceTextManager;
        private readonly List<ElinkReferences> _unProcessedRefAccessionNumbers;
        private readonly IArticleService _articleService;

        public ArticleConversionManager(IControlData controlData, IPreferences preferences, SearchManager manager, DateTimeFormatter dateTimeFormatter, IResourceTextManager resourceTextManager, IArticleService articleService)
        {
            _manager = manager;
            _preferences = preferences;
            _postProcessing = PostProcessing.UnSpecified;
            _controlData = controlData;
            _dateTimeFormatter = dateTimeFormatter;
            _resourceTextManager = resourceTextManager;
            _articleService = articleService;
            FileHandlerUrl = DefaultFileHandlerUrl;
            _refAccessionNumbers = new List<ElinkReferences>();
            _unProcessedRefAccessionNumbers = new List<ElinkReferences>();
            ShowCompanyEntityReference = true;
        }

        public bool ShowCompanyEntityReference { get; set; }

        public bool ShowExecutiveEntityReference { get; set; }

        public bool EnableELinks { get; set; }

        public bool EmbedHtmlBasedExternalLinks { get; set; }

        public PictureSize PictureSize { get; set; }

        /// <summary>
        /// Gets or sets the FileHandlerUrl.
        /// </summary>
        /// <value>
        /// The file handler URL.
        /// </value>
        public string FileHandlerUrl { get; set; }

        public IResourceTextManager ResourceTextManager
        {
            get { return _resourceTextManager; }
        }

        public ArticleResultset Convert(Article source)
        {
            return Process(source);
        }

        public ArticleResultset Process(Article article, ContentItems contentItems = null)
        {
            if (article == null)
            {
                return new ArticleResultset
                           {
                               Status = -1
                           };
            }

            if (article.status != null && article.status.value != 0)
            {
                return new ArticleResultset
                           {
                               Status = article.status.value,
                               AccessionNo = article.accessionNo,
                           };
            }

            var articleResult = new ArticleResultset
                                    {
                                        PictureSize = PictureSize,
                                        Status = (article.status != null) ? article.status.value : 0,
                                        AccessionNo = article.accessionNo,
                                        Head = ProcessHeadSection(article),
                                        Source = ProcessSourceSection(article),
                                        SourceCode = article.sourceCode,
                                        SourceName = article.sourceName,
                                        ByLine = ProcessBylineSection(article),
                                        Correction = ProcessParagraphs(article, article.corrections),
                                        LeadParagraph = ProcessParagraphs(article, article.leadParagraph),
                                        TailParagraphs = ProcessParagraphs(article, article.tailParagraphs),
                                        WordCount = article.wordCount,
                                        PublisherName = article.publisherName,
                                        PublisherGroupCode = article.publisherGroupCode,
                                        Copyright = article.copyright != null ? GetRenderItems(article.copyright.Items, article.accessionNo) : new List<IRenderItem>(),
                                        ContentCategory = MapContentCategory(article),
                                        ContentSubCategory = MapContentSubCategory(article),
                                        OriginalContentCategory = GetOriginalContentType(article),
                                    };

            // Map a couple of descriptors
            articleResult.ContentCategoryDescriptor = articleResult.ContentCategory.ToString();
            articleResult.ContentSubCategoryDescriptor = articleResult.ContentSubCategory.ToString();
            articleResult.ExternalUri = GetReferenceUrl(articleResult.ContentCategory, article);
            articleResult.Headline = ProcessHeadline(article, articleResult.OriginalContentCategory, articleResult.ExternalUri);

            if (!CheckCodeSN(Codes.PD.ToString()))
            {
                articleResult.PublicationDate = GetPublicationDate(article.publicationDate, article.publicationTime, article.publicationTimeSpecified);
            }

            if (!CheckCodeSN(Codes.ET.ToString()))
            {
                articleResult.PublicationTime = GetPublicationTime(article.publicationDate, article.publicationTime, article.publicationTimeSpecified);
            }

            if (!string.IsNullOrEmpty(article.baseLanguage))
            {
                articleResult.Language = GetLanguageToContentLanguage(article.baseLanguage);
                articleResult.LanguageCode = article.baseLanguage;
            }

            if (article.contact != null && article.contact.Items != null)
            {
                articleResult.Contact = GetRenderItems(article.contact.Items, article.accessionNo);
            }

            if (article.notes != null && article.notes.Items != null)
            {
                articleResult.Notes = GetRenderItems(article.notes.Items, article.accessionNo);
            }

            if (article.artWork != null && article.artWork.Items != null)
            {
                articleResult.ArtWorks = GetRenderItems(article.artWork.Items, article.accessionNo);
            }

            if (article.pages != null)
            {
                articleResult.Pages = new List<string>(article.pages);
            }

            if (article.authors != null && !article.authors.author.IsNullOrEmpty())
            {
                var tAuthors = article.authors.author
                    .Select(author => new RenderItem
                                          {
                                              ItemMarkUp = MarkUpType.EntityLink,
                                              ItemEntityData = new EntityLinkData
                                                                   {
                                                                       Code = author.nnId,
                                                                       Category = Category.author.ToString(),
                                                                       Name = author.normalizedName
                                                                   }
                                          }).Cast<IRenderItem>().ToList();
                articleResult.Authors = tAuthors;
            }

            //Indexing code sets start
            if (article.indexingCodeSets != null)
            {
                if (article.indexingCodeSets.codeSet != null)
                {
                    var sets = new Dictionary<string, Dictionary<string, string>>();
                    var ipcs = new List<string>();
                    var ipds = new List<string>();
                    foreach (var set in article.indexingCodeSets.codeSet)
                    {
                        switch (set.codeCategory.ToLower())
                        {
                            case "co":
                            case "in":
                            case "ns":
                            case "re":
                            case "pe":
                            case "au":
                                var codes = new Dictionary<string, string>();
                                foreach (var code in set.code
                                    .Where(code => !codes.ContainsKey(code.value) && code.codeDescription != null))
                                    {
                                        codes.Add(code.value, code.codeDescription.FirstOrDefault(v => !string.IsNullOrEmpty(v.Value)).Value);
                                    }

                                sets.Add(set.codeCategory, codes);
                                break;
                            case "tpc":
                                foreach (var code in set.code)
                                {
                                    switch (code.cat.ToLower())
                                    {
                                        case "ipc":
                                            ipcs.Add(code.value.Trim());
                                            break;
                                        case "ipd":
                                            ipds.Add(code.value.Trim());
                                            break;
                                    }
                                }

                                break;
                        }
                    }

                    articleResult.IndexingCodeSets = sets;
                    articleResult.Ipds = ipds;
                    articleResult.Ipcs = ipcs;
                }
            }

            ReplaceElinkPlaceholders(articleResult, contentItems);
            return articleResult;
        }

        /// <summary>
        /// Maps the language to content language.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <returns>A string representing the language</returns>
        private string GetLanguageToContentLanguage(string language)
        {
            ContentLanguage? contentLanguage = Mapper.Map<ContentLanguage>(language);
            return _resourceTextManager.GetAssignedToken(contentLanguage);
        }

        /// <summary>
        ///   The return an array.
        /// </summary>
        /// <param name = "notProcessedRefAccessionNumbers">The un processed ref accession numbers.</param>
        /// <returns>An array of strings</returns>
        private static IEnumerable<string> ReturnANArray(ICollection<ElinkReferences> notProcessedRefAccessionNumbers)
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

        private List<Dictionary<string, List<IRenderItem>>> ProcessParagraphs(Article article, IEnumerable paras)
        {
            var listBody = new List<Dictionary<string, List<IRenderItem>>>();
            if (paras == null)
            {
                return null;
            }
            foreach (Paragraph para in paras)
            {
                var dictionaryItem = new Dictionary<string, List<IRenderItem>>();
                var tempItem = GetRenderItems(para.Items, article.accessionNo);
                dictionaryItem.Add(ParagraphDisplay.Proportional == para.display ? "display" : "pre", tempItem);
                listBody.Add(dictionaryItem);
            }
            return listBody;
        }

        private static List<IRenderItem> ProcessHeadline(Article article, string contentType, string externalUri)
        {
            var tempItem = new List<IRenderItem>();
            var headlineText = GetParagraphText(article.headline);
            switch (contentType)
            {
                case "article":
                case "picture":
                case "articlewithgraphics":
                case "analyst":
                    tempItem.Add(new RenderItem
                                     {
                                         ItemMarkUp = MarkUpType.Plain,
                                         ItemText = headlineText,
                                     });
                    break;
                default:
                    tempItem.Add(new RenderItem
                                     {
                                         ItemMarkUp = MarkUpType.Anchor,
                                         ItemValue = externalUri,
                                         ItemText = headlineText,
                                     });
                    break;
            }
            return tempItem;
        }

        private static List<IRenderItem> ProcessBylineSection(Article article)
        {
            var tempItem = new List<IRenderItem>();
            if (article.byline != null)
            {
                foreach (var item in article.byline.Items)
                {
                    Text text;
                    try
                    {
                        text = (Text) item;
                        tempItem.Add(new RenderItem
                                         {
                                             ItemMarkUp = MarkUpType.Plain,
                                             ItemText = text.Value.Replace("By ", String.Empty)
                                         });
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                }
            }
            return tempItem;
        }

        private static List<IRenderItem> ProcessSourceSection(Article article)
        {
            var tempItem = new List<IRenderItem>();
            if (article.sourceCode != null)
            {
                tempItem.Add(new RenderItem
                                 {
                                     ItemMarkUp = MarkUpType.EntityLink,
                                     ItemEntityData = new EntityLinkData
                                                          {
                                                              Code = article.sourceCode,
                                                              Category = Category.source.ToString(),
                                                              Name = article.sourceName
                                                          }
                                 });
            }
            else if (article.sourceName != null)
            {
                tempItem.Add(new RenderItem
                                 {
                                     ItemMarkUp = MarkUpType.Span,
                                     ItemText = article.sourceName
                                 });
            }
            return tempItem;
        }

        private List<IRenderItem> ProcessHeadSection(Article article)
        {
            string logo;
            var tempItem = new List<IRenderItem>();
            if (article.sourceLogo != null && article.sourceLogo.image != null)
            {
                logo = article.sourceLogo.image;
            }
            else
            {
                logo = !String.IsNullOrEmpty(article.sourceCode) ? String.Format("{0}Logo.gif", article.sourceCode) : null;
            }

            if (!string.IsNullOrEmpty(logo))
            {
                if ((_postProcessing == PostProcessing.UnSpecified) ||
                    (_postProcessing != PostProcessing.RTF && _postProcessing != PostProcessing.Save))
                {
                    var urlBuilder = new UrlBuilder(LogoSiteUri + logo);
                    tempItem.Add(new RenderItem
                                     {
                                         ItemMarkUp = MarkUpType.HeadLogo,
                                         ItemValue = urlBuilder.ToString(),
                                         ItemText = article.sourceCode + " Logo"
                                     });
                }
            }

            if (article.contentParts != null && !string.IsNullOrEmpty(article.contentParts.contentType) && article.contentParts.contentType.ToLower() == "picture")
            {
                tempItem.Add(new RenderItem
                                 {
                                     ItemMarkUp = MarkUpType.HeadImageLarge,
                                     ItemValue = GetImageUrl(ImageType.Display,
                                                             article.accessionNo,
                                                             article.contentParts.parts)
                                 });

                tempItem.Add(new RenderItem
                                 {
                                     ItemMarkUp = MarkUpType.HeadImageSmall,
                                     ItemValue = GetImageUrl(ImageType.Thumbnail,
                                                             article.accessionNo,
                                                             article.contentParts.parts)
                                 });
            }
            return tempItem;
        }

        /// <summary>
        ///   The replace elink placeholders.
        /// </summary>
        private void ReplaceElinkPlaceholders(ArticleResultset articleResult, ContentItems contentItems)
        {
            var arrAccessionNumbers = new List<string>();

            foreach (var elink in _refAccessionNumbers)
            {
                var processed = false;
                var elink1 = elink;
                if (contentItems != null &&
                    contentItems.ItemCollection.IsNullOrEmpty() &&
                    (contentItems.ItemCollection.Any(item => item.Ref.Equals(elink1.Reference, StringComparison.InvariantCultureIgnoreCase) || item.Ref.EndsWith(elink1.Reference, StringComparison.InvariantCultureIgnoreCase))))
                {
                    arrAccessionNumbers.Add(elink.Reference);
                    processed = true;
                }

                if (!processed)
                {
                    // Collect unprocessed ANs
                    _unProcessedRefAccessionNumbers.Add(elink);
                }
            }

            if (arrAccessionNumbers.Count > 0)
            {
                ProcessContent(arrAccessionNumbers, articleResult);
            }

            if (_unProcessedRefAccessionNumbers.Count <= 0)
            {
                return;
            }

            // Process unprocessed ANs
            var accessNos = ReturnANArray(_unProcessedRefAccessionNumbers);
            ProcessContent(accessNos, articleResult);
        }

        /// <summary>
        ///   The render items.
        /// </summary>
        /// <param name = "items">
        ///   The items.
        /// </param>
        /// <param name = "accessionNumber"></param>
        private List<IRenderItem> GetRenderItems(object[] items, string accessionNumber)
        {
            var renderItems = new List<IRenderItem>();
            foreach (object item in items)
            {
                if (item is HighlightedText)
                {
                    var highlight = (HighlightedText) item;
                    if (highlight.text != null && highlight.text.Value != null)
                    {
                        renderItems.Add(new RenderItem {ItemMarkUp = MarkUpType.ArticleHighlight, ItemText = highlight.text.Value.Replace("<", "&lt;").Replace(">", "&gt;")});
                    }
                }
                else if (item is ELink)
                {
                    var entityLink = (ELink) item;
                    RenderEntityLink(entityLink, renderItems, accessionNumber);
                }
                else if (item is EntityReference)
                {
                    var entityReference = (EntityReference) item;
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
                            if ((_postProcessing == PostProcessing.UnSpecified) && ShowCompanyEntityReference)
                            {
                                BuildEntityLink(companyName, entityReference.code, Category.company.ToString(), renderItems);
                            }
                            else
                            {
                                renderItems.Add(new RenderItem {ItemMarkUp = MarkUpType.Span, ItemText = renderTextStringBuilder.ToString()});
                            }
                        }
                        else if (entityReference.category.Equals(Codes.pe.ToString(), StringComparison.InvariantCultureIgnoreCase))
                        {
                            if ((_postProcessing == PostProcessing.UnSpecified) && ShowCompanyEntityReference)
                            {
                                BuildEntityLink(companyName, entityReference.code, Category.executive.ToString(), renderItems);
                            }
                            else
                            {
                                renderItems.Add(new RenderItem {ItemMarkUp = MarkUpType.Span, ItemText = renderTextStringBuilder.ToString()});
                            }
                        }
                        else
                        {
                            renderItems.Add(new RenderItem {ItemMarkUp = MarkUpType.Plain, ItemText = renderTextStringBuilder.ToString()});
                        }
                    }
                    else
                    {
                        renderItems.Add(new RenderItem {ItemMarkUp = MarkUpType.Plain, ItemText = renderTextStringBuilder.ToString()});
                    }
                }
                else
                {
                    var text = (Text) item;
                    if (text.Value != null)
                    {
                        renderItems.Add(new RenderItem
                                            {
                                                ItemMarkUp = MarkUpType.Plain,
                                                ItemText = text.Value.Replace("<", "&lt;").Replace(">", "&gt;")
                                            });
                    }
                }
            }

            return renderItems;
        }

        /// <summary>
        ///   The render e link.
        /// </summary>
        /// <param name = "elink">The elink.</param>
        /// <param name = "renderItems"></param>
        /// <param name = "accessionNumber"></param>
        private void RenderEntityLink(ELink elink, List<IRenderItem> renderItems, string accessionNumber)
        {
            if (elink.type.Equals(ElinkType.pro.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                if (_postProcessing != PostProcessing.Save)
                {
                    renderItems.Add(new RenderItem {ItemMarkUp = MarkUpType.Image, ItemValue = GetImageUrl(ImageType.Display, accessionNumber, elink.parts)});
                }
            }
            else if (elink.type.Equals(ElinkType.webpage.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                switch (_postProcessing)
                {
                    case PostProcessing.Print:
                        renderItems.Add(new RenderItem {ItemMarkUp = MarkUpType.PostProcessing, ItemPostProcessData = new PostProcessData {Type = PostProcessing.Print, ElinkValue = (elink.text != null) ? elink.text.Value : elink.reference, ElinkText = elink.reference}});
                        break;
                    case PostProcessing.Save:
                        renderItems.Add(new RenderItem {ItemMarkUp = MarkUpType.PostProcessing, ItemPostProcessData = new PostProcessData {Type = PostProcessing.Save, ElinkValue = (elink.text != null) ? elink.text.Value : elink.reference}});
                        renderItems.Add(new RenderItem {ItemMarkUp = MarkUpType.PostProcessing, ItemPostProcessData = new PostProcessData {Type = PostProcessing.Save, ElinkText = elink.reference}});
                        break;
                    default:
                        if (_postProcessing != PostProcessing.UnSpecified || !EnableELinks)
                        {
                            renderItems.Add(new RenderItem {ItemMarkUp = MarkUpType.Plain, ItemText = (elink.text != null) ? elink.text.Value : elink.reference});
                        }
                        else
                        {
                            renderItems.Add(new RenderItem {ItemMarkUp = MarkUpType.ArticleElink, ItemText = (elink.text != null) ? elink.text.Value : elink.reference, ItemValue = elink.reference});
                        }

                        break;
                }
            }
            else if (elink.type.Equals(ElinkType.company.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                if (_postProcessing != PostProcessing.UnSpecified || !EnableELinks)
                {
                    renderItems.Add(new RenderItem {ItemMarkUp = MarkUpType.Plain, ItemText = elink.text.Value});
                }
                else
                {
                    BuildSpanLink(elink.text.Value, string.Empty, Category.companynews.ToString(), renderItems);
                }
            }
            else if (elink.type.Equals(ElinkType.executive.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                if (_postProcessing != PostProcessing.UnSpecified || !EnableELinks)
                {
                    renderItems.Add(new RenderItem {ItemMarkUp = MarkUpType.Plain, ItemText = elink.text.Value});
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

                    renderItems.Add(new RenderItem {ItemMarkUp = MarkUpType.Plain, ItemText = "##" + elink.reference + "##"});

                    elinkReference.Reference = elink.reference;
                    elinkReference.Text = elinkText;
                    _refAccessionNumbers.Add(elinkReference);
                }
                else
                {
                    if (elink.text != null)
                    {
                        renderItems.Add(new RenderItem {ItemMarkUp = MarkUpType.Plain, ItemText = elink.text.Value});
                    }
                    else if (elink.reference != null)
                    {
                        renderItems.Add(new RenderItem {ItemMarkUp = MarkUpType.Plain, ItemText = "##" + elink.reference + "##"});
                    }
                }
            }
        }

        /// <summary>
        ///   The build entity link.
        /// </summary>
        /// <param name = "name">The name of the entitly link.</param>
        /// <param name = "code">The code of the entitly link.</param>
        /// <param name = "category">The category.</param>
        /// <param name = "renderItem">The render item.</param>
        private static void BuildEntityLink(string name, string code, string category, ICollection<IRenderItem> renderItem)
        {
            renderItem.Add(new RenderItem {ItemClass = "dj_article_entity", ItemMarkUp = MarkUpType.EntityLink, ItemEntityData = new EntityLinkData {Code = code ?? string.Empty, Name = name ?? string.Empty, Category = category}});
        }

        private static void BuildSpanLink(string name, string code, string category, ICollection<IRenderItem> renderItem)
        {
            renderItem.Add(new RenderItem {ItemClass = "dj_article_entity", ItemMarkUp = MarkUpType.SpanAnchor, ItemEntityData = new EntityLinkData {Code = code ?? string.Empty, Name = name ?? string.Empty, Category = category}});
        }


        private static bool CheckCodeSN(string code)
        {
            return code.Equals(Codes.SN.ToString(), StringComparison.InvariantCultureIgnoreCase) &&
                   Equals(PostProcessing.Print, StringComparison.InvariantCultureIgnoreCase);
        }


        private string GetPublicationDate(DateTime publicationDate, DateTime publicationTime, bool publicationTimeSpecified)
        {
            var tempararyPublicationDate = publicationDate;
            if (publicationTimeSpecified)
                tempararyPublicationDate = DateTimeFormatter.Merge(publicationDate, publicationTime);

            return _dateTimeFormatter.FormatDate(tempararyPublicationDate);
        }

        private string GetPublicationTime(DateTime publicationDate, DateTime publicationTime, bool publicationTimeSpecified)
        {
            var publicationTm = string.Empty;
            if (publicationTimeSpecified)
            {
                publicationTm = _dateTimeFormatter.FormatTime(DateTimeFormatter.Merge(publicationDate, publicationTime));
            }
            return publicationTm;
        }

        /// <summary>
        ///   The get image url.
        /// </summary>
        /// <param name = "imageType">The image type.</param>
        /// <param name = "accessionNo">The accession no.</param>
        /// <param name = "parts">The parts.</param>
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

            if (FileHandlerUrl.HasValue())
            {
                var ub = new UrlBuilder(FileHandlerUrl);
                ub.Append(UrlBuilder.GetParameterName(typeof (ArchiveFileRequestDTO), "AccessionNumber"), accessionNo);
                ub.Append(UrlBuilder.GetParameterName(typeof (ArchiveFileRequestDTO), "Reference"), reference);
                ub.Append(UrlBuilder.GetParameterName(typeof (ArchiveFileRequestDTO), "MimeType"), mimeType);
                ub.Append(UrlBuilder.GetParameterName(typeof (ArchiveFileRequestDTO), "ImageType"), (imageType == ImageType.Display) ? "dispix" : "tnail");

                if (!string.IsNullOrEmpty(_controlData.AccessPointCode))
                {
                    ub.Append(UrlBuilder.GetParameterName(typeof (SessionRequestDTO), "AccessPointCode"), _controlData.AccessPointCode);
                }

                if (!string.IsNullOrEmpty(_preferences.InterfaceLanguage))
                {
                    ub.Append(UrlBuilder.GetParameterName(typeof (SessionRequestDTO), "InterfaceLanguage"), _preferences.InterfaceLanguage);
                }

                if (!string.IsNullOrEmpty(_controlData.ProductID))
                {
                    ub.Append(UrlBuilder.GetParameterName(typeof (SessionRequestDTO), "ProductID"), _controlData.ProductID);
                }

                if (!string.IsNullOrEmpty(_controlData.SessionID))
                {
                    ub.Append(UrlBuilder.GetParameterName(typeof (SessionRequestDTO), "SessionID"), _controlData.SessionID);
                }
                else if (!string.IsNullOrEmpty(_controlData.EncryptedToken)) // assume this is a lightweight user
                {
                    ub.Append(UrlBuilder.GetParameterName(typeof (SessionRequestDTO), "EncryptedToken"), _controlData.EncryptedToken);
                }
                else
                {
                    ub.Append(UrlBuilder.GetParameterName(typeof (SessionRequestDTO), "UserID"), _controlData.UserID);
                    ub.Append(UrlBuilder.GetParameterName(typeof (SessionRequestDTO), "Password"), _controlData.UserPassword);
                }

                if (!string.IsNullOrEmpty(_controlData.AccessPointCodeUsage))
                {
                    ub.Append(UrlBuilder.GetParameterName(typeof (SessionRequestDTO), "AccessPointCodeUsage"), _controlData.AccessPointCodeUsage);
                }

                if (!string.IsNullOrEmpty(_controlData.ClientCode))
                {
                    ub.Append(UrlBuilder.GetParameterName(typeof (SessionRequestDTO), "ClientCodeType"), _controlData.ClientCode);
                }
                return ub.ToString();
            }

            return null;
        }

        /// <summary>
        ///   The process content.
        /// </summary>
        /// <param name = "arrAccessionNumbers">
        ///   The arr accession numbers.
        /// </param>
        /// <param name = "articleResult"></param>
        private void ProcessContent(IEnumerable<string> arrAccessionNumbers, ArticleResultset articleResult)
        {
            var requestDTO = new AccessionNumberSearchRequestDTO
                                 {
                                     SortBy = SortBy.FIFO,
                                     MetaDataController =
                                         {
                                             Mode = CodeNavigatorMode.None
                                         },
                                     DescriptorControl =
                                         {
                                             Mode = DescriptorControlMode.None,
                                             Language = "en"
                                         }, AccessionNumbers = arrAccessionNumbers.ToArray()
                                 };
            requestDTO.MetaDataController.ReturnCollectionCounts = false;
            requestDTO.MetaDataController.ReturnKeywordsSet = false;
            requestDTO.MetaDataController.TimeNavigatorMode = TimeNavigatorMode.None;      
            // add all the search collections to the search.
            requestDTO.SearchCollectionCollection.AddRange(Enum.GetValues(typeof(SearchCollection)).Cast<SearchCollection>()); 
            
            var response = _manager.PerformAccessionNumberSearch<PerformContentSearchRequest, PerformContentSearchResponse>(requestDTO);
            foreach (var contentItem in  response.AccessionNumberBasedContentItemSet.AccessionNumberBasedContentItemCollection)
            {
                if (contentItem != null && contentItem.HasBeenFound && contentItem.ContentHeadline != null)
                {
                    var text = GetText(contentItem);
                    var found = false;
                    var numberBasedContentItem = contentItem;
                    foreach (var curContentItem in from item 
                                       in contentItem.ContentHeadline.ContentItems.ItemCollection
                                       where !string.IsNullOrEmpty(item.Mimetype)
                                       select item)
                    {
                        // replace the accession number placeholders
                        var strHref = GetHandlerUrl( ImageType.Display, numberBasedContentItem.AccessionNumber, curContentItem ); 
                        var type = contentItem.ContentHeadline.ContentItems.ContentType;
                        switch (type)
                        {
                            case "picture":
                            case "file":
                            case "pdf":
                            case "summary":
                                var accessionNumberBasedContentItem = contentItem;
                                foreach (var item in from dc in articleResult.LeadParagraph
                                                     from kvp in dc
                                                     from ritem in kvp.Value
                                                     where ritem.ItemText != null && ritem.ItemText.Equals("##" + accessionNumberBasedContentItem.AccessionNumber + "##")
                                                     select ritem)
                                {
                                    item.ItemMarkUp = Map(curContentItem.Mimetype);
                                    item.ItemText = text;
                                    item.ItemValue = strHref;
                                    if(EmbedHtmlBasedExternalLinks && item.ItemMarkUp == MarkUpType.Html )
                                    {
                                        UpdateItem( item, curContentItem, numberBasedContentItem.AccessionNumber );
                                    }
                                }

                                // scan the entire document.
                                if (!articleResult.TailParagraphs.IsNullOrEmpty())
                                {                           

                                    foreach (var item in from dc in articleResult.TailParagraphs
                                                         from kvp in dc
                                                         from ritem in kvp.Value
                                                         where ritem.ItemText != null && ritem.ItemText.Equals("##" + accessionNumberBasedContentItem.AccessionNumber + "##")
                                                         select ritem)
                                    {
                                        item.ItemMarkUp = Map(curContentItem.Mimetype);
                                        item.ItemText = text;
                                        item.ItemValue = strHref;
                                        if (EmbedHtmlBasedExternalLinks && item.ItemMarkUp == MarkUpType.Html)
                                        {
                                            UpdateItem( item, curContentItem, numberBasedContentItem.AccessionNumber);
                                        }
                                    }
                                }

                                found = true;
                                break;
                        }
                    }

                    if (!found)
                    {
                        var accessionNumberBasedContentItem = contentItem;
                        foreach (var item in from dc in articleResult.LeadParagraph
                                             from kvp in dc
                                             from item in kvp.Value
                                             where item.ItemText.Equals("##" + accessionNumberBasedContentItem.AccessionNumber + "##")
                                             select item)
                        {
                            item.ItemMarkUp = MarkUpType.Anchor;
                            item.ItemText = text;
                            item.ItemValue = contentItem.AccessionNumber;
                        }
                    }
                }
                else if (contentItem != null)
                {
                    var text = GetText(contentItem);
                    var accessionNumberBasedContentItem = contentItem;
                    foreach (var item in articleResult.LeadParagraph
                        .SelectMany(dc => dc
                                              .SelectMany(kvp => kvp.Value
                                                                    .Where(item => item.ItemText.Equals("##" + accessionNumberBasedContentItem.AccessionNumber + "##")))))
                    {
                        item.ItemMarkUp = MarkUpType.Anchor;
                        item.ItemText = text;
                        item.ItemValue = contentItem.AccessionNumber;
                    }
                }
            }
        }

        private void UpdateItem(IRenderItem item, ContentItem contentItem, string accessionNumber)
        {
            var request = new GetBinaryRequest
                              {
                                  mimeType = contentItem.Mimetype,
                                  reference = contentItem.Ref,
                                  accessionNumber = accessionNumber,
                                  imageType = "dispix",
                                  usageAggregator = "",
                              };
            try
            {
                var binaryResponse = _articleService.GetBinary(request);  
                Encoding encoding = new UTF8Encoding();
                var tempStr = encoding.GetString(binaryResponse.binaryData, 0, 8);  
                if (tempStr.IsNumeric())
                {
                    item.ItemMarkUp = MarkUpType.Anchor;
                }             
                item.ItemText = ExtractHtml(encoding.GetString(binaryResponse.binaryData));  
            }
            catch(Exception)
            {
                item.ItemMarkUp = MarkUpType.Anchor;
            }    
        }

        private static string ExtractHtml(string html)
        {                          
            using( var tr = new StringReader( html ) )
            {
                // setup SGMLReader
                var sgmlReader = new SgmlReader
                                     {
                                         DocType = "HTML",
                                         WhitespaceHandling = WhitespaceHandling.All,
                                         CaseFolding = CaseFolding.ToLower,
                                         IgnoreDtd = true,
                                         InputStream = tr,
                                     };

                var doc = new XmlDocument
                              {
                                  PreserveWhitespace = true, 
                                  XmlResolver = null
                              };
                doc.Load(sgmlReader);

                var t  = doc.GetElementsByTagName("body");
                if (t.Count > 0)
                {
                    return t[0].InnerXml;
                }
            }
            return string.Empty;
        }

        private static MarkUpType Map(string mimeType)
        {
            switch( mimeType )
            {
                case "image/gif":
                case "image/jpeg":
                case "image/png":           
                    return MarkUpType.Image;            
                case "text/html":
                    return MarkUpType.Html;
                case "application/msexcel":
                case "application/msword":
                case "application/mspowerpoint":
                case "application/pdf":
                    return MarkUpType.Anchor;
                default:
                    return MarkUpType.Anchor;
            }
        }

        /// <summary>
        ///   The get text.
        /// </summary>
        /// <param name = "contentItem">The an content item.</param>
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
        ///   The get handler url.
        /// </summary>
        /// <param name = "imageType">The image type.</param>
        /// <param name = "accessionNo">The accession no.</param>
        /// <param name = "contentItem">The content item.</param>
        /// <returns>A string representing the Handerl Url</returns>
        private string GetHandlerUrl(ImageType imageType, string accessionNo, ContentItem contentItem)
        {
            var reference = contentItem.Ref;
            var mimeType = contentItem.Mimetype;

            if (FileHandlerUrl.HasValue())
            {
                var ub = new UrlBuilder(FileHandlerUrl);
                ub.Append(UrlBuilder.GetParameterName(typeof (ArchiveFileRequestDTO), "AccessionNumber"), accessionNo);
                ub.Append(UrlBuilder.GetParameterName(typeof (ArchiveFileRequestDTO), "Reference"), reference);
                ub.Append(UrlBuilder.GetParameterName(typeof (ArchiveFileRequestDTO), "MimeType"), mimeType);
                ub.Append(UrlBuilder.GetParameterName(typeof (ArchiveFileRequestDTO), "ImageType"), (imageType == ImageType.Display) ? "dispix" : "tnail");

                if (!string.IsNullOrEmpty(_controlData.AccessPointCode))
                {
                    ub.Append(UrlBuilder.GetParameterName(typeof (SessionRequestDTO), "AccessPointCode"), _controlData.AccessPointCode);
                }

                if (!string.IsNullOrEmpty(_preferences.InterfaceLanguage))
                {
                    ub.Append(UrlBuilder.GetParameterName(typeof (SessionRequestDTO), "InterfaceLanguage"), _preferences.InterfaceLanguage);
                }

                if (!string.IsNullOrEmpty(_controlData.ProductID))
                {
                    ub.Append(UrlBuilder.GetParameterName(typeof (SessionRequestDTO), "ProductID"), _controlData.ProductID);
                }

                if (!string.IsNullOrEmpty(_controlData.SessionID))
                {
                    ub.Append(UrlBuilder.GetParameterName(typeof (SessionRequestDTO), "SessionID"), _controlData.SessionID);
                }
                else if (!string.IsNullOrEmpty(_controlData.EncryptedToken)) // assume this is a lightweight user
                {
                    ub.Append(UrlBuilder.GetParameterName(typeof (SessionRequestDTO), "EncryptedToken"), _controlData.EncryptedToken);
                }
                else
                {
                    ub.Append(UrlBuilder.GetParameterName(typeof (SessionRequestDTO), "UserID"), _controlData.UserID);
                    ub.Append(UrlBuilder.GetParameterName(typeof (SessionRequestDTO), "Password"), _controlData.UserPassword);
                }

                if (!string.IsNullOrEmpty(_controlData.AccessPointCodeUsage))
                {
                    ub.Append(UrlBuilder.GetParameterName(typeof (SessionRequestDTO), "AccessPointCodeUsage"), _controlData.AccessPointCodeUsage);
                }

                if (!string.IsNullOrEmpty(_controlData.CacheKey))
                {
                    ub.Append(UrlBuilder.GetParameterName(typeof (SessionRequestDTO), "CacheKey"), _controlData.CacheKey);
                }

                if (!string.IsNullOrEmpty(_controlData.ClientCode))
                {
                    ub.Append(UrlBuilder.GetParameterName(typeof (SessionRequestDTO), "ClientCodeType"), _controlData.ClientCode);
                }

                return ub.ToString();
            }
            return null;
        }


        public static ContentSubCategory MapContentSubCategory(Article article)
        {
            if (article == null || article.contentParts == null || article.contentParts.contentType == null)
            {
                return ContentSubCategory.UnSpecified;
            }

            switch (article.contentParts.contentType.ToLower())
            {
                case "analyst":
                    return ContentSubCategory.Analyst;
                case "pdf":
                    return ContentSubCategory.PDF;
                case "webpage":
                    return ContentSubCategory.WebPage;
                case "file":
                    return ContentSubCategory.HTML;
                case "articlewithgraphics":
                case "picture":
                    return ContentSubCategory.Graphic;
                case "multimedia":
                    foreach (Part item in article.contentParts.parts)
                    {
                        switch (item.type.ToLower())
                        {
                            case "audio":
                                return ContentSubCategory.Audio;
                            case "video":
                                return ContentSubCategory.Video;
                        }
                    }
                    return ContentSubCategory.Multimedia;
                case "article":
                    return ContentSubCategory.Article;
                case "summary":
                    return ContentSubCategory.Summary;
                case "board":
                    return ContentSubCategory.Board;
                case "blog":
                    return ContentSubCategory.Blog;
                case "customerdoc":
                    return ContentSubCategory.CustomerDoc;
            }

            return ContentSubCategory.UnSpecified;
        }

        /// <summary>
        ///   Gets the type of the original content.
        /// </summary>
        /// <param name = "article">The article.</param>
        /// <returns></returns>
        public static string GetOriginalContentType(Article article)
        {
            if (article == null || article.contentParts == null || article.contentParts.contentType == null)
            {
                return string.Empty;
            }

            return article.contentParts.contentType.ToLower();
        }

        protected internal string GetReferenceUrl(ContentCategory contentCategory, Article article)
        {
            if (article == null || article.contentParts == null || article.contentParts.parts == null)
            {
                return null;
            }

            // look through based on type and provide the correct Uri
            switch (contentCategory)
            {
                case ContentCategory.Blog:
                case ContentCategory.Board:
                case ContentCategory.CustomerDoc:
                case ContentCategory.Internal:
                    foreach (var item in article.contentParts.parts.Where(item => item.type.ToLower() == "webpage"))
                    {
                        return item.reference;
                    }

                    if (article.contentParts.primaryReference.HasValue())
                    {
                        return article.contentParts.primaryReference;
                    }

                    break;
            }

            return string.Empty;
        }

        /// <summary>
        ///   Maps the type of the content.
        /// </summary>
        /// <param name = "article">The article.</param>
        /// <returns></returns>
        public static ContentCategory MapContentCategory(Article article)
        {
            if (article == null || article.contentParts == null || article.contentParts.contentType == null)
            {
                return ContentCategory.UnSpecified;
            }

            switch (article.contentParts.contentType.ToLower())
            {
                case "webpage":
                    return ContentCategory.Website;
                case "file":
                case "article":
                case "pdf":
                case "analyst":
                case "html":
                case "articlewithgraphics":
                    return ContentCategory.Publication;
                case "picture":
                    return ContentCategory.Picture;
                case "multimedia":
                    return ContentCategory.Multimedia;
                case "internal":
                    return ContentCategory.Internal;
                case "summary":
                    return ContentCategory.Summary;
                case "board":
                    return ContentCategory.Board;
                case "blog":
                    return ContentCategory.Blog;
                case "customerdoc":
                    return ContentCategory.CustomerDoc;
                default:
                    return ContentCategory.External;
            }
        }

        #region public helper method

        /// <summary>
        ///   The get paragraph text.
        /// </summary>
        /// <param name = "paragraphs">The paragraphs.</param>
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
        ///   The get object text.
        /// </summary>
        /// <param name = "items">The items.</param>
        /// <returns>
        ///   The string of the text extracted from the items.
        /// </returns>
        public static string GetObjectText(object[] items)
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
                    var highlight = (HighlightedText) item;
                    if (highlight.text != null && highlight.text.Value != null)
                    {
                        sb.Append(highlight.text.Value.Replace("<", "&lt;").Replace(">", "&gt;"));
                    }
                }
                else if (item is ELink)
                {
                    var elink = (ELink) item;
                    if (elink.text != null && elink.text.Value != null)
                    {
                        sb.Append(elink.text.Value);
                    }
                }
                else if (item is EntityReference)
                {
                    var entityReference = (EntityReference) item;

                    sb.Append(GetEntityReferenceText(entityReference.Items));
                }
                else
                {
                    var text = (Text) item;
                    if (text.Value != null)
                    {
                        sb.Append(text.Value.Replace("<", "&lt;").Replace(">", "&gt;"));
                    }
                }
            }

            return sb.ToString();
        }

        /// <summary>
        ///   The get entity reference text.
        /// </summary>
        /// <param name = "items">The items.</param>
        /// <returns>
        ///   The string representing the entity reference text.
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
                    var erefHighlight = ((HighlightedText) item).text;
                    if (erefHighlight != null && erefHighlight.Value != null)
                    {
                        sb.Append(erefHighlight.Value.Replace("<", "&lt;").Replace(">", "&gt;"));
                    }
                }
                else if (item is Text)
                {
                    var erefText = (Text) item;
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
    }
}