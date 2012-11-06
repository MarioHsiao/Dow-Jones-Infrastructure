namespace DowJones.API.Common.Utilities
{
    public class ApiFramework
    {
        public static bool PimLoggingEnabled = true;
        public static bool OverrideHttpStatus;
        public static bool AuthenticationEnabled = true;
        public static string RestStubsFolderPath = "RestStubs";
        public static bool StubEnabled = false;
        public static bool ProxyThirdPartyContent = false;
        public static string ContentProxyUrl = "";

        public static string MadeClient = "";
        public static string MadeTransportType = "rts";
        public static string ProxyUrl = "";
        public static string SearchVersion = "";

        public static string JsonContentType = "application/json; charset=utf-8";
        public static string JsonContentWithCallbackType = "text/javascript; charset=utf-8";
        public static string XmlContentType = "text/xml; charset=utf-8";

        public static string AlertRssUrl = "";
        
        public static string FactivaRampUrl = "" ;
        public static string DataServiceRampUrl = "";

        public static string UtilityServerGetItemUrl = "";
        public static string SourceAttributesProviderPath = @"Assets\SourceAttributesProvider.xml";

        public static bool CreateLoginFeedEnabled = true;
        public static bool SingleSessionInstance = false;
        public static string ChartApiCacheExpiryInMinutes = "";
        public static string ChartApiCacheRefreshInMinutes = "";
        public static string ChartApiCachePrefix = "";

    }
}
