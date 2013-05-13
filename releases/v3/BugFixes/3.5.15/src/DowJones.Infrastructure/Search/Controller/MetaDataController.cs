using System;
using System.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using Factiva.Gateway.Messages.Search.V2_0;

namespace DowJones.Search.Controller
{
    public enum NavigatorCode
    {
        CO,
        IN,
        NS,
        RE,
        SC,
        PE,
        CO_OCCUR,
    }

    [XmlType(Namespace = Declarations.SchemaVersion, TypeName = "MetaDataController")]
    [Serializable]
    public class MetaDataController
    {
        protected const int DefaultMaxKeyWords = 10;
        protected const int DefaultMaxBuckets = 10;
        protected const int DefaultMinBucketValue = 1;
        protected const double DefaultMinWeightKeywords = 0.9;

        //public NavigatorControllerMode Mode;
        [XmlElement(ElementName = "mode", Namespace = Declarations.SchemaVersion, Form = XmlSchemaForm.Qualified)]
        public CodeNavigatorMode Mode;

        [XmlElement(ElementName = "customCodeNavigatorIds", Type = typeof(string[]), Namespace = Declarations.SchemaVersion, Form = XmlSchemaForm.Qualified)]
        public string[] CustomCodeNavigatorIds;

        // contextual
        [XmlElement(ElementName = "customContextualNavigatorIds", Type = typeof(string[]), Namespace = Declarations.SchemaVersion, Form = XmlSchemaForm.Qualified)]
        public string[] CustomContextualNavigatorIds;

        [XmlElement(ElementName = "countCustomContextualNavigatorIdsOncePerDocument", Namespace = Declarations.SchemaVersion, Form = XmlSchemaForm.Qualified)]
        public bool CountCustomContextualNavigatorIdsOncePerDocument = true;

        // time 
        [XmlElement(ElementName = "timeNavigatorMode", Namespace = Declarations.SchemaVersion, Form = XmlSchemaForm.Qualified)]
        public TimeNavigatorMode TimeNavigatorMode = TimeNavigatorMode.PublicationDate;

        // return collection counts
        [XmlElement(ElementName = "returnCollectionCounts", Namespace = Declarations.SchemaVersion, Form = XmlSchemaForm.Qualified)]
        public bool ReturnCollectionCounts = true;

        // return keyword set
        [XmlElement(ElementName = "returnKeywordsSet", Namespace = Declarations.SchemaVersion, Form = XmlSchemaForm.Qualified)]
        public bool ReturnKeywordsSet = true;

        [XmlElement(ElementName = "maxKeywords", Namespace = Declarations.SchemaVersion, Form = XmlSchemaForm.Qualified)]
        public int MaxKeywords = DefaultMaxKeyWords;

        [XmlElement(ElementName = "minWeightKeywords", Namespace = Declarations.SchemaVersion, Form = XmlSchemaForm.Qualified)]
        public double MinWeightKeywords = DefaultMinWeightKeywords;

        // Goes across all buckets
        [XmlElement(ElementName = "minBucketValue", Namespace = Declarations.SchemaVersion, Form = XmlSchemaForm.Qualified)]
        public int MinBucketValue = DefaultMinBucketValue;

        [XmlElement(ElementName = "maxBuckets", Namespace = Declarations.SchemaVersion, Form = XmlSchemaForm.Qualified)]
        public int MaxBuckets = DefaultMaxBuckets;


        public static string[] GetNavigatorCodeValue(NavigatorCode[] navCodes)
        {
            return navCodes.Select(navCode => navCode == NavigatorCode.CO_OCCUR ? "co:occur" : navCode.ToString().ToLower()).ToArray();
        }
    }
}
