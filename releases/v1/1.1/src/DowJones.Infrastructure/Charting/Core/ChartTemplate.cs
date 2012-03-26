using System.Xml.Serialization;

namespace EMG.Tools.Charting.Core
{
    /// <summary>
    /// <para>Chart configuration template.</para>
    /// <para>Used for deserialization/serialization of configuration settings.</para>
    /// </summary>
    [XmlType(Namespace = "")]
    [XmlRoot("chartTemplate", Namespace = "", IsNullable = false)]
    internal class ChartTemplate
    {
        /// <summary>
        /// 
        /// </summary>
        public string appearanceFile;

        /// <summary>
        /// 
        /// </summary>
        public int width;

        /// <summary>
        /// 
        /// </summary>
        public int height;

        /// <summary>
        /// 
        /// </summary>
        public ChartType chartType;
    }
}
