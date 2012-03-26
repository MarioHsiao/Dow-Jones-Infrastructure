using System.Collections.Generic;
using System.ComponentModel;
using EMG.Tools.Charting.Data;

namespace EMG.Tools.Charting.Core.Data
{
    public class BarSeries : BaseItem
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        private string[] names;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private List<SeriesItem> items;

        /// <remarks/>
        public string[] Names
        {
            get { return names; }
            set { names = value; }
        }

        public List<SeriesItem> Items
        {
            get { return items; }
            set { items = value; }
        }
    }
}
