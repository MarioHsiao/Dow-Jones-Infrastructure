using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace DowJones.Tools.Charting.Data
{
    public class TrendSeries : BaseItem
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        private List<string> categoryNames;

        [EditorBrowsable(EditorBrowsableState.Never)]
        private List<SeriesItem> items;


        [XmlElement("categoryName")]
        public List<string> CategoryNames
        {
            get
            {
                if (categoryNames == null)
                {
                    categoryNames = new List<string>();
                }
                return categoryNames;
            }
            set { categoryNames = value; }
        }

        public List<SeriesItem> Items
        {
            get
            {
                if (items == null)
                {
                    items = new List<SeriesItem>();
                }
                return items;
            }
            set { items = value; }
        }
    }
}
