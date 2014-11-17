using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonObject(Title = "SharePropertiesResponse")]
    [XmlType(TypeName = "SharePropertiesResponse", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class SharePropertiesResponse : ShareProperties
    {
        [JsonProperty(PropertyName = "shareStatus")]
        [XmlElement(ElementName = "shareStatus", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "shareStatus")]
        public ShareStatus __shareStatus;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __shareStatusSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public ShareStatus ShareStatus
        {
            get { return __shareStatus; }
            set { __shareStatus = value; __shareStatusSpecified = true; }
        }

        [JsonProperty(PropertyName = "isOwner")]
        [XmlElement(ElementName = "isOwner", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "boolean", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "isOwner")]
        public bool __isOwner;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __isOwnerSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public bool IsOwner
        {
            get { return __isOwner; }
            set { __isOwner = value; __isOwnerSpecified = true; }
        }

        [JsonProperty(PropertyName = "shareType")]
        [XmlElement(ElementName = "shareType", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "shareType")]
        public ShareType __shareType;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __shareTypeSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public ShareType ShareType
        {
            get { return __shareType; }
            set { __shareType = value; __shareTypeSpecified = true; }
        }

        [JsonProperty(PropertyName = "rootID")]
        [XmlElement(ElementName = "rootID", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "long", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "rootID")]
        public long __rootID;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __rootIDSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public long RootID
        {
            get { return __rootID; }
            set { __rootID = value; __rootIDSpecified = true; }
        }

        [JsonProperty(PropertyName = "rootAccessControlScope")]
        [XmlElement(ElementName = "rootAccessControlScope", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "rootAccessControlScope")]
        public ShareScope __rootAccessControlScope;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __rootAccessControlScopeSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public ShareScope RootAccessControlScope
        {
            get { return __rootAccessControlScope; }
            set { __rootAccessControlScope = value; __rootAccessControlScopeSpecified = true; }
        }

        [JsonProperty(PropertyName = "lastModifiedDate")]
        [XmlElement(ElementName = "lastModifiedDate", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "lastModifiedDate")]
        public string __lastModifiedDate;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public string LastModifiedDate
        {
            get { return __lastModifiedDate; }
            set { __lastModifiedDate = value; }
        }

        [JsonProperty(PropertyName = "previousACScope")]
        [XmlElement(ElementName = "previousACScope", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "previousACScope")]
        public ShareScope __previousACScope;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __previousACScopeSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public ShareScope PreviousACScope
        {
            get { return __previousACScope; }
            set { __previousACScope = value; __previousACScopeSpecified = true; }
        }

        [JsonProperty(PropertyName = "internalAccess")]
        [XmlElement(ElementName = "internalAccess", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "boolean", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "internalAccess")]
        public bool __internalAccess;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __internalAccessSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public bool InternalAccess
        {
            get { return __internalAccess; }
            set { __internalAccess = value; __internalAccessSpecified = true; }
        }

        [JsonProperty(PropertyName = "allowCopy")]
        [XmlElement(ElementName = "allowCopy", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "boolean", Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "allowCopy")]
        public bool __allowCopy;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __allowCopySpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public bool AllowCopy
        {
            get { return __allowCopy; }
            set { __allowCopy = value; __allowCopySpecified = true; }
        }

        public SharePropertiesResponse()
        {

        }
    }
}
