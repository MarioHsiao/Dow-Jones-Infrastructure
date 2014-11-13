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

    public partial class Delivery
    {
        public Delivery()
        {
            this.content = new List<Content>();
            this.deliveryDayandTime = new DeliveryDayandTime();
            this.active = true;
            this.contentAsAttachment = false;
            this.showDuplicates = false;
            this.enableDaylightSaving = true;
            //this.id = "0";
        }

        public string id
        {
            get;
            set;
        }

        public string name
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "toEmailAddress", Required = Required.Always)]
        public string toEmailAddress
        {
            get;
            set;
        }

        public ProductType productType
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "deliveryType", Required = Required.Always)]
        public DeliveryType deliveryType
        {
            get;
            set;
        }

        public bool contentAsAttachment
        {
            get;
            set;
        }

        public EmailDisplayFormat emailDisplayFormat
        {
            get;
            set;
        }

        public Language emailDisplaylanguage
        {
            get;
            set;
        }

        public EmailContentType emailContentType
        {
            get;
            set;
        }

        public bool enableDaylightSaving
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "deliveryDayandTime", Required = Required.Always)]
        public DeliveryDayandTime deliveryDayandTime
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "timeZone", Required = Required.Always)]
        public string timeZone
        {
            get;
            set;
        }

        public bool showDuplicates
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "content", Required = Required.Always)]
        public List<Content> content
        {
            get;
            set;
        }

        public bool active
        {
            get;
            set;
        }

        public string createdBy
        {
            get;
            set;
        }

        public DateTime createdDate
        {
            get;
            set;
        }

        public DateTime lastModifiedDate
        {
            get;
            set;
        }

        public bool enableHighlight
        {
            get;
            set;
        }
    }

    public partial class DeliveryEx
    {
        public DeliveryEx()
        {
            this.content = new List<Content>();
            this.deliveryDayandTime = new DeliveryDayandTime();
            this.active = true;
            this.contentAsAttachment = false;
            this.showDuplicates = false;
            this.enableDaylightSaving = true;
        }

        public string id
        {
            get;
            set;
        }

        public string name
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "toEmailAddress", Required = Required.Always)]
        public string toEmailAddress
        {
            get;
            set;
        }


        public ProductType productType
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "deliveryType", Required = Required.Always)]
        public DeliveryType deliveryType
        {
            get;
            set;
        }

        public bool contentAsAttachment
        {
            get;
            set;
        }

        public EmailDisplayFormat emailDisplayFormat
        {
            get;
            set;
        }

        public Language emailDisplaylanguage
        {
            get;
            set;
        }

        public EmailContentType emailContentType
        {
            get;
            set;
        }

        public bool enableDaylightSaving
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "deliveryDayandTime", Required = Required.Always)]
        public DeliveryDayandTime deliveryDayandTime
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "timeZone", Required = Required.Always)]
        public string timeZone
        {
            get;
            set;
        }

        public bool showDuplicates
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "content", Required = Required.Always)]
        public List<Content> content
        {
            get;
            set;
        }

        public bool active
        {
            get;
            set;
        }

        public string createdBy
        {
            get;
            set;
        }

        public System.DateTime createdDate
        {
            get;
            set;
        }

        public System.DateTime lastModifiedDate
        {
            get;
            set;
        }

        public bool enableHighlight
        {
            get;
            set;
        }

        public int emailLimit
        {
            get;
            set;
        }

        public string clientTypeCode
        {
            get;
            set;
        }
    }

    public partial class DeliveryDayandTime
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [XmlElement(ElementName = "deliveryDay", Form = XmlSchemaForm.Unqualified, Type = typeof(List<Day>))]
        public List<Day> deliveryDay
        {
            get;
            set;
        }

        [XmlElement(ElementName = "repeat", Form = XmlSchemaForm.Unqualified)]
        public Repeat repeat
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "deliveryTime", Required = Required.Always)]
        [XmlElement(ElementName = "deliveryTime", Form = XmlSchemaForm.Unqualified, Type = typeof(List<string>))]
        public List<string> deliveryTime
        {
            get;
            set;
        }

        public DeliveryDayandTime()
        {
            this.deliveryDay = new List<Day>();
            this.deliveryTime = new List<string>();
        }

    }

    public partial class Content
    {
        public Content()
        {
            this.headlineSort = HeadlineSort.ByDate;

        }

        [JsonProperty(PropertyName = "contentID", Required = Required.Always)]
        public string contentID
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "contentName", Required = Required.Always)]
        public string contentName
        {
            get;
            set;
        }

        public int position
        {
            get;
            set;
        }

        public HeadlineSort headlineSort
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = "maxHits", Required = Required.Always)]
        public int maxHits
        {
            get;
            set;
        }
    }

    public partial class UserInformation
    {
        public string emailAddress
        {
            get;
            set;
        }

        public TimeZone timeZone
        {
            get;
            set;
        }

        public Language interfaceLanguage
        {
            get;
            set;
        }

        public string countryCode
        {
            get;
            set;
        }

        public string region
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
