using System;

namespace DowJones.Ajax.Article
{
    public class ArticleDataResult
    {
        public string AccessionNumber { get; set; }
        public int WordCount { get; set; }
        public string BodyHtml { get; set; }
        public string Logo { get; set; }
        public string Headline { get; set; }
        public string BaseLanguage { get; set; }
        public string Volume { get; set; }
        public DateTime PublicationDate { get; set; }
        public string SourceName { get; set; }
        public string SourceCode { get; set; }
        public string PublisherGroupName { get; set; }
        public string PublisherGroupCode { get; set; }
        public string[] Pages { get; set; }
        public string Edition { get; set; }
        public string ColumnName { get; set; }
        public string SectionName { get; set; }
        public string ByLine { get; set; }
        public string Credit { get; set; }
        public string Copyright { get; set; }
        
    }
}
