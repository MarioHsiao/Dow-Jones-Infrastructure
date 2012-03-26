using System.Xml.Serialization;

namespace DowJones.Tools.Managers.Charting
{
    /// <summary>
    /// <para>Chart configuration template.</para>
    /// <para>Used for deserialization/serialization of configuration settings.</para>
    /// </summary>
    [XmlType(Namespace = "")]
    [XmlRoot("chartTemplate", Namespace = "", IsNullable = false)]
    public class ChartTemplate
    {
        /// <summary>
        /// 
        /// </summary>
        public string AppearanceFile;

        /// <summary>
        /// 
        /// </summary>
        public int Width;

        /// <summary>
        /// 
        /// </summary>
        public int Height;

        /// <summary>
        /// 
        /// </summary>
        public OutputChartType OutputChartType;

        public ChartTemplate()
        {
            
        }

        public ChartTemplate(string appearanceFile, int width, int height, OutputChartType outputChartType)
        {
            AppearanceFile = appearanceFile;
            Width = width;
            Height = height;
            OutputChartType = outputChartType;
        }
    }
}
