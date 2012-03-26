using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Script.Serialization;

namespace DowJones.Tools.Ajax.HeadlineList
{
    public class Para
    {
        public List<MarkupItem> items;

        public Para()
        {
        }

        public Para(MarkupItem item)
        {
            items = new List<MarkupItem> { item };
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class MarkupItem
    {
        /// <summary>
        /// 
        /// </summary>
        public EntityType _type;

        /// <summary>
        /// 
        /// </summary>
        public string guid;

        /// <summary>
        /// 
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string type;

        /// <summary>
        /// 
        /// </summary>
        public string value;


        /// <summary>
        /// Initializes a new instance of the <see cref="MarkupItem"/> class.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="value">The value.</param>
        public MarkupItem(EntityType entityType, string value)
        {
            this.value = value;
            EntityType = entityType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MarkupItem"/> class.
        /// </summary>
        public MarkupItem()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [ScriptIgnore]
        public EntityType EntityType
        {
            get { return _type; }
            set
            {
                _type = value;
                type = value.ToString();
            }
        }
    }
}