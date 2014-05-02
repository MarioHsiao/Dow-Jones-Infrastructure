/* 
 * Author: Infosys
 * Date: 8/6/09
 * Purpose: Time Line Series.
 * 
 * 
 * Mod Log
 * -----------------------------------------------------------------------------
 * Modified by                          Date                    Purpose
 * -----------------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DowJones.Charting.Core.Data
{
    [Obsolete("This class should no longer be used due to Corda replacement project")] 
    public class TimeSeries : BaseItem
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        private string name;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private List<TimeItem> timeItems;

        #region Constructors

        public TimeSeries(string name)
        {
            this.name = name;
        }

        public TimeSeries()
        {
        }

        #endregion

        /// <remarks/>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public List<TimeItem> TimeItems
        {
            get
            {
                if (timeItems == null)
                    timeItems = new List<TimeItem>();
                return timeItems;
            }
            set { timeItems = value; }
        }
    }
}
