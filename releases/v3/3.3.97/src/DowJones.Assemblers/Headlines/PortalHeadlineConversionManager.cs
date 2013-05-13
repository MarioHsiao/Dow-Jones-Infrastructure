// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PortalHeadlineConversionManager.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using DowJones.Ajax.HeadlineList;
using DowJones.Ajax.PortalHeadlineList;
using DowJones.Formatters;
using DowJones.Formatters.Globalization.DateTime;
using DowJones.Mapping;

namespace DowJones.Assemblers.Headlines
{
    public class PortalHeadlineConversionManager : TypeMapper<HeadlineListDataResult, PortalHeadlineListDataResult>
    {
        private static readonly Regex HTMLTags = new Regex("<[^>]*>");
        private const int TruncationLimitForAuthors = 35;

        public override PortalHeadlineListDataResult Map(HeadlineListDataResult source)
        {
            return Convert(source);
        }

        public static PortalHeadlineListDataResult Convert(HeadlineListDataResult headlineListDataResult, TruncationType truncationType = TruncationType.XSmall)
        {
            if (null == headlineListDataResult)
            {
                return null;
            }
            
            var headlineListDataResultSet = headlineListDataResult.resultSet;
            var portalDataResult = new PortalHeadlineListDataResult { HitCount = headlineListDataResult.hitCount };
            
            if (null == headlineListDataResultSet.headlines || !headlineListDataResultSet.headlines.Any())
            {
                portalDataResult.ResultSet.Count = headlineListDataResult.hitCount;
                portalDataResult.ResultSet.First = headlineListDataResult.hitCount;
                portalDataResult.ResultSet.DuplicateCount = headlineListDataResult.hitCount;

                return portalDataResult;
            }
            
            foreach (var info in headlineListDataResultSet.headlines)
            {
                portalDataResult.ResultSet.Headlines.Add(new PortalHeadlineInfo
                                                             {
                                                                 Index = info.index,
                                                                 SourceCode = info.sourceReference,
                                                                 SourceDescriptor = info.sourceDescriptor,
                                                                 
                                                                 PublicationDateTime = DateTimeFormatter.ConvertToUtc(info.publicationDateTime),
                                                                 PublicationDateTimeDescriptor = info.publicationDateTimeDescriptor,
                                                                 PublicationDateDescriptor = info.publicationDateDescriptor,
                                                                 PublicationTimeDescriptor = info.publicationTimeDescriptor,
                                                                 HasPublicationTime = info.hasPublicationTime,

                                                                 ModificationDateTime = (info.modificationDateTime == DateTime.MinValue) ? (DateTime?)null : DateTimeFormatter.ConvertToUtc(info.modificationDateTime),
                                                                 ModificationDateTimeDescriptor = info.modificationDateTimeDescriptor,
                                                                 ModificationDateDescriptor = info.modificationDateDescriptor,
                                                                 ModificationTimeDescriptor = info.modificationTimeDescriptor,

                                                                 HeadlineUrl = info.reference.externalUri,
                                                                 TruncatedTitle = GetTruncatedTitle(info, truncationType),
                                                                 Title = string.Join(" ", ProcessParas(info.title)),
                                                                 Authors = new AuthorCollection(Truncate(ProcessParas(info.byline).ToArray())),
                                                                 CodedAuthors = info.codedAuthors,
                                                                 Reference = info.reference,
                                                                 Snippets = new SnippetCollection(ProcessParas(info.snippet).ToArray()),
                                                                 ContentCategoryDescriptor = info.contentCategoryDescriptor,
                                                                 ContentSubCategoryDescriptor = info.contentSubCategoryDescriptor,
                                                                 WordCount = new WholeNumber(info.wordCount),
                                                                 WordCountDescriptor = info.wordCountDescriptor,
                                                                 BaseLanguage = info.baseLanguage,
                                                                 BaseLanguageDescriptor = info.baseLanguageDescriptor,
                                                                 ThumbnailImage = info.thumbnailImage,
                                                                 MediaLength = info.time,
                                                                 IsFree = info.isFree,
                                                                 IsValid = info.isValid,
                                                                 DocumentVector = info.documentVector
                });
            }

            portalDataResult.ResultSet.Count = headlineListDataResultSet.count;
            portalDataResult.ResultSet.First = headlineListDataResultSet.first;
            portalDataResult.ResultSet.DuplicateCount = headlineListDataResultSet.duplicateCount;
         
            return portalDataResult;
        }

        protected static string[]Truncate(string[] authors)
        {
            if (authors != null)
            {
                string temp = string.Join(" ", authors);
                return (temp.Length > TruncationLimitForAuthors) ? new[] { temp.Substring(0, TruncationLimitForAuthors) } : new[] { temp };
            }

            return null;
        }

        protected static string GetSnippetParas(HeadlineInfo headline, TruncationType truncationType)
        {
            var sb = new StringBuilder();
            if (headline == null)
            {
                return string.Empty;
            }

            if (headline.snippet != null && headline.snippet.Count > 0)
            {
                sb.Append(ProcessParas(headline.snippet));
            }

            return HttpContext.Current.Server.HtmlEncode(sb.ToString());
        }
        
        private static List<string> ProcessParas(IEnumerable<Para> paras)
        {
            var temp = new List<string>();

            if (paras == null || !paras.Any() || !paras.First().items.Any())
            {
                return temp;
            }
            
            foreach (var para in paras)
            {
                var sb = new StringBuilder();
                foreach (var markupItem in para.items)
                {
                    var tempParaValue = StripHTML(markupItem.value.Trim());
                    sb.Append(tempParaValue);
                    sb.Append(" ");
                }

                temp.Add(sb.ToString());
            }

            return temp;
        }

        private static string GetTruncatedTitle(HeadlineInfo headline, TruncationType truncationType)
        {
            var index = 0;

            switch (truncationType)
            {
                case TruncationType.XSmall:
                    if (headline.truncationRules.Extrasmall > 0)
                    {
                        index = headline.truncationRules.Extrasmall;
                    }

                    break;
                case TruncationType.Small:
                    if (headline.truncationRules.Small > 0)
                    {
                        index = headline.truncationRules.Small;
                    }

                    break;
                case TruncationType.Medium:
                    if (headline.truncationRules.Medium > 0)
                    {
                        index = headline.truncationRules.Medium;
                    }

                    break;
                case TruncationType.Large:
                    if (headline.truncationRules.Large > 0)
                    {
                        index = headline.truncationRules.Large;
                    }

                    break;
                default:
                    break;
            }

            var tempTitle = string.Join(" ", ProcessParas(headline.title)).Trim();
            if (index <= 0 || index >= tempTitle.Length)
            {
                return tempTitle;
            }

            if (index < tempTitle.Length)
            {
                for (var i = index; i > 0; i--)
                {
                    var c = tempTitle[i];
                    if (c == ' ')
                    {
                        return tempTitle.Substring(0, i)  + "..."; // add ellipses to the end
                    }
                }
            }

            return tempTitle; 
        }

        // De-HTMLize the title
        private static string StripHTML(string feedTitle)
        {
            return string.IsNullOrWhiteSpace(feedTitle) ? string.Empty : HTMLTags.Replace(feedTitle, string.Empty);
        }
    }
}
