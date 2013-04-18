// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BubbleMapSeries.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   The bubble map series.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DowJones.Charting.Core.Data
{
    /// <summary>
    /// The bubble map series.
    /// </summary>
    [Obsolete("This class should no longer be used due to Corda replacement project")] 
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