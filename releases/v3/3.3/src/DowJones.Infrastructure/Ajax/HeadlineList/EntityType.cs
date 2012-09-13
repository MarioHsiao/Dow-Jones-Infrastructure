using System.Runtime.Serialization;
using DowJones.Attributes;

namespace DowJones.Ajax
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract(Name = "contentCategory", Namespace = "")]
    public enum ContentCategory
    {
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        UnSpecified,
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        External,
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        Publication,
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        Website,
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        Picture,
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        Multimedia,
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        Blog,
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        Board,
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        Internal,
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        Summary,
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        CustomerDoc,
    }

    /// <summary>
    /// 
    /// </summary>
    [DataContract(Name = "contentSubCategory", Namespace = "")]
    public enum ContentSubCategory
    {
        /// <summary>
        ///
        /// </summary>
        [EnumMember]
        UnSpecified,
        /// <summary>
        ///
        /// </summary>
        [EnumMember]
        Analyst,
        /// <summary>
        ///
        /// </summary>
        [EnumMember]
        Blog,
        /// <summary>
        ///
        /// </summary>
        [EnumMember]
        Newspaper,
        /// <summary>
        ///
        /// </summary>
        [EnumMember]
        Audio,
        /// <summary>
        ///
        /// </summary>
        [EnumMember]
        Video,
        /// <summary>
        ///
        /// </summary>
        [EnumMember]
        PDF,
        /// <summary>
        ///
        /// </summary>
        [EnumMember]
        HTML,
        /// <summary>
        ///
        /// </summary>
        [EnumMember]
        Graphic,
        /// <summary>
        ///
        /// </summary>
        [EnumMember]
        Article,
        /// <summary>
        ///
        /// </summary>
        [EnumMember]
        Rss,
        /// <summary>
        ///
        /// </summary>
        [EnumMember]
        Atom,
        /// <summary>
        ///
        /// </summary>
        [EnumMember]
        Multimedia,
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        Board,
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        Internal,
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        Summary,
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        CustomerDoc,
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        File,
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        WebPage,
    }
}

namespace DowJones.Ajax.HeadlineList
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract(Name = "entityType", Namespace = "")]
    public enum EntityType
    {
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        UnSpecified,
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        Company,
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        Person,
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        Organization,
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        Industry,
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        NewsSubject,
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        Region,
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        City,
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        Highlight,
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        Textual,
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        InsightIssue,
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        InsightDiscovery,
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        Author,
    }

    /// <summary>
    /// 
    /// </summary>
    [DataContract(Name = "eventType", Namespace = "")]
    public enum EventType
    {
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        MergersAndAcquisitions,
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        Bankrupcy,
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        Sale,
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        ProductRelease,
    }


    /// <summary>
    /// 
    /// </summary>
    [DataContract(Name = "eventPartType", Namespace = "")]
    public enum EventPartType
    {
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        Company,
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        Industry,
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        Person,
    }

    /// <summary>
    /// 
    /// </summary>
    [DataContract(Name = "importance", Namespace = "")]
    public enum Importance
    {
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        [AssignedToken("importance")]
        Normal,
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        [AssignedToken("breaking")]
        Breaking_Hot,
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        [AssignedToken("new")]
        New,
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        [AssignedToken("mustRead")]
        MustRead
    }
}
