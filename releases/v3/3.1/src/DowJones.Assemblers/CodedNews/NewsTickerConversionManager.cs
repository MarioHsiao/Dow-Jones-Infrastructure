using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using DowJones.Ajax.HeadlineList;
using DowJones.Ajax.NewsStandTicker;
using DowJones.Managers.Search.CodedNewsQueries;

namespace DowJones.Assemblers.CodedNews
{
    public class NewsTickerConversionManager
    {
        public static NewsTickerDataResult Convert(HeadlineListDataResult headlineListDataResult, TruncationType truncationType)
        {
            if (null == headlineListDataResult)
            {
                return null;
            }

            var headlineListDataResultSet = headlineListDataResult.resultSet;
            var newsTickerDataResult = new NewsTickerDataResult { HitCount = headlineListDataResult.hitCount };

            if (null == headlineListDataResultSet.headlines || !headlineListDataResultSet.headlines.Any())
            {
                newsTickerDataResult.ResultSet.Count = headlineListDataResult.hitCount;
                newsTickerDataResult.ResultSet.First = headlineListDataResult.hitCount;
                newsTickerDataResult.ResultSet.DuplicateCount = headlineListDataResult.hitCount;

                return newsTickerDataResult;
            }

            foreach (var info in headlineListDataResultSet.headlines)
            {
                newsTickerDataResult.ResultSet.Headlines.Add(new NewsTickerInfo
                {
                    Source = info.sourceDescriptor,
                    PublicationDateTimeDescriptor = info.publicationDateTimeDescriptor,
                    HeadlineUrl = info.reference.externalUri,
                    Title = GetTitle(info, truncationType),
                    //// ToolTip = GetSnippetParas(info, truncationType),
                    Author = ProcessParas(info.byline),
                    Reference = info.reference,
                    Snippet = ProcessParas(info.snippet)
                });
            }

            newsTickerDataResult.ResultSet.Count = headlineListDataResultSet.count;
            newsTickerDataResult.ResultSet.First = headlineListDataResultSet.first;
            newsTickerDataResult.ResultSet.DuplicateCount = headlineListDataResultSet.duplicateCount;

            return newsTickerDataResult;
        }

        private static List<string> ProcessParas(IEnumerable<Para> paras)
        {
            if (paras == null || !paras.Any() || !paras.First().items.Any())
            {
                return null;
            }

            var temp = new List<string>();
            foreach (var para in paras)
            {
                var sb = new StringBuilder();
                foreach (var markupItem in para.items)
                {
                    sb.Append(markupItem.value.Trim());
                    sb.Append(" ");
                }

                temp.Add(sb.ToString());
            }

            return temp;
        }

        private static string GetTitle(HeadlineInfo headline, TruncationType truncationType)
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

            var tempTitle = string.Join(" ", ProcessParas(headline.title));
            if (index == 0)
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
                        return tempTitle.Substring(0, i);
                    }
                }
            }

            return tempTitle;
        }

        protected static string GetSnippetParas(HeadlineInfo headline, TruncationType truncationType)
        {
            var sb = new StringBuilder();
            if (headline == null)
            {
                return string.Empty;
            }

            if (headline.thumbnailImage != null && headline.thumbnailImage.SRC != null)
            {
                sb.AppendFormat("<span class=\"{0}\"><img src=\"{1}\"/></span>", headline.thumbnailImage.SRC);
            }

            if (headline.snippet != null && headline.snippet.Count > 0)
            {
                sb.Append(ProcessParas(headline.snippet));
            }

            return HttpContext.Current.Server.HtmlEncode(sb.ToString());
        }
    }
}
