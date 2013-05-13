// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseItem.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the BaseItem type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel;

namespace DowJones.Charting.Core.Data
{
    [Obsolete("This class should no longer be used due to Corda replacement project")] 
    public class BaseItem
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

        /// <summary>
        /// Gets or sets the drilldown.
        /// </summary>
        /// <value>The drilldown.</value>
        /// <remarks/>
        public string Drilldown
        {
            get { return drilldown; }
            set { drilldown = value; }
        }

        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        /// <value>The target.</value>
        /// <remarks/>
        public string Target
        {
            get { return target; }
            set { target = value; }
        }

        /// <summary>
        /// Gets or sets the hover.
        /// </summary>
        /// <value>The hover.</value>
        /// <remarks/>
        public string Hover
        {
            get { return hover; }
            set { hover = value; }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        /// <remarks/>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// Gets or sets the note.
        /// </summary>
        /// <value>The note associate with the object.</value>
        /// <remarks/>
        public string Note
        {
            get { return note; }
            set { note = value; }
        }

        /// <summary>
        /// Gets or sets the note target.
        /// </summary>
        /// <value>The note target.</value>
        /// <remarks/>
        public string NoteTarget
        {
            get { return noteTarget; }
            set { noteTarget = value; }
        }
    }
}
