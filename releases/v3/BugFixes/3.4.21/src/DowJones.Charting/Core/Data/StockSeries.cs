// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StockSeries.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the StockSeries type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

/* 
 * Author: Infosys
 * Date: 9/12/09
 * Purpose: Stock Series.
 * 
 * 
 * Mod Log
 * -----------------------------------------------------------------------------
 * Modified by                          Date                    Purpose
 * -----------------------------------------------------------------------------
 */
using System.Collections.Generic;
using System.ComponentModel;

namespace DowJones.Charting.Core.Data
{
    public class StockSeries : BaseItem
    {    
        [EditorBrowsable(EditorBrowsableState.Never)]
        private string name;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private List<StockItem> stockItems;

        #region Constructors

        public StockSeries(string name)
        {
            this.name = name;
        }

        public StockSeries()
        {
        }

        #endregion

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public List<StockItem> StockItems
        {
            get { return stockItems ?? (stockItems = new List<StockItem>()); }
            set { stockItems = value; }
        }
    }
}

