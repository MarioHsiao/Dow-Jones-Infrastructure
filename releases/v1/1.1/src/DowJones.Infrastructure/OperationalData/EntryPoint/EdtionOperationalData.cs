

using System.Collections.Generic;

namespace DowJones.Utilities.OperationalData.EntryPoint.Assets
{
    public class EdtionOperationalData : AbstractOperationalData
    {
        public EdtionOperationalData()
        {
        }

        /// <summary>
        /// Newsletter format (html | PDF | RTF | Mobile)
        /// </summary>
        public string EditionFormat
        {
            get { return Get(ODSConstants.KEY_FORMAT); }
            set { Add(ODSConstants.KEY_FORMAT, value); }
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

        public EdtionOperationalData(IDictionary<string, string> list)
            : base(list)
        {

        }
    }
}
