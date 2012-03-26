using EMG.Utility.Search.Controller;
using Factiva.Gateway.Messages.Search.V2_0;

namespace EMG.Utility.Search.Core
{
    internal class Utility
    {
        public static NavigationControl GenerateNavigationControl( MetaDataController metaDataController)
        {
            NavigationControl objNavigationControl = new NavigationControl();

            CodeNavigatorControl objCodeNavigatorControl = new CodeNavigatorControl();
            objCodeNavigatorControl.MaxBuckets = metaDataController.MaxBuckets;
            objCodeNavigatorControl.MinBucketValue = metaDataController.MinBucketValue;
            objCodeNavigatorControl.Mode = metaDataController.Mode;
            objNavigationControl.CodeNavigatorControl = objCodeNavigatorControl;

            ContextualNavigatorControl objContextualNavigatorControl = new ContextualNavigatorControl();
            objContextualNavigatorControl.CountOncePerDocument = true;
            objContextualNavigatorControl.CountOnlyMatchingTerms = true;
            objContextualNavigatorControl.Id = "ContextualNavigatorControl1";
            objContextualNavigatorControl.MaxBuckets = metaDataController.MaxBuckets;
            objContextualNavigatorControl.MinBucketValue = metaDataController.MinBucketValue;
            objNavigationControl.ContextualNavigatorControlCollection.Add(objContextualNavigatorControl);

            KeywordControl objKeywordControl = new KeywordControl();
            objKeywordControl.MaxKeywords = metaDataController.MaxKeywords;
            objKeywordControl.MinWeight = (float) metaDataController.MinWeightKeywords;
            objKeywordControl.ReturnKeywords = metaDataController.ReturnKeywordsSet;
            objNavigationControl.KeywordControl = objKeywordControl;

            objNavigationControl.ReturnCollectionCounts = metaDataController.ReturnCollectionCounts;
            objNavigationControl.TimeNavigatorMode = metaDataController.TimeNavigatorMode;
            return objNavigationControl;
        }
    }
}