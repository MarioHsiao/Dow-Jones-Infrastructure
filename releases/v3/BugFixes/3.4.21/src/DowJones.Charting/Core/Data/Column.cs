// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Column.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the Column type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.ComponentModel;
using System.Xml.Serialization;
using DowJones.Charting.Manager;
using DowJones.Managers.Core;

namespace DowJones.Charting.Core.Data
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

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Gets or sets the drilldown.
        /// </summary>
        /// <value>The drilldown.</value>
        public string Drilldown
        {
            get { return drilldown; }
            set { drilldown = value; }
        }

        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        /// <value>The target.</value>
        public string Target
        {
            get { return target; }
            set { target = value; }
        }

        /// <summary>
        /// Gets or sets the hover.
        /// </summary>
        /// <value>The hover.</value>
        public string Hover
        {
            get { return hover; }
            set { hover = value; }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// Gets or sets the note string.
        /// </summary>
        /// <value>The note string.</value>
        public string Note
        {
            get { return note; }
            set { note = value; }
        }

        /// <summary>
        /// Gets or sets the note target.
        /// </summary>
        /// <value>The note target.</value>
        public string NoteTarget
        {
            get { return noteTarget; }
            set { noteTarget = value; }
        }

        public virtual string ToITXML()
        {
            return string.Format(
                Declarations.CORDA_COLUMN,
                StringUtilitiesManager.XmlEncode(ChartingManager.PrepareString(Name, true, 11)),
                string.IsNullOrEmpty(Drilldown) ? string.Empty : string.Concat(" drilldown=\"", StringUtilitiesManager.XmlAttributeEncode(Drilldown), "\""),
                string.IsNullOrEmpty(Target) ? string.Empty : string.Concat(" target=\"", StringUtilitiesManager.XmlAttributeEncode(Target), "\""),
                string.IsNullOrEmpty(Hover) ? string.Empty : string.Concat(" hover=\"", StringUtilitiesManager.XmlAttributeEncode(ChartingManager.PrepareString(Hover)), "\""),
                string.IsNullOrEmpty(Note) ? string.Empty : string.Concat(" note=\"", StringUtilitiesManager.XmlAttributeEncode(ChartingManager.PrepareString(Note)), "\""),
                string.IsNullOrEmpty(NoteTarget) ? string.Empty : string.Concat(" noteTarget=\"", StringUtilitiesManager.XmlAttributeEncode(ChartingManager.PrepareString(NoteTarget)), "\""),
                string.IsNullOrEmpty(Description) ? string.Empty : string.Concat(" description=\"", StringUtilitiesManager.XmlAttributeEncode(ChartingManager.PrepareString(Description, true)), "\""));
        }
    }
}
