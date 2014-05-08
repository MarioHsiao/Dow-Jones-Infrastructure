namespace EMG.Utility.OperationalData
{
    public class NewsletterArticleOperationalData : AbstractOperationalData
    {
        /// <summary>
        /// Newsletter format (html | PDF | RTF | Mobile)
        /// </summary>
        public string NewsletterFormat
        {
            get { return Get(ODSConstants.KEY_FORMAT); }
            set { Add(ODSConstants.KEY_FORMAT, value); }
        }

        public string NewsletterID
        {
            get { return Get(ODSConstants.KEY_NLID); }
            set { Add(ODSConstants.KEY_NLID, value); }
        }

        public string NewsletterName
        {
            get { return Get(ODSConstants.KEY_NL_NAME); }
            set { Add(ODSConstants.KEY_NL_NAME, value); }
        }

        public string EditionID
        {
            get { return Get(ODSConstants.KEY_EID); }
            set { Add(ODSConstants.KEY_EID, value); }
        }

        public string EditionName
        {
            get { return Get(ODSConstants.KEY_ED_NAME); }
            set { Add(ODSConstants.KEY_ED_NAME, value); }
        }

        /// <summary>
        /// LinkType (Show Article link or View All)
        /// sa, va
        /// </summary>
        public string LinkType
        {
            get { return Get(ODSConstants.KEY_LINK_TYPE); }
            set { Add(ODSConstants.KEY_LINK_TYPE, value); }
        }

        /// <summary>
        /// Dissemination Option * xrdr requires additional parameters, see next heading
        /// inacct, outacct, ttlt, xrdr
        /// </summary>
        public string DisseminationOption
        {
            get { return Get(ODSConstants.KEY_DISSEMINATION); }
            set { Add(ODSConstants.KEY_DISSEMINATION, value); }
        }

        public bool GlobalTemplate
        {
            get
            {
                string v = Get(ODSConstants.KEY_TMPL);
                return (v != null && v == "CAND");
            }
            set
            {
                string v = "CUST";
                if (value)
                {
                    v = "CAND";
                }
                Add(ODSConstants.KEY_TMPL, v);
            }
        }

        public bool CustomCSS
        {
            get
            {
                string v = Get(ODSConstants.KEY_CUSTOM_CSS);
                return (v != null && v == "CSS");
            }
            set
            {
                string v = "WIZ";
                if (value)
                {
                    v = "CSS";
                }
                Add(ODSConstants.KEY_CUSTOM_CSS, v);
            }
        }
    }
}