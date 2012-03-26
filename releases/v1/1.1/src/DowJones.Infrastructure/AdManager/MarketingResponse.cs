using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace DowJones.Utilities.AdManager
{
    /// <remarks/>
    [GeneratedCode("System.Xml", "2.0.50727.832")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public partial class MarketingResponse
    {
        private marketingImageContainer imageContainerField;

        private marketingWhatsnew whatsnewField;

        private marketingAdContainer adContainerField;

        /// <remarks/>
        public marketingImageContainer ImageContainer
        {
            get { return imageContainerField; }
            set { imageContainerField = value; }
        }

        /// <remarks/>
        [XmlElement("Whats-New")]
        public marketingWhatsnew WhatsNew
        {
            get { return whatsnewField; }
            set { whatsnewField = value; }
        }

        /// <remarks/>
        public marketingAdContainer AdContainer
        {
            get { return adContainerField; }
            set { adContainerField = value; }
        }
    }

    /// <remarks/>
    [GeneratedCode("System.Xml", "2.0.50727.832")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public partial class marketingImageContainer
    {
        private marketingImageContainerContent contentField;

        private string titleField;

        private string srcField;

        /// <remarks/>
        public marketingImageContainerContent Content
        {
            get { return contentField; }
            set { contentField = value; }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string title
        {
            get { return titleField; }
            set { titleField = value; }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string src
        {
            get { return srcField; }
            set { srcField = value; }
        }
    }

    /// <remarks/>
    [GeneratedCode("System.Xml", "2.0.50727.832")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public partial class marketingImageContainerContent
    {
        private string titleField;

        private string paragraphField;

        private marketingImageContainerContentFindoutMoreLink findoutMoreLinkField;

        /// <remarks/>
        public string Title
        {
            get { return titleField; }
            set { titleField = value; }
        }

        /// <remarks/>
        public string Paragraph
        {
            get { return paragraphField; }
            set { paragraphField = value; }
        }

        /// <remarks/>
        public marketingImageContainerContentFindoutMoreLink FindoutMoreLink
        {
            get { return findoutMoreLinkField; }
            set { findoutMoreLinkField = value; }
        }
    }

    /// <remarks/>
    [GeneratedCode("System.Xml", "2.0.50727.832")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public partial class marketingImageContainerContentFindoutMoreLink
    {
        private string urlField;

        private string textField;

        /// <remarks/>
        [XmlAttribute()]
        public string url
        {
            get { return urlField; }
            set { urlField = value; }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string text
        {
            get { return textField; }
            set { textField = value; }
        }
    }

    /// <remarks/>
    [GeneratedCode("System.Xml", "2.0.50727.832")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public partial class marketingWhatsnew
    {
        private marketingWhatsnewSubmenulink[] submenulinkField;

        private string idField;

        private string laField;

        /// <remarks/>
        [XmlElement("Sub-Menu-Link")]
        public marketingWhatsnewSubmenulink[] SubMenuLink
        {
            get { return submenulinkField; }
            set { submenulinkField = value; }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string id
        {
            get { return idField; }
            set { idField = value; }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string la
        {
            get { return laField; }
            set { laField = value; }
        }
    }

    /// <remarks/>
    [GeneratedCode("System.Xml", "2.0.50727.832")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public partial class marketingWhatsnewSubmenulink
    {
        private string idField;

        private string nameField;

        private string relativeField;

        private string newBrowserField;

        private string urlField;

        /// <remarks/>
        [XmlAttribute()]
        public string id
        {
            get { return idField; }
            set { idField = value; }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string name
        {
            get { return nameField; }
            set { nameField = value; }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string relative
        {
            get { return relativeField; }
            set { relativeField = value; }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string newBrowser
        {
            get { return newBrowserField; }
            set { newBrowserField = value; }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string url
        {
            get { return urlField; }
            set { urlField = value; }
        }
    }

    /// <remarks/>
    [GeneratedCode("System.Xml", "2.0.50727.832")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public partial class marketingAdContainer
    {
        private marketingAdContainerResultset resultsetField;

        /// <remarks/>
        public marketingAdContainerResultset Resultset
        {
            get { return resultsetField; }
            set { resultsetField = value; }
        }
    }

    /// <remarks/>
    [GeneratedCode("System.Xml", "2.0.50727.832")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public partial class marketingAdContainerResultset
    {
        private marketingAdContainerResultsetResult resultField;

        private byte countField;

        /// <remarks/>
        public marketingAdContainerResultsetResult Result
        {
            get { return resultField; }
            set { resultField = value; }
        }

        /// <remarks/>
        [XmlAttribute()]
        public byte count
        {
            get { return countField; }
            set { countField = value; }
        }
    }

    /// <remarks/>
    [GeneratedCode("System.Xml", "2.0.50727.832")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public partial class marketingAdContainerResultsetResult
    {
        private string languageField;

        private string productField;

        private string sizeField;

        private string titleField;

        private string landingpageUrlField;

        private string imageUrlField;

        /// <remarks/>
        public string Language
        {
            get { return languageField; }
            set { languageField = value; }
        }

        /// <remarks/>
        public string Product
        {
            get { return productField; }
            set { productField = value; }
        }

        /// <remarks/>
        public string Size
        {
            get { return sizeField; }
            set { sizeField = value; }
        }

        /// <remarks/>
        public string Title
        {
            get { return titleField; }
            set { titleField = value; }
        }

        /// <remarks/>
        public string LandingpageUrl
        {
            get { return landingpageUrlField; }
            set { landingpageUrlField = value; }
        }

        /// <remarks/>
        public string ImageUrl
        {
            get { return imageUrlField; }
            set { imageUrlField = value; }
        }
    }
}