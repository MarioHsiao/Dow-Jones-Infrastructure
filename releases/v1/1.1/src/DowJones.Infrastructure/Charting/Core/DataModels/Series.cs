using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using EMG.Tools.Charting.DataModels;

namespace EMG.Utility.Charting.Core.DataModels
{
    public class BarSeries
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        private string description;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string drilldown;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string hover;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string name;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string note;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string noteTarget;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string target;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private List<DataItem> items;

        /// <remarks/>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <remarks/>
        public string Drilldown
        {
            get { return drilldown; }
            set { drilldown = value; }
        }

        /// <remarks/>
        public string Target
        {
            get { return target; }
            set { target = value; }
        }

        /// <remarks/>
        public string Hover
        {
            get { return hover; }
            set { hover = value; }
        }

        /// <remarks/>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <remarks/>
        public string Note
        {
            get { return note; }
            set { note = value; }
        }

        /// <remarks/>
        public string NoteTarget
        {
            get { return noteTarget; }
            set { noteTarget = value; }
        }

        public List<DataItem> Items
        {
            get { return items; }
            set { items = value; }
        }
    }
}
