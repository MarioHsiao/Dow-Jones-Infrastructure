// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StackedBarSeries.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the StackedBarSeries type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace DowJones.Charting.Core.Data
{
    [Obsolete("This class should no longer be used due to Corda replacement project")] 
    public class StackedBarSeries : BaseItem
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        private List<string> categoryNames;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private List<SeriesItem> items;

        [XmlElement("categoryName")]
        public List<string> CategoryNames
        {
            get { return categoryNames ?? (categoryNames = new List<string>()); }
            set { categoryNames = value; }
        }

        public List<SeriesItem> Items
        {
            get { return items ?? (items = new List<SeriesItem>()); }
            set { items = value; }
        }
    }
}
