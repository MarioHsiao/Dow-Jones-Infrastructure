// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PieSeries.cs" company="Dow Jones">
//   � 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the PieSeries type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;

namespace DowJones.Tools.Charting.Data
{
    public class PieSeries : BaseItem
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        private List<SeriesItem> items;

        public List<SeriesItem> Items
        {
            get { return items ?? (items = new List<SeriesItem>()); }
            set { items = value; }
        }
    }
}
