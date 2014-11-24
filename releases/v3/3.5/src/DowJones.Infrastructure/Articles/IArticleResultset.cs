using System.Collections.Generic;
using DowJones.Articles;

namespace DowJones.Ajax.Article
{
    public interface IArticleResultset
    {
        string AccessionNo { get; set; }
        int Status { get; set; }
        PictureSize PictureSize { get; set; }
        ContentCategory ContentCategory { get; set; }
        string ContentCategoryDescriptor { get; set; }
        ContentSubCategory ContentSubCategory { get; set; }
        string ContentSubCategoryDescriptor { get; set; }
        string OriginalContentCategory { get; set; }
        string ExternalUri { get; set; }
        List<RenderItem> Head { get; set; }
        List<RenderItem> Html { get; set; }
        List<RenderItem> Source { get; set; }
        string SourceCode { get; set; }
        string SourceName { get; set; }
        List<RenderItem> ByLine { get; set; }
        List<RenderItem> Authors { get; set; }
        string PublicationDate { get; set; }
        string PublicationTime { get; set; }
        List<RenderItem> Headline { get; set; }
        List<RenderItem> Copyright { get; set; }
        List<Dictionary<string, List<RenderItem>>> Correction { get; set; }
        List<Dictionary<string, List<RenderItem>>> LeadParagraph { get; set; }
        List<Dictionary<string, List<RenderItem>>> TailParagraphs { get; set; }
        List<Dictionary<string, List<RenderItem>>> Notes { get; set; }
        List<RenderItem> Contact { get; set; }
        List<RenderItem> ArtWorks { get; set; }
        string Language { get; set; }
        string LanguageCode { get; set; }
        int WordCount { get; set; }
        List<string> Pages { get; set; }
        Dictionary<string, Dictionary<string, string>> IndexingCodeSets { get; set; }
        string PublisherName { get; set; }
        string PublisherGroupCode { get; set; }
        List<string> Ipds { get; set; }
        List<string> Ipcs { get; set; }
        string MimeType { get; set; }
    }
}