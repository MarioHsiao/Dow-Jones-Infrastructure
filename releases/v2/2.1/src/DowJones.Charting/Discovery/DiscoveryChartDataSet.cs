// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DiscoveryChartDataSet.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Discovery Chart DataSet object
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using DowJones.Charting.Core.Data;

namespace DowJones.Charting.Discovery
{
    /// <summary>
    /// Discovery Chart DataSet object
    /// </summary>
    /// <remarks/>
    [XmlType(Namespace = "")]
    public class DiscoveryChartDataSet
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        private List<DataItem> itemsField;

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>The items.</value>
        /// <remarks/>
        [XmlElement("items")]
        public List<DataItem> Items
        {
            get { return itemsField ?? (itemsField = new List<DataItem>()); }
            set { itemsField = value; }
        }
    }
}
