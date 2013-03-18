using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace DowJones.Ajax.HeadlineList
{
    [DataContract( Name = "para", Namespace = "" )]
    [JsonObject( MemberSerialization.OptIn, Id = "para" )] 
    public class Para
    {
        [JsonProperty( "items" )]
        [DataMember( Name = "items" )] 
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
    [DataContract( Name = "markupItem", Namespace = "" )]
    [JsonObject( MemberSerialization.OptIn, Id = "markupItem" )]    
    public class MarkupItem
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        [IgnoreDataMember]
        public EntityType _type;

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty( "guid" )]
        [DataMember( Name = "guid" )]  
        public string guid;

        /// <summary>
        /// 
        /// </summary>
        [DataMember( Name = "entityTypeDescriptor" )]
        [JsonProperty( "entityTypeDescriptor" )]  
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string type;

        /// <summary>
        /// 
        /// </summary>
        [DataMember( Name = "value" )]
        [JsonProperty( "value" )]
        public string value;


        /// <summary>
        /// Initializes a new instance of the <see cref="MarkupItem"/> class.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="value">The value.</param>
        public MarkupItem(EntityType entityType, string value, string code = null)
        {
            this.value = value;
            this.guid = code;
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
        [JsonProperty( "entityType" )]
        [DataMember( Name = "entityType" )]       
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