// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataItem.cs" company="Dow Jones">
//   
// </copyright>
// <summary>
//   Defines the DataItem type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.ComponentModel;
using System.Xml.Serialization;
using DowJones.Charting.Manager;
using DowJones.Formatters;
using DowJones.Formatters.Numerical;
using DowJones.Managers.Core;

namespace DowJones.Charting.Core.Data
{
    [XmlType(Namespace = "")]
    public class DataItem : BaseItem
    {
        /// <summary>
        ///  The value of the DataItem
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        private double _value;

        #region Constructors

        public DataItem(double value)
        {
            _value = value;
        }

        public DataItem()
        {
        }

        #endregion

        public DataItem(double value, string drilldownUrl) : this(value)
        {
            Drilldown = drilldownUrl;
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public double Value
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        /// String getting the ITXML.
        /// </summary>
        /// <returns>A string of itxml</returns>
        internal virtual string ToITXML()
        {
            return string.Format(
                Declarations.CORDA_DATA_ITEM, 
                NumberFormatter.GetFormattedText(Value, NumberFormatType.Raw).Value, 
                string.IsNullOrEmpty(Drilldown) ? string.Empty : string.Concat(" drilldown=\"", StringUtilitiesManager.XmlAttributeEncode(Drilldown), "\""),
                string.IsNullOrEmpty(Target) ? string.Empty : string.Concat(" target=\"", StringUtilitiesManager.XmlAttributeEncode(Target), "\""),
                string.IsNullOrEmpty(Hover) ? string.Empty : string.Concat(" hover=\"", StringUtilitiesManager.XmlAttributeEncode(ChartingManager.PrepareString(Hover)), "\""),
                string.IsNullOrEmpty(Note) ? string.Empty : string.Concat(" note=\"", StringUtilitiesManager.XmlAttributeEncode(ChartingManager.PrepareString(Note)), "\""),
                string.IsNullOrEmpty(NoteTarget) ? string.Empty : string.Concat(" noteTarget=\"", StringUtilitiesManager.XmlAttributeEncode(ChartingManager.PrepareString(NoteTarget)), "\""),
                string.IsNullOrEmpty(Description) ? string.Empty : string.Concat(" description=\"", StringUtilitiesManager.XmlAttributeEncode(ChartingManager.PrepareString(Description)), "\""));
        }
    }
}