using System;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    public struct Declarations
    {
        public const string SchemaVersion = "";

    }

    #region All enumerations

    [JsonConverter(typeof(StringEnumConverter))]
    public enum AuthenticationScheme
    {
        [XmlEnum(Name = "UserId")]
        UserId,
        [XmlEnum(Name = "Email")]
        Email
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ShareType
    {
        [XmlEnum(Name = "Personal")]
        Personal,
        [XmlEnum(Name = "Subscribed")]
        Subscribed,
        [XmlEnum(Name = "Assigned")]
        Assigned,
        [XmlEnum(Name = "All")]
        All
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ShareStatus
    {
        [XmlEnum(Name = "Active")]
        Active,
        [XmlEnum(Name = "Inactive")]
        Inactive
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum AudienceOptions
    {
        [XmlEnum(Name = "InternalAccount")]
        InternalAccount,
        [XmlEnum(Name = "OutsideAccount")]
        OutsideAccount,
        [XmlEnum(Name = "TimeToLive_Proxy")]
        TimeToLive_Proxy,
        [XmlEnum(Name = "ExternalReader")]
        ExternalReader
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum OutputFormat
    {
        [XmlEnum(Name = "None")]
        None,
        [XmlEnum(Name = "HTMLEmailBody")]
        HTMLEmailBody,
        [XmlEnum(Name = "PlainTextEmailBody")]
        PlainTextEmailBody,
        [XmlEnum(Name = "HTMLAttachmentOnly")]
        HTMLAttachmentOnly,
        [XmlEnum(Name = "RTFAttachmentOnly")]
        RTFAttachmentOnly,
        [XmlEnum(Name = "PDFAttachmentOnly")]
        PDFAttachmentOnly,
        [XmlEnum(Name = "Feed")]
        Feed
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum AttachmentType
    {
        [XmlEnum(Name = "None")]
        None,
        [XmlEnum(Name = "PDF")]
        PDF,
        [XmlEnum(Name = "RTF")]
        RTF,
        [XmlEnum(Name = "HTML")]
        HTML
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ShareRole
    {
        [XmlEnum(Name = "NLE")]
        NLE
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum GroupListType
    {
        [XmlEnum(Name = "Group")]
        Group,
        [XmlEnum(Name = "User")]
        User
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ShareScope
    {
        [XmlEnum(Name = "Personal")]
        Personal,
        [XmlEnum(Name = "AccountAdmin")]
        AccountAdmin,
        [XmlEnum(Name = "Group")]
        Group,
        [XmlEnum(Name = "Account")]
        Account,
        [XmlEnum(Name = "Everyone")]
        Everyone
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum AccessScope
    {
        [XmlEnum(Name = "Personal")]
        Personal,
        [XmlEnum(Name = "Platform")]
        Platform
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ElementImageType
    {
        [XmlEnum(Name = "External")]
        External,
        [XmlEnum(Name = "Internal")]
        Internal
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum AccessQualifier
    {
        [XmlEnum(Name = "User")]
        User,
        [XmlEnum(Name = "Account")]
        Account,
        [XmlEnum(Name = "Factiva")]
        Factiva
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum DisseminatedAssetType
    {
        [XmlEnum(Name = "Widget")]
        Widget,
        [XmlEnum(Name = "Edition")]
        Edition
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ElementType
    {
        [XmlEnum(Name = "Image")]
        Image,
        [XmlEnum(Name = "Text")]
        Text,
        [XmlEnum(Name = "Link")]
        Link
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum SortOrder
    {
        [XmlEnum(Name = "Ascending")]
        Ascending,
        [XmlEnum(Name = "Descending")]
        Descending
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum LinkType
    {
        [XmlEnum(Name = "Url")]
        Url,
        [XmlEnum(Name = "DocUrl")]
        DocUrl,
        [XmlEnum(Name = "RssHeadlineUrl")]
        RssHeadlineUrl
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum DisseminatedAssetSortBy
    {
        [XmlEnum(Name = "CreationDate")]
        CreationDate,
        [XmlEnum(Name = "UpdateDate")]
        UpdateDate,
        [XmlEnum(Name = "Name")]
        Name,
        [XmlEnum(Name = "Type")]
        Type,
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum Importance
    {
        [XmlEnum(Name = "Normal")]
        Normal,
        [XmlEnum(Name = "Hot")]
        Hot,
        [XmlEnum(Name = "New")]
        @New,
        [XmlEnum(Name = "MustRead")]
        MustRead
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ContentCategory
    {
        [XmlEnum(Name = "Unspecified")]
        Unspecified,
        [XmlEnum(Name = "Publications")]
        Publications,
        [XmlEnum(Name = "Blogs")]
        Blogs,
        [XmlEnum(Name = "WebSites")]
        WebSites,
        [XmlEnum(Name = "Pictures")]
        Pictures,
        [XmlEnum(Name = "Multimedia")]
        Multimedia,
        [XmlEnum(Name = "Boards")]
        Boards,
        [XmlEnum(Name = "CustomerDoc")]
        CustomerDoc
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ComplianceStatus
    {
        Pending = 1,
        Approved,
        Rejected,
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum AddAtPositionMode
    {
        [XmlEnum(Name = "EndOfList")]
        EndOfList,
        [XmlEnum(Name = "StartOfList")]
        StartOfList
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum DateFilterTarget
    {
        [XmlEnum(Name = "CreateDate")]
        CreateDate,
        [XmlEnum(Name = "LastModified")]
        LastModified,
        [XmlEnum(Name = "LastContentModified")]
        LastContentModified
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum WorkspaceType
    {
        [XmlEnum(Name = "Automatic")]
        Automatic,
        [XmlEnum(Name = "Newsletter")]
        Newsletter,
        [XmlEnum(Name = "Collection")]
        Collection,
        [XmlEnum(Name = "ComplianceBin")]
        ComplianceBin,
        //[XmlEnum(Name = "DJAPTopic")]
        //DJAPTopic,
        //[XmlEnum(Name = "DJAPFavorites")]
        //DJAPFavorites,
        //[XmlEnum(Name = "DJAPNewsletter")]
        //DJAPNewsletter,
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum CollectionType
    {
        [XmlEnum(Name = "InvestmentBanking")]
        InvestmentBanking,
        [XmlEnum(Name = "WealthManagement")]
        WealthManagement,
        [XmlEnum(Name = "Energy")]
        Energy
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum UpdateWorkspaceMode
    {
        [XmlEnum(Name = "Save")]
        Save,
        [XmlEnum(Name = "SaveAndPublish")]
        SaveAndPublish
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum DataContentFilterOperator
    {
        [XmlEnum(Name = "Published")]
        Published,
        [XmlEnum(Name = "Saved")]
        Saved
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ComplianceBinScope
    {
        [XmlEnum(Name = "Personal")]
        Personal,
        [XmlEnum(Name = "Account")]
        Account
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum Status
    {
        [XmlEnum(Name = "On")]
        On,
        [XmlEnum(Name = "Off")]
        Off
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum WorkspaceSortBy
    {
        [XmlEnum(Name = "FeedActive")]
        FeedActive,
        [XmlEnum(Name = "CreatedDate")]
        CreatedDate,
        [XmlEnum(Name = "LastModified")]
        LastModified,
        [XmlEnum(Name = "Name")]
        Name
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum WorkspaceScopeType
    {
        [XmlEnum(Name = "Personal")]
        Personal,
        [XmlEnum(Name = "Platform")]
        Platform
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum FilterOperator
    {
        [XmlEnum(Name = "AND")]
        AND,
        [XmlEnum(Name = "OR")]
        OR
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum FilterSearchOperator
    {
        [XmlEnum(Name = "BeginsWith")]
        BeginsWith,
        [XmlEnum(Name = "Contains")]
        Contains,
        [XmlEnum(Name = "Exact")]
        Exact
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum Segment
    {
        Unspecified,
        WM,
        IB,
        RKW,
        PRCC,
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum Product
    {

        Unspecified = 0,
        //BRI,
        IB,
        Advisor,
        PrivateMarkets,
        //BRITriggers,
        //MM,
        //Consultancy,
        //Newsplus,
        //DJAP,
        //DJA,
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum FeedAssetType
    {
        [XmlEnum(Name = "Alert")]
        Alert,
        [XmlEnum(Name = "RSS")]
        RSS,
        [XmlEnum(Name = "SavedSearch")]
        SavedSearch,
        [XmlEnum(Name = "SimpleSearch")]
        SimpleSearch,
        [XmlEnum(Name = "Workspace")]
        Workspace
    }


    #endregion
   
}


