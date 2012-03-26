using System;
using System.Collections.Generic;
using System.ComponentModel;
using EMG.Tools.Charting.Data;

namespace EMG.Tools.Charting.Core.Data
{
    public class RowItem : BaseItem
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        private string name;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private List<DataItem> items;

        /// <remarks/>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public List<DataItem> Items
        {
            get { return items; }
            set { items = value; }
        }
    }
}
