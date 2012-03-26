using System;

namespace EMG.Tools.Charting.DataModels
{
    public class TimeItem : DataItem
    {
        private DateTime dateField;

        /// <remarks/>
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
