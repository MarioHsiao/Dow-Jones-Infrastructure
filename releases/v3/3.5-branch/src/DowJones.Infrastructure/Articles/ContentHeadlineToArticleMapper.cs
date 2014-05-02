using System.Collections.Generic;
using System.Linq;
using System.Xml;
using DowJones.Extensions;
using DowJones.Mapping;
using Factiva.Gateway.Messages.Archive.V2_0;
using Factiva.Gateway.Messages.Search.V2_0;

namespace DowJones.Articles
{
    public class ContentHeadlineToArticleMapper : TypeMapper<ContentHeadline, Article>
    {
        public override Article Map(ContentHeadline headline)
        {
            var article = new Article
                              {
                                  accessionNo = headline.AccessionNo,
                                  baseLanguage = headline.BaseLanguage,
                                  sourceCode = headline.SourceCode,
                                  sourceName = headline.SourceName,
                                  wordCount = headline.WordCount
                              };

            if (headline.__publicationDateSpecified)
            {
                article.publicationDate = headline.PublicationDate;
            }

            if (headline.__publicationTimeSpecified)
            {
                article.publicationTime = headline.PublicationTime;
            }

            article.headline = MapHeadline(headline.Headline);
            article.byline = ParseMarkup(headline.Byline);
            article.contentParts = MapContentItems(headline.ContentItems);
            article.copyright = ParseMarkup(headline.Copyright);
            article.credit = ParseMarkup(headline.Credit);
            article.leadParagraph = MapSnippet(headline.Snippet);
            return article;
        }

        private ContentParts MapContentItems(ContentItems contentItems)
        {
            if (contentItems == null)
            {
                return null;
            }
            var a =  new ContentParts
                        {
                            contentType = contentItems.ContentType,
                            primaryReference = contentItems.PrimaryRef,
                            parts =  contentItems.ItemCollection.Select( item => new Part
                                        {
                                            mimeType = item.Mimetype,
                                            reference = item.Ref,
                                            subType = item.Subtype,
                                            type = item.Type,
                                            size = item.Size.ConvertTo<int>( 0, true ),
                                            sizeSpecified = item.IsNumeric(),
                                        } ).ToArray()
                            
                        };     
            return a;
        }

        private ArticleContent ParseMarkup(Markup byline)
        {
            return ParseMarkup(byline.Any);
        }

        private ArticleContent ParseMarkup(IEnumerable<XmlNode> any)
        {
            if (any == null)
            {
                return null;
            }
            var c = new ArticleContent();
            IList<Text> items = new List<Text>();
            foreach (var p in any.Select(MapEachElement))
            {
                items.AddRange(p.Items.OfType<Text>());
            }
            c.Items = items.ToArray();
            return c;
        }

        private static Paragraph[] MapHeadline(Markup headline)
        {
            return MapAny(headline.Any);
        }

        private static Paragraph[] MapSnippet(Markup snippet)
        {
            return MapAny(snippet.Any);
        }

        private static Paragraph[] MapAny(IEnumerable<XmlNode> any)
        {
            if (any == null)
            {
                return null;
            }
            return any.Select(MapEachElement).WhereNotNull().ToArray();
        }

        private static Paragraph MapEachElement(XmlNode xmlElement)
        {
            if (xmlElement == null)
            {
                return null;
            }
            var p = new Paragraph();
            IList<Text> t = (from XmlNode node in xmlElement.ChildNodes select new Text {Value = node.InnerText}).ToList();
            p.Items = t.ToArray();
            return p;
        }

    }
}