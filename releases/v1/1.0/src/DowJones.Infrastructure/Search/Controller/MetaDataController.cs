using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Factiva.Gateway.Messages.Search.V2_0;

namespace DowJones.Utilities.Search.Controller
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

    public class MetaDataController
    {
        protected const int DefaultMaxKeyWords = 10;
        protected const int DefaultMaxBuckets = 10;
        protected const int DefaultMinBucketValue = 1;
        protected const double DefaultMinWeightKeywords = 0.9;

        //public NavigatorControllerMode Mode;
        public CodeNavigatorMode Mode;
        public string[] CustomCodeNavigatorIds;

        // contextual
        public string[] CustomContextualNavigatorIds;
        public bool CountCustomContextualNavigatorIdsOncePerDocument = true;

        // time 
        public TimeNavigatorMode TimeNavigatorMode = TimeNavigatorMode.PublicationDate;

        // return collection counts
        public bool ReturnCollectionCounts = true;

        // return keyword set
        public bool ReturnKeywordsSet = true;
        public int MaxKeywords = DefaultMaxKeyWords;
        public double MinWeightKeywords = DefaultMinWeightKeywords;


        // Goes across all buckets
        public int MinBucketValue = DefaultMinBucketValue;
        public int MaxBuckets = DefaultMaxBuckets;


        public static string[] GetNavigatorCodeValue(NavigatorCode[] navCodes)
        {
            return navCodes.Select(navCode => navCode == NavigatorCode.CO_OCCUR ? "co:occur" : navCode.ToString().ToLower()).ToArray();
        }
    }
}
