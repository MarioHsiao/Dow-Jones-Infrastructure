// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SeriesItem.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the SeriesItem type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;

namespace DowJones.Tools.Charting.Data
{
    public class SeriesItem : BaseItem
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        private string name;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private List<DataItem> dataItems;

        #region Constructors

        public SeriesItem(string name)
        {
            this.name = name;
        }
        
        public SeriesItem()
        {
        }

        #endregion

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name of the series item.</value>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Gets or sets the data items.
        /// </summary>
        /// <value>The data items.</value>
        public List<DataItem> DataItems
        {
            get { return dataItems ?? (dataItems = new List<DataItem>()); }
            set { dataItems = value; }
        }
    }
}
