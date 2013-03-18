using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Schema;
using System.Xml.Serialization;
using Factiva.Gateway.Messages.Search.V2_0;

namespace DowJones.Managers.Search.Responses
{
    public struct Declarations
    {
        public const string SchemaVersion = "http://types.factiva.com/search";
    }

    public class AccessionNumberBasedContentItem : IComparable
    {

        [XmlElement(ElementName = "publicationDate", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "dateTime", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public DateTime __publicationDate;

        [XmlIgnore]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __publicationDateSpecified;

        [XmlIgnore]
        public DateTime PublicationDate
        {
            get { return __publicationDate; }
            set { __publicationDate = value; __publicationDateSpecified = true; }
        }

        [XmlIgnore]
        public DateTime PublicationDateUtc
        {
            get { return __publicationDate; }
            set { __publicationDate = value.ToLocalTime(); __publicationDateSpecified = true; }
        }

        [XmlElement(ElementName = "accessionNumber", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string __accessionNumber;

        [XmlIgnore]
        public string AccessionNumber
        {
            get { return __accessionNumber; }
            set { __accessionNumber = value; }
        }

        [XmlElement(ElementName = "hasBeenFound", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "boolean", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __hasBeenFound;

        [XmlIgnore]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __hasBeenFoundSpecified;

        [XmlIgnore]
        public bool HasBeenFound
        {
            get { return __hasBeenFound; }
            set { __hasBeenFound = value; __hasBeenFoundSpecified = true; }
        }

        public AccessionNumberBasedContentItem()
        {
            __publicationDate = DateTime.MinValue;
        }



        [XmlElement(Type = typeof(ContentHeadline), ElementName = "contentHeadline", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Factiva.Gateway.Messages.Search.V2_0.Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ContentHeadline __contentHeadline;

        [XmlIgnore]
        public ContentHeadline ContentHeadline
        {
            get
            {
                if (__contentHeadline == null) __contentHeadline = new ContentHeadline();
                return __contentHeadline;
            }
            set { __contentHeadline = value; }
        }

        #region IComparable Members

        ///<summary>
        ///Compares the current instance with another object of the same type.
        ///</summary>
        ///
        ///<returns>
        ///A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance is less than obj. Zero This instance is equal to obj. Greater than zero This instance is greater than obj. 
        ///</returns>
        ///
        ///<param name="obj">An object to compare with this instance. </param>
        ///<exception cref="T:System.ArgumentException">obj is not the same type as this instance. </exception><filterpriority>2</filterpriority>
        public int CompareTo(object obj)
        {
            AccessionNumberBasedContentItem _x = obj as AccessionNumberBasedContentItem;
            if (_x == null)
                throw new ArgumentException("obj must be of type AccessionNumberBasedContentItem.");
            return __publicationDate.CompareTo(_x.__publicationDate);
        }

        public int CompareTo(AccessionNumberBasedContentItem x)
        {
            if (x == null)
                throw new ArgumentException("x must be of type AccessionNumberBasedContentItem.");
            return __publicationDate.CompareTo(x.__publicationDate);
        }

        #endregion
    }

    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class AccessionNumberBasedContentItemCollection : List<AccessionNumberBasedContentItem>
    {
    }


    public class ContentCatagorizationSet
    {
        [XmlElement(Type = typeof(HeadlineRef), ElementName = "publicationRef", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public HeadlineRefCollection __publicationRefCollection;

        [XmlIgnore]
        public HeadlineRefCollection PublicationRefCollection
        {
            get
            {
                if (__publicationRefCollection == null) __publicationRefCollection = new HeadlineRefCollection();
                return __publicationRefCollection;
            }
            set { __publicationRefCollection = value; }
        }

        [XmlElement(Type = typeof(HeadlineRef), ElementName = "multimediaRef", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public HeadlineRefCollection __multiMediaRefCollection;

        [XmlIgnore]
        public HeadlineRefCollection MultimediaRefCollection
        {
            get
            {
                if (__multiMediaRefCollection == null) __multiMediaRefCollection = new HeadlineRefCollection();
                return __multiMediaRefCollection;
            }
            set { __multiMediaRefCollection = value; }
        }

        [XmlElement(Type = typeof(HeadlineRef), ElementName = "boardRef", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public HeadlineRefCollection __boardRefRefCollection;

        [XmlIgnore]
        public HeadlineRefCollection BoardRefCollection
        {
            get
            {
                if (__boardRefRefCollection == null) __boardRefRefCollection = new HeadlineRefCollection();
                return __boardRefRefCollection;
            }
            set { __boardRefRefCollection = value; }
        }

        [XmlElement(Type = typeof(HeadlineRef), ElementName = "blogRef", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public HeadlineRefCollection __blogRefRefCollection;

        [XmlIgnore]
        public HeadlineRefCollection BlogRefCollection
        {
            get
            {
                if (__blogRefRefCollection == null) __blogRefRefCollection = new HeadlineRefCollection();
                return __blogRefRefCollection;
            }
            set { __blogRefRefCollection = value; }
        }

        [XmlElement(Type = typeof(HeadlineRef), ElementName = "internalRef", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public HeadlineRefCollection __internalRefRefCollection;

        [XmlIgnore]
        public HeadlineRefCollection InternalRefCollection
        {
            get
            {
                if (__internalRefRefCollection == null) __internalRefRefCollection = new HeadlineRefCollection();
                return __internalRefRefCollection;
            }
            set { __internalRefRefCollection = value; }
        }

        [XmlElement(Type = typeof(HeadlineRef), ElementName = "customerDocRef", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public HeadlineRefCollection __customerDocRefRefCollection;

        [XmlIgnore]
        public HeadlineRefCollection CustomerDocRefCollection
        {
            get
            {
                if (__customerDocRefRefCollection == null) __customerDocRefRefCollection = new HeadlineRefCollection();
                return __customerDocRefRefCollection;
            }
            set { __customerDocRefRefCollection = value; }
        }

        [XmlElement(Type = typeof(HeadlineRef), ElementName = "summaryRef", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public HeadlineRefCollection __summaryRefRefCollection;

        [XmlIgnore]
        public HeadlineRefCollection SummaryRefCollection
        {
            get
            {
                if (__summaryRefRefCollection == null) __summaryRefRefCollection = new HeadlineRefCollection();
                return __summaryRefRefCollection;
            }
            set { __summaryRefRefCollection = value; }
        }

        [XmlElement(Type = typeof(HeadlineRef), ElementName = "pictureRef", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public HeadlineRefCollection __pictureRefCollection;

        [XmlIgnore]
        public HeadlineRefCollection PictureRefCollection
        {
            get
            {
                if (__pictureRefCollection == null) __pictureRefCollection = new HeadlineRefCollection();
                return __pictureRefCollection;
            }
            set { __pictureRefCollection = value; }
        }

        [XmlElement(Type = typeof(HeadlineRef), ElementName = "webSiteRef", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public HeadlineRefCollection __webSiteRefCollection;

        [XmlIgnore]
        public HeadlineRefCollection WebSiteRefCollection
        {
            get
            {
                if (__webSiteRefCollection == null) __webSiteRefCollection = new HeadlineRefCollection();
                return __webSiteRefCollection;
            }
            set { __webSiteRefCollection = value; }
        }

        [XmlElement(Type = typeof(HeadlineRef), ElementName = "unknownRef", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public HeadlineRefCollection __unknownRefCollection;

        [XmlIgnore]
        public HeadlineRefCollection UnknownRefCollection
        {
            get
            {
                if (__unknownRefCollection == null) __unknownRefCollection = new HeadlineRefCollection();
                return __unknownRefCollection;
            }
            set { __unknownRefCollection = value; }
        }
    }

    [XmlType(TypeName = "AccessionNumberBasedContentItemSet", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class AccessionNumberBasedContentItemSet
    {
        [XmlAttribute(AttributeName = "count", Form = XmlSchemaForm.Unqualified, DataType = "int", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int __count;

        [XmlIgnore]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __countSpecified;

        [XmlIgnore]
        public int Count
        {
            get { return __count; }
            set { __count = value; __countSpecified = true; }
        }

        [XmlElement(Type = typeof(AccessionNumberBasedContentItem), ElementName = "accessionNumberBasedContentItem", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public AccessionNumberBasedContentItemCollection __accessionNumberBasedContentItemCollection;

        [XmlIgnore]
        public AccessionNumberBasedContentItemCollection AccessionNumberBasedContentItemCollection
        {
            get
            {
                if (__accessionNumberBasedContentItemCollection == null) __accessionNumberBasedContentItemCollection = new AccessionNumberBasedContentItemCollection();
                return __accessionNumberBasedContentItemCollection;
            }
            set { __accessionNumberBasedContentItemCollection = value; }
        }
    }

    [XmlRoot(ElementName = "AccessionNumberSearchResponse", Namespace = Declarations.SchemaVersion, IsNullable = false), Serializable]
    public class AccessionNumberSearchResponse
    {

        [XmlElement(Type = typeof(AccessionNumberBasedContentItemSet), ElementName = "accessionNumberBasedContentItemSet", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public AccessionNumberBasedContentItemSet __accessionNumberBasedContentItemSet;

        [XmlIgnore]
        public AccessionNumberBasedContentItemSet AccessionNumberBasedContentItemSet
        {
            get
            {
                if (__accessionNumberBasedContentItemSet == null) __accessionNumberBasedContentItemSet = new AccessionNumberBasedContentItemSet();
                return __accessionNumberBasedContentItemSet;
            }
            set { __accessionNumberBasedContentItemSet = value; }
        }

        [XmlElement(Type = typeof(CollectionCountSet), ElementName = "collectionCountSet", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public CollectionCountSet __collectionCountSet;

        [XmlIgnore]
        public CollectionCountSet CollectionCountSet
        {
            get
            {
                if (__collectionCountSet == null) __collectionCountSet = new CollectionCountSet();
                return __collectionCountSet;
            }
            set { __collectionCountSet = value; }
        }

        [XmlElement(Type = typeof(NavigatorSet), ElementName = "codeNavigatorSet", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public NavigatorSet __codeNavigatorSet;

        [XmlIgnore]
        public NavigatorSet CodeNavigatorSet
        {
            get
            {
                if (__codeNavigatorSet == null) __codeNavigatorSet = new NavigatorSet();
                return __codeNavigatorSet;
            }
            set { __codeNavigatorSet = value; }
        }

        [XmlElement(Type = typeof(NavigatorSet), ElementName = "timeNavigatorSet", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public NavigatorSet __timeNavigatorSet;

        [XmlIgnore]
        public NavigatorSet TimeNavigatorSet
        {
            get
            {
                if (__timeNavigatorSet == null) __timeNavigatorSet = new NavigatorSet();
                return __timeNavigatorSet;
            }
            set { __timeNavigatorSet = value; }
        }

        [XmlElement(Type = typeof(NavigatorSet), ElementName = "contextualNavigatorSet", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public NavigatorSet __contextualNavigatorSet;

        [XmlIgnore]
        public NavigatorSet ContextualNavigatorSet
        {
            get
            {
                if (__contextualNavigatorSet == null) __contextualNavigatorSet = new NavigatorSet();
                return __contextualNavigatorSet;
            }
            set { __contextualNavigatorSet = value; }
        }

        [XmlElement(Type = typeof(KeywordSet), ElementName = "keywordSet", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public KeywordSet __keywordSet;

        [XmlIgnore]
        public KeywordSet KeywordSet
        {
            get
            {
                if (__keywordSet == null) __keywordSet = new KeywordSet();
                return __keywordSet;
            }
            set { __keywordSet = value; }
        }

        [XmlElement(Type = typeof(ClusterSet), ElementName = "clusterSet", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ClusterSet __clusterSet;

        [XmlIgnore]
        public ClusterSet ClusterSet
        {
            get
            {
                if (__clusterSet == null) __clusterSet = new ClusterSet();
                return __clusterSet;
            }
            set { __clusterSet = value; }
        }


        [XmlElement(Type = typeof(ContentCatagorizationSet), ElementName = "contentCatagorizationSet", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ContentCatagorizationSet __contentCatagorizationSet;

        [XmlIgnore]
        public ContentCatagorizationSet ContentCatagorizationSet
        {
            get
            {
                if (__contentCatagorizationSet == null) __contentCatagorizationSet = new ContentCatagorizationSet();
                return __contentCatagorizationSet;
            }
            set { __contentCatagorizationSet = value; }
        }
    }
}
