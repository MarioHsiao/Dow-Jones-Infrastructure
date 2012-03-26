

using System.Collections.Generic;

namespace DowJones.Utilities.OperationalData.EntryPoint
{
    public class BaseRssOperationalData : AbstractOperationalData
    {

        /// <summary>
        /// Gets or sets the type of the RSS. Values Are [Public, Private]
        /// </summary>
        /// <value>The type of the RSS.</value>
        public string RssType
        {
            get { return Get(ODSConstants.KEY_RSS_TYPE); }
            set { Add(ODSConstants.KEY_RSS_TYPE, value); }
        }

        public BaseRssOperationalData(IDictionary<string, string> list)
            : base(list)
        {

        }
    }
}
