namespace DowJones.Utilities.OperationalData.EntryPoint
{
    public class RssOperationalData : CoreAlertOperationalData
    {
        /// <summary>
        /// Editor's choice feed name
        /// </summary>
        public string EditorChoiceFeedName
        {
            get { return Get(ODSConstants.KEY_FEED_NAME); }
            set { Add(ODSConstants.KEY_FEED_NAME, value); }
        }

        /// <summary>
        /// Public or Private RSS alert
        /// </summary>
        public string RssType
        {
            get { return Get(ODSConstants.KEY_RSS_TYPE); }
            set { Add(ODSConstants.KEY_RSS_TYPE, value); }
        }
    }
}