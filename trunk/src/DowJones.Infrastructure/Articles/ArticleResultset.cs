    using System.Collections.Generic;
using DowJones.Articles;

namespace DowJones.Ajax.Article
{
    public class ArticleResultset : IArticleResultset
    {
        public string AccessionNo { get; set; }

        public int Status { get; set; }

        public PictureSize PictureSize { get; set; }

        public ContentCategory ContentCategory { get; set; }

        public string ContentCategoryDescriptor { get; set; }

        public ContentSubCategory ContentSubCategory { get; set; }
        
        public string ContentSubCategoryDescriptor { get; set; }

        public string OriginalContentCategory { get; set; }

        public string ExternalUri { get; set; }

        public List<RenderItem> Head { get; set; }

        public List<RenderItem> Html { get; set; }

        public List<RenderItem> Source { get; set; }

        public string SourceCode { get; set; }

        public string SourceName { get; set; }

        public List<RenderItem> ByLine { get; set; }

        public List<RenderItem> Authors { get; set; }

        public List<RenderItem> Credit { get; set; }

        public List<RenderItem> ColumnName { get; set; }

        public List<RenderItem> SectionName { get; set; }

        public string PublicationDate { get; set; }

        public string PublicationFormattedDate { get; set; }

        public string PublicationTime { get; set; }
        
        public string ModificationDate { get; set; }

        public string ModificationTime { get; set; }

        public List<RenderItem> Headline { get; set; }

        public List<RenderItem> Copyright { get; set; } 

        public List<Dictionary<string, List<RenderItem>>> Correction { get; set; }

        public List<Dictionary<string, List<RenderItem>>> LeadParagraph { get; set; }

        public List<Dictionary<string, List<RenderItem>>> TailParagraphs { get; set; }

        public List<Dictionary<string, List<RenderItem>>> Notes { get; set; }

        public List<RenderItem> Contact { get; set; }

        public List<RenderItem> ArtWorks { get; set; }

        public string Language { get; set; }

        public string LanguageCode { get; set; }

        public int WordCount { get; set; }

        public List<string> Pages { get; set; }

        public Dictionary<string, Dictionary<string, string>> IndexingCodeSets { get; set; }

        public string PublisherName { get; set; }

        public string PublisherGroupCode { get; set; }

        public List<string> Ipds { get; set; }

        public List<string> Ipcs { get; set; }

        public string MimeType { get; set; }

        public string Ref;

        public string SubType;

        public string MediaLength { get; set; }
        
        public string MediaTitle { get; set; }

        public List<Ajax.HeadlineList.HeadlineContentItem> ContentItems { get; set; }
    }
}
