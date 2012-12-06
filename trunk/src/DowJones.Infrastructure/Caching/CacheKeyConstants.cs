namespace DowJones.Caching
{
    public class CacheKeyConstants
    {
        public const string Name = "n";
        public const string Product = "p";
        public const string PageId = "pid";
        public const string ModuleId = "mid";
        public const string RootId = "rid";
        public const string InterfaceLanguage = "il";
        public const string RootAccessControlScope = "racs";
        public const string PageAccessControlScope = "pacs";
        public const string PageAccessQualifier = "paq";
        public const string CacheScope = "cs";
        public const string ContentLanguages = "cl";
        public const string NumberOfHeadlines = "nhr";
        public const string Guid = "guid";
        public const string MaxHeadlinesToReturn = "mhr";
        public const string TimeFrame = "tf";
        public const string TimePeriod = "tp";
        public const string SectionIds = "secids";
        public const string FCodes = "codes";
        public const string ReportType = "rt";
        public const string RegionCode = "rc";
        public const string Version = "v";
        public const string Enviroment = "e";
        public const string EntityType = "et";
        public const string ShareScopeCollection = "ssc";
        public const string Date = "d";


        public static int DefaultCacheExpirationTime
        {
            get { return Settings.Default.DefaultCacheExpirationTime; }
        }

        public static int DefaultCacheRefreshInterval
        {
            get { return Settings.Default.DefaultCacheRefreshInterval; }
        }

        public static CacheExiprationPolicy DefaultCacheExiprationPolicy
        {
            get { return Settings.Default.DefaultCacheExpirationPolicy; }
        }

        public static bool IncludeCacheKeyGeneration
        {
            get { return Settings.Default.IncludeCacheKeyGeneration; }
        }

    }
}