// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BubbleMapSeries.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   The bubble map series.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;

namespace DowJones.Tools.Charting.Data
{
    /// <summary>
    /// The bubble map series.
    /// </summary>
    public class BubbleMapSeries : BaseItem
    {
        /// <summary>
        /// The items.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)] private List<SeriesItem> items;

        /// <summary>
        /// Gets or sets Items.
        /// </summary>
        public List<SeriesItem> Items
        {
            get { return items ?? (items = new List<SeriesItem>()); }
            set { items = value; }
        }
    }
}