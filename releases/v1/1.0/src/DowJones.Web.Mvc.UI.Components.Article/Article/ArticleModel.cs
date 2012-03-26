using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Web.Mvc;
namespace DowJones.Web.Mvc.UI.Components.Models.Article
{
    public class ArticleModel// : ViewComponentModel
    {

        string _publicationDateFormat = "MM/dd/yyyy";

        #region Client Properties

        [DowJones.Web.Mvc.UI.ClientProperty("publicationDateFormat")]
        public string PublicationDateFormat
        {
            get { return _publicationDateFormat; }
            set { _publicationDateFormat = value; }
        }

        //[ClientProperty("showSourceLinks")]
        //public bool ShowSourceLinks { get; set; }

        //[ClientProperty("enableELinks")]
        //public bool EnableELinks { get; set; }

        //[ClientProperty("showCompanyEntityReference")]
        //public bool ShowCompanyEntityReference { get; set; }

        //[ClientProperty("showExecutiveEntityReference")]
        //public bool ShowExecutiveEntityReference { get; set; }

        //[ClientProperty("showHighlighting")]
        //public bool ShowHighlighting { get; set; }

        //[ClientProperty("showReadSpeaker")]
        //public bool ShowReadSpeaker { get; set; }

        //[ClientProperty("showSocialButtons")]
        //public bool ShowSocialButtons { get; set; }

        //[ClientProperty("showTranslator")]
        //public bool ShowTranslator { get; set; }

        [ClientProperty("showArticleLogo")]
        public bool ShowArticleLogo { get; set; }
        #endregion

        public string AccessionNumber { get; set; }
        public string ArticleLogoUrl { get; set; }
        public string Headline { get; set; }
        public DateTime PublicationDate { get; set; }
        public string PublicationDateDisplay { get { return this.PublicationDate.ToString(_publicationDateFormat); } }
        public string Author { get; set; }
        public string Byline { get; set; }
        public string Body { get; set; }
        public string Source { get; set; }
        public string SourceCode { get; set; }

        public ArticleModel()
        {

        }
        public ArticleModel(DowJones.Tools.Ajax.Article.ArticleDataResult result)
        {            
            this.AccessionNumber = result.AccessionNumber;
            this.Byline = result.ByLine;
            this.Headline = result.Headline;
            this.PublicationDate = result.PublicationDate;
            this.Source = result.SourceName;
            this.SourceCode = result.SourceCode;
            this.Body = result.BodyHtml;
            this.ArticleLogoUrl = result.Logo;
        }
    }
}
