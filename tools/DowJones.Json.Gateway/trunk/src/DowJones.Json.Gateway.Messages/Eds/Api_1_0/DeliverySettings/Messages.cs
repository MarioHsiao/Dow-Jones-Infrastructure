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
    public enum ReturnType
    {
        [EnumMember(Value = "Full")]
        Full,
        [EnumMember(Value = "Summary")]
        Summary
    }

    [Serializable]
    public enum ProductType
    {
        [EnumMember(Value = "global")]
        [XmlEnum(Name = "global")]
        global,
    }

    [Serializable]
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

        public string Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "toEmailAddress", Required = Required.Always)]
        public string ToEmailAddress
        {
            get;
            set;
        }

        public ProductType ProductType
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "deliveryType", Required = Required.Always)]
        public DeliveryType DeliveryType
        {
            get;
            set;
        }

        public bool ContentAsAttachment
        {
            get;
            set;
        }

        public EmailDisplayFormat EmailDisplayFormat
        {
            get;
            set;
        }

        public Language EmailDisplaylanguage
        {
            get;
            set;
        }

        public EmailContentType EmailContentType
        {
            get;
            set;
        }

        public bool EnableDaylightSaving
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "deliveryDayandTime", Required = Required.Always)]
        public DeliveryDayandTime DeliveryDayandTime
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "timeZone", Required = Required.Always)]
        public string TimeZone
        {
            get;
            set;
        }

        public bool ShowDuplicates
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "content", Required = Required.Always)]
        public List<Content> Content
        {
            get;
            set;
        }

        public bool Active
        {
            get;
            set;
        }

        public string CreatedBy
        {
            get;
            set;
        }

        public DateTime CreatedDate
        {
            get;
            set;
        }

        public DateTime LastModifiedDate
        {
            get;
            set;
        }

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

        public string Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "toEmailAddress", Required = Required.Always)]
        public string ToEmailAddress
        {
            get;
            set;
        }


        public ProductType ProductType
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "deliveryType", Required = Required.Always)]
        public DeliveryType DeliveryType
        {
            get;
            set;
        }

        public bool ContentAsAttachment
        {
            get;
            set;
        }

        public EmailDisplayFormat EmailDisplayFormat
        {
            get;
            set;
        }

        public Language EmailDisplaylanguage
        {
            get;
            set;
        }

        public EmailContentType EmailContentType
        {
            get;
            set;
        }

        public bool EnableDaylightSaving
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "deliveryDayandTime", Required = Required.Always)]
        public DeliveryDayandTime DeliveryDayandTime
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "timeZone", Required = Required.Always)]
        public string TimeZone
        {
            get;
            set;
        }

        public bool ShowDuplicates
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "content", Required = Required.Always)]
        public List<Content> Content
        {
            get;
            set;
        }

        public bool Active
        {
            get;
            set;
        }

        public string CreatedBy
        {
            get;
            set;
        }

        public System.DateTime CreatedDate
        {
            get;
            set;
        }

        public System.DateTime LastModifiedDate
        {
            get;
            set;
        }

        public bool EnableHighlight
        {
            get;
            set;
        }

        public int EmailLimit
        {
            get;
            set;
        }

        public string ClientTypeCode
        {
            get;
            set;
        }
    }

    public class DeliveryDayandTime
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "deliveryDay", Form = XmlSchemaForm.Unqualified, Type = typeof(List<Day>))]
        public List<Day> DeliveryDay
        {
            get;
            set;
        }

        [XmlElement(ElementName = "repeat", Form = XmlSchemaForm.Unqualified)]
        public Repeat Repeat
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "deliveryTime", Required = Required.Always)]
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

        [JsonProperty(PropertyName = "contentID", Required = Required.Always)]
        public string ContentID
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "contentName", Required = Required.Always)]
        public string ContentName
        {
            get;
            set;
        }

        public int Position
        {
            get;
            set;
        }

        public HeadlineSort HeadlineSort
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "maxHits", Required = Required.Always)]
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

    #region GetDeliverySettings Request and Response

    public partial class GetDeliverySettings
    {
        public ReturnType ReturnType
        {
            get;
            set;
        }

        public string deliveryID
        {
            get;
            set;
        }

        public DeliveryType? DeliveryType
        {
            get;
            set;
        }
    }

    public partial class GetDeliverySettingsResponse
    {
        public GetDeliverySettingsResponse()
        {
            this.delivery = new List<Delivery>();
        }

        public List<Delivery> delivery
        {
            get;
            set;
        }
    }
    #endregion


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
