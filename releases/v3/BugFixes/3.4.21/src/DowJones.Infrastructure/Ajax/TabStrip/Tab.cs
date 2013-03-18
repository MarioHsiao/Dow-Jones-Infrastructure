using System.Collections.Generic;

namespace DowJones.Ajax.TabStrip
{
    public enum TabStatus
    {
        Locked,
        UnLocked,
        Personal
    }

    public enum OptionTypes
    {
        Share,
        UnShare,
        Print,
    }

    //public class TabStripDataResult
    //{
    //    public List<Tab> Tabs = new List<Tab>();
    //}

    public class Tab
    {
        /// <summary>
        /// Gets or sets the title of the tag
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the id of the tab
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the tab position
        /// </summary>
        public int TabPosition;

        /// <summary>
        /// Gets or sets the lock type of the tab
        /// </summary>
        public TabStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the uri value of the tab
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// Gets or sets the selected value of the tab
        /// </summary>
        public string Selected { get; set; }

        /// <summary>
        /// Gets or sets the options of the tab
        /// </summary>
        public List<OptionTypes> Options { get; set; }

    }
}
