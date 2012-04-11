using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DowJones.Ajax.HeadlineList;

namespace DowJones.Web.Mvc.UI.Components.HeadlineList
{
    public class HeadlineModel
    {
        [ClientProperty("headlineInfo")]
        public Reference HeadlineInfo { get; set; }

        public string AccessionNumber { get; set; }
        public string ParentAccessionNumber { get; set; }
        public string ExternalUrl { get; set; }
        public string Title { get; set; }
        public string Source { get; set; }
        public string SourceCode { get; set; }
        public DateTime PublicationDate { get; set; }
        public string PublicationDateDisplay { get; set;}
        public IEnumerable<Author> Authors { get; set; }
        public string Byline { get; set; }
        public string Snippet { get; set; }
        public string WordCount { get; set; }
        public string Language { get; set; }
        public IEnumerable<HeadlineModel> DuplicateHeadlines { get; set; }
        public bool IsLocked { get; set; }
        public bool IsValid { get; set; }
        public string ThumbnailUrl { get; set; }
        public string TruncatedTitle { get; set; }

        public bool HasThumbnail
        {
            get { return !string.IsNullOrEmpty(ThumbnailUrl); }
        }
        public bool IsPressClipArticle
        {
            get
            {
                return HeadlineInfo != null 
                       && !string.IsNullOrEmpty(HeadlineInfo.subType)
                       && HeadlineInfo.subType.Equals("nlapressclip", StringComparison.InvariantCultureIgnoreCase);
            }
        }
        public bool HasDuplicates
        {
            get { return DuplicateHeadlines != null && DuplicateHeadlines.Any(); }
        }

        public HeadlineModel()
        {
            IsValid = true;
            DuplicateHeadlines = Enumerable.Empty<HeadlineModel>();
            Authors = Enumerable.Empty<Author>();

        }

        public HeadlineModel(HeadlineInfo headline)
            : this()
        {
            HeadlineInfo = headline.reference;
            ExternalUrl = headline.reference.externalUri;
            Byline = Map(headline.byline);
            Title = Map(headline.title);
            PublicationDate = headline.publicationDateTime;
            PublicationDateDisplay = headline.publicationDateTimeDescriptor;
            Snippet = Map(headline.snippet);
            Source = headline.sourceDescriptor;
            SourceCode = headline.sourceReference;
            AccessionNumber = headline.reference.guid;
            WordCount = headline.wordCountDescriptor;
            Language = headline.baseLanguageDescriptor;
            IsLocked = !headline.isFree;
            IsValid = headline.isValid;
            ThumbnailUrl = headline.thumbnailImage != null ? headline.thumbnailImage.SRC : string.Empty;

            var authorCodes = headline.author ?? Enumerable.Empty<Code>();
            if (authorCodes.Any())
                Authors = authorCodes.Select(a => new Author() { Name = a.value, Code = a.id });
            else if (headline.byline != null
                && headline.byline.Count > 0
                && headline.byline[0].items.Count > 0)
            {
                Authors = (headline.byline).SelectMany(b => b.items).Select(m => new Author { Name = m.value, Code = m.guid });
            }


            if (headline.duplicateHeadlines != null && headline.duplicateHeadlines.Count > 0)
                DuplicateHeadlines = headline.duplicateHeadlines.Select(dh => new HeadlineModel(dh));

        }

        private string GetThumbnailUrl(ThumbnailImage thumbnailImage)
        {
            if (thumbnailImage == null)
                return null;

            return thumbnailImage.SRC;
        }

        private HeadlineModel(DedupHeadlineInfo headline) 
            : this()
        {
            ExternalUrl = headline.reference.externalUri;
            Byline = Map(headline.byline);
            Title = Map(headline.title);
            PublicationDate = headline.publicationDateTime;
            PublicationDateDisplay = headline.publicationDateTimeDescriptor;
            Snippet = Map(headline.snippet);
            Source = headline.sourceDescriptor;
            AccessionNumber = headline.reference.guid;
            ParentAccessionNumber = headline.ParentAccessionNo;
            HeadlineInfo = headline.reference;
            WordCount = headline.wordCountDescriptor;
            Language = headline.baseLanguageDescriptor;
            TruncatedTitle = headline.truncatedTitle;
            IsLocked = !headline.isFree;
            IsValid = headline.isValid;
            ThumbnailUrl = headline.thumbnailImage != null ? headline.thumbnailImage.SRC : string.Empty;
        }

        private static string Map(List<Para> paras)
        {

            if (paras == null || paras.Count() == 0) return "";
            var sb = new StringBuilder();
            paras.ForEach(p => p.items.ForEach(m => sb.Append(StripHtml(m.value + " "))));
            return sb.ToString().Trim();

        }

        // De-HTMLize the title
        private static readonly Regex HtmlTags = new Regex("<[^>]*>");

        private static string StripHtml(string markup)
        {
            return string.IsNullOrWhiteSpace(markup) ? string.Empty : HtmlTags.Replace(markup, string.Empty);
        }


        
    }
}
