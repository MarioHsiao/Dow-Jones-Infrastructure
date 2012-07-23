using System.Collections.Generic;
using System.Linq;
using System.Xml;
using DowJones.Extensions;
using DowJones.Mapping;
using Factiva.Gateway.Messages.Archive.V2_0;
using Factiva.Gateway.Messages.Search.V2_0;
using Status = Factiva.Gateway.Messages.Common.Status;

namespace DowJones.Assemblers.Articles
{
    public class DocumentToArticleMapper : TypeMapper<Document, Article>
    {
        public override Article Map(Document document)
        {
            if (document != null)
            {
                var article = new Article
                                  {
                                      accessionNo = document.AccessionNo,
                                      baseLanguage = document.BaseLanguage,
                                      sourceCode = document.SourceCode,
                                      sourceName = document.SourceName,
                                      wordCount = document.WordCount,
                                      publisherName = document.PublisherName,
                                      publisherGroupCode = document.PublisherGroupCode,
                                      publisherGroupName = document.PublisherGroupName,
                                      publicationDate = document.PublicationDate,
                                      publicationTimeSpecified = document.__publicationTimeSpecified,
                                      publicationTime = document.PublicationTime,
                                      status = new Status
                                                   {
                                                       value = document.Status.Value,
                                                       message = document.Status.Message,
                                                       type = document.Status.Type
                                                   }, contentParts = new ContentParts
                                                                         {
                                                                             contentType = "article"
                                                                         }, sourceLogo = new SourceLogo
                                                                                             {
                                                                                                 image = document.Logo.Image
                                                                                             }
                                  };


                if (!document.Headline.Any.IsNullOrEmpty())
                {
                    article.headline = GetParagraphs(document.Headline);
                }

                if (!document.ArtWork.Any.IsNullOrEmpty())
                {
                    article.artWork = GetParagraphs(document.ArtWork).First();
                }

                if (!document.Credit.Any.IsNullOrEmpty())
                {
                    article.credit = GetParagraphs(document.Credit).First();
                }

                if (!document.Byline.Any.IsNullOrEmpty())
                {
                    article.byline = GetParagraphs(document.Byline).First();
                }

                if (!document.LeadParagraph.Any.IsNullOrEmpty())
                {
                    article.leadParagraph = GetParagraphs(document.LeadParagraph);
                }

                if (!document.TailParagraphs.Any.IsNullOrEmpty())
                {
                    article.tailParagraphs = GetParagraphs(document.TailParagraphs);
                }

                if (!document.Corrections.Any.IsNullOrEmpty())
                {
                    article.corrections = GetParagraphs(document.Corrections);
                }

                if (!document.Copyright.Any.IsNullOrEmpty())
                {
                    article.copyright = GetParagraphs(document.Copyright).First();
                }

                if (!document.Notes.Any.IsNullOrEmpty())
                {
                    article.notes = GetParagraphs(document.Copyright).First();
                }
                return article;
            }

            return null;
        }

        private static Paragraph[] GetParagraphs(Markup markup)
        {
           return markup.Any.Select(element => new Paragraph
                                                    {
                                                        Items = new object[]
                                                                    {
                                                                        new Text
                                                                            {
                                                                                Value = element.InnerXml
                                                                            }
                                                                    },

                                                        display = (element.GetAttribute("display") == "asis") ? ParagraphDisplay.Fixed : ParagraphDisplay.Proportional,
                                                    }).ToArray();
        }
    }
}