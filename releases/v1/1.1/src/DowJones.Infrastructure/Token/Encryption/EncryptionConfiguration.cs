namespace DowJones.Utilities.TokenEncryption
{
    internal class EncryptionConfiguration
    {
        public static readonly string ACCOUNT_ID = "accountId";
                
        public static readonly string NAMESPACE = "namespace";

        public static readonly string USER_ID = "userId";

        public static readonly string ACCESSION_NUMBER = "accNo";

        public static readonly string CONTENT_LANGUAGE = "lang";

        public static readonly string CONTENT_CATEGORY = "cat";

        public static readonly string TIME_TO_LIVE = "ttl";

        public static readonly string INCLUDE_MARKETING_MESSAGE = "imm";

        public static readonly string OPERATIONAL_DATA_STRING = "ods";

        public static readonly string ACCESS_POINT_CODE = "napc";

        public static readonly string MEDIA_REDIRECTION_TYPE = "mrt";

        public static readonly string DEVICE = "dvc";

        public static readonly string CLIENT_TYPE = "ct";

        public static readonly string PRODUCT_TYPE = "pt";
    }

    internal class EID4Configuration
    {
        public static readonly string PROXY_TOKEN = "proxyxsid";

        public static readonly string ACCESSION_NUMBER = "an";
    }

    internal class KeyConfiguration
    {
        public static readonly string DEFAULT_ENCRYPTION_KEY = "dharma";
        public static readonly string EID4_TTL_PROXY_ENCRYPTION_KEY = "FRGKA8384";
    }
}