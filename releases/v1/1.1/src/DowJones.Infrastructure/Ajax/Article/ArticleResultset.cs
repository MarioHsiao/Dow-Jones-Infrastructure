using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DowJones.Ajax.Article
{
    public class ArticleResultset
    {
        public string AccessionNo { get; set; }

        public List<IRenderItem> ArticleHead { get; set; }

        public List<IRenderItem> ArticleSource { get; set; }

        public string ArticlePublicationDate { get; set; }

        public string ArticlePublicationTime { get; set; }

        public List<IRenderItem> ArticleHeadline { get; set; }

        public List<IRenderItem> ArticleCopyright { get; set; } 

        public List<Dictionary<string, List<IRenderItem>>> ArticleCorrection { get; set; }

        public List<Dictionary<string, List<IRenderItem>>> ArticleLeadParagraph { get; set; }

        public List<Dictionary<string, List<IRenderItem>>> ArticleTailParagraph { get; set; }

        public List<IRenderItem> ArticleContact { get; set; }

        public List<IRenderItem> ArticleNotes { get; set; }

        public List<IRenderItem> ArticleArtWorks { get; set; }
    }
}
