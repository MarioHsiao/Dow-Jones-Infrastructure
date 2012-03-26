using System;

namespace DowJones.Tools.Charting.Data
{
    public class TimeItem : DataItem
    {
        private DateTime dateField;

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The datetime object of the Date.</value>
        public DateTime Date
        {
            get { return dateField; }
            set { dateField = value; }
        }

        internal override string ToITXML()
        {
            return string.Format(Declarations.CORDA_TIME_ITEM, Date.ToString("mm/DD/YYYY"), Value, Drilldown, Target, Hover, Note, NoteTarget, Description);
        }
    }
}
