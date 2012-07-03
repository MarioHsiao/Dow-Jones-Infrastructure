using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace DowJones.Charting.Discovery
{
    /// <remarks/>
    [XmlType(Namespace = "")]
    public class TextBoxDataSet
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        private List<string> itemsField;

        /// <remarks/>
        [XmlElement("items")]
        public List<string> Items
        {
            get
            {
                if ((itemsField == null))
                {
                    itemsField = new List<string>();
                }
                return itemsField;
            }
            set { itemsField = value; }
        }
    }
}