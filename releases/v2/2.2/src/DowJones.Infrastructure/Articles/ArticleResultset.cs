using System.Collections.Generic;
using DowJones.Articles;

namespace DowJones.Ajax.Article
{
    public class ArticleResultset
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

        public List<IRenderItem> Head { get; set; }  

        public List<IRenderItem> Html { get; set; }

        public List<IRenderItem> Source { get; set; }

        public string SourceCode { get; set; }

        public string SourceName { get; set; }

        public List<IRenderItem> ByLine { get; set; }

        public List<IRenderItem> Authors { get; set; }

        public string PublicationDate { get; set; }

        public string PublicationTime { get; set; }

        public List<IRenderItem> Headline { get; set; }

        public List<IRenderItem> Copyright { get; set; } 

        public List<Dictionary<string, List<IRenderItem>>> Correction { get; set; }

        public List<Dictionary<string, List<IRenderItem>>> LeadParagraph { get; set; }

        public List<Dictionary<string, List<IRenderItem>>> TailParagraphs { get; set; }

        public List<IRenderItem> Contact { get; set; }

        public List<IRenderItem> Notes { get; set; }

        public List<IRenderItem> ArtWorks { get; set; }

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
    }
}
