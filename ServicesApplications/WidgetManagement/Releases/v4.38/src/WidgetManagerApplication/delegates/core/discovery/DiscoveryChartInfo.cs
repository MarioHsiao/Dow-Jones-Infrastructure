
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Script.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using EMG.Tools.Managers.Charting;

namespace EMG.widgets.ui.delegates.core.discovery
{
    /// <summary>
    /// 
    /// </summary>
    public class DiscoveryChartInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DiscoveryChartInfo"/> class.
        /// </summary>
        public DiscoveryChartInfo()
        {
            data = new List<ChartDataItem>();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [ScriptIgnore]
        [XmlIgnore]
        private List<ChartDataItem> __data;

        /// <summary>
        /// Gets or sets the first.
        /// </summary>
        /// <value>The first.</value>
        [XmlElement(Type = typeof(ChartDataItem), ElementName = "data", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = "")]
        public List<ChartDataItem> data
        {
            get
            {
                if (__data == null) __data = new List<ChartDataItem>();
                return __data;
            }
            set { __data = value; }
        }

        public string chartUri;
        public int width;
        public int height;
        public OutputChartType chartType;
        public string version;
    }

    /// <summary>
    /// 
    /// </summary>
    public class ChartDataItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChartDataItem"/> class.
        /// </summary>
        public ChartDataItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartDataItem"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="uri">The URI.</param>
        /// <param name="code">The code.</param>
        public ChartDataItem(string name, string value, string uri, string code)
        {
            this.name = name;
            this.value = value;
            this.uri = uri;
            this.code = code;
        }

        public string name;
        public string value;
        public string uri;
        public string code;
    }
}
