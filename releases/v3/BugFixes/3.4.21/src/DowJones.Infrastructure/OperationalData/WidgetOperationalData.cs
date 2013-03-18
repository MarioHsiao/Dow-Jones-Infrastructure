namespace EMG.Utility.OperationalData
{
    public class WidgetOperationalData : AlertOperationalData
    {
        /// <summary>
        /// Widget ID that the user is viewing
        /// </summary>
        public string WidgetID
        {
            get { return Get(ODSConstants.KEY_WIDGET_ID); }
            set { Add(ODSConstants.KEY_WIDGET_ID, value); }
        }

        /// <summary>
        /// Gets or sets the name of the widget.
        /// </summary>
        /// <value>The name of the widget.</value>
        public string WidgetName
        {
            get { return Get(ODSConstants.KEY_WIDGET_NAME); }
            set { Add(ODSConstants.KEY_WIDGET_NAME, value); }
        }

        /// <summary>
        /// Userid of the publisher/creator
        /// </summary>
        public string PublisherID
        {
            get { return Get(ODSConstants.KEY_USER_ID); }
            set { Add(ODSConstants.KEY_USER_ID, value); }
        }

        /// <summary>
        /// Namespace of the publisher/creator
        /// </summary>
        public string PublisherNamespace
        {
            get { return Get(ODSConstants.KEY_USER_NAMESPACE); }
            set { Add(ODSConstants.KEY_USER_NAMESPACE, value); }
        }

        /// <summary>
        /// Widget Publishing Domain 
        /// </summary>
        public string PublishingDomain
        {
            get { return Get(ODSConstants.KEY_DOMAIN); }
            set { Add(ODSConstants.KEY_DOMAIN, value); }
        }

        /// <summary>
        /// Dissemination Option * xrdr requires additional parameters, see External Reader section
        /// inacct, outacct, xrdr
        /// </summary>
        public string DisseminationOption
        {
            get { return Get(ODSConstants.KEY_DISSEMINATION); }
            set { Add(ODSConstants.KEY_DISSEMINATION, value); }
        }

        /// <summary>
        /// Headline format HeadlineOnly or Headline+Snippet
        /// </summary>
        public string HeadlineFormat
        {
            get { return Get(ODSConstants.KEY_HEADLINE_VIEW); }
            set { Add(ODSConstants.KEY_HEADLINE_VIEW, value); }
        }

        /// <summary>
        /// Folder count
        /// </summary>
        public string FolderCount
        {
            get { return Get(ODSConstants.KEY_ALERT_COUNT); }
            set { Add(ODSConstants.KEY_ALERT_COUNT, value); }
        }
    }
}