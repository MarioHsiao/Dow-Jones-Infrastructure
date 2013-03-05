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
using Para = Factiva.Gateway.Messages.Search.V2_0.Markup;
using PerformContentSearchRequest = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchRequest;
using PerformContentSearchResponse = Factiva.Gateway.Messages.Search.FreeSearch.V1_0.PerformContentSearchResponse;

namespace DowJones.Assemblers.Articles
{
    public class ArticleConversionManager
    {
        public const string DefaultFileHandlerUrl = "~/DowJones.Web.Handlers.Article.ContentHandler.ashx";
        internal readonly string LogoSiteUrl = Properties.Settings.Default.LogosSiteUrl;
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
            ShowSourceLogo = true;
            EmbededImageType = ImageType.Display;
        }

        public bool ShowCompanyEntityReference { get; set; }

        public bool ShowExecutiveEntityReference { get; set; }

        public bool ShowSourceLogo { get; set; }

        public bool ShowImagesAsFigures { get; set; }

        public bool EnableELinks { get; set; }

        public bool EnableEnlargedImage { get; set; }

        public bool EmbedHtmlBasedExternalLinks { get; set; }

        public bool EmbedHtmlBasedArticles { get; set; }

        public bool SuppressLinksInHeadlineTitle { get; set; }

        public ImageType EmbededImageType{ get; set; }
        
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
                                        Html = ProcessHtmlSection(article),
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
                                        Copyright = article.copyright != null ? GetRenderItems(article.copyright.Items, article.accessionNo) : new List<RenderItem>(),
                                        ContentCategory = MapContentCategory(article),
                                        ContentSubCategory = MapContentSubCategory(article),
                                        OriginalContentCategory = GetOriginalContentType(article)
                                    };

            // Map a couple of descriptors
            articleResult.ContentCategoryDescriptor = articleResult.ContentCategory.ToString();
            articleResult.ContentSubCategoryDescriptor = articleResult.ContentSubCategory.ToString();
            articleResult.Headline = ProcessHeadline(article, articleResult.OriginalContentCategory, articleResult.ExternalUri);
            
            //To set the MimeType and Ref
            MapExtraReferenceInformation(articleResult, article);

            // To generate a possible External Url
            articleResult.ExternalUri = GetExternalUrl(articleResult, article);

            if (!CheckCodeSn(Codes.PD.ToString()))
            {
                articleResult.PublicationDate = GetDate(article.publicationDate, article.publicationTime, article.publicationTimeSpecified);
            }

            if (!CheckCodeSn(Codes.ET.ToString()) && article.publicationTime > DateTime.MinValue) 
            {
                articleResult.PublicationTime = GetTime(article.publicationDate, article.publicationTime);
            }

            articleResult.ModificationDate = GetDate(article.modDate, article.modTime, true);
            articleResult.ModificationTime = GetTime(article.modDate, article.modTime);

            if (!string.IsNullOrEmpty(article.baseLanguage))
            {
                articleResult.Language = GetLanguageToContentLanguage(article.baseLanguage);
                articleResult.LanguageCode = article.baseLanguage;
            }

            if (article.contact != null && article.contact.Items != null)
            {
                articleResult.Contact = GetRenderItems(article.contact.Items, article.accessionNo);
            }

            if (article.credit != null && article.credit.Items != null)
            {
                articleResult.Credit = GetRenderItems(article.credit.Items, article.accessionNo);
            }

            if (article.sectionName != null && article.sectionName.Items != null)
            {
                articleResult.SectionName = GetRenderItems(article.sectionName.Items, article.accessionNo);
            }

            if (article.columnName != null && article.columnName.Items != null)
            {
                articleResult.ColumnName = GetRenderItems(article.columnName.Items, article.accessionNo);
            }

            if (article.notes != null && article.notes.Items != null)
            {
                var dictionaryItem = new Dictionary<string, List<RenderItem>>
                                         {
                                             {
                                                 "display", 
                                                 GetRenderItems(article.notes.Items, article.accessionNo)
                                             }
                                         };

                var listBody = new List<Dictionary<string, List<RenderItem>>>
                                   {
                                       dictionaryItem
                                   };
                
                articleResult.Notes = listBody;

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
                                          }).ToList();
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
                                foreach (var code in set.code.Where(code => !code.cat.IsNullOrEmpty()))
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
        private static IEnumerable<string> ReturnAnArray(ICollection<ElinkReferences> notProcessedRefAccessionNumbers)
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
        /// Parses the markup.
        /// </summary>
        /// <param name="paras">The paras.</param>
        /// <returns></returns>
        private static string ParseMarkup(IEnumerable<XmlNode> paras)
        {
            var sb = new StringBuilder();
            if (paras == null)
            {
                return null;
            }

            foreach (var node in from para in paras where para != null && para.HasChildNodes from XmlNode node in para.ChildNodes select node)
            {
                switch (node.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (node.Name)
                        {
                            case "hlt":
                                sb.Append(ReplaceSpecialChars(node.InnerText));
                                break;
                            case "en":
                                if (node.Attributes != null)
                                {
                                    sb.Append(ReplaceSpecialChars(node.InnerText));
                                }
                                break;
                        }

                        break;
                    case XmlNodeType.Text:
                        sb.Append(ReplaceSpecialChars(node.InnerText));

                        break;
                }

                if (sb.Length <= 0)
                {
                }
            }

            return sb.ToString();
        }

        protected static string ReplaceSpecialChars(string s)
        {
            /*StringBuilder sb = new StringBuilder(s);
            sb.Replace("\u2028;", "")
                .Replace("\u2029;", "")
                .Replace("\u2019;", "'");*/
            return string.IsNullOrEmpty(s) ? s : s.Trim();
        }

        private List<Dictionary<string, List<RenderItem>>> ProcessParagraphs(Article article, IEnumerable paras)
        {
            var listBody = new List<Dictionary<string, List<RenderItem>>>();
            if (paras == null)
            {
                return null;
            }
            foreach (Paragraph para in paras)
            {
                var dictionaryItem = new Dictionary<string, List<RenderItem>>();
                var tempItem = GetRenderItems(para.Items, article.accessionNo);
                dictionaryItem.Add(ParagraphDisplay.Proportional == para.display ? "display" : "pre", tempItem);
                listBody.Add(dictionaryItem);
            }
            return listBody;
        }

        private List<RenderItem> ProcessHeadline(Article article, string contentType, string externalUri)
        {
            var tempItem = new List<RenderItem>();
            var headlineText = GetParagraphText(article.headline);
            switch (contentType.ToLowerInvariant())
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
                    // SuppressLinksInHeadlineTitle is used to fix a PM issue on 4/13/12 --dacostad
                    if (SuppressLinksInHeadlineTitle && contentType.ToLowerInvariant() == "html")
                    {
                        tempItem.Add(new RenderItem
                        {
                            ItemMarkUp = MarkUpType.Plain,
                            ItemText = headlineText,
                        });
                    }
                    else
                    {
                        tempItem.Add(new RenderItem
                                         {
                                             ItemMarkUp = MarkUpType.Anchor,
                                             ItemValue = externalUri,
                                             ItemText = headlineText,
                                         });
                    }
                    break;
            }
            return tempItem;
        }

        private static List<RenderItem> ProcessBylineSection(Article article)
        {
            var tempItem = new List<RenderItem>();
            if (article.byline != null)
            {
                foreach (var item in article.byline.Items)
                {
                    try
                    {
                        if (item is Text)
                        {
                            var text = (Text)item;
                            tempItem.Add(new RenderItem
                            {
                                ItemMarkUp = MarkUpType.Plain,
                                ItemText = text.Value.Replace("By ", String.Empty)
                            });
                        }
                        else if (item is HighlightedText)
                        {
                            var text = (HighlightedText)item;
                            tempItem.Add(new RenderItem
                            {
                                ItemMarkUp = MarkUpType.ArticleHighlight,
                                ItemText = text.text.Value.Replace("By ", String.Empty)
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                }
            }
            return tempItem;
        }

        private static List<RenderItem> ProcessSourceSection(Article article)
        {
            var tempItem = new List<RenderItem>();
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

        private List<RenderItem> ProcessHtmlSection(Article article)
        {
            var renderItems = new List<RenderItem>();
            if (EmbedHtmlBasedArticles && article.contentParts.contentType.ToLower() == "html")
            {
                // look at the conntent items
                var tempRenderItem = GetHtmlRenderItem( article.contentParts.parts, article.accessionNo );
                if (tempRenderItem != null)
                {
                    renderItems.Add(tempRenderItem);
                }                                   
            }

            return renderItems;
        }

        private List<RenderItem> ProcessHeadSection(Article article)
        {
            string logo;
            var renderItems = new List<RenderItem>();
            if (article.sourceLogo != null && article.sourceLogo.image != null)
            {
                logo = article.sourceLogo.image;
            }
            else
            {
                logo = !String.IsNullOrEmpty(article.sourceCode) ? String.Format("{0}Logo.gif", article.sourceCode) : null;
            }

            if (!string.IsNullOrEmpty(logo) && ShowSourceLogo)
            {
                if ((_postProcessing == PostProcessing.UnSpecified) ||
                    (_postProcessing != PostProcessing.RTF && _postProcessing != PostProcessing.Save))
                {
                    var urlBuilder = new UrlBuilder(LogoSiteUrl + logo);
                    renderItems.Add(new RenderItem
                                     {
                                         ItemMarkUp = MarkUpType.HeadLogo,
                                         ItemValue = urlBuilder.ToString(),
                                         ItemText = article.sourceCode + " Logo"
                                     });
                }
            }

            if (article.contentParts != null && article.contentParts.contentType.IsNotEmpty())
            {
                if (article.contentParts.contentType.ToLower() == "picture")
                {
                    renderItems.Add(new RenderItem
                                     {
                                         ItemMarkUp = MarkUpType.HeadImageLarge,
                                         ItemValue = GetImageUrl(ImageType.Display,
                                                                 article.accessionNo,
                                                                 article.contentParts.parts)
                                     });
                    renderItems.Add(new RenderItem
                                 {
                                     ItemMarkUp = MarkUpType.HeadImageSmall,
                                     ItemValue = GetImageUrl(ImageType.Thumbnail,
                                                             article.accessionNo,
                                                             article.contentParts.parts)
                                 });
                    renderItems.Add(new RenderItem
                                 {
                                     ItemMarkUp = MarkUpType.HeadImageXSmall,
                                     ItemValue = GetImageUrl(ImageType.Fingernail,
                                                             article.accessionNo,
                                                             article.contentParts.parts)
                                 });
                }
            }

            return renderItems;
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
            var accessNos = ReturnAnArray(_unProcessedRefAccessionNumbers);
            ProcessContent(accessNos, articleResult);
        }

        /// <summary>
        ///   The render items.
        /// </summary>
        /// <param name = "items">
        ///   The items.
        /// </param>
        /// <param name = "accessionNumber"></param>
        private List<RenderItem> GetRenderItems(IEnumerable<object> items, string accessionNumber)
        {
            var renderItems = new List<RenderItem>();
            foreach (var item in items)
            {
                var highlightedText = item as HighlightedText;
                if (highlightedText != null )
                {
                    if (highlightedText.text != null && highlightedText.text.Value != null)
                    {
                        renderItems.Add(new RenderItem { ItemMarkUp = MarkUpType.ArticleHighlight, ItemText = highlightedText.text.Value });
                    }
                }
                else
                {
                    var entityLink = item as ELink;
                    if (entityLink != null)
                    {
                        RenderEntityLink(entityLink, renderItems, accessionNumber);
                    }
                    else
                    {
                        var entityReference = item as EntityReference;
                        if (entityReference != null)
                        {
                            if (entityReference.Items != null && entityReference.Items.Length > 0)
                            {
                                var len = entityReference.Items.Count();
                                var name = string.Join(string.Empty, entityReference.Items);
                                for (var i = 0; i < len; i++)
                                {
                                    var entityText = entityReference.Items[i];
                                    var entityElementName = entityReference.entityElementName[i];

                                    if (ShowCompanyEntityReference || ShowExecutiveEntityReference)
                                    {
                                        if (ShowCompanyEntityReference && entityReference.category.Equals(Codes.co.ToString(), StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            if ((_postProcessing == PostProcessing.UnSpecified) && ShowCompanyEntityReference)
                                            {
                                                BuildEntityLink(name, entityReference.code, Category.company.ToString(), entityText, renderItems, entityElementName == EntityReferenceType.hlt);
                                            }
                                            else
                                            {
                                                renderItems.Add(new RenderItem
                                                                    {
                                                                        ItemMarkUp = (entityElementName == EntityReferenceType.hlt) ? MarkUpType.ArticleHighlight : MarkUpType.Span,
                                                                        ItemText = entityText,
                                                                    });
                                            }
                                        }
                                        else if (ShowExecutiveEntityReference && entityReference.category.Equals(Codes.pe.ToString(), StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            if ((_postProcessing == PostProcessing.UnSpecified) && ShowCompanyEntityReference)
                                            {
                                                BuildEntityLink(name, entityReference.code, Category.executive.ToString(), entityText, renderItems, entityElementName == EntityReferenceType.hlt);
                                            }
                                            else
                                            {
                                                renderItems.Add(new RenderItem
                                                                    {
                                                                        ItemMarkUp = (entityElementName == EntityReferenceType.hlt) ? MarkUpType.ArticleHighlight : MarkUpType.Span,
                                                                        ItemText = entityText,
                                                                    });
                                            }
                                        }
                                        else
                                        {
                                            renderItems.Add(new RenderItem
                                                                {
                                                                    ItemMarkUp = MarkUpType.Plain, 
                                                                    ItemText = entityText,
                                                                });
                                        }
                                    }
                                    else
                                    {
                                        renderItems.Add(new RenderItem
                                                            {
                                                                ItemMarkUp = MarkUpType.Plain,
                                                                ItemText = entityText,
                                                            });
                                    }    
                                }
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
                                                        ItemText = text.Value
                                                    });
                            }
                        }
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
        private void RenderEntityLink(ELink elink, ICollection<RenderItem> renderItems, string accessionNumber)
        {
            var elinkItems = GetElinkItems(elink);

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
                            renderItems.Add(new RenderItem { ItemMarkUp = MarkUpType.ArticleElink, ItemText = elink.reference, ItemValue = elink.reference, ElinkItems = elinkItems });
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
        /// Get Elink Items
        /// </summary>
        /// <param name="elink"></param>
        /// <returns></returns>
        private static List<RenderElinkItem> GetElinkItems(ELink elink)
        {
            var elinkItems = new List<RenderElinkItem>();
            if (!elink.Items.IsNullOrEmpty())
            {
                foreach (var eLinkItem in elink.Items)
                {
                    var e = eLinkItem as HighlightedText;
                    if (e != null)
                    {
                        var heText = e;
                        if (heText.text != null)
                        {
                            elinkItems.Add(new RenderElinkItem
                                               {
                                                   ItemMarkUp = MarkUpType.ArticleElinkHighlight,
                                                   ItemText = heText.text.Value
                                               });
                        }
                    }
                    else
                    {
                        var eText = (Text) eLinkItem;
                        if (eText.Value != null)
                        {
                            elinkItems.Add(new RenderElinkItem
                                               {
                                                   ItemMarkUp = MarkUpType.Plain,
                                                   ItemText = eText.Value
                                               });
                        }
                    }
                }
            }
            return elinkItems;
        }

        /// <summary>
        /// The build entity link.
        /// </summary>
        /// <param name="name">The name of the entitly link.</param>
        /// <param name="code">The code of the entitly link.</param>
        /// <param name="category">The category.</param>
        /// <param name="text">The text.</param>
        /// <param name="renderItem">The render item.</param>
        /// <param name="highlight">if set to <c>true</c> [highlight].</param>
        private static void BuildEntityLink(string name, string code, string category, string text, ICollection<RenderItem> renderItem, bool highlight)
        {
            renderItem.Add(new RenderItem
                               {
                                   ItemClass = "dj_article_entity", 
                                   ItemMarkUp = MarkUpType.EntityLink, 
                                   ItemText = text,
                                   ItemEntityData = new EntityLinkData
                                                        {
                                                            Code = code ?? string.Empty, 
                                                            Name = name ?? string.Empty, 
                                                            Category = category  
                                                        },
                                   Highlight = highlight
                               });
        }

        private static void BuildSpanLink(string name, string code, string category, ICollection<RenderItem> renderItem)
        {
            renderItem.Add(new RenderItem {ItemClass = "dj_article_entity", ItemMarkUp = MarkUpType.SpanAnchor, ItemEntityData = new EntityLinkData {Code = code ?? string.Empty, Name = name ?? string.Empty, Category = category}});
        }

        private static bool CheckCodeSn(string code)
        {
            return code.Equals(Codes.SN.ToString(), StringComparison.InvariantCultureIgnoreCase) &&
                   Equals(PostProcessing.Print, StringComparison.InvariantCultureIgnoreCase);
        }
        
        private string GetDate(DateTime publicationDate, DateTime publicationTime, bool publicationTimeSpecified)
        {
            var tempararyPublicationDate = publicationDate;
            if (publicationTimeSpecified)
                tempararyPublicationDate = DateTimeFormatter.Merge(publicationDate, publicationTime);

            return _dateTimeFormatter.FormatDate(tempararyPublicationDate);
        }

        private string GetTime(DateTime publicationDate, DateTime publicationTime)
        {
            return _dateTimeFormatter.FormatTime(DateTimeFormatter.Merge(publicationDate, publicationTime));
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
                foreach (var part in parts.Where(part => part.type.ToLower() == imageType.ToString().ToLower()))
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
                AddControlData(ub);
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
            var requestDto = new AccessionNumberSearchRequestDTO
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
            requestDto.MetaDataController.ReturnCollectionCounts = false;
            requestDto.MetaDataController.ReturnKeywordsSet = false;
            requestDto.MetaDataController.TimeNavigatorMode = TimeNavigatorMode.None;      
            // add all the search collections to the search.
            requestDto.SearchCollectionCollection.Clear();
            requestDto.SearchCollectionCollection.AddRange(Enum.GetValues(typeof(SearchCollection)).Cast<SearchCollection>()); 
            
            var response = _manager.PerformAccessionNumberSearch<PerformContentSearchRequest, PerformContentSearchResponse>(requestDto);
            foreach (var contentItem in  response.AccessionNumberBasedContentItemSet.AccessionNumberBasedContentItemCollection)
            {
                if (contentItem != null && contentItem.HasBeenFound && contentItem.ContentHeadline != null)
                {
                    var text = GetText(contentItem);
                    var found = false;
                    var numberBasedContentItem = contentItem;
                    var tItems = new List<ContentItem>();
                    var enlargedImgHandlerUrl = string.Empty;
                    switch(contentItem.ContentHeadline.ContentItems.ContentType)
                    {
                        case "picture":
                            tItems = contentItem.ContentHeadline.ContentItems.ItemCollection
                                .Where(tItem => !string.IsNullOrEmpty(tItem.Mimetype) && tItem.Type.ToLowerInvariant() == Map(EmbededImageType)).ToList();
                        
                            if (tItems.Count == 0)
                            {
                                tItems = contentItem.ContentHeadline.ContentItems.ItemCollection
                                        .Where( tItem => !string.IsNullOrEmpty( tItem.Mimetype ) ).ToList();    
                            }
                            if (EnableEnlargedImage && EmbededImageType != ImageType.Display)
                            {
                                var enlargedImageContentItem = contentItem.ContentHeadline.ContentItems.ItemCollection.FirstOrDefault(tItem => !string.IsNullOrEmpty(tItem.Mimetype) && tItem.Type.ToLowerInvariant() == Map(ImageType.Display));
                                if (enlargedImageContentItem != null)
                                {
                                    enlargedImgHandlerUrl = GetHandlerUrl(EmbededImageType, numberBasedContentItem.AccessionNumber, enlargedImageContentItem);
                                }
                            }
                            break; 
                        case "file":
                        case "pdf":
                        case "summary":
                            tItems = contentItem.ContentHeadline.ContentItems.ItemCollection
                                        .Where( tItem => !string.IsNullOrEmpty( tItem.Mimetype ) ).ToList();
                            break;
                    }   

                    foreach( var curContentItem in tItems)
                    {
                        var strHref = GetHandlerUrl(EmbededImageType, numberBasedContentItem.AccessionNumber, curContentItem);
                        var accessionNumberBasedContentItem = contentItem;
                       
                        // scan the entire document.
                        if (!articleResult.LeadParagraph.IsNullOrEmpty())
                        {
                            foreach (var item in from dc in articleResult.LeadParagraph
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
                                    UpdateItem(item, curContentItem, numberBasedContentItem.AccessionNumber);
                                }

                                if (item.ItemMarkUp != MarkUpType.Image || !ShowImagesAsFigures) continue;
                                item.ItemMarkUp = MarkUpType.ImageFigure;
                                item.Title = ParseMarkup(contentItem.ContentHeadline.Headline.Any);
                                item.Credit = ParseMarkup(contentItem.ContentHeadline.Credit.Any);
                                item.Caption = ParseMarkup(contentItem.ContentHeadline.Snippet.Any);
                                item.Source = ParseMarkup(contentItem.ContentHeadline.Byline.Any);
                                item.EnlargedImageUrl = enlargedImgHandlerUrl;

                                found = true;
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
                                    UpdateItem(item, curContentItem, numberBasedContentItem.AccessionNumber);
                                   
                                }

                                if (item.ItemMarkUp != MarkUpType.Image || !ShowImagesAsFigures) continue;
                                item.ItemMarkUp = MarkUpType.ImageFigure;
                                item.Title = ParseMarkup(contentItem.ContentHeadline.Headline.Any);
                                item.Credit = ParseMarkup(contentItem.ContentHeadline.Credit.Any);
                                item.Caption = ParseMarkup(contentItem.ContentHeadline.Snippet.Any);
                                item.Source = ParseMarkup(contentItem.ContentHeadline.Byline.Any);
                                item.EnlargedImageUrl = enlargedImgHandlerUrl;
                                found = true;
                            }
                        }

                        // scan the entire document.
                        if (!articleResult.Notes.IsNullOrEmpty())
                        {

                            foreach (var item in from dc in articleResult.Notes
                                                 from kvp in dc
                                                 from item in kvp.Value
                                                 where item.ItemText != null && item.ItemText.Equals("##" + accessionNumberBasedContentItem.AccessionNumber + "##")
                                                 select item)
                            {
                                item.ItemMarkUp = Map(curContentItem.Mimetype);
                                item.ItemText = text;
                                item.ItemValue = strHref;
                                
                                if (EmbedHtmlBasedExternalLinks && item.ItemMarkUp == MarkUpType.Html)
                                {
                                    UpdateItem(item, curContentItem, numberBasedContentItem.AccessionNumber);
                                   
                                }

                                if (item.ItemMarkUp != MarkUpType.Image || !ShowImagesAsFigures) continue;
                                item.ItemMarkUp = MarkUpType.ImageFigure;
                                item.Title = ParseMarkup(contentItem.ContentHeadline.Headline.Any);
                                item.Credit = ParseMarkup(contentItem.ContentHeadline.Credit.Any);
                                item.Caption = ParseMarkup(contentItem.ContentHeadline.Snippet.Any);
                                item.Source = ParseMarkup(contentItem.ContentHeadline.Byline.Any);
                                item.EnlargedImageUrl = enlargedImgHandlerUrl;
                                found = true;
                            }
                        }
                    }

                    if (!found)
                    {
                        var accessionNumberBasedContentItem = contentItem;
                        if (!articleResult.LeadParagraph.IsNullOrEmpty())
                        {
                            foreach (var item in from dc in articleResult.LeadParagraph
                                                 from kvp in dc
                                                 from item in kvp.Value
                                                 where item.ItemText != null && item.ItemText.Equals("##" + accessionNumberBasedContentItem.AccessionNumber + "##")
                                                 select item)
                            {
                                item.ItemMarkUp = MarkUpType.Anchor;
                                item.ItemText = text;
                                item.ItemValue = contentItem.AccessionNumber;
                            }
                        }

                        if (!articleResult.TailParagraphs.IsNullOrEmpty())
                        {
                            foreach (var item in from dc in articleResult.TailParagraphs
                                                 from kvp in dc
                                                 from item in kvp.Value
                                                 where item.ItemText != null && item.ItemText.Equals("##" + accessionNumberBasedContentItem.AccessionNumber + "##")
                                                 select item)
                            {
                                item.ItemMarkUp = MarkUpType.Anchor;
                                item.ItemText = text;
                                item.ItemValue = contentItem.AccessionNumber;
                            }
                        }

                        if (!articleResult.Notes.IsNullOrEmpty())
                        {
                            foreach (var item in from dc in articleResult.Notes
                                                 from kvp in dc
                                                 from item in kvp.Value
                                                 where item.ItemText != null && item.ItemText.Equals("##" + accessionNumberBasedContentItem.AccessionNumber + "##")
                                                 select item)
                            {
                                item.ItemMarkUp = MarkUpType.Anchor;
                                item.ItemText = text;
                                item.ItemValue = contentItem.AccessionNumber;
                            }
                        }
                    }
                }
                else if (contentItem != null)
                {
                    var text = GetText(contentItem);
                    var accessionNumberBasedContentItem = contentItem;
                    if (!articleResult.LeadParagraph.IsNullOrEmpty())
                    {
                        foreach (var item in articleResult.LeadParagraph
                            .SelectMany(dc => dc
                            .SelectMany(kvp => kvp.Value
                            .Where(item => item.ItemText.Equals("##" + accessionNumberBasedContentItem.AccessionNumber + "##")))))
                        {
                            item.ItemMarkUp = MarkUpType.Unknown;
                            item.ItemText = text;
                            item.ItemValue = contentItem.AccessionNumber;
                        }
                    }

                    if (!articleResult.TailParagraphs.IsNullOrEmpty())
                    {
                        foreach (var item in articleResult.TailParagraphs
                            .SelectMany(dc => dc
                            .SelectMany(kvp => kvp.Value
                            .Where(item => item.ItemText.Equals("##" + accessionNumberBasedContentItem.AccessionNumber + "##")))))
                        {
                            item.ItemMarkUp = MarkUpType.Unknown;
                            item.ItemText = text;
                            item.ItemValue = contentItem.AccessionNumber;
                        }
                    }

                    if (!articleResult.Notes.IsNullOrEmpty())
                    {
                        foreach (var item in articleResult.Notes
                            .SelectMany(dc => dc
                            .SelectMany(kvp => kvp.Value
                            .Where(item => item.ItemText.Equals("##" + accessionNumberBasedContentItem.AccessionNumber + "##")))))
                        {
                            item.ItemMarkUp = MarkUpType.Unknown;
                            item.ItemText = text;
                            item.ItemValue = contentItem.AccessionNumber;
                        }
                    }
                }
            }
        }

        private RenderItem GetHtmlRenderItem(ICollection<Part> parts, string accessionNumber)
        {
            if (parts.IsNullOrEmpty())
            {
                return null;
            }

            var item = new RenderItem();

            var part = parts.FirstOrDefault(tPart => tPart.type.ToLower() == "html");

            if (part != null)
            {
                var request = new GetBinaryRequest
                                  {
                                      mimeType = part.mimeType,
                                      reference = part.reference,
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
                        item.ItemMarkUp = MarkUpType.Html;
                        item.ItemText = tempStr;
                    }
                    item.ItemMarkUp = MarkUpType.Html;
                    item.ItemText = ExtractHtml(encoding.GetString(binaryResponse.binaryData));
                    return item;
                }
                catch (Exception ex)
                {
                    log.Error("Caught Exception", ex);
                }
            }
            return null;
        }

        private void UpdateItem(RenderItem item, ContentItem contentItem, string accessionNumber)
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
        /// The get handler url.
        /// </summary>
        /// <param name="imageType">The image type.</param>
        /// <param name="accessionNo">The accession no.</param>
        /// <param name="contentItem">The content item.</param>
        /// <param name="isBlob">if set to <c>true</c> [is BLOB].</param>
        /// <returns>
        /// A string representing the Handerl Url
        /// </returns>
        public string GetHandlerUrl( ImageType imageType, string accessionNo, ContentItem contentItem, bool isBlob = false )
        {
            var reference = contentItem.Ref;
            var mimeType = contentItem.Mimetype;

            if( FileHandlerUrl.HasValue() )
            {
                var ub = new UrlBuilder( FileHandlerUrl );
                ub.Append( UrlBuilder.GetParameterName( typeof( ArchiveFileRequestDTO ), "AccessionNumber" ), accessionNo );
                ub.Append( UrlBuilder.GetParameterName( typeof( ArchiveFileRequestDTO ), "Reference" ), reference );
                ub.Append( UrlBuilder.GetParameterName( typeof( ArchiveFileRequestDTO ), "MimeType" ), mimeType );
                ub.Append( UrlBuilder.GetParameterName( typeof( ArchiveFileRequestDTO ), "ImageType" ), Map( imageType ) );
                ub.Append( UrlBuilder.GetParameterName( typeof( ArchiveFileRequestDTO ), "IsBlob" ), ( isBlob ) ? "y" : "" );
                AddControlData(ub);
                return ub.ToString();
            }
            return null;
        }

        private string GetHandlerUrl(ImageType imageType, string accessionNo, Part part, bool isBlob = false)
        {
            var reference = part.reference;
            var mimeType = part.mimeType;

            if (FileHandlerUrl.HasValue())
            {
                var ub = new UrlBuilder(FileHandlerUrl);
                ub.Append(UrlBuilder.GetParameterName(typeof(ArchiveFileRequestDTO), "AccessionNumber"), accessionNo);
                ub.Append(UrlBuilder.GetParameterName(typeof(ArchiveFileRequestDTO), "Reference"), reference);
                ub.Append(UrlBuilder.GetParameterName(typeof(ArchiveFileRequestDTO), "MimeType"), mimeType);
                ub.Append(UrlBuilder.GetParameterName(typeof(ArchiveFileRequestDTO), "ImageType"), Map(imageType));
                ub.Append(UrlBuilder.GetParameterName(typeof(ArchiveFileRequestDTO), "IsBlob"), (isBlob) ? "y" : "");
                AddControlData(ub);
                return ub.ToString();
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
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
                    foreach (var item in article.contentParts.parts)
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
                    foreach (var item in article.contentParts.parts.Where(i => i.mimeType.IsNotEmpty()))
                    {
                        switch (item.mimeType.ToLower())
                        {
                            case "image/gif":
                            case "image/jpeg":
                            case "image/png":
                                return ContentSubCategory.Graphic;
                        }
                    }
                    return ContentSubCategory.Article;
                case "summary":
                    return ContentSubCategory.Summary;
                case "board":
                    return ContentSubCategory.Board;
                case "blog":
                    return ContentSubCategory.Blog;
                case "customerdoc":
                    return ContentSubCategory.CustomerDoc;
                case "html":
                    return ContentSubCategory.HTML;
            }

            return ContentSubCategory.UnSpecified;
        }

        /// <summary>
        /// Maps the mime-type.
        /// </summary>
        /// <param name="articleResultset">The article resultset.</param>
        /// <param name="article">The article.</param>
        protected internal static void MapExtraReferenceInformation(ArticleResultset articleResultset, Article article)
        {
            if (article.leadParagraph != null && article.contentParts.parts != null)
            {
                articleResultset.ContentItems = new List<Ajax.HeadlineList.HeadlineContentItem>();
                foreach (var item in article.contentParts.parts.WhereNotNull())
                {
                    articleResultset.ContentItems.Add(new Ajax.HeadlineList.HeadlineContentItem
                    {
                        mimeType = item.mimeType,
                        reference = item.reference,
                        size = item.size.ToString(),
                        subType = item.subType,
                        type = item.type
                    });
                }

            }


            switch (articleResultset.ContentCategory)
            {
                case ContentCategory.Picture:
                    switch (articleResultset.ContentSubCategory)
                    {
                        case ContentSubCategory.Graphic:
                            foreach (var item in article.contentParts.parts.Where(item => (!string.IsNullOrEmpty(item.type) && !string.IsNullOrEmpty(item.type.Trim())) && item.type.ToLower() == "tnail"))
                            {
                                articleResultset.Ref = item.reference;
                                articleResultset.MimeType = (item.mimeType != null) ? item.mimeType.ToLower() : "image/jpeg";
                            }

                            return;
                    }
                    break;
                case ContentCategory.Publication:
                    switch (articleResultset.ContentSubCategory)
                    {
                        case ContentSubCategory.HTML:
                            foreach (var item in article.contentParts.parts.Where(item => (!string.IsNullOrEmpty(item.type) && !string.IsNullOrEmpty(item.type.Trim())) && item.type.ToLower() == "html"))
                            {
                                articleResultset.Ref = item.reference;
                                articleResultset.MimeType = (item.mimeType != null) ? item.mimeType.ToLower() : "text/html";
                            }

                            return;

                        case ContentSubCategory.PDF:
                            foreach (var item in article.contentParts.parts.Where(item => (!string.IsNullOrEmpty(item.type) && !string.IsNullOrEmpty(item.type.Trim())) && item.type.ToLower() == "pdf"))
                            {
                                articleResultset.Ref = item.reference;
                                articleResultset.MimeType = (item.mimeType != null) ? item.mimeType.ToLower() : "application/pdf";
                            }

                            return;
                    }
                    break;
                case ContentCategory.Website:
                    foreach (var item in article.contentParts.parts.Where(item => (!string.IsNullOrEmpty(item.type) && !string.IsNullOrEmpty(item.type.Trim()) && item.type.ToLower() == "webpage"
                        && !string.IsNullOrEmpty(item.subType) && !string.IsNullOrEmpty(item.subType.Trim()) && item.subType.ToLower() == "nlapressclip")))
                    {
                        articleResultset.Ref = item.reference;
                        articleResultset.SubType = item.subType;
                    }
                    break;
                case ContentCategory.Multimedia:
                    foreach (var part in article.contentParts.parts.Where(part => part.type == "audio" || part.type == "video"))
                    {
                        articleResultset.MediaLength = new TimeSpan(0, 0, part.size).ToString("mm':'ss");
                        break;
                    }
                    articleResultset.MediaTitle = GetParagraphText(article.headline);
                    break;
            }

            articleResultset.MimeType = "text/xml";
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

        protected internal string GetExternalUrl(ArticleResultset articleResultset, Article article)
        {
            if (article == null || article.contentParts == null || article.contentParts.parts == null)
            {
                return null;
            }

            // look through based on type and provide the correct Uri
            switch (articleResultset.ContentCategory)
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
                case ContentCategory.Website:
                    switch (articleResultset.ContentSubCategory)
                    {
                        case ContentSubCategory.WebPage:
                            return GenerateWebRedirectionUrl(articleResultset.AccessionNo);
                    }
                    break;
                case ContentCategory.Publication:
                    switch(articleResultset.ContentSubCategory)
                    {
                        case ContentSubCategory.PDF:
                        case ContentSubCategory.HTML:
                            var tItems = article.contentParts.parts
                                        .Where(tItem => !string.IsNullOrEmpty(tItem.mimeType)).ToList();

                            foreach (var strHref in tItems.Select(item => GetHandlerUrl(EmbededImageType, articleResultset.AccessionNo, item)))
                            {
                                return strHref;
                            }
                            break;
                    }
                    break;
            }

            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessionNo"></param>
        /// <returns></returns>
        public string GenerateWebRedirectionUrl (string accessionNo)
        {
            if (FileHandlerUrl.HasValue())
            {
                var ub = new UrlBuilder(FileHandlerUrl);
                ub.Append(UrlBuilder.GetParameterName(typeof(ArchiveFileRequestDTO), "AccessionNumber"), accessionNo);
                ub.Append("redirect", "y");
                AddControlData(ub);
                return ub.ToString();
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ub"></param>
        public void AddControlData(UrlBuilder ub)
        {
             if (!string.IsNullOrEmpty(_controlData.AccessPointCode))
            {
                ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "AccessPointCode"), _controlData.AccessPointCode);
            }

            if (!string.IsNullOrEmpty(_preferences.InterfaceLanguage))
            {
                ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "InterfaceLanguage"), _preferences.InterfaceLanguage);
            }

            if (!string.IsNullOrEmpty(_controlData.ProductID))
            {
                ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "ProductID"), _controlData.ProductID);
            }

            if (!string.IsNullOrEmpty(_controlData.SessionID))
            {
                ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "SessionID"), _controlData.SessionID);
            }
            else if (!string.IsNullOrEmpty(_controlData.EncryptedToken)) // assume this is a lightweight user
            {
                ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "EncryptedToken"), _controlData.EncryptedToken);
            }
            else
            {
                ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "UserID"), _controlData.UserID);
                ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "Password"), _controlData.UserPassword);
            }

            if (!string.IsNullOrEmpty(_controlData.AccessPointCodeUsage))
            {
                ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "AccessPointCodeUsage"), _controlData.AccessPointCodeUsage);
            }

            if (!string.IsNullOrEmpty(_controlData.ClientCode))
            {
                ub.Append(UrlBuilder.GetParameterName(typeof(SessionRequestDTO), "ClientCodeType"), _controlData.ClientCode);
            }
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
                        sb.Append(highlight.text.Value);
                    }
                }
                else if (item is ELink)
                {
                    var elink = (ELink)item;
                    if (elink.Items != null)
                    {
                        foreach (var elinkItem in elink.Items)
                        {
                            if (elinkItem is HighlightedText)
                            {
                                var highlight = (HighlightedText) elinkItem;
                                if (highlight.text != null && highlight.text.Value != null)
                                {
                                    sb.Append(highlight.text.Value);
                                }
                            }
                            else
                            {
                                var text = (Text) elinkItem;
                                if (text.Value != null)
                                {
                                    sb.Append(text.Value);
                                }
                            }
                        }
                    }
                }
                else if (item is EntityReference)
                {
                    var entityReference = (EntityReference) item;
                    sb.Append(entityReference.Items.Join(string.Empty));
                }
                else
                {
                    var text = (Text) item;
                    if (text.Value != null)
                    {
                        sb.Append(text.Value);
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
                        sb.Append(erefHighlight.Value);
                    }
                }
                else if (item is Text)
                {
                    var erefText = (Text) item;
                    if (erefText.Value != null)
                    {
                        sb.Append(erefText.Value);
                    }
                }
            }

            return sb.ToString();
        }

        #endregion

        private static string Map( ImageType imageType )
        {
            switch( imageType )
            {
                default:
                    return "dispix";
                case ImageType.Fingernail:
                    return "fnail";
                case ImageType.Thumbnail:
                    return "tnail";
            }
        }

      
    }
}
