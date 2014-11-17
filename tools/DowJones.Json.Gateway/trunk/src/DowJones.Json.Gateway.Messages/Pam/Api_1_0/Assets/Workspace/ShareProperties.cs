using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonObject(Title = "ShareProperties")]
    [XmlType(TypeName = "ShareProperties", Namespace = Declarations.SchemaVersion), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [XmlInclude(typeof(SharePropertiesResponse))]
    public class ShareProperties
    {
        [JsonProperty(PropertyName = "accessPermission")]
        [XmlElement(Type = typeof(Permission), ElementName = "accessPermission", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "accessPermission")]
        public Permission __accessPermission;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public Permission AccessPermission
        {
            get
            {
                if (__accessPermission == null) __accessPermission = new Permission();
                return __accessPermission;
            }
            set { __accessPermission = value; }
        }

        [JsonProperty(PropertyName = "qualifier")]
        [XmlElement(ElementName = "qualifier", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "qualifier")]
        public AccessQualifier __qualifier;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __qualifierSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public AccessQualifier Qualifier
        {
            get { return __qualifier; }
            set { __qualifier = value; __qualifierSpecified = true; }
        }

        [JsonProperty(PropertyName = "assignPermission")]
        [XmlElement(Type = typeof(Permission), ElementName = "assignPermission", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "assignPermission")]
        public Permission __assignPermission;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public Permission AssignPermission
        {
            get
            {
                if (__assignPermission == null) __assignPermission = new Permission();
                return __assignPermission;
            }
            set { __assignPermission = value; }
        }

        [JsonProperty(PropertyName = "updatePermission")]
        [XmlElement(Type = typeof(Permission), ElementName = "updatePermission", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "updatePermission")]
        public Permission __updatePermission;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public Permission UpdatePermission
        {
            get
            {
                if (__updatePermission == null) __updatePermission = new Permission();
                return __updatePermission;
            }
            set { __updatePermission = value; }
        }

        [JsonProperty(PropertyName = "deletePermission")]
        [XmlElement(Type = typeof(Permission), ElementName = "deletePermission", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "deletePermission")]
        public Permission __deletePermission;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public Permission DeletePermission
        {
            get
            {
                if (__deletePermission == null) __deletePermission = new Permission();
                return __deletePermission;
            }
            set { __deletePermission = value; }
        }

        [JsonProperty(PropertyName = "listingScope")]
        [XmlElement(ElementName = "listingScope", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "listingScope")]
        public ShareScope __listingScope;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __listingScopeSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public ShareScope ListingScope
        {
            get { return __listingScope; }
            set { __listingScope = value; __listingScopeSpecified = true; }
        }

        [JsonProperty(PropertyName = "sharePromotion")]
        [XmlElement(ElementName = "sharePromotion", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DataMember(Name = "sharePromotion")]
        public ShareScope __sharePromotion;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool __sharePromotionSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public ShareScope SharePromotion
        {
            get { return __sharePromotion; }
            set { __sharePromotion = value; __sharePromotionSpecified = true; }
        }



        public ShareProperties()
        {
        }
    }
}
