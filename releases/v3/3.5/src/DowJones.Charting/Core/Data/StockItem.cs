// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StockItem.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the StockItem type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace DowJones.Charting.Core.Data
{
    [Obsolete("This class should no longer be used due to Corda replacement project")] 
    public class StockItem : AbstractStockDataItem
    {
        public DateTime Date { get; set; }

        internal override string ToITXML()
        {
            return string.Format(Declarations.CORDA_STOCK_ITEM, Date.ToString("mm/DD/YYYY"), High, Low, Open, Close, Hover, Drilldown);
        }
    }
}
