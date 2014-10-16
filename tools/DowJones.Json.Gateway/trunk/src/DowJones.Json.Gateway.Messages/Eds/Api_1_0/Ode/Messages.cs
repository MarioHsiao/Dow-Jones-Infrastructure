using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;
using System.Collections.Generic;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace DowJones.Json.Gateway.Messages.Eds.Api_1_0.Ode
{

    #region EDS-ODE


    public struct Declarations
    {
        public const string SchemaVersion = "";

    }

    [JsonConverter(typeof (StringEnumConverter))]
    public enum ProductType
    {
        [XmlEnum("GLOBAL")] Global,
        [XmlEnum("DJX")] DJX,
        [XmlEnum("DOWJONES")] DowJones,
        [XmlEnum("FS-ALERT-IB")] FSALERTIB,
        [XmlEnum("MADE-NEWS")] MADENEWS,
        [XmlEnum("DJR")] DJR,
        [XmlEnum("DJRC")] DJRC,
        [XmlEnum("OR")] OR,
        [XmlEnum("DJXADMIN")] DJXADMIN,
        [XmlEnum("FACTIVA")] FACTIVA,
        [XmlEnum("RNC")] RNC
    }

    [JsonConverter(typeof (StringEnumConverter))]
    public enum ContentTypeForContentByData
    {
        [XmlEnum(Name = "HTML")] HTML,
        [XmlEnum(Name = "XML")] XML,
        [XmlEnum(Name = "FolderInvite")] FolderInvite,
        [XmlEnum(Name = "UserWelcome")] UserWelcome,
        [XmlEnum(Name = "EmailValidate")] EmailValidate,
        [XmlEnum(Name = "EmailLoginEnabled")] EmailLoginEnabled,
        [XmlEnum(Name = "UserWelcomeConfirmation")] UserWelcomeConfirmation,
        [XmlEnum(Name = "PasswordReset")] PasswordReset,
        [XmlEnum(Name = "AlertConfirmation")] AlertConfirmation,
        [XmlEnum(Name = "AlertUnsubscribe")] AlertUnsubscribe
    }

    [JsonConverter(typeof (StringEnumConverter))]
    public enum ContentTypeForContentByID
    {
        [XmlEnum(Name = "AN")] AN,
        [XmlEnum(Name = "URL")] URL
    }

    [JsonConverter(typeof (StringEnumConverter))]
    public enum ImageType
    {
        [XmlEnum("1")] Thumbnail,
        [XmlEnum("2")] ThumbnailAndIndexing,
        [XmlEnum("3")] ScreenResolution,
        [XmlEnum("4")] ScreenResolutionAndIndexing
    }

    [JsonConverter(typeof (StringEnumConverter))]
    public enum EmailDisplayFormat
    {
        [XmlEnum(Name = "HTML")] HTML,
        [XmlEnum(Name = "TEXT")] TEXT,
        [XmlEnum(Name = "MobileHTML")] MobileHTML,
        [XmlEnum(Name = "MobileTEXT")] MobileTEXT
    }

    [JsonConverter(typeof (StringEnumConverter))]
    public enum EmailContentType
    {
        [XmlEnum(Name = "Headlines")] Headlines,
        [XmlEnum(Name = "FullText")] FullText,
        [XmlEnum(Name = "HeadlineIndexing")] HeadlineIndexing,
        [XmlEnum(Name = "FullTextIndexing")] FullTextIndexing,
        [XmlEnum(Name = "KeywordsInContext")] KWIC,
        [XmlEnum(Name = "Custom")] Custom
    }

    [JsonConverter(typeof (StringEnumConverter))]
    public enum Source
    {
        [XmlEnum(Name = "P")] P,
        [XmlEnum(Name = "I")] I,
        [XmlEnum(Name = "W")] W,
        [XmlEnum(Name = "H")] H,
        [XmlEnum(Name = "M")] M
    }

    [JsonConverter(typeof (StringEnumConverter))]
    [DataContract(Name = "RequestMode", Namespace = "")]
    public enum RequestMode
    {
        [EnumMember(Value = "Async")]
        [XmlEnum(Name = "ASync")] 
        ASync,
        [EnumMember(Value = "Sync")]
        [XmlEnum(Name = "Sync")] 
        Sync
    }

    [JsonArray]
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class TicketCollection : List<Ticket>
    {

    }

    [JsonArray]
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class BCCEmailCollection : List<BCCEmailAddress>
    {

    }

    [JsonArray]
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class ToEmailCollection : List<ToEmailAddress>
    {

    }

    [JsonArray]
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class CCEmailCollection : List<CCEmailAddress>
    {

    }

    [JsonArray]
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class AnCollection : List<string>
    {

    }


    [JsonObject(Title = "ContentByData")]
    [XmlType(TypeName = "ContentByData"), Serializable]
    [EditorBrowsable(EditorBrowsableState.Always)]
    public class ContentByData : Content
    {

        [JsonProperty(PropertyName = "ContentType")] [XmlAttribute(AttributeName = "ContentType", Form = XmlSchemaForm.Unqualified)] [EditorBrowsable(EditorBrowsableState.Advanced)] public ContentTypeForContentByData __ContentType;

        [JsonIgnore] [XmlIgnore] [EditorBrowsable(EditorBrowsableState.Advanced)] private bool __ContentTypeSpecified;

        [JsonIgnore]
        [XmlIgnore]
        public ContentTypeForContentByData ContentType
        {
            get { return __ContentType; }
            set
            {
                __ContentType = value;
                __ContentTypeSpecified = true;
            }
        }

        [JsonProperty(PropertyName = "EmailCommand")] [XmlElement(ElementName = "EmailCommand", IsNullable = false, Form = XmlSchemaForm.Qualified,
            Type = typeof (EmailCommand))] [EditorBrowsable(EditorBrowsableState.Advanced)] public EmailCommand
            __EmailCommand;

        [JsonIgnore]
        [XmlIgnore]
        public EmailCommand EmailCommand
        {
            get { return __EmailCommand; }
            set { __EmailCommand = value; }
        }

        public ContentByData()
            : base()
        {
        }
    }

    [JsonObject(Title = "ContentByID")]
    [XmlType(TypeName = "ContentByID"), Serializable]
    [EditorBrowsable(EditorBrowsableState.Always)]
    public class ContentByID : Content
    {

        [JsonProperty(PropertyName = "Source")] [XmlAttribute(AttributeName = "Source", Form = XmlSchemaForm.Unqualified)] [EditorBrowsable(EditorBrowsableState.Advanced)] public Source __Source;

        [JsonIgnore] [XmlIgnore] [EditorBrowsable(EditorBrowsableState.Advanced)] public bool __SourceSpecified;


        [JsonIgnore]
        [XmlIgnore]
        public Source Source
        {
            get { return __Source; }
            set
            {
                __Source = value;
                __SourceSpecified = true;
            }
        }

        [JsonProperty(PropertyName = "EmailDisplayLanguage")] [XmlElement(ElementName = "EmailDisplayLanguage", Form = XmlSchemaForm.Unqualified, DataType = "string")] [EditorBrowsable(EditorBrowsableState.Advanced)] public string __EmailDisplayLanguage;


        [JsonIgnore]
        [XmlIgnore]
        public string EmailDisplayLanguage
        {
            get { return __EmailDisplayLanguage; }
            set { __EmailDisplayLanguage = value; }
        }

        [JsonProperty(PropertyName = "ImageType")] [XmlAttribute(AttributeName = "ImageType", Form = XmlSchemaForm.Unqualified)] [EditorBrowsable(EditorBrowsableState.Advanced)] public ImageType __ImageType;


        [JsonIgnore] [XmlIgnore] public bool __ImageTypeSpecified;


        [JsonIgnore]
        [XmlIgnore]
        public ImageType ImageType
        {
            get { return __ImageType; }
            set
            {
                __ImageType = value;
                __ImageTypeSpecified = true;
            }
        }

        [JsonProperty(PropertyName = "ContentType")] [XmlAttribute(AttributeName = "ContentType", Form = XmlSchemaForm.Unqualified)] [EditorBrowsable(EditorBrowsableState.Advanced)] public ContentTypeForContentByID __ContentType;


        [JsonIgnore] [XmlIgnore] public bool __ContentTypeSpecified;

        [JsonProperty(PropertyName = "AsAttachment")] [XmlAttribute(AttributeName = "AsAttachment", Form = XmlSchemaForm.Qualified, DataType = "boolean")] [EditorBrowsable(EditorBrowsableState.Advanced)] public bool __AsAttachment;


        [JsonIgnore] [XmlIgnore] public bool __AsAttachmentSpecified;

        [JsonIgnore]
        [XmlIgnore]
        public bool AsAttachment
        {
            get { return __AsAttachment; }
            set
            {
                __AsAttachment = value;
                __AsAttachmentSpecified = true;
            }
        }

        [JsonIgnore]
        [XmlIgnore]
        public ContentTypeForContentByID ContentType
        {
            get { return __ContentType; }
            set
            {
                __ContentType = value;
                __ContentTypeSpecified = true;
            }
        }

        public ContentByID()
            : base()
        {
        }
    }

    [JsonObject(Title = "EmailAddress")]
    [XmlType(TypeName = "EmailAddress"), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class EmailAddress
    {

        [JsonProperty(PropertyName = "Name")] [XmlAttribute(AttributeName = "Name", Form = XmlSchemaForm.Unqualified, DataType = "string")] [EditorBrowsable(EditorBrowsableState.Advanced)] public string __Name = "";

        [JsonIgnore]
        [XmlIgnore]
        public string Name
        {
            get { return __Name; }
            set { __Name = value; }
        }

        [JsonProperty(PropertyName = "Address")] [XmlAttribute(AttributeName = "Address", Form = XmlSchemaForm.Unqualified, DataType = "string")] [EditorBrowsable(EditorBrowsableState.Advanced)] public string __Address;

        [JsonIgnore]
        [XmlIgnore]
        public string Address
        {
            get { return __Address; }
            set { __Address = value; }
        }

        public EmailAddress()
        {
        }
    }

    [JsonObject(Title = "ToEmailAddress")]
    [XmlRoot(ElementName = "ToEmailAddress", IsNullable = false), Serializable]
    public class ToEmailAddress : EmailAddress
    {


    }

    [JsonObject(Title = "BCCEmailAddress")]
    [XmlRoot(ElementName = "BCCEmailAddress", IsNullable = false), Serializable]
    public class BCCEmailAddress : EmailAddress
    {


    }

    [JsonObject(Title = "ReplyToEmailAddress")]
    [XmlRoot(ElementName = "ReplyToEmailAddress", IsNullable = false), Serializable]
    public class ReplyToEmailAddress : EmailAddress
    {


    }

    [JsonObject(Title = "FromEmailAddress")]
    [XmlRoot(ElementName = "FromEmailAddress", IsNullable = false), Serializable]
    public class FromEmailAddress : EmailAddress
    {


    }

    [JsonObject(Title = "Ticket")]
    public class Ticket
    {
        [JsonProperty(PropertyName = "Recepient")] [XmlElement(Type = typeof (Recepient), ElementName = "Recepient", IsNullable = false,
            Form = XmlSchemaForm.Qualified)] [EditorBrowsable(EditorBrowsableState.Advanced)] public Recepient
            __Recepient;

        [JsonIgnore]
        [XmlIgnore]
        public Recepient Recepient
        {
            get
            {
                if (__Recepient == null) __Recepient = new Recepient();
                return __Recepient;
            }
            set { __Recepient = value; }
        }

        [JsonProperty(PropertyName = "FreeText")] [XmlElement(ElementName = "FreeText", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string")] [EditorBrowsable(EditorBrowsableState.Advanced)] public string __FreeText;

        [JsonIgnore]
        [XmlIgnore]
        public string FreeText
        {
            get { return __FreeText; }
            set { __FreeText = value; }
        }

        [JsonProperty(PropertyName = "ProductType")] [XmlElement(ElementName = "ProductType", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string"
            )] [EditorBrowsable(EditorBrowsableState.Advanced)] public string __ProductType;

        [JsonIgnore]
        [XmlIgnore]
        public string ProductType
        {
            get { return __ProductType; }
            set { __ProductType = value; }
        }

        [JsonProperty(PropertyName = "EmailContentType")] [XmlElement(ElementName = "EmailContentType", IsNullable = false, Form = XmlSchemaForm.Qualified)] [EditorBrowsable(EditorBrowsableState.Advanced)] public EmailContentType __EmailContentType;


        [JsonIgnore] [XmlIgnore] [EditorBrowsable(EditorBrowsableState.Advanced)] public bool
            __EmailContentTypeSpecified;

        [JsonIgnore]
        [XmlIgnore]
        public EmailContentType EmailContentType
        {
            get { return __EmailContentType; }
            set
            {
                __EmailContentType = value;
                __EmailContentTypeSpecified = true;
            }
        }

        [JsonProperty(PropertyName = "Fids")] [XmlElement(Type = typeof (Factiva.Gateway.Messages.Archive.V2_0.DistDocField), ElementName = "Fids",
            IsNullable = false, Form = XmlSchemaForm.Qualified)] [EditorBrowsable(EditorBrowsableState.Advanced)] public
            FidsCollection __FidsCollection;


        [JsonIgnore]
        [XmlIgnore]
        public FidsCollection FidsCollection
        {
            get
            {
                if (__FidsCollection == null) __FidsCollection = new FidsCollection();
                return __FidsCollection;
            }
            set { __FidsCollection = value; }
        }

        [JsonProperty(PropertyName = "EmailDisplayLanguage")] [XmlElement(ElementName = "EmailDisplayLanguage", Form = XmlSchemaForm.Unqualified, DataType = "string")] [EditorBrowsable(EditorBrowsableState.Advanced)] public string __EmailDisplayLanguage;


        [JsonIgnore]
        [XmlIgnore]
        public string EmailDisplayLanguage
        {
            get { return __EmailDisplayLanguage; }
            set { __EmailDisplayLanguage = value; }
        }

        [JsonProperty(PropertyName = "ContentAsAttachment")] [XmlElement(ElementName = "ContentAsAttachment", IsNullable = false, Form = XmlSchemaForm.Qualified,
            DataType = "boolean")] [EditorBrowsable(EditorBrowsableState.Advanced)] public bool __ContentAsAttachment;


        [JsonIgnore] [XmlIgnore] public bool __ContentAsAttachmentSpecified;


        [JsonIgnore]
        [XmlIgnore]
        public bool ContentAsAttachment
        {
            get { return __ContentAsAttachment; }
            set
            {
                __ContentAsAttachment = value;
                __ContentAsAttachmentSpecified = true;
            }
        }

        [JsonProperty(PropertyName = "AddClientCode")] [XmlElement(ElementName = "AddClientCode", IsNullable = false, Form = XmlSchemaForm.Qualified,
            DataType = "boolean")] [EditorBrowsable(EditorBrowsableState.Advanced)] public bool __AddClientCode;

        [JsonIgnore] [XmlIgnore] public bool __AddClientCodeSpecified;

        [JsonIgnore]
        [XmlIgnore]
        public bool AddClientCode
        {
            get { return __AddClientCode; }
            set
            {
                __AddClientCode = value;
                __AddClientCodeSpecified = true;
            }
        }

        [JsonProperty(PropertyName = "IncludeHeadlinesInCover")] [XmlElement(ElementName = "IncludeHeadlinesInCover", IsNullable = false, Form = XmlSchemaForm.Qualified,
            DataType = "boolean")] [EditorBrowsable(EditorBrowsableState.Advanced)] public bool
            __IncludeHeadlinesInCover;


        [JsonIgnore] [XmlIgnore] public bool __IncludeHeadlinesInCoverSpecified;

        [JsonIgnore]
        [XmlIgnore]
        public bool IncludeHeadlinesInCover
        {
            get { return __IncludeHeadlinesInCover; }
            set
            {
                __IncludeHeadlinesInCover = value;
                __IncludeHeadlinesInCoverSpecified = true;
            }
        }

        [JsonProperty(PropertyName = "Content")] [XmlElement(Type = typeof (Content), ElementName = "Content", IsNullable = false, Form = XmlSchemaForm.Qualified
            )] [EditorBrowsable(EditorBrowsableState.Advanced)] public ContentCollection __ContentCollection;

        [JsonIgnore]
        [XmlIgnore]
        public ContentCollection ContentCollection
        {
            get
            {
                if (__ContentCollection == null) __ContentCollection = new ContentCollection();
                return __ContentCollection;
            }
            set { __ContentCollection = value; }
        }

        public Ticket()
        {
            //this.ContentCollection = new ContentCollection();
        }
    }

    [JsonConverter(typeof (MyCustomConverter))]
    [JsonObject(Title = "Content")]
    [XmlInclude(typeof (ContentByID))]
    [XmlInclude(typeof (ContentByData))]

    [KnownType(typeof(ContentByID))]
    [KnownType(typeof(ContentByData))]
    [XmlType(TypeName = "Content", Namespace = Declarations.SchemaVersion), Serializable]
    public abstract partial class Content
    {
        private class MyCustomConverter : JsonCreationConverter<Content>
        {
            protected override Content Create(Type objectType,
                Newtonsoft.Json.Linq.JObject jObject)
            {
                if ("ContentByData".Equals(jObject.Value<string>("$type")))
                    return new ContentByData();
                else
                    return new ContentByID();
            }
        }


        [JsonProperty(PropertyName = "ContentData")] [XmlElement(ElementName = "ContentData", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string"
            )] [EditorBrowsable(EditorBrowsableState.Advanced)] public string __ContentData;


        [JsonIgnore]
        [XmlIgnore]
        public string ContentData
        {
            get { return __ContentData; }
            set { __ContentData = value; }
        }

        public Content()
        {
        }
    }

    [JsonArray]
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class ContentCollection : List<Content>
    {
    }

    [JsonArray]
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
//[KnownType(typeof(Factiva.Gateway.Messages.Archive.V2_0.DistDocField))]
    public class FidsCollection : List<Factiva.Gateway.Messages.Archive.V2_0.DistDocField>
    {
    }


    [JsonObject(Title = "CCEmailAddress")]
    [XmlRoot(ElementName = "CCEmailAddress", IsNullable = false), Serializable]
    public class CCEmailAddress : EmailAddress
    {


    }

    [JsonObject(Title = "Recepient")]
    [XmlRoot(ElementName = "Recepient", IsNullable = false), Serializable]
    public class Recepient
    {

        [JsonProperty(PropertyName = "ToEmailAddress")] [XmlElement(Type = typeof (ToEmailAddress), ElementName = "ToEmailAddress", IsNullable = false,
            Form = XmlSchemaForm.Qualified)] [EditorBrowsable(EditorBrowsableState.Advanced)] public ToEmailCollection
            __ToEmailCollection;

        [JsonIgnore]
        [XmlIgnore]
        public ToEmailCollection ToEmailCollection
        {
            get
            {
                if (__ToEmailCollection == null) __ToEmailCollection = new ToEmailCollection();
                return __ToEmailCollection;
            }
            set { __ToEmailCollection = value; }
        }

        [JsonProperty(PropertyName = "CCEmailAddress")] [XmlElement(Type = typeof (CCEmailAddress), ElementName = "CCEmailAddress", IsNullable = false,
            Form = XmlSchemaForm.Qualified)] [EditorBrowsable(EditorBrowsableState.Advanced)] public CCEmailCollection
            __CCEmailCollection;

        [JsonIgnore]
        [XmlIgnore]
        public CCEmailCollection CCEmailCollection
        {
            get
            {
                if (__CCEmailCollection == null) __CCEmailCollection = new CCEmailCollection();
                return __CCEmailCollection;
            }
            set { __CCEmailCollection = value; }
        }

        [JsonProperty(PropertyName = "BCCEmailAddress")] [XmlElement(Type = typeof (BCCEmailAddress), ElementName = "BCCEmailAddress", IsNullable = false,
            Form = XmlSchemaForm.Qualified)] [EditorBrowsable(EditorBrowsableState.Advanced)] public BCCEmailCollection
            __BCCEmailCollection;


        [JsonIgnore]
        [XmlIgnore]
        public BCCEmailCollection BCCEmailCollection
        {
            get
            {
                if (__BCCEmailCollection == null) __BCCEmailCollection = new BCCEmailCollection();
                return __BCCEmailCollection;
            }
            set { __BCCEmailCollection = value; }
        }

        [JsonProperty(PropertyName = "FromEmailAddress")] [XmlElement(Type = typeof (FromEmailAddress), ElementName = "FromEmailAddress", IsNullable = false,
            Form = XmlSchemaForm.Qualified)] [EditorBrowsable(EditorBrowsableState.Advanced)] public FromEmailAddress
            __FromEmailAddress;


        [JsonIgnore]
        [XmlIgnore]
        public FromEmailAddress FromEmailAddress
        {
            get
            {
                if (__FromEmailAddress == null) __FromEmailAddress = new FromEmailAddress();
                return __FromEmailAddress;
            }
            set { __FromEmailAddress = value; }
        }

        [JsonProperty(PropertyName = "ReplyToEmailAddress")] [XmlElement(Type = typeof (ReplyToEmailAddress), ElementName = "ReplyToEmailAddress", IsNullable = false,
            Form = XmlSchemaForm.Qualified)] [EditorBrowsable(EditorBrowsableState.Advanced)] public ReplyToEmailAddress
            __ReplyToEmailAddress;

        [JsonIgnore]
        [XmlIgnore]
        public ReplyToEmailAddress ReplyToEmailAddress
        {
            get
            {
                if (__ReplyToEmailAddress == null) __ReplyToEmailAddress = new ReplyToEmailAddress();
                return __ReplyToEmailAddress;
            }
            set { __ReplyToEmailAddress = value; }
        }

        [JsonProperty(PropertyName = "EmailDisplayFormat")] [XmlElement(ElementName = "EmailDisplayFormat", IsNullable = false, Form = XmlSchemaForm.Qualified)] [EditorBrowsable(EditorBrowsableState.Advanced)] public EmailDisplayFormat __EmailDisplayFormat;


        [JsonIgnore] [XmlIgnore] public bool __EmailDisplayFormatSpecified;


        [JsonIgnore]
        [XmlIgnore]
        public EmailDisplayFormat EmailDisplayFormat
        {
            get { return __EmailDisplayFormat; }
            set
            {
                __EmailDisplayFormat = value;
                __EmailDisplayFormatSpecified = true;
            }
        }

        [JsonProperty(PropertyName = "Subject")] [XmlElement(ElementName = "Subject", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string")] [EditorBrowsable(EditorBrowsableState.Advanced)] public string __Subject;


        [JsonIgnore]
        [XmlIgnore]
        public string Subject
        {
            get { return __Subject; }
            set { __Subject = value; }
        }

        public Recepient()
        {
        }
    }

    public abstract class JsonCreationConverter<T> : JsonConverter
    {
        public override bool CanWrite
        {
            get { return false; }
        }

        protected abstract T Create(Type objectType, JObject jObject);

        public override bool CanConvert(Type objectType)
        {
            return typeof (T).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType,
            object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;
            // Load JObject from stream 
            JObject jObject = JObject.Load(reader);

            // Create target object based on JObject 
            T target = Create(objectType, jObject);

            // Populate the object properties 
            try
            {
                serializer.Populate(jObject.CreateReader(), target);
            }
            catch
            {

            }

            return target;
        }

        public override void WriteJson(JsonWriter writer, object value,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    [JsonConverter(typeof (MyCustomConverter))]
    [JsonObject(Title = "EmailCommand")]
    [XmlType(TypeName = "EmailCommand"), Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    [XmlInclude(typeof (FolderInvite))]
    [XmlInclude(typeof (UserWelcome))]
    [XmlInclude(typeof (UserWelcomeConfirmation))]
    [XmlInclude(typeof (EmailValidate))]
    [XmlInclude(typeof (EmailLoginEnabled))]
    [XmlInclude(typeof (PasswordReset))]
    [XmlInclude(typeof (AlertConfirmation))]
    [XmlInclude(typeof (AlertUnsubscribe))]
    public abstract class EmailCommand
    {

        private class MyCustomConverter : JsonCreationConverter<EmailCommand>
        {
            protected override EmailCommand Create(Type objectType,
                Newtonsoft.Json.Linq.JObject jObject)
            {
                if ("FolderInvite".Equals(jObject.Value<string>("$type")))
                    return new FolderInvite();
                else if ("UserWelcome".Equals(jObject.Value<string>("$type")))
                    return new UserWelcome();
                else if ("UserWelcomeConfirmation".Equals(jObject.Value<string>("$type")))
                    return new UserWelcomeConfirmation();
                else if ("EmailValidate".Equals(jObject.Value<string>("$type")))
                    return new EmailValidate();
                else if ("EmailLoginEnabled".Equals(jObject.Value<string>("$type")))
                    return new EmailLoginEnabled();
                else if ("PasswordReset".Equals(jObject.Value<string>("$type")))
                    return new PasswordReset();
                else if ("AlertConfirmation".Equals(jObject.Value<string>("$type")))
                    return new AlertConfirmation();
                else if ("AlertUnsubscribe".Equals(jObject.Value<string>("$type")))
                    return new AlertUnsubscribe();
                else
                    return null;
            }
        }


        public EmailCommand()
        {
        }
    }

    [JsonObject]
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Always)]
    public class FolderInvite : EmailCommand
    {
        [JsonProperty(PropertyName = "FolderDisplayName")] [XmlElement(ElementName = "FolderDisplayName", IsNullable = false, Form = XmlSchemaForm.Qualified,
            DataType = "string")] [EditorBrowsable(EditorBrowsableState.Advanced)] public string __FolderDisplayName;


        [JsonIgnore]
        [XmlIgnore]
        public string FolderDisplayName
        {
            get { return __FolderDisplayName; }
            set
            {
                __FolderDisplayName = value;
                __FolderDisplayNameSpecified = true;
            }
        }


        [JsonIgnore] [XmlIgnore] [EditorBrowsable(EditorBrowsableState.Advanced)] public bool
            __FolderDisplayNameSpecified;

        [JsonProperty(PropertyName = "SubscribeURL")] [XmlElement(ElementName = "SubscribeURL", IsNullable = false, Form = XmlSchemaForm.Qualified,
            DataType = "string")] [EditorBrowsable(EditorBrowsableState.Advanced)] public string __SubscribeURL;


        [JsonIgnore]
        [XmlIgnore]
        public string SubscribeURL
        {
            get { return __SubscribeURL; }
            set
            {
                __SubscribeURL = value;
                __SubscribeURLSpecified = true;
            }
        }


        [JsonIgnore] [XmlIgnore] [EditorBrowsable(EditorBrowsableState.Advanced)] private bool __SubscribeURLSpecified;

        [JsonProperty(PropertyName = "ANs")] [XmlElement(Type = typeof (string), ElementName = "ANs", IsNullable = false, Form = XmlSchemaForm.Qualified)] [EditorBrowsable(EditorBrowsableState.Advanced)] public AnCollection __AnCollection;


        [JsonIgnore]
        [XmlIgnore]
        public AnCollection AnCollection
        {
            get
            {
                if (__AnCollection == null) __AnCollection = new AnCollection();
                return __AnCollection;
            }
            set { __AnCollection = value; }
        }

        public FolderInvite()
        {
        }
    }

    [JsonObject]
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Always)]
    public class UserWelcome : EmailCommand
    {
        [JsonProperty(PropertyName = "UserID")] [XmlElement(ElementName = "UserID", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string")] [EditorBrowsable(EditorBrowsableState.Advanced)] public string __UserID;


        [JsonIgnore]
        [XmlIgnore]
        public string UserID
        {
            get { return __UserID; }
            set { __UserID = value; }
        }

        [JsonProperty(PropertyName = "Namespace")] [XmlElement(ElementName = "Namespace", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string")] [EditorBrowsable(EditorBrowsableState.Advanced)] public string __Namespace;


        [JsonIgnore]
        [XmlIgnore]
        public string Namespace
        {
            get { return __Namespace; }
            set { __Namespace = value; }
        }


        [JsonProperty(PropertyName = "SubmitURL")] [XmlElement(ElementName = "SubmitURL", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string")] [EditorBrowsable(EditorBrowsableState.Advanced)] public string __SubmitURL;


        [JsonIgnore]
        [XmlIgnore]
        public string SubmitURL
        {
            get { return __SubmitURL; }
            set
            {
                __SubmitURL = value;
                __SubmitURLSpecified = true;
            }
        }

        [JsonProperty(PropertyName = "ProductURL")] [XmlElement(ElementName = "ProductURL", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string")
                                                    ] [EditorBrowsable(EditorBrowsableState.Advanced)] public string __ProductURL;


        [JsonIgnore]
        [XmlIgnore]
        public string ProductURL
        {
            get { return __ProductURL; }
            set { __ProductURL = value; }
        }


        [JsonIgnore] [XmlIgnore] [EditorBrowsable(EditorBrowsableState.Advanced)] private bool __SubmitURLSpecified;

        public UserWelcome()
        {
        }
    }

    [JsonObject]
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Always)]
    public class UserWelcomeConfirmation : UserWelcome
    {
    }


    [JsonObject]
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Always)]
    public class EmailValidate : UserWelcome
    {
    }

    [JsonObject]
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Always)]
    public class EmailLoginEnabled : UserWelcome
    {
    }

    [JsonObject]
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Always)]
    public class PasswordReset : EmailCommand
    {
        [JsonProperty(PropertyName = "UserID")] [XmlElement(ElementName = "UserID", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string")] [EditorBrowsable(EditorBrowsableState.Advanced)] public string __UserID;

        [JsonIgnore]
        [XmlIgnore]
        public string UserID
        {
            get { return __UserID; }
            set { __UserID = value; }
        }

        [JsonProperty(PropertyName = "Namespace")] [XmlElement(ElementName = "Namespace", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string")] [EditorBrowsable(EditorBrowsableState.Advanced)] public string __Namespace;


        [JsonIgnore]
        [XmlIgnore]
        public string Namespace
        {
            get { return __Namespace; }
            set { __Namespace = value; }
        }


        [JsonProperty(PropertyName = "ResetURL")] [XmlElement(ElementName = "ResetURL", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string")] [EditorBrowsable(EditorBrowsableState.Advanced)] public string __ResetURL;


        [JsonIgnore]
        [XmlIgnore]
        public string ResetURL
        {
            get { return __ResetURL; }
            set
            {
                __ResetURL = value;
                __ResetURLSpecified = true;
            }
        }

        [JsonProperty(PropertyName = "SourceIP")] [XmlElement(ElementName = "SourceIP", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string")] [EditorBrowsable(EditorBrowsableState.Advanced)] public string __SourceIP;

        [JsonIgnore]
        [XmlIgnore]
        public string SourceIP
        {
            get { return __SourceIP; }
            set { __SourceIP = value; }
        }

        [JsonProperty(PropertyName = "LoginURL")] [XmlElement(ElementName = "LoginURL", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string")] [EditorBrowsable(EditorBrowsableState.Advanced)] public string __LoginURL;


        [JsonIgnore]
        [XmlIgnore]
        public string LoginURL
        {
            get { return __LoginURL; }
            set
            {
                __LoginURL = value;
                __LoginURLSpecified = true;
            }
        }

        [JsonProperty(PropertyName = "DomainReplaceString")] [XmlElement(ElementName = "DomainReplaceString", IsNullable = false, Form = XmlSchemaForm.Qualified,
            DataType = "string")] [EditorBrowsable(EditorBrowsableState.Advanced)] public string __DomainReplaceString;

        [JsonIgnore]
        [XmlIgnore]
        public string DomainReplaceString
        {
            get { return __DomainReplaceString; }
            set { __DomainReplaceString = value; }
        }

        [JsonProperty(PropertyName = "LoginStatus")] [XmlElement(ElementName = "LoginStatus", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string"
            )] [EditorBrowsable(EditorBrowsableState.Advanced)] public string __LoginStatus;

        [JsonIgnore]
        [XmlIgnore]
        public string LoginStatus
        {
            get { return __LoginStatus; }
            set { __LoginStatus = value; }
        }

        [JsonProperty(PropertyName = "Domain")] [XmlElement(ElementName = "Domain", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string")] [EditorBrowsable(EditorBrowsableState.Advanced)] public string __Domain;

        [JsonIgnore]
        [XmlIgnore]
        public string Domain
        {
            get { return __Domain; }
            set
            {
                __Domain = value;
                __DomainSpecified = true;
            }
        }

        [JsonIgnore] [XmlIgnore] [EditorBrowsable(EditorBrowsableState.Advanced)] private bool __ResetURLSpecified;

        [JsonIgnore] [XmlIgnore] [EditorBrowsable(EditorBrowsableState.Advanced)] private bool __LoginURLSpecified;

        [JsonIgnore] [XmlIgnore] [EditorBrowsable(EditorBrowsableState.Advanced)] private bool __DomainSpecified;

        public PasswordReset()
        {
        }
    }

    [JsonObject]
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Always)]
    public class AlertConfirmation : EmailCommand
    {
        [JsonProperty(PropertyName = "UserID")] [XmlElement(ElementName = "UserID", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string")] [EditorBrowsable(EditorBrowsableState.Advanced)] public string __UserID;

        [JsonIgnore]
        [XmlIgnore]
        public string UserID
        {
            get { return __UserID; }
            set { __UserID = value; }
        }

        [JsonProperty(PropertyName = "Namespace")] [XmlElement(ElementName = "Namespace", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string")] [EditorBrowsable(EditorBrowsableState.Advanced)] public string __Namespace;

        [JsonIgnore]
        [XmlIgnore]
        public string Namespace
        {
            get { return __Namespace; }
            set { __Namespace = value; }
        }


        [JsonProperty(PropertyName = "FolderName")] [XmlElement(ElementName = "FolderName", IsNullable = false, Form = XmlSchemaForm.Qualified, DataType = "string")
                                                    ] [EditorBrowsable(EditorBrowsableState.Advanced)] public string __FolderName;

        [JsonIgnore]
        [XmlIgnore]
        public string FolderName
        {
            get { return __FolderName; }
            set
            {
                __FolderName = value;
                __FolderNameSpecified = true;
            }
        }

        [JsonIgnore] [XmlIgnore] [EditorBrowsable(EditorBrowsableState.Advanced)] private bool __FolderNameSpecified;

        public AlertConfirmation()
        {
        }
    }

    [JsonObject]
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Always)]
    public class AlertUnsubscribe : AlertConfirmation
    {
    }


// Type: Factiva.Gateway.Messages.Archive.V2_0.DistDocField
// Assembly: factiva.gateway.messages, Version=7.5.0.81, Culture=neutral, PublicKeyToken=null
// MVID: 913B510B-3748-47D4-ADE4-419E7D96D936
// Assembly location: C:\TFS\Factiva Web UI\trunk\src\Dotcom\OpenUrlUtility\bin\Debug\factiva.gateway.messages.dll

    namespace Factiva.Gateway.Messages.Archive.V2_0
    {
        public enum DistDocField
        {
            AccessionNo,
            AdocTOC,
            AllowTranslation,
            ArchiveDoc,
            Art,
            AttribCode,
            AuthorList,
            AuxDocumentInformation,
            BaseLang,
            By,
            CharCount,
            Circulation,
            Clm,
            Co,
            CodeSets,
            Cr,
            Ct,
            Cur,
            Cx,
            Cy,
            Dict,
            DistDoc,
            Djn,
            DocData,
            DocType,
            Fields,
            FirstDate,
            HandL,
            Hd,
            Hlp,
            Icb,
            Id,
            Idx,
            In,
            LoadDate,
            LoadTime,
            Lp,
            MetaDataPT,
            ModDate,
            ModTime,
            Ncat,
            Ns,
            LogoLink,
            LogoImage,
            LogoSrc,
            OrigSource,
            ParentAccessionNo,
            Pe,
            Properties,
            PubData,
            PubDate,
            PubEdition,
            PubGroupC,
            PubGroupN,
            PublisherN,
            PubPage,
            PubTime,
            PubVol,
            Re,
            ReplyItem,
            Rf,
            Rst,
            Se,
            SegmentCode,
            Snippet,
            SrcCode,
            SrcName,
            SrcPrimaryType,
            SrcRightsType,
            SrcSecondaryType,
            Teaser,
            Td,
            Title,
            Tpc,
            TypeCode,
            WebHits,
            WordCount,
            RevisionNo,
            Url,
            Editor,
            AuxDocInfo,
            SubDocType,
            CrossRef,
            IpId,
        }
    }

    #endregion
}

