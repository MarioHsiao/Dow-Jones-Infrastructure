// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StockDataItem.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the StockDataItem type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.ComponentModel;
using System.Xml.Serialization;

namespace DowJones.Charting.Core.Data
{
    [XmlType(Namespace = "")]
    public abstract class AbstractStockDataItem : BaseItem
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        private double value;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private double high;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private double low;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private double open;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private double close; 

        #region Constructors
        protected AbstractStockDataItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractStockDataItem"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        protected AbstractStockDataItem(double value)
        {
            this.value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractStockDataItem"/> class.
        /// </summary>
        /// <param name="value">The current value.</param>
        /// <param name="high">The high value.</param>
        /// <param name="low">The low value.</param>
        /// <param name="open">The open value.</param>
        /// <param name="close">The close value.</param>
        protected AbstractStockDataItem(double value, double high, double low, double open, double close)
        {
            Value = value;
            High = high;
            Low = low;
            Open = open;
            Close = close;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractStockDataItem"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="drilldownUrl">The drilldown URL.</param>
        protected AbstractStockDataItem(double value, string drilldownUrl)
            : this(value)
        {
            Drilldown = drilldownUrl;
        }

        public double Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public double High
        {
            get { return high; }
            set { this.high = value; }
        }

        public double Low
        {
            get { return low; }
            set { this.low = value; }
        }

        public double Open
        {
            get { return open; }
            set { this.open = value; } 
        }

        public double Close
        {
            get { return close; }
            set { this.close = value; }
        }

        internal abstract string ToITXML();
   }
}
