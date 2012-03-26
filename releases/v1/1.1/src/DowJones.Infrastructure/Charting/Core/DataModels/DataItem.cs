using System.ComponentModel;
using System.Xml.Serialization;
using EMG.Utility.Formatters;
using EMG.Utility.Formatters.Numerical;

namespace EMG.Tools.Charting.DataModels
{
    [XmlType(Namespace = "")]
    public class DataItem
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        private string description;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string drilldown;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string hover;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string note;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string noteTarget;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private string target;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private double value;

        /// <remarks/>
        public double Value
        {
            get { return value; }
            set { this.value = value; }
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

        internal virtual string ToITXML()
        {
            return string.Format(Declarations.CORDA_DATA_ITEM, NumberFormatter.GetFormattedText(Value, NumberFormatType.Raw), Drilldown, Target, Hover, Note, NoteTarget, Description);
        }
    }
}