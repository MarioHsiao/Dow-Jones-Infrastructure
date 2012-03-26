using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DowJones.Web.Mvc.UI.Components.Models.HeadlineList
{
    public class HeadlineModel
    {
        string _publicationDateFormat = "MM/dd/yyyy";
        [ClientProperty("publicationDateFormat")]
        public string PublicationDateFormatString
        {
            get { return _publicationDateFormat; }
            set { _publicationDateFormat = value; }
        }

        public string AccessionNumber { get; set; }
        public string ParentAccessionNumber { get; set; }
        public string ExternalUrl { get; set; }
        public string Title { get; set; }
        public string Source { get; set; }
        public string SourceCode { get; set; }
        public DateTime PublicationDate { get; set; }
        public string PublicationDateDisplay { get { return this.PublicationDate.ToString(_publicationDateFormat); } }
        public string Author { get; set; }
        public string Byline {get;set;}
        public string Snippet { get; set; }
        public List<HeadlineModel> DuplicateHeadlines { get; set; }

        public HeadlineModel()
        {
            this.DuplicateHeadlines = new List<HeadlineModel>();
        }
        public HeadlineModel(DowJones.Tools.Ajax.HeadlineList.HeadlineInfo headline): this()
        {
            this.ExternalUrl = headline.reference.externalUri;
            this.Byline = Map(headline.byline);
            this.Title = Map(headline.title);
            this.PublicationDate = headline.publicationDateTime;
            this.Snippet = Map(headline.snippet);
            this.Source = headline.sourceDescriptor;
            this.SourceCode = headline.sourceReference;
            this.AccessionNumber = headline.reference.guid.ToString();
            
            if(headline.duplicateHeadlines != null)
            {
                headline.duplicateHeadlines.ForEach(dh => this.DuplicateHeadlines.Add(new HeadlineModel(dh)));
            }
        }
        private HeadlineModel(DowJones.Tools.Ajax.HeadlineList.DedupHeadlineInfo headline)
        {
            this.ExternalUrl = headline.reference.externalUri;
            this.Byline = Map(headline.byline);
            this.Title = Map(headline.title);
            this.PublicationDate = headline.publicationDateTime;
            this.Snippet = Map(headline.snippet);
            this.Source = headline.sourceDescriptor;
            this.AccessionNumber = headline.reference.guid.ToString();
            this.ParentAccessionNumber = headline.ParentAccessionNo;
        }
        private string Map(List<DowJones.Tools.Ajax.HeadlineList.Para> paras)
        {

            if (paras == null || paras.Count() == 0) return "";
            StringBuilder sb = new StringBuilder();
            paras.ForEach(p => p.items.ForEach(m => sb.Append(StripHTML(m.value + " "))));
            return sb.ToString();
                 
        }
        // De-HTMLize the title
        private static readonly Regex HTMLTags = new Regex("<[^>]*>");
        private static string StripHTML(string markup)
        {
            return string.IsNullOrWhiteSpace(markup) ? string.Empty : HTMLTags.Replace(markup, string.Empty);
        }
    }
}
