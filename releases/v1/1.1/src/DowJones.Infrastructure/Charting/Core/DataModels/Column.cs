using System.ComponentModel;
using System.Xml.Serialization;

namespace EMG.Tools.Charting.DataModels
{
    [XmlType(Namespace = "")]
    internal class Column : IGeneratesITXML
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

        public virtual string ToITXML()
        {
            return string.Format(Declarations.CORDA_COLUMN, Name, Drilldown, Target, Hover, Note, NoteTarget, Description);
        }
    }
}
