using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using DowJones.Json.Gateway.Attributes;
using DowJones.Json.Gateway.Converters;
using DowJones.Json.Gateway.Interfaces;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace.Transactions
{
    [ServicePath("2.0/Workspace")]
    [DataContract(Name = "GetWorkspacesDetailsList", Namespace = "")]
    [JsonObject(Title = "GetWorkspacesDetailsList")]
    public class GetWorkspacesDetailsListRequest : IPostJsonRestRequest
    {
        [JsonProperty(PropertyName = "maxResultsToReturn")] 
        [XmlElement(ElementName = "maxResultsToReturn", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "int", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "maxResultsToReturn")]
        public int __maxResultsToReturn;

        [JsonIgnore]
        [XmlIgnore]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __maxResultsToReturnSpecified;

        [JsonIgnore]
        [XmlIgnore]
        public int MaxResultsToReturn
        {
            get { return __maxResultsToReturn; }
            set { __maxResultsToReturn = value; __maxResultsToReturnSpecified = true; }
        }

        [JsonProperty(PropertyName = "paging")] 
        [XmlElement(ElementName = "paging", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "paging")]
        public Paging __paging;

        [JsonIgnore]
        [XmlIgnore]
        public Paging Paging
        {
            get
            {
                if (__paging == null) __paging = new Paging();
                return __paging;
            }
            set { __paging = value; }
        }

        [JsonProperty(PropertyName = "type")] 
        [XmlElement(Type = typeof(WorkspaceType), ElementName = "type", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "type")]
        public WorkspaceType __workspaceType;

        [JsonIgnore]
        [XmlIgnore]
        public WorkspaceType WorkspaceType
        {
            get { return __workspaceType; }
            set { __workspaceType = value; }
        }

        [JsonProperty(PropertyName = "sortBy")]
        [XmlElement(ElementName = "sortBy", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "sortBy")]
        public WorkspaceSortBy __sortBy;

        [JsonIgnore]
        [XmlIgnore]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __sortBySpecified;

        [JsonIgnore]
        [XmlIgnore]
        public WorkspaceSortBy SortBy
        {
            get { return __sortBy; }
            set { __sortBy = value; __sortBySpecified = true; }
        }

        [JsonProperty(PropertyName = "sortOrder")]
        [XmlElement(ElementName = "sortOrder", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "sortOrder")]
        public SortOrder __sortOrder;

        [JsonIgnore]
        [XmlIgnore]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __sortOrderSpecified;

        [JsonIgnore]
        [XmlIgnore]
        public SortOrder SortOrder
        {
            get { return __sortOrder; }
            set { __sortOrder = value; __sortOrderSpecified = true; }
        }

        [JsonProperty(PropertyName = "dataItemsCountController")]
        [XmlElement(Type = typeof(DataItemsCountController), ElementName = "dataItemsCountController", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "dataItemsCountController")]
        public DataItemsCountController __dataItemsCountController;

        [JsonIgnore]
        [XmlIgnore]
        public DataItemsCountController DataItemsCountController
        {
            get
            {
                if (__dataItemsCountController == null) __dataItemsCountController = new DataItemsCountController();
                return __dataItemsCountController;
            }
            set { __dataItemsCountController = value; }
        }

        [JsonProperty(PropertyName = "itemDateFilter")]
        [XmlElement(Type = typeof(ItemDateFilter), ElementName = "itemDateFilter", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "itemDateFilter")]
        public ItemDateFilter __itemDateFilter;

        [JsonIgnore]
        [XmlIgnore]
        public ItemDateFilter ItemDateFilter
        {
            get
            {
                if (__itemDateFilter == null) __itemDateFilter = new ItemDateFilter();
                return __itemDateFilter;
            }
            set { __itemDateFilter = value; }
        }

       

        [JsonProperty(PropertyName = "filterGroups")] 
        [XmlElement(Type = typeof(FilterGroup), ElementName = "filterGroups", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "filterGroups")]
        public FilterGroupCollection __filterGroupsCollection;

        [JsonIgnore]
        [XmlIgnore]
        public FilterGroupCollection FilterGroupsCollection
        {
            get
            {
                if (__filterGroupsCollection == null) __filterGroupsCollection = new FilterGroupCollection();
                return __filterGroupsCollection;
            }
            set { __filterGroupsCollection = value; }
        }

        public virtual string ToJson(ISerialize decorator)
        {
            return decorator.Serialize(this);
        }
    }
}
