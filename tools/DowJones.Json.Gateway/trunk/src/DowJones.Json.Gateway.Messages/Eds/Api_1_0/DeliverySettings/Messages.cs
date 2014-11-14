using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DowJones.Json.Gateway.Messages.Eds.Api_1_0.DeliverySettings
{

    # region Enums

    [Serializable]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ReturnType
    {
        [EnumMember(Value = "Full")]
        Full,
        [EnumMember(Value = "Summary")]
        Summary
    }

    [Serializable]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProductType
    {
        [EnumMember(Value = "global")]
        [XmlEnum(Name = "global")]
        global,
    }

    [Serializable]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DeliveryType
    {
        [EnumMember(Value = "S")]
        [XmlEnum(Name = "S")]
        S,
        [EnumMember(Value = "B")]
        [XmlEnum(Name = "B")]
        B,
        [EnumMember(Value = "C")]
        [XmlEnum(Name = "C")]
        C,
    }

    [Serializable]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EmailDisplayFormat
    {
        [EnumMember(Value = "HTML")]
        [XmlEnum(Name = "HTML")]
        HTML,
        [EnumMember(Value = "ASCII")]
        [XmlEnum(Name = "ASCII")]
        TEXT,
    }

    [Serializable]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EmailContentType
    {
        [EnumMember(Value = "Headline")]
        [XmlEnum(Name = "Headline")]
        Headline,
        [EnumMember(Value = "FullText")]
        [XmlEnum(Name = "Fulltext")]
        Fulltext,
        [EnumMember(Value = "FullTextI")]
        [XmlEnum(Name = "FulltextI")]
        FulltextI,
    }

    [Serializable]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum HeadlineSort
    {
        [EnumMember(Value = "ByDefault")]
        [XmlEnum(Name = "0")]
        ByDefault,
        [EnumMember(Value = "ByDate")]
        [XmlEnum(Name = "1")]
        ByDate,
        [EnumMember(Value = "ByRelevance")]
        [XmlEnum(Name = "2")]
        ByRelevance,
    }

    [Serializable]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum YesNo
    {
        [EnumMember(Value = "yes")]
        [XmlEnum(Name = "yes")]
        Yes,
        [EnumMember(Value = "no")]
        [XmlEnum(Name = "no")]
        No,
    }

    [Serializable]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Language
    {
        [EnumMember(Value = "en")]
        [XmlEnum(Name = "1")]
        en,
        [EnumMember(Value = "fr")]
        [XmlEnum(Name = "2")]
        fr,
        [EnumMember(Value = "de")]
        [XmlEnum(Name = "3")]
        de,
        [EnumMember(Value = "it")]
        [XmlEnum(Name = "4")]
        it,
        [EnumMember(Value = "nl")]
        [XmlEnum(Name = "5")]
        nl,
        [EnumMember(Value = "es")]
        [XmlEnum(Name = "6")]
        es,
        [EnumMember(Value = "da")]
        [XmlEnum(Name = "7")]
        da,
        [EnumMember(Value = "sv")]
        [XmlEnum(Name = "8")]
        sv,
        [EnumMember(Value = "no")]
        [XmlEnum(Name = "9")]
        no,
        [EnumMember(Value = "pt")]
        [XmlEnum(Name = "10")]
        pt,
        [EnumMember(Value = "tr")]
        [XmlEnum(Name = "11")]
        tr,
        [EnumMember(Value = "fi")]
        [XmlEnum(Name = "12")]
        fi,
        [EnumMember(Value = "bg")]
        [XmlEnum(Name = "13")]
        bg,
        [EnumMember(Value = "ca")]
        [XmlEnum(Name = "14")]
        ca,
        [EnumMember(Value = "zhtw")]
        [XmlEnum(Name = "15")]
        zhtw,
        [EnumMember(Value = "zhcn")]
        [XmlEnum(Name = "16")]
        zhcn,
        [EnumMember(Value = "cs")]
        [XmlEnum(Name = "17")]
        cs,
        [EnumMember(Value = "hu")]
        [XmlEnum(Name = "18")]
        hu,
        [EnumMember(Value = "ja")]
        [XmlEnum(Name = "19")]
        ja,
        [EnumMember(Value = "pl")]
        [XmlEnum(Name = "20")]
        pl,
        [EnumMember(Value = "ru")]
        [XmlEnum(Name = "21")]
        ru,
        [EnumMember(Value = "sk")]
        [XmlEnum(Name = "22")]
        sk,
    }

    [Serializable]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Day
    {
        [EnumMember(Value = "Sunday")]
        [XmlEnum(Name = "Sunday")]
        Sunday,
        [EnumMember(Value = "Monday")]
        [XmlEnum(Name = "Monday")]
        Monday,
        [EnumMember(Value = "Tuesday")]
        [XmlEnum(Name = "Tuesday")]
        Tuesday,
        [EnumMember(Value = "Wednesday")]
        [XmlEnum(Name = "Wednesday")]
        Wednesday,
        [EnumMember(Value = "Thursday")]
        [XmlEnum(Name = "Thursday")]
        Thursday,
        [EnumMember(Value = "Friday")]
        [XmlEnum(Name = "Friday")]
        Friday,
        [EnumMember(Value = "Saturday")]
        [XmlEnum(Name = "Saturday")]
        Saturday,
    }

    [Serializable]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Repeat
    {
        [EnumMember(Value = "Daily")]
        [XmlEnum(Name = "Daily")]
        Daily,
        [EnumMember(Value = "Weekly")]
        [XmlEnum(Name = "Weekly")]
        Weekly,
    }

    #endregion

    public class Delivery
    {
        public Delivery()
        {
            this.Content = new List<Content>();
            this.DeliveryDayandTime = new DeliveryDayandTime();
            this.Active = true;
            this.ContentAsAttachment = false;
            this.ShowDuplicates = false;
            this.EnableDaylightSaving = true;
            //this.id = "0";
        }

        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "name")]
        public string Name
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "toEmailAddress")]
        public string ToEmailAddress
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "productType")]
        public ProductType ProductType
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "deliveryType")]
        public DeliveryType DeliveryType
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "contentAsAttachment")]
        public bool ContentAsAttachment
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "emailDisplayFormat")]
        public EmailDisplayFormat EmailDisplayFormat
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "emailDisplaylanguage")]
        public Language EmailDisplaylanguage
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "emailContentType")]
        public EmailContentType EmailContentType
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "enableDaylightSaving")]
        public bool EnableDaylightSaving
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "deliveryDayandTime")]
        public DeliveryDayandTime DeliveryDayandTime
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "timeZone")]
        public string TimeZone
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "showDuplicates")]
        public bool ShowDuplicates
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "content")]
        public List<Content> Content
        {
            get;
            set;
        }

        [JsonIgnore]
        public bool Active
        {
            get;
            set;
        }

        [JsonIgnore]
        public string CreatedBy
        {
            get;
            set;
        }

        [JsonIgnore]
        public DateTime CreatedDate
        {
            get;
            set;
        }

        [JsonIgnore]
        public DateTime LastModifiedDate
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "enableHighlight")]
        public bool EnableHighlight
        {
            get;
            set;
        }
    }

    public class DeliveryEx
    {
        public DeliveryEx()
        {
            this.Content = new List<Content>();
            this.DeliveryDayandTime = new DeliveryDayandTime();
            this.Active = true;
            this.ContentAsAttachment = false;
            this.ShowDuplicates = false;
            this.EnableDaylightSaving = true;
        }

        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "name")]
        public string Name
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "toEmailAddress")]
        public string ToEmailAddress
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "productType")]
        public ProductType ProductType
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "deliveryType")]
        public DeliveryType DeliveryType
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "contentAsAttachment")]
        public bool ContentAsAttachment
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "emailDisplayFormat")]
        public EmailDisplayFormat EmailDisplayFormat
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "emailDisplaylanguage")]
        public Language EmailDisplaylanguage
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "emailContentType")]
        public EmailContentType EmailContentType
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "enableDaylightSaving")]
        public bool EnableDaylightSaving
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "deliveryDayandTime")]
        public DeliveryDayandTime DeliveryDayandTime
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "timeZone")]
        public string TimeZone
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "showDuplicates")]
        public bool ShowDuplicates
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "content")]
        public List<Content> Content
        {
            get;
            set;
        }

        [JsonIgnore]
        public bool Active
        {
            get;
            set;
        }

        [JsonIgnore]
        public string CreatedBy
        {
            get;
            set;
        }

        [JsonIgnore]
        public DateTime CreatedDate
        {
            get;
            set;
        }

        [JsonIgnore]
        public System.DateTime LastModifiedDate
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "enableHighlight")]
        public bool EnableHighlight
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "emailLimit")]
        public int EmailLimit
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "clientTypeCode")]
        public string ClientTypeCode
        {
            get;
            set;
        }
    }

    [JsonObject(Title = "deliveryDayandTime")]
    [DataContract(Name = "deliveryDayandTime", Namespace = "")]
    public class DeliveryDayandTime
    {
        [JsonProperty(PropertyName = "deliveryDay")]
        [XmlElement(ElementName = "deliveryDay", Form = XmlSchemaForm.Unqualified, Type = typeof(List<Day>))]
        public List<Day> DeliveryDay
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "repeat")]
        [XmlElement(ElementName = "repeat", Form = XmlSchemaForm.Unqualified)]
        public Repeat Repeat
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "deliveryTime")]
        [XmlElement(ElementName = "deliveryTime", Form = XmlSchemaForm.Unqualified, Type = typeof(List<string>))]
        public List<string> DeliveryTime
        {
            get;
            set;
        }

        public DeliveryDayandTime()
        {
            this.DeliveryDay = new List<Day>();
            this.DeliveryTime = new List<string>();
        }

    }

    public class Content
    {
        public Content()
        {
            this.HeadlineSort = HeadlineSort.ByDate;

        }

        [JsonProperty(PropertyName = "contentID")]
        public string ContentID
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "contentName")]
        public string ContentName
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "position")]
        public int Position
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "headlineSort")]
        public HeadlineSort HeadlineSort
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "maxHits")]
        public int MaxHits
        {
            get;
            set;
        }
    }

    public class UserInformation
    {
        public string emailAddress
        {
            get;
            set;
        }

        public TimeZone TimeZone
        {
            get;
            set;
        }

        public Language InterfaceLanguage
        {
            get;
            set;
        }

        public string CountryCode
        {
            get;
            set;
        }

        public string Region
        {
            get;
            set;
        }
    }

    #region Transactions

    #region DeleteDeliverySettings Request and Response
    public partial class DeleteDeliverySettings
    {
        public string DeliveryID
        {
            get;
            set;
        }

    }

    public partial class DeleteDeliverySettingsEx
    {
        public string DeliveryID
        {
            get;
            set;
        }

        public string FolderID
        {
            get;
            set;
        }
    }

    public partial class DeleteDeliverySettingsResponse
    {
        public bool IsSuccess
        {
            get;
            set;
        }
    }
    #endregion

    #endregion

}
