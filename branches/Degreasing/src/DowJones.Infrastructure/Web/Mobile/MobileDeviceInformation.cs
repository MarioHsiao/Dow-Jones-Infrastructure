using System.Text.RegularExpressions;

namespace DowJones.Web.Mobile
{
    public partial class MobileDeviceInformation
    {
        public const string ViewDataKey = "MobileDeviceInfo";
        public const string GenericCategory = "Generic";
        public const string GenericFolderName = "Mobile";

        public string Category { get; private set; }

        public string Platform { get; private set; }

        internal Regex UserAgentExpression { get; set; }

        public bool IsGeneric
        {
            get { return Category == GenericCategory; }
        }

        public string FolderName
        {
            get { return (IsGeneric) ? GenericFolderName : Platform; }
        }


        private MobileDeviceInformation()
        {
            Category = GenericCategory;
            Platform = string.Empty;
            UserAgentExpression = new Regex(string.Empty);
        }

        public MobileDeviceInformation(string platform, string category = GenericCategory)
        {
            Category = category;
            Platform = platform;
            UserAgentExpression = new Regex(string.Empty);
        }

        public bool MatchesUserAgent(string userAgent)
        {
            return UserAgentExpression.IsMatch(userAgent);
        }

        public override string ToString()
        {
            return Platform;
        }
    }
}