// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StockItem.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the StockItem type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace DowJones.Tools.Charting.Data
{
    public class StockItem : AbstractStockDataItem
    {
        public DateTime Date { get; set; }

        internal override string ToITXML()
        {
            return string.Format(Declarations.CORDA_STOCK_ITEM, Date.ToString("mm/DD/YYYY"), High, Low, Open, Close, Hover, Drilldown);
        }
    }
}
