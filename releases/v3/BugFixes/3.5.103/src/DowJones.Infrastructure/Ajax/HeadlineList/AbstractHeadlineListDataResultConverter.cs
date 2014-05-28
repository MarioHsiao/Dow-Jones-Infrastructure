using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using EMG.Tools.Ajax.HeadlineList;
using EMG.Utility.Attributes;
using EMG.Utility.Core;
using EMG.Utility.Exceptions;
using EMG.Utility.Formatters.Globalization;
using EMG.Utility.Formatters.Numerical;
using EMG.Utility.Managers.Core;
using Factiva.Gateway.Messages.Search.V2_0;

namespace EMG.Tools.Ajax.Converters.HeadlineList
{
    public abstract class AbstractHeadlineListDataResultSetConverter : IListDataResultConverter
    {
        private readonly DateTimeFormatter _dateTimeFormatter;
        private readonly NumberFormatter _numberFormatter;
        private readonly ResourceTextManager _resourceText;


        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractHeadlineListDataResultSetConverter"/> class.
        /// </summary>
        /// <param name="interfaceLanguage">The interface language.</param>
        protected AbstractHeadlineListDataResultSetConverter(string interfaceLanguage)
        {
            _resourceText = ResourceTextManager.Instance;
            _numberFormatter = new NumberFormatter();
            _dateTimeFormatter = new DateTimeFormatter(interfaceLanguage);
        }

        #region Properties

        public DateTimeFormatter DateTimeFormatter
        {
            get { return _dateTimeFormatter; }
        }

        public NumberFormatter NumberFormatter
        {
            get { return _numberFormatter; }
        }

        public ResourceTextManager ResourceText
        {
            get { return _resourceText; }
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
            List<Para> output = new List<Para>();
            if (paras == null)
                return null;

            foreach (XmlNode para in paras)
            {
                if (para == null || !para.HasChildNodes)
                    continue;
                Para tPara = new Para();
                List<MarkupItem> items = new List<MarkupItem>();
                foreach (XmlNode node in para.ChildNodes)
                {
                    MarkupItem item = new MarkupItem();
                    switch (node.NodeType)
                    {
                        case XmlNodeType.Element:
                            switch (node.Name)
                            {
                                case "hlt":
                                    item.type = EntityType.Highlight.ToString();
                                    item.value = node.InnerText;
                                    items.Add(item);
                                    break;
                                case "en":
                                    item.type = MapCatToEntityType(node.Attributes.GetNamedItem("cat").Value).ToString();
                                    item.guid = node.Attributes.GetNamedItem("ref").Value;
                                    item.value = node.InnerText;
                                    items.Add(item);
                                    break;
                            }
                            break;
                        case XmlNodeType.Text:
                            item.type = EntityType.Textual.ToString();
                            item.value = node.InnerText;
                            items.Add(item);
                            break;
                    }
                    if (items.Count <= 0)
                        continue;
                    tPara.items = items;
                }
                if (tPara.items != null && tPara.items.Count > 0)
                {
                    output.Add(tPara);
                }
            }
            return output;
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

        protected string GetAssignedToken<T>(T value)
        {
            Type enumType = typeof (T);
            string s = value.ToString();
            AssignedToken assignedToken = (AssignedToken) Attribute.GetCustomAttribute(enumType.GetField(s), typeof (AssignedToken));
            return assignedToken != null ? _resourceText.GetString(assignedToken.Token) : string.Empty;
        }

        /// <summary>
        /// Maps the language to content language.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <returns></returns>
        protected string GetLanguageToContentLanguage(string language)
        {
            if (!string.IsNullOrEmpty(language) && !string.IsNullOrEmpty(language.Trim()))
            {
                FieldInfo fieldInfo = typeof (ContentLanguage).GetField(language.ToLower());
                if (fieldInfo != null)
                {
                    AssignedToken assignedToken = (AssignedToken) Attribute.GetCustomAttribute(fieldInfo, typeof (AssignedToken));
                    if (assignedToken != null)
                    {
                        return _resourceText.GetString(assignedToken.Token);
                    }
                }
            }
            return language;
        }

        /// <summary>
        /// Maps the mimetype.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="headline">The headline.</param>
        protected static void MapExtraReferenceInformation(HeadlineInfo info, ContentHeadline headline)
        {
            switch (info.contentCategory)
            {
                case ContentCategory.Publication:
                    switch (info.contentSubCategory)
                    {
                        case ContentSubCategory.HTML:
                            foreach (ContentItem item in headline.ContentItems.ItemCollection)
                            {
                                if (string.IsNullOrEmpty(item.Type) ||
                                    string.IsNullOrEmpty(item.Type.Trim()) ||
                                    item.Type.ToLower() != "html")
                                    continue;
                                info.reference.@ref = item.Ref;
                                info.reference.mimetype = item.Mimetype.ToLower();
                            }
                            return;

                        case ContentSubCategory.PDF:
                            foreach (ContentItem item in headline.ContentItems.ItemCollection)
                            {
                                if (string.IsNullOrEmpty(item.Type) ||
                                    string.IsNullOrEmpty(item.Type.Trim()) ||
                                    item.Type.ToLower() != "pdf")
                                    continue;
                                info.reference.@ref = item.Ref;
                                info.reference.mimetype = item.Mimetype.ToLower();
                            }
                            return;
                    }
                    break;
            }
            info.reference.mimetype = "text/xml";
        }

        /// <summary>
        /// Maps the type of the content.
        /// </summary>
        /// <param name="contentHeadline">The content headline.</param>
        /// <returns></returns>
        protected static ContentCategory MapContentCategory(ContentHeadline contentHeadline)
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
                return "en";
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
                        int index = language.IndexOf('-');
                        if (index > 0)
                            return ValidateLanguageName(language.Substring(0, index));
                    }
                    break;
            }
            throw new EmgUtilitiesException(EmgUtilitiesException.RSS_LANGUAGE_IS_NOT_SUPPORTED);
        }


        /// <summary>
        /// Maps the content sub category.
        /// </summary>
        /// <param name="contentHeadline">The content headline.</param>
        /// <returns></returns>
        protected static ContentSubCategory MapContentSubCategory(ContentHeadline contentHeadline)
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
    }
}