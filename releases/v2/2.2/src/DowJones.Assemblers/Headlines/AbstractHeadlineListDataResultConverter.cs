// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AbstractHeadlineListDataResultConverter.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Xml;
using DowJones.Ajax;
using DowJones.Ajax.HeadlineList;
using DowJones.Exceptions;
using DowJones.Formatters.Globalization.DateTime;
using DowJones.Formatters.Numerical;
using DowJones.Globalization;
using Factiva.Gateway.Messages.Search.V2_0;

namespace DowJones.Assemblers.Headlines
{
    public abstract class AbstractHeadlineListDataResultSetConverter : IListDataResultConverter
    {
        private readonly DateTimeFormatter dateTimeFormatter;
        private readonly NumberFormatter numberFormatter;
        private readonly ResourceTextManager resourceText;

        protected internal GenerateExternalUrlForHeadlineInfo GenerateExternalUrlForHeadlineInfo { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractHeadlineListDataResultSetConverter"/> class.
        /// </summary>
        /// <param name="interfaceLanguage">The interface language.</param>
        protected AbstractHeadlineListDataResultSetConverter(string interfaceLanguage)
        {
            resourceText = ResourceTextManager.Instance;
            numberFormatter = new NumberFormatter();
            dateTimeFormatter = new DateTimeFormatter(interfaceLanguage);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractHeadlineListDataResultSetConverter"/> class.
        /// </summary>
        protected AbstractHeadlineListDataResultSetConverter(DateTimeFormatter dateTimeFormatter)
        {
            resourceText = ResourceTextManager.Instance;
            numberFormatter = new NumberFormatter();
            this.dateTimeFormatter = dateTimeFormatter;
        }

        #region Properties

        public DateTimeFormatter DateTimeFormatter
        {
            get { return dateTimeFormatter; }
        }

        public NumberFormatter NumberFormatter
        {
            get { return numberFormatter; }
        }

        public ResourceTextManager ResourceText
        {
            get { return resourceText; }
        }

        #endregion

        #region IHeadlineListDataResultConverter Members

        public abstract IListDataResult Process();

        #endregion

        /// <summary>
        /// Parses the markup.
        /// </summary>
        /// <param name="paras">The paras.</param>
        /// <returns></returns>
        private static List<Para> ParseMarkup(IEnumerable<XmlNode> paras)
        {
            var output = new List<Para>();
            if (paras == null)
            {
                return null;
            }

            foreach (var para in paras)
            {
                if (para == null || !para.HasChildNodes)
                {
                    continue;
                }

                var tempPara = new Para();
                var items = new List<MarkupItem>();
                foreach (XmlNode node in para.ChildNodes)
                {
                    var item = new MarkupItem();
                    switch (node.NodeType)
                    {
                        case XmlNodeType.Element:
                            switch (node.Name)
                            {
                                case "hlt":
                                    item.type = EntityType.Highlight.ToString();
                                    item.value = ReplaceSpecialChars(node.InnerText);
                                    items.Add(item);
                                    break;
                                case "en":
                                    if (node.Attributes != null)
                                    {
                                        item.type = MapCatToEntityType(node.Attributes.GetNamedItem("cat").Value).ToString();
                                        item.guid = node.Attributes.GetNamedItem("ref").Value;
                                        item.value = ReplaceSpecialChars(node.InnerText);
                                        items.Add(item);
                                    }
                                    break;
                            }

                            break;
                        case XmlNodeType.Text:
                            item.type = EntityType.Textual.ToString();
                            item.value = ReplaceSpecialChars(node.InnerText);
                            items.Add(item);
                            break;
                    }

                    if (items.Count <= 0)
                    {
                        continue;
                    }

                    tempPara.items = items;
                }

                if (tempPara.items != null && tempPara.items.Count > 0)
                {
                    output.Add(tempPara);
                }
            }

            return output;
        }

        protected static string ReplaceSpecialChars(string s)
        {
            /*StringBuilder sb = new StringBuilder(s);
            sb.Replace("\u2028;", "")
                .Replace("\u2029;", "")
                .Replace("\u2019;", "'");*/
            return string.IsNullOrEmpty(s) ? s : s.Trim();
        }

        /// <summary>
        /// Parses the markup.
        /// </summary>
        /// <param name="markup">The markup.</param>
        /// <returns></returns>
        protected static List<Para> ParseMarkup(Markup markup)
        {
            return ParseMarkup(markup.Any);
        }

        protected static List<Para> ReplaceMarkup(Markup markup, CodeSets codeSets)
        {
            if (codeSets != null && codeSets.Count > 0)
            {
                var tempParas = new List<Para>();
                foreach( var codeSet in codeSets.CodeSetCollection.Where( codeSet => codeSet.Id == "au" && codeSet.CodeCollection.Count > 0 ) )
                {
                    var temp = new Para
                                   {
                                       items = new List<MarkupItem>()
                                   };
                    foreach( var code in codeSet.CodeCollection )
                    {
                        temp.items.Add( new MarkupItem( EntityType.Author, code.Value, code.Id ) );
                    }

                    tempParas.Add(temp);
                } 
                return tempParas;
            }     

            return ParseMarkup( markup.Any );   
        }

        /// <summary>
        /// Maps the type of the cat to entity.
        /// </summary>
        /// <param name="cat">The cat.</param>
        /// <returns></returns>
        protected static EntityType MapCatToEntityType(string cat)
        {
            switch (cat.ToLower())
            {
                case "pe":
                    return EntityType.Person;
                case "au":
                    return EntityType.Author;
                case "in":
                    return EntityType.Industry;
                case "ns":
                    return EntityType.NewsSubject;
                case "re":
                    return EntityType.Region;
                case "co":
                    return EntityType.Company;
                default:
                    return EntityType.UnSpecified;
            }
        }

        /// <summary>
        /// Maps the language to content language.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <returns></returns>
        protected internal string GetLanguageToContentLanguage(string language)
        {
            ContentLanguage? contentLanguage = Mapper.Map<ContentLanguage>(language);
            return ResourceText.GetAssignedToken(contentLanguage);
        }

        protected internal string UpdateReferenceUrl(ContentCategory contentCategory, ContentHeadline contentHeadline, bool isDuplicate)
        {
            if (contentHeadline == null || contentHeadline.ContentItems == null)
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
                    foreach (var item in contentHeadline.ContentItems.ItemCollection.Where(item => item.Type.ToLower() == "webpage"))
                    {
                        return item.Ref;
                    }

                    break;
            }

            return GenerateExternalUrlForHeadlineInfo != null ? GenerateExternalUrlForHeadlineInfo(contentHeadline, isDuplicate) : null;
        }
        

        /// <summary>
        /// Maps the mime-type.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="headline">The headline.</param>
        protected internal static void MapExtraReferenceInformation(HeadlineInfo info, ContentHeadline headline)
        {
            switch (info.contentCategory)
            {
                case ContentCategory.Picture:
                    switch (info.contentSubCategory)
                    {
                        case ContentSubCategory.Graphic:
                            foreach (var item in headline.ContentItems.ItemCollection.Where(item => (!string.IsNullOrEmpty(item.Type) && !string.IsNullOrEmpty(item.Type.Trim())) && item.Type.ToLower() == "tnail"))
                            {
                                info.reference.@ref = item.Ref;
                                info.reference.mimetype = (item.Mimetype != null) ? item.Mimetype.ToLower() : "image/jpeg";
                            }

                            return;
                    }
                    break;
                case ContentCategory.Publication:
                    switch (info.contentSubCategory)
                    {
                        case ContentSubCategory.HTML:
                            foreach (var item in headline.ContentItems.ItemCollection.Where(item => (!string.IsNullOrEmpty(item.Type) && !string.IsNullOrEmpty(item.Type.Trim())) && item.Type.ToLower() == "html"))
                            {
                                info.reference.@ref = item.Ref;
                                info.reference.mimetype = ( item.Mimetype != null ) ? item.Mimetype.ToLower() : "text/html";
                            }

                            return;

                        case ContentSubCategory.PDF:
                            foreach (var item in headline.ContentItems.ItemCollection.Where(item => (!string.IsNullOrEmpty(item.Type) && !string.IsNullOrEmpty(item.Type.Trim())) && item.Type.ToLower() == "pdf"))
                            {
                                info.reference.@ref = item.Ref;
                                info.reference.mimetype = (item.Mimetype != null) ? item.Mimetype.ToLower() : "application/pdf";
                            }

                            return;
                    }
                    break;
                case ContentCategory.Website:
                    foreach (var item in headline.ContentItems.ItemCollection.Where(item => (!string.IsNullOrEmpty(item.Type) && !string.IsNullOrEmpty(item.Type.Trim()) && item.Type.ToLower() == "webpage"
                        && !string.IsNullOrEmpty(item.Subtype) && !string.IsNullOrEmpty(item.Subtype.Trim()) && item.Subtype.ToLower() == "nlapressclip")))
                    {
                        info.reference.@ref = item.Ref;
                        info.reference.subType = item.Subtype;
                    }
                    break;
            }

            info.reference.mimetype = "text/xml";
        }

        /// <summary>
        /// Gets the type of the original content.
        /// </summary>
        /// <param name="contentHeadline">The content headline.</param>
        /// <returns></returns>
        public static string GetOriginalContentType(ContentHeadline contentHeadline)
        {
            if (contentHeadline == null || contentHeadline.ContentItems == null || contentHeadline.ContentItems.ContentType == null)
            {
                return string.Empty;
            }

            return contentHeadline.ContentItems.ContentType.ToLower();
        }


        /// <summary>
        /// Maps the type of the content.
        /// </summary>
        /// <param name="contentHeadline">The content headline.</param>
        /// <returns></returns>
        public static ContentCategory MapContentCategory(ContentHeadline contentHeadline)
        {
            if (contentHeadline == null || contentHeadline.ContentItems == null || contentHeadline.ContentItems.ContentType == null)
            {
                return ContentCategory.UnSpecified;
            }

            switch (contentHeadline.ContentItems.ContentType.ToLower())
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

        /// <summary>
        /// Validates the name of the language.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <returns></returns>
        public static string ValidateLanguageName(string language)
        {
            if (string.IsNullOrEmpty(language))
            {
                return "en";
            }

            switch (language.ToLower())
            {
                case "en":
                case "fr":
                case "de":
                case "es":
                case "it":
                case "ja":
                case "ru":
                    return language.ToLower();
                case "zh":
                case "zh-cn":
                case "zhcn":
                    return "zhcn";
                case "zh-tw":
                case "zhtw":
                    return "zhtw";
                default:
                    if (language.Contains("-"))
                    {
                        var index = language.IndexOf('-');
                        if (index > 0)
                        {
                            return ValidateLanguageName(language.Substring(0, index));
                        }
                    }

                    break;
            }

            throw new DowJonesUtilitiesException(DowJonesUtilitiesException.RSSLanguageIsNotSupported);
        }


        /// <summary>
        /// Maps the content sub category.
        /// </summary>
        /// <param name="contentHeadline">The content headline.</param>
        /// <returns></returns>
        public static ContentSubCategory MapContentSubCategory(ContentHeadline contentHeadline)
        {
            if (contentHeadline == null || contentHeadline.ContentItems == null || contentHeadline.ContentItems.ContentType == null)
            {
                return ContentSubCategory.UnSpecified;
            }
            switch (contentHeadline.ContentItems.ContentType.ToLower())
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
                    foreach (ContentItem item in contentHeadline.ContentItems.ItemCollection)
                    {
                        switch (item.Type.ToLower())
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

        public Truncation GetTruncationRules(TruncationRules truncationRules)
        {
            var truncation = new Truncation();

            if (truncationRules == null)
            {
                return null;
            }

            if (truncationRules.__extraSmallSpecified)
            {
                truncation.Extrasmall = truncationRules.ExtraSmall;
            }

            if (truncationRules.__smallSpecified)
            {
                truncation.Small = truncationRules.Small;
            }

            if (truncationRules.__mediumSpecified)
            {
                truncation.Medium = truncationRules.Medium;
            }

            if (truncationRules.__largeSpecified)
            {
                truncation.Large = truncationRules.Large;
            }

            return truncation;
        }
    }
}